using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiSound : MonoBehaviour {
    public AudioClip steps;
    public float pitch_steps;
    public AudioClip slash_air;
    public float pitch_slash_air = 1.1f;
    public float volume_slash_air = 0.496f;
    public AudioClip jump;
    public float volume_jump = 1f;
    public AudioClip dash;
    public float volume_dash = 1f;
    public AudioClip hurt;
    public float volume_hurt = 0.7f;

    private SamuraiController sc;
    private SamuraiAnimator sa;
    private AudioSource source;

    private bool slash_air_played = false;
    void Start () {
        sc = this.GetComponent<SamuraiController>();
        sa = this.GetComponent<SamuraiAnimator>();
        source = this.GetComponent<AudioSource>();

    }
	
	void Update () {
        if (sa.IsHitting() && !slash_air_played)
        {
            PlaySlashAir();
        }
        else if (!sa.IsHitting() && slash_air_played)
            slash_air_played = false;

    }

    private void PlaySlashAir()
    {
        source.volume = volume_slash_air;
        source.pitch = pitch_slash_air;
        source.PlayOneShot(slash_air);
        slash_air_played = true;
        ResetSource();
    }

    public void PlayJump()
    {
        source.volume = volume_jump;
        source.PlayOneShot(jump);
        ResetSource();
    }
    public void PlayHurt()
    {
        source.volume = volume_hurt;
        source.PlayOneShot(hurt);
        //ResetSource();
    }
    public void PlayDash()
    {
        source.volume = volume_dash;
        source.PlayOneShot(dash);
        ResetSource();
    }

    private void ResetSource()
    {
        source.volume = 1f;
        source.pitch = 1f;
    }

}
