using UnityEngine;
using System.Collections;

public class ParticlePool 
{
    Particle particle;

    public ParticlePool()
    {
        particle = new Particle();
    }

    public Particle HeldParticle()
    {
        return particle;
    }
}
