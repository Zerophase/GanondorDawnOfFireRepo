using UnityEngine;
using System.Collections;

public class Grapher1 : MonoBehaviour 
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
        SINE
    }

    public FunctionOptions Function;

    private delegate float functionDelegate(float x);
    private static functionDelegate[] functionDelegates = 
    {
        linear,
        exponential,
        parabola,
        sine
    };

	void Update () 
    {
        if (currentResolution != Resolution || points == null)
            createPoints();

        functionDelegate f = functionDelegates[(int)Function];
        for (int i = 0; i < Resolution; i++)
        {
            Vector3 p = points[i].position;
            p.y = f(p.x);
            points[i].position = p;

            Color c = points[i].color;
            c.g = p.y;
            points[i].color = c;
        }

        particleSystem.SetParticles(points, points.Length);
	}

    private static float linear(float x)
    {
        return x;
    }

    private static float exponential (float x)
    {
        return x * x;
    }

    private static float parabola(float x)
    {
        x = 2f * x - 1f;
        return x * x;
    }

    private static float sine (float x)
    {
        return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
    }

    private void createPoints()
    {
        currentResolution = Resolution;
        points = new ParticleSystem.Particle[Resolution];

        float increment = 1.0f / (Resolution - 1);
        for (int i = 0; i < Resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].color = new Color(x, 0f, 0f);
            points[i].size = 0.1f;
        }
    }
}
