using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOnFloor : MonoBehaviour {
    public Splatter splatter;
    public float max_blood;
    public bool is_death = true;

    private float blood_count;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;
    private ParticleSystem particles_sys;
    private ParticleSystem.EmissionModule emission;

    void Start () {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        particles_sys = this.GetComponent<ParticleSystem>();
        emission = particles_sys.emission;
        /*if (is_death)
        {
            emission.enabled = false;
        }*/
        
    }
	
	void Update () {
		
	}
    
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;
        if(!is_death)
        {
            if (part.gameObject != null && other.CompareTag("Surface") && blood_count < max_blood)
            {
                
                while (i < numCollisionEvents)
                {
                    Vector3 pos = collisionEvents[i].intersection;
                    pos.z = 0.1f;
                    GameObject parent_anti_scale = new GameObject();
                    parent_anti_scale.name = "anti_scaling";
                    parent_anti_scale.transform.parent = other.gameObject.transform;

                    Splatter splatterObj = (Splatter)Instantiate(splatter, pos, Quaternion.identity, parent_anti_scale.transform);//this.transform.position
                    i++;
                    blood_count++;
                }
                blood_count = 0;
            }
        }
        else
        {
            if (part.gameObject != null && other.CompareTag("Surface") && blood_count < max_blood)
            {
                emission.enabled = true;
                while (i < numCollisionEvents && blood_count < max_blood)
                {
                    Vector3 pos = collisionEvents[i].intersection;
                    pos.z = 0.1f;
                    GameObject parent_anti_scale = new GameObject();
                    parent_anti_scale.name = "anti_scaling";
                    parent_anti_scale.transform.parent = other.gameObject.transform;

                    Splatter splatterObj = (Splatter)Instantiate(splatter, pos, Quaternion.identity, parent_anti_scale.transform);//this.transform.position
                    i++;
                    blood_count++;
                }
            }
            return;
        }
        
    }

}
