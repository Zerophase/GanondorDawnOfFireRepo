using UnityEngine;
using System.Collections;

public class Grapher3 : MonoBehaviour 
{
    [Range(10, 30)]
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
    public bool absolute;
    public float threshold = .5f;

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

        if (absolute)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Color c = points[i].color;
                c.a = f(points[i].position, t) >= threshold ? 1f : 0f;
                points[i].color = c;
            }
        }
        else
        {
            for (int i = 0; i < points.Length; i++)
            {
                Color c = points[i].color;
                c.a = f(points[i].position, t);
                points[i].color = c;
            }
        }

        particleSystem.SetParticles(points, points.Length);
    }

    private static float linear(Vector3 p, float t)
    {
        return 1f - p.x - p.y - p.z + .5f * Mathf.Sin(t); 
    }

    private static float exponential(Vector3 p, float t)
    {
        return 1f - p.x * p.x - p.y * p.y - p.z * p.z + .5f * Mathf.Sin(t);
    }

    private static float parabola(Vector3 p, float t)
    {
        p.x += p.x - 1f;
        p.z += p.z - 1f;
        return 1f - p.x * p.x -  p.z * p.z + .5f * Mathf.Sin(t);
    }

    private static float sine(Vector3 p, float t)
    {
        float x = Mathf.Sin(2 * Mathf.PI * p.x);
        float y = Mathf.Sin(2 * Mathf.PI * p.y);
        float z = Mathf.Sin(2 * Mathf.PI * p.y);
        return x * x * y * y * z * z;
    }

    private static float ripple(Vector3 p, float t)
    {
        p.x -= .5f;
        p.y -= .5f;
        p.z -= .5f;
        float squareRadius = p.x * p.x + p.y * p.y + p.z * p.z;
        return Mathf.Sin(4f * Mathf.PI * squareRadius - 2f * t);
    }

    private void createPoints()
    {
        currentResolution = Resolution;
        points = new ParticleSystem.Particle[Resolution * Resolution * Resolution];

        float increment = 1.0f / (Resolution - 1);
        int i = 0;
        for (int x = 0; x < Resolution; x++)
        {
            for (int z = 0; z < Resolution; z++)
            {
                for (int y = 0; y < Resolution; y++)
                {
                    Vector3 p = new Vector3(x, y, z) * increment;
                    points[i].position = p;
                    points[i].color = new Color(p.x, p.y, p.z);
                    points[i++].size = 0.1f;
                }
            }
        }
    }
}
