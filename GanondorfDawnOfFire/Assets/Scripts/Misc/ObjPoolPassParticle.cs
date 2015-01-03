using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjPoolPassParticle : MonoBehaviour
{
    private List<ParticleSystem> particleSystems;
    public ParticleSystem particleSystem;

    private static ObjPoolPassParticle instance;
    public static ObjPoolPassParticle Instance
    {
        get { return instance ?? (instance = new ObjPoolPassParticle()); }
    }

    public void Add(ParticleSystem particleSystem, int initialBufferSize)
    {
        if (particleSystems == null)
        {
            particleSystems = new List<ParticleSystem>(initialBufferSize);
            for (int i = 0; i < initialBufferSize; i++)
            {
                particleSystems.Add(particleSystem);
                //particleSystems[i].active = false;
            }
        }
    }
    public ParticleSystem Retrieve(GameObject obj)
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            if (particleSystems.Last<ParticleSystem>().isPlaying)
            {
                particleSystems.Add(new ParticleSystem());
               
                FlipGameObjectComponents(obj);
                return TurnOnParticleSystem(obj, particleSystems.Last<ParticleSystem>()); ;
            }

            if (!particleSystems[i].isPlaying)
            {
                FlipGameObjectComponents(obj);
                return TurnOnParticleSystem(obj, particleSystems[i]);
            }
        }

        return null;
    }

    private ParticleSystem TurnOnParticleSystem(GameObject obj, ParticleSystem particleSystem)
    {
        particleSystem.transform.position = obj.transform.position;
        particleSystem.transform.rotation = Quaternion.LookRotation(ThirdPerson_Controller.Instance.transform.forward * -1);
        particleSystem.startColor = Color.black;
        particleSystem.Play();
        return particleSystem;
    }

    public void Store(GameObject obj)
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            if (particleSystems[i].isPlaying)
            {
                particleSystems[i].Stop();
                //FlipGameObjectComponents(obj);
                return;
            }
        }
    }

    private static void FlipGameObjectComponents(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().enabled = !obj.GetComponent<MeshRenderer>();
        obj.GetComponent<BoxCollider>().enabled = !obj.GetComponent<BoxCollider>();
    }
}
