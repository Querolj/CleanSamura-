using UnityEngine;
using System.Collections;

public class MayoAnimator : MonoBehaviour
{
    private MayoController mc;
    //private Tongue tongue;

    public Sprite[] spritesLeft;
    public Sprite[] spritesRight;
    public Sprite[] spritesStanding;

    public Sprite[] spritesJumpingRight;
    public Sprite[] spritesJumpingLeft;

    public Sprite[] spritesGrabingLeft;
    public Sprite[] spritesGrabingRight;

    public Sprite[] spritesStandingRight;
    public Sprite[] spritesStandingLeft;

    public Sprite[] spriteNoArms;

    //Module Buisson
    public Sprite[] spritesBuisson;
    //Détails
    public float fps;

    private SpriteRenderer spriteRenderer;
    private bool facing = false; // false = right, true = left
    public float moveSpeed;

    private bool jumping;

    void Start()
    {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        mc = GameObject.Find("Mayo").GetComponent<MayoController>();
        //tongue = mc.GetComponent<Tongue>();

    }
    

    void Update()
    {

        int index = (int)(Time.timeSinceLevelLoad * fps);
        if(mc.inside_buisson) //animation dans un buisson
        {
            moveBuissonAnimation(index);
        }
        else if(mc.normal_mode)//animations normales
        {
            grabingAnimation(index);
            moveAnimation(index);
        }   
        //tongueAnimation(index);
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void moveAnimation(int index)
    {
        if (Input.GetKey("right") && mc.isGround)
        {
            facing = false;
            index = index % spritesRight.Length;
            spriteRenderer.sprite = spritesRight[index];
        }
        else if (Input.GetKey("left") && mc.isGround)
        {
            facing = true;
            index = index % spritesLeft.Length;
            spriteRenderer.sprite = spritesLeft[index];
        }
        else if (mc.isGrabbing)
        {
            if (mc.directed)
            {
                index = index % spritesGrabingRight.Length;
                spriteRenderer.sprite = spritesGrabingRight[index];
            }
            else
            {
                index = index % spritesGrabingLeft.Length;
                spriteRenderer.sprite = spritesGrabingLeft[index];
            }
        }
        else if (mc.isJumping)
        {
            if (Input.GetKey("left") || !mc.directed)
            {
                index = index % spritesJumpingLeft.Length;
                spriteRenderer.sprite = spritesJumpingLeft[index];
            }
            else if (Input.GetKey("right") || mc.directed)
            {
                index = index % spritesJumpingRight.Length;
                spriteRenderer.sprite = spritesJumpingRight[index];
            }
        }
        else if (!facing)
        {
            index = index % spritesStandingRight.Length;
            spriteRenderer.sprite = spritesStandingRight[index];
        }
        else
        {
            index = index % spritesStandingLeft.Length;
            spriteRenderer.sprite = spritesStandingLeft[index];
        }

    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void grabingAnimation(int index)
    {
        if (Input.GetKey("left ctrl"))
        {

            if (Input.GetKey("right"))
            {
                facing = false;
                index = index % spritesGrabingRight.Length;
                spriteRenderer.sprite = spritesGrabingRight[index];
            }
            else if (Input.GetKey("left"))
            {
                facing = true;
                index = index % spritesGrabingLeft.Length;
                spriteRenderer.sprite = spritesGrabingLeft[index];
            }
        }
    }
    /*public void tongueAnimation(int index)
    {
        index = 1;
        if(tongue.tongueAnime&&!facing)
        {
            spriteRenderer.sprite = spriteNoArms[0];
        }
        else if(tongue.tongueAnime && facing)
        {
            spriteRenderer.sprite = spriteNoArms[1];
        }
    }*/
    //------------------------------------------------------------------------------------------------------------------------------------

    public void moveBuissonAnimation(int index)
    {
        if ((Input.GetKey("right") || Input.GetKey("left")) && mc.isGround)
        {
            facing = false;
            index = index % spritesBuisson.Length;
            spriteRenderer.sprite = spritesBuisson[index];
        }
        else
            spriteRenderer.sprite = spritesBuisson[0];
    }
}
