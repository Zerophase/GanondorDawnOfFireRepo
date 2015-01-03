using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Flags]
public enum ActiveSteeringBehaviors
{
    NONE = 0,
    SEPERATION = 2,
    ALLIGNMENT = 4,
    COHESION = 6,
    SEEK = 8
};

public class SteeringBehaviors 
{
    const float prSeek = 1.0f;
    const float prCohesion = 0.3f;
    float prSeperation;
    const float prAllignmnet = 0.6f;

    float weightSeperation = 1f;
    float weightCohesion = 1f;
    float weightSeek = 1f;
    float weightAllignment = 1f;

    ActiveSteeringBehaviors flags;

    private List<Unit> units;
    public List<Unit> Units { get { return units; } set { units = value; } }

    public SteeringBehaviors()
    {
        seekOn();
        seperationOn();
        prSeperation = 0.3f;
    }

    public SteeringBehaviors(List<Unit> units)
    {
        seperationOn();
        allignmentOn();
        cohesionOn();
        seekOn();

        prSeperation = 0.8f;

        this.units = units;
    }

    bool On(ActiveSteeringBehaviors asb) { return (flags & asb) == asb; }
    void seperationOn() { flags |= ActiveSteeringBehaviors.SEPERATION; }
    void allignmentOn() { flags |= ActiveSteeringBehaviors.ALLIGNMENT; }
    void cohesionOn() { flags |= ActiveSteeringBehaviors.COHESION; }
    void seekOn() { flags |= ActiveSteeringBehaviors.SEEK; }

    //TODO write other methods to turn off steering behaviors
    void seperationOff() { if (On(ActiveSteeringBehaviors.SEPERATION)) flags ^= ActiveSteeringBehaviors.SEPERATION; }

    public Vector3 PriotizedDithering(Unit unit)
    {
        Vector3 steeringForce = Vector3.zero;

        if (On(ActiveSteeringBehaviors.SEPERATION) && Random.Range(0.0f, 1.0f) < prSeperation)
        {
            if (!unit.Incombat)
            {
                steeringForce += seperation(unit) *
                            (weightSeperation / prSeperation);
            }
            else
                steeringForce += seperation(unit, unit.Target) * //Target assignment for combat must come before calling prioritized dithering
                            (weightSeperation / prSeperation);
            

            if (isZero(steeringForce))
               return Vector3.ClampMagnitude(steeringForce, unit.MaxForce);
        }

        if (On(ActiveSteeringBehaviors.ALLIGNMENT) && Random.Range(0.0f, 1.0f) < prAllignmnet)
        {
            steeringForce += allignment(unit) *
                            (weightAllignment / prAllignmnet);

            if (isZero(steeringForce))
                return Vector3.ClampMagnitude(steeringForce, unit.MaxForce);
        }

        if (On(ActiveSteeringBehaviors.COHESION) && Random.Range(0.0f, 1.0f) < prCohesion)
        {
            steeringForce += cohesion(unit) *
                            (weightCohesion / prCohesion);

            if (isZero(steeringForce))
                return Vector3.ClampMagnitude(steeringForce, unit.MaxForce);
        }

        if(On(ActiveSteeringBehaviors.SEEK) && Random.Range(0.0f, 1.0f) < prSeek)
        {

            if (units == null)
                steeringForce += seek(unit) * weightSeek / prSeek;
            else if (units.Find(item => item.IsCommander == unit.IsCommander))
                steeringForce += seek(unit) * weightSeek / prSeek;
            else
                steeringForce += seek(units.Find(item => item.IsCommander == true).Target, unit) *
                                weightSeek / prSeek;

            if (isZero(steeringForce))
                return Vector3.ClampMagnitude(steeringForce, unit.MaxForce);
        }

        return steeringForce;
    }

    private bool isZero(Vector3 vector3)
    {
        if (vector3.sqrMagnitude < .01f)
            return false;
        else
            return true;
    }

    private Vector3 seperation(Unit unit, Vector3 targetPosition)
    {
        Vector3 steeringForce = Vector3.zero;
        
        Vector3 toAgent = (unit.transform.position - targetPosition);
        steeringForce += Vector3.Normalize(toAgent) / Vector3.Magnitude(toAgent);
          
        return steeringForce;
    }

    private Vector3 seperation(Unit unit)
    {
        Vector3 steeringForce = Vector3.zero;

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].ID != unit.ID)
            {
                Vector3 toAgent = (unit.transform.position - units[i].transform.position);
                steeringForce += Vector3.Normalize(toAgent) / Vector3.Magnitude(toAgent);
            }
        }
        return steeringForce;
    }

    private Vector3 allignment(Unit unit)
    {
        Vector3 averageHeading = Vector3.zero;
        int neighborCount = 0;

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].ID != unit.ID)
            {
                averageHeading += units[i].transform.forward;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            averageHeading /= neighborCount;
            averageHeading -= unit.transform.forward;
        }

        return averageHeading;
    }

    private Vector3 cohesion(Unit unit, Vector3 targetUnit)
    {
        Vector3 centerOfMass = Vector3.zero, steeringForce = Vector3.zero;

        int neighborCount = 0;

        centerOfMass += unit.transform.position;
        neighborCount++;

        if (neighborCount > 0)
        {
            centerOfMass /= neighborCount;
            steeringForce = seek(centerOfMass, unit);
        }

        return steeringForce;
    }

    private Vector3 cohesion(Unit unit)
    {
        Vector3 centerOfMass = Vector3.zero, steeringForce = Vector3.zero;

        int neighborCount = 0;

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].ID != unit.ID)
            {
                centerOfMass += units[i].transform.position;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            centerOfMass /= neighborCount;
            steeringForce = seek(centerOfMass, unit);
        }

        return steeringForce;
    }

    private Vector3 seek(Unit unit)
    {
        Vector3 desiredVelocity = Vector3.Normalize(unit.Target - unit.transform.position)
                                   * unit.MaxSpeed;

        return (desiredVelocity - unit.Velocity);
    }

    private Vector3 seek(Vector3 target, Unit unit)
    {
        Vector3 desiredVelocity = Vector3.Normalize(target - unit.transform.position) 
                                    * unit.MaxSpeed;

        return (desiredVelocity - unit.Velocity);
    }
}
