using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGMParasiteAnimation : EnnemiAnimation {
    public Sprite[] spritePrepaCharge;
    public Sprite[] spriteLaunchCharge;
    public Sprite[] spritePrepaGerbe;
    public Sprite[] spriteStartingGerbe;
    public Sprite[] spriteGerbing;

    public float fps_prepa_charge;
    public float fps_prepa_gerbe;
    public float cd_prepa_gerbe;
    public float cd_gerbing;
    public float fps_gerbing;
    private BossGMParasiteBehaviour behaviour;
    private Sprite[] initSpriteWalking;
    private bool start_charge = false;
    private bool start_gerbe = false;
    private int old_index_prepa_charge = -1;
    private int old_index_gerbe = -1;
    private int count_anime_prepa_charge = 0;
    private int count_anime_gerbe = 0;
    private float timer_prepa_gerbe;
    private float timer_gerbing;
    private bool damage_gerbe = false;
    //Splash
    private ParticleSystem blood_splash;
    private Transform blood_splash_t;
    private bool splashed = false;
    protected override void CustomStart()
    {
        initSpriteWalking = spritesWalking;
        behaviour = this.GetComponent<BossGMParasiteBehaviour>();
        timer_prepa_gerbe = cd_prepa_gerbe;
        timer_gerbing = cd_gerbing;
        GameObject blood_gerbe = GameObject.Find(this.name + "GerbeCollider/Blood");

        blood_splash = blood_gerbe.GetComponent<ParticleSystem>();//this.GetComponentInChildren<ParticleSystem>();
        blood_splash_t = blood_gerbe.GetComponent<Transform>();
    }
    protected override void ChangeAnimation()
    {

        if (behaviour.charging)
        {
            spritesWalking = spriteLaunchCharge;
            
            
        }
        else
        {
            spritesWalking = initSpriteWalking;
        }
    }
    protected override bool ReadySpecialAttack()
    {
        if(behaviour.charging)
        {
            if (!start_charge)//Préparation de la charge
            {
                states.SetBlockMove(true);
                int index_prepa_charge = (int)(Time.timeSinceLevelLoad * fps_prepa_charge);

                index_prepa_charge = index_prepa_charge % spritePrepaCharge.Length;
                if (old_index_prepa_charge != index_prepa_charge)
                {
                    spriteRenderer.sprite = spritePrepaCharge[count_anime_prepa_charge];
                    count_anime_prepa_charge++;
                }
                if (count_anime_prepa_charge == spritePrepaCharge.Length)
                {
                    count_anime_prepa_charge = 0;
                    start_charge = true;
                }
                old_index_prepa_charge = index_prepa_charge;
                return true;
            }
            else//Lancement de la charge
            {
                spritesWalking = spriteLaunchCharge;
                states.SetBlockMove(false);
            }
            
        }
        else if(behaviour.gerbing)
        {
            if(!start_gerbe && timer_prepa_gerbe > 0)
            {
                timer_prepa_gerbe -= Time.deltaTime;
                states.SetBlockMove(true);
                int index_prepa_gerbe = (int)(Time.timeSinceLevelLoad * fps_prepa_gerbe);
                index_prepa_gerbe = index_prepa_gerbe % spritePrepaGerbe.Length;
                spriteRenderer.sprite = spritePrepaGerbe[index_prepa_gerbe];
            }
            else
            {
                
                start_gerbe = true;
                timer_prepa_gerbe = cd_prepa_gerbe;
                int index_start_gerbing = (int)(Time.timeSinceLevelLoad * fps_gerbing);
                index_start_gerbing = index_start_gerbing % spriteStartingGerbe.Length;   
                if(old_index_gerbe != index_start_gerbing && count_anime_gerbe != spriteStartingGerbe.Length)
                {
                    spriteRenderer.sprite = spriteStartingGerbe[count_anime_gerbe];
                    count_anime_gerbe++;
                }
                if(count_anime_gerbe == spriteStartingGerbe.Length)
                {
                    if(!splashed)
                    {
                        splashed = true;
                        blood_splash.Play();
                    }
                    if (timer_gerbing > 0)
                    {
                        damage_gerbe = true;
                        timer_gerbing -= Time.deltaTime;
                        int index_gerbing = (int)(Time.timeSinceLevelLoad * fps_gerbing);
                        index_gerbing = index_gerbing % spriteGerbing.Length;
                        spriteRenderer.sprite = spriteGerbing[index_gerbing];
                    }
                    else
                    {
                        StopGerbe();
                    }
                    
                }
                old_index_gerbe = index_start_gerbing;

            }
            return true;
        }

        return false;
    }
    public void StopChargeAnime()
    {
        start_charge = false;
    }
    public void StopGerbe()
    {
        splashed = false;
        count_anime_gerbe = 0;
        states.SetBlockMove(false);
        start_gerbe = false;
        timer_gerbing = cd_gerbing;
        damage_gerbe = false;
        behaviour.StopGerbe();
    }
    public bool IsGerbing()
    {
        return start_gerbe;
    }
    public bool CanDealDamageGerbe()
    {
        return damage_gerbe;
    }

}
