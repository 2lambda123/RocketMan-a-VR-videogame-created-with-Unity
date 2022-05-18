using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    private int timeOnFire = 0;
    private bool onFire = false;
    private bool trigger = false;
    private bool done = false;
    private ParticleSystem foamEffect;
    List<ParticleSystem.Particle> enter;
    [SerializeField]
    public GameObject fireExtinguisher;
    public ParticleSystem[] fireEffects;
    public GameObject fireCollider;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        enter = new List<ParticleSystem.Particle>();
        foamEffect = gameObject.GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (fireExtinguisher.GetComponent<OVRGrabbable>().isGrabbed )
        {
            if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f )
            {
                trigger = true;
            }
            else
            {
                trigger = false;
            }
        }
        else
        {
            trigger = false;
        }


        if (trigger)
        {
            foamEffect.Play();
            foamEffect.playbackSpeed= 10;
            if (audio.time > 4f)
            {
                audio.Stop();
            }
            if (!audio.isPlaying)
            {
                audio.time = 2f;
                audio.Play();
            }
            OVRInput.SetControllerVibration(0.3f, 0.3f, fireExtinguisher.GetComponent<OVRGrabbable>().grabbedBy.m_controller);
            if (onFire)
            {
                foreach(ParticleSystem fireEffect in fireEffects)
                {
                    fireEffect.startLifetime = Mathf.Lerp(fireEffect.startLifetime, 0f, Time.deltaTime);
                }            
                
                timeOnFire += 1;
            }

        }
        else
        {
            audio.Stop();
            //OVRInput.SetControllerVibration(0f, 0f, fireExtinguisher.GetComponent<OVRGrabbable>().grabbedBy.m_controller);
            foamEffect.Stop();
        }

        if (timeOnFire >= 120)
        {
            if (!done)
            {
                foreach (ParticleSystem fireEffect in fireEffects)
                {
                    fireEffect.Stop();
                    
                }
                if (fireCollider != null)
                {
                    Destroy(fireCollider);
                }
                GameManager.Instance.UpdateGameState(RiddlesProgress.FireEstinguished);

                onFire = false;
                done = true;
                timeOnFire = 0;
            }
           

        }
    }

    private void OnParticleTrigger()
    {
        if( foamEffect.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter) > 0)
        {
            onFire = true;
            enter = new List<ParticleSystem.Particle>();
        }
        else
        {
            onFire = false;
        }

        
    }
}
