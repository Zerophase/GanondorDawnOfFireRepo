using UnityEngine;
using System.Collections;

public class Grapher2 : MonoBehaviour 
{
    [Range(10, 100)]
    public int Resolution = 10;

    private int currentResolution;
    private ParticleSystem.Particle[] points;

    public enum FunctionOptions
    {
        LINEAR,
        EXPONENTIAL,
        PARABOLA,
        SINE,
        RIPPLE
    }

    public FunctionOptions Function;

    private delegate float functionDelegate(Vector3 p, float t);
    private static functionDelegate[] functionDelegates = 
    {
        linear,
        exponential,
        parabola,
        sine,
        ripple
    };

    void Update()
    {
        if (currentResolution != Resolution || points == null)
            createPoints();

        functionDelegate f = functionDelegates[(int)Function];
        float t = Time.timeSinceLevelLoad;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 p = points[i].position;
            p.y = f(p, t);
            points[i].position = p;

            Color c = points[i].color;
            c.g = p.y;
            points[i].color = c;
        }

        particleSystem.SetParticles(points, points.Length);
    }

    private static float linear(Vector3 p, float t)
    {
        return p.x;
    }

    private static float exponential(Vector3 p, float t)
    {
        return p.x * p.x;
    }

    private static float parabola(Vector3 p, float t)
    {
        p.x += p.x - 1f;
        p.z += p.z - 1f;
        return 1f - p.x * p.x * p.z * p.z;
    }

    private static float sine(Vector3 p, float t)
    {
        return 0.5f +
                  0.25f * Mathf.Sin(4f * Mathf.PI * p.x + 4f * t) * Mathf.Sin(2f * Mathf.PI * p.z + t) +
                  0.10f * Mathf.Cos(3f * Mathf.PI * p.x + 5f * t) * Mathf.Cos(5f * Mathf.PI * p.z + 3f * t) +
                  0.15f * Mathf.Sin(Mathf.PI * p.x + .06f * t);
    }

    private static float ripple(Vector3 p, float t)
    {
        p.x -= .5f;
        p.z -= .5f;
        float squareRadius = p.x * p.x + p.z * p.z;
        return .5f + Mathf.Sin(15f * Mathf.PI * squareRadius - 2f * t) / (2f + 100f * squareRadius);
    }

    private void createPoints()
    {
        currentResolution = Resolution;
        points = new ParticleSystem.Particle[Resolution * Resolution];

        float increment = 1.0f / (Resolution - 1);
        int i = 0;
        for (int x = 0; x < Resolution; x++)
        {
            for (int z = 0; z < Resolution; z++)
            {
                Vector3 p = new Vector3(x * increment, 0f, z * increment);
                points[i].position = p;
                points[i].color = new Color(p.x, 0f, p.z);
                points[i++].size = 0.1f;
            }
        }
    }
}
