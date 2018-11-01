
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiAnimation : MonoBehaviour {
    protected EnnemiStates states;
    public Sprite[] spritesStanding;
    public Sprite[] spritesWalking;
    public Sprite[] spritesHitting;
    public int sprite_hitting;
    public int sprite_ready = 0;
    public Sprite[] spritesJumping;
    public Sprite[] spritesDying;
    public Sprite[] spriteOnBlood;
    public Sprite spriteHurted;
    public Sprite firstStand;
    protected SpriteRenderer spriteRenderer;
    protected EnnemiAI AI;

    public float fps_walking;
    public float fps_standing;
    public float fps_hitting;
    public float fps_dying;

    public bool directed;

    private int old_index_dying = -1;
    private int old_index_attacking = -1;
    private int count_anime_dead = 0;
    private int count_anime_hit = 0;
    private bool first_stand = false;

    //Action reçues
    public bool attacking = false;
    public bool ready_attack = false;
    protected virtual void Start () {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        states = this.GetComponent<EnnemiStates>();
        AI = this.GetComponent<EnnemiAI>();
        CustomStart();
    }

    // Update is called once per frame
    void Update()
    {
        int index_walking = (int)(Time.timeSinceLevelLoad * fps_walking);
        int index_standing = (int)(Time.timeSinceLevelLoad * fps_standing);
        int index_hitting = (int)(Time.timeSinceLevelLoad * fps_hitting);
        int index_dying = (int)(Time.timeSinceLevelLoad * fps_dying);
        if(states.life != 0)
        {
            FlipRenderer();
            
        }


        ChangeAnimation(); //MOD
        

        if (count_anime_dead >= spritesDying.Length)
        {
            //L'ennemi est mort !
            AfterDeathAnime(); //MOD
        }
        else if (states.life <= 0) //Animation de mort
        {
            index_dying = index_dying % spritesDying.Length;
            if(old_index_dying != index_dying)
            {
                spriteRenderer.sprite = spritesDying[count_anime_dead];
                count_anime_dead++;
            }
            if(count_anime_dead == spritesDying.Length)
            {
                states.Death();
            }

            old_index_dying = index_dying;
        }
        else if (!states.time_end) //Ennemi blessé, rewind l'animation d'attaque
        {
            AI.attacking = false;
            attacking = false;
            ready_attack = true;
            count_anime_hit = 0;
            old_index_attacking = -1;
            spriteRenderer.sprite = spriteHurted;
        }
        else if(ReadySpecialAttack())
        {
            //Empêche les autrse animes de tourner;
        }
        else if(ready_attack)
        {
            index_hitting = index_hitting % spritesHitting.Length;
            if (old_index_attacking != index_hitting && count_anime_hit <= sprite_ready)
            {
                spriteRenderer.sprite = spritesHitting[count_anime_hit];
                count_anime_hit++;
            }
            
        }
        else if(attacking) // Attack!
        { 
            if(spritesHitting.Length == 0)
            {
                print("aucun sprite d'attaque");
            }
            else if(!AI.is_ranged)
            {
                
                index_hitting = index_hitting % spritesHitting.Length;
                if (old_index_attacking != index_hitting && count_anime_hit < spritesHitting.Length)
                {
                    spriteRenderer.sprite = spritesHitting[count_anime_hit];
                    
                    if (count_anime_hit == sprite_hitting)
                    {
                        AI.Cac();
                    }
                    count_anime_hit++;
                }
                if (count_anime_hit == spritesHitting.Length)
                {
                    count_anime_hit = 0;
                    states.SetBlockMove(false);
                    attacking = false;
                    AI.attacking = false;
                    
                    AfterAttack();
                }
                
                old_index_attacking = index_hitting;
            }
            else //throw projectile (Identique par défaut)
            {
                index_hitting = index_hitting % spritesHitting.Length;
                if (old_index_attacking != index_hitting)
                {
                    spriteRenderer.sprite = spritesHitting[count_anime_hit];
                    count_anime_hit++;
                }
                if (count_anime_hit == spritesHitting.Length)
                {
                    count_anime_hit = 1;
                    states.SetBlockMove(false);
                    attacking = false;
                    AI.attacking = false;

                    AfterAttack();
                }
                if (count_anime_hit == sprite_hitting)
                {
                    AI.Ranged();
                }
                old_index_attacking = index_hitting;

            }
            
        }
        else if (AI.on_blood && !states.is_daemon)
        {
            index_walking = index_walking % spriteOnBlood.Length;
            spriteRenderer.sprite = spriteOnBlood[index_walking];
        }
        else if (states.GetMod() == 0) //Standing
        {
            if(firstStand != null && !first_stand)
            {
                spriteRenderer.sprite = firstStand;
            }
            else
            {
                index_standing = index_standing % spritesStanding.Length;
                spriteRenderer.sprite = spritesStanding[index_standing];
            }
            
        }
        else
        {
            first_stand = true;
            index_walking = index_walking % spritesWalking.Length;
            spriteRenderer.sprite = spritesWalking[index_walking];
        }

        RewindOldAnimation();
    }

    //----------------------------------------------------------SPECIAL ANIMATION----------------------------------------------------------------------------
    protected virtual void CustomStart()
    {

    }

    protected virtual bool ReadySpecialAttack()
    {
        return false;
    }
    
    protected virtual void FlipRenderer()
    {
        if (states.GetDirection())
            spriteRenderer.flipX = directed;
        else
            spriteRenderer.flipX = !directed;
    }

    protected virtual void AfterDeathAnime()
    {

    }

    protected virtual void ChangeAnimation()//Pour changer des sprites d'animation
    {

    }

    protected virtual void RewindOldAnimation()//Pour remettre des anciens sprites d'animation
    {

    }

    protected virtual void AfterAttack()//Après l'animation d'attaque
    {

    }
}
