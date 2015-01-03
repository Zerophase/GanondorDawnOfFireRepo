using UnityEngine;
using System.Collections;

public class FuzzySet
{
    protected float domain;
    public float Domain { get { return domain; } set { domain = value; } }
    protected float representativeValue;
    public float RepresentativeValue { get { return representativeValue; } }
    public FuzzySet(float repValue)
    {
        this.domain = 0.0f;
        this.representativeValue = repValue;
    }

    public virtual float CalculateDOM(float val)
    {
        //TODO write CalculateDom
        return 1f;
    }

    public void ORwithDOM(float val)
    {
        if (val > domain)
            domain = val;
    }

    public void ClearDomain()
    {
        domain = 0.0f;
    }

    public static bool IsEqual(float a, float b)
    {
        float min = float.MinValue;
        if (Mathf.Abs(a - b) < min)
        {
            return true;
        }

        return false;
    }
}
