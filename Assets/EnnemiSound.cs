using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSound : MonoBehaviour {
    public AudioClip cut;
    public float pitch_cut = 1f;
    public AudioClip death;
    public float pitch_death;
    public AudioClip hit_air;

    protected AudioSource source;
    protected EnnemiStates states;
    protected bool death_played = false;
    void Start () {
        states = this.GetComponent<EnnemiStates>();
        source = this.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        if (states.TakingDamage)
            PlayCut();

        if (states.IsDead() && !death_played)
            PlayDeath();
	}

    private void PlayCut()
    {
        states.TakingDamage = false;
        source.pitch = pitch_cut;
        source.PlayOneShot(cut);
    }
    private void PlayDeath()
    {
        death_played = true;
    }
    public void PlayHitAir()
    {
        if(hit_air != null)
            source.PlayOneShot(hit_air);
    }

    private void ResetSource()
    {
        source.volume = 1f;
        source.pitch = 1f;
    }
}
