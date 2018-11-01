using UnityEngine;

/*
 * TODO : 
 * -infinite jump OK
 * -takeobject
 */
public class MayoController : MonoBehaviour
{
    public float max_speed = 4;
    private float current_speed = 0;
    public float acceleration = 0.1f;
    public float jumpVelocity = 10;
    public LayerMask playerMask;

    [HideInInspector]
    public Transform myTrans { get; set; }
    [HideInInspector]
    public Rigidbody2D myBody { get; set; }
    [HideInInspector]
    public CapsuleCollider2D myCollider { get; set; }

    /*
     * Touche vérification
     */
    [HideInInspector]
    public bool canJump = false;


    public bool isJumping { get; set; }
    public bool isGround { get; set; }
    public bool take { get; set; }
    public bool launch { get; set; }
    public bool down { get; set; }
    public bool take_in_bag { get; set; }
    public bool isGrabbing { get; set; }
    public bool dialogue_mode { get; set; }
    /*
     * Walls var & grab & no stuck in wall
     * 
     */


    private Transform tagGroundL, tagGroundR, tagGrabRight, tagGrabLeft;
    [HideInInspector]
    public float direction { get; set; } //Négatif = Mayo dirigé vers la gauche, sinon vers la droite
    public bool directed = false;
    public float oldDirection;
    public bool directionHasChanged;


    /*
     * 
     * Descending Slopes
     * */
    public float maxDescendAngle = 75;
    private float angle;
    const float skinWidth = .015f;

    /*
     * Anti Sliding
     * */
    public bool moving = false;
    /*
     * Objets de Mayo
     */



    private Transform objectTransS; //Le petit transform que Mayo a choppé


    /*
     * Modules
     */
    //Checkpoints
    [HideInInspector]
    public Vector3 respawn_point;
    //Wall gestion
    private WallCollision WallCollision;
    [HideInInspector]
    public GameObject wall { get; set; }
    [HideInInspector]
    public SpriteRenderer wall_renderer { get; set; }
    [HideInInspector]
    public Transform wall_transform { get; set; }
    private Transform wall_detector_transform;
    public bool direction_wall;
    public bool grabable_wall;
    //Prendres des objets
    private ObjectCarrying ObjectCarrying;
    [HideInInspector]
    public GameObject object_locked { get; set; }
    [HideInInspector]
    public Transform object_locked_trans;
    [HideInInspector]
    public Rigidbody2D object_locked_rb;

    [HideInInspector]
    private Transform ray_obj_start;
    public float ray_dist_action_y;
    public float ray_dist_action_x;
    //Mode Normal
    public bool normal_mode { get; set; }
    //Mode Buisson
    public bool inside_buisson { get; set; }
    private Transform trans_buisson;
    //Mode Crawling
    public bool crawling_mode { get; set; }
    //Mode Throwing
    public bool throwing_mode { get; set; }
    private InventoryObject item_to_throw;
    private GameObject item_to_throw_go;
    private Rigidbody2D item_to_throw_rb;
    private Transform sight;
    public float sight_velocity = 1f;
    private LineRenderer line_sight;
    private Transform point_to_throw;
    private bool throw_NOW = false;
    private bool charging = false;
    [Range(1, 300)]
    public int charging_speed = 1;
    [Range(5, 800)]
    public float max_throw_velocity = 200;
    private float throw_velocity = 0;
    //Inventaire
    public Inventory bag;
    //QAI
    private QAIGestion QAI;
    //Lock
    private Transform lock_trans;
    [HideInInspector]
    public bool locking = false;

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */





    //------------------------------------------------------------------------------------------------------------------------------------
    void InitObject()
    {
        WallCollision = new WallCollision(this);
        wall = null;
        ObjectCarrying = new ObjectCarrying(this);
        object_locked = null;

        normal_mode = true;
        crawling_mode = false;
        isGround = false;
        isJumping = false;
        take = false;
        launch = false;
        down = false;
        isGrabbing = false;
        inside_buisson = false;
        direction_wall = false;
        dialogue_mode = false;

        GameObject mayo = GameObject.Find("Mayo");
        myBody = this.GetComponent<Rigidbody2D>();
        myTrans = this.transform;
        myCollider = this.GetComponent<CapsuleCollider2D>();
        tagGroundL = GameObject.Find(this.name + "/tagGroundL").transform;
        tagGroundR = GameObject.Find(this.name + "/tagGroundR").transform;
        playerMask = GameObject.Find(this.name).layer;
        ray_obj_start = GameObject.Find(this.name + "/ObjectRay").transform;
        wall_detector_transform = GameObject.Find(this.name + "/WallDetector").transform;
        bag = this.GetComponent<Inventory>();
        lock_trans = GameObject.Find("Spécials/Lock").GetComponent<Transform>();
        sight = GameObject.Find(this.name + "/Sight").transform;
        point_to_throw = GameObject.Find(this.name + "/PointToThrow").transform;
        respawn_point = new Vector3(-3.93f, 1.9f, 0);
        QAI = GameObject.Find("Quick Action Item").GetComponent<QAIGestion>();
        //Line renderer pour la visée
        line_sight = mayo.GetComponent<LineRenderer>();
        line_sight.startColor = line_sight.startColor = new Color(1, 0, 0, 255);
        line_sight.endColor = new Color(0, 0, 0, 0);
        line_sight.enabled = false;
    }
    void GeneralLayerSetup()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Mob"), LayerMask.NameToLayer("Object"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Mob"), LayerMask.NameToLayer("Mob"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PNJ"), LayerMask.NameToLayer("Player"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Mob"), LayerMask.NameToLayer("Buisson"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Buisson"), LayerMask.NameToLayer("Object"), true);
    }
    void Start()
    {
        InitObject();
        GeneralLayerSetup();    
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        
        RaycastHit2D[] hits = new RaycastHit2D[3];
        int h = Physics2D.RaycastNonAlloc(myTrans.position, -Vector2.up, hits);
        if (h > 1)
        { //if we hit something do stuff

            angle = Mathf.Abs(Mathf.Atan2(hits[2].normal.x, hits[2].normal.y) * Mathf.Rad2Deg); //get angle

            if (angle > 1)
            {
                //Debug.Log("Angle :" + angle);

            }
        }
        CheckKeyPressedUpdate();
        ObjectRaycasting();
    }

    public void ObjectRaycasting()
    {
        //Vector2 v2 = new Vector2(0, -ray_distance_action);
        RaycastHit2D obj_ray_info = Physics2D.Raycast(ray_obj_start.transform.position, Vector2.down, ray_dist_action_y,
            1 << LayerMask.NameToLayer("Object")| 1 << LayerMask.NameToLayer("PNJ"));
        Debug.DrawRay(ray_obj_start.transform.position, Vector2.down, Color.red);
        if (obj_ray_info.transform != null)
        {
            if (obj_ray_info.transform.gameObject.tag == "PortableObjectS")
            {
                Lock(obj_ray_info.transform.gameObject);
                SetObjectLocked(obj_ray_info.transform.gameObject, obj_ray_info.transform, obj_ray_info.rigidbody);
                
            }
            else if (obj_ray_info.transform.gameObject.tag == "InventoryObject")
            {
                
                if(take_in_bag)
                {
                    bool itemAdded = bag.addItem(obj_ray_info.transform.gameObject);
                    if (itemAdded)
                        Debug.Log("Item ajouté : " + obj_ray_info.transform.gameObject.name);
                    else
                        Debug.Log("Impossible de prendre l'objet");
                }
                Lock(obj_ray_info.transform.gameObject);

            }
            else if(obj_ray_info.transform.gameObject.tag == "PNJ" && !dialogue_mode)
            {
                if (take_in_bag)
                    StartDialogue(obj_ray_info.transform.gameObject);
            }
            /*else if(obj_ray_info.transform.gameObject.layer == LayerMask.NameToLayer("Buisson") && down)
            {
                
            }*/
            else
            {
                SetObjectLocked(null, null, null);
                if(locking)
                    Delock();
            }
        }
        else
        {
            SetObjectLocked(null, null, null);
            if (locking)
                Delock();
        }
        //take_in_bag est mis à false ici parce qu'il dépend du raycasting
        take_in_bag = false;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void CheckKeyPressedUpdate() //Vérifie les boutons pressés pour ne pas en louper
    {
        // Read the jump input in Update so button presses aren't missed.
        if (!canJump)
        {
            canJump = Input.GetButtonDown("Jump");
        }

        if (Input.GetKey("left") || Input.GetKey("right"))
            moving = true;
        else
            moving = false;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            take = true;
        if (Input.GetKeyUp(KeyCode.A))
            launch = true;
        if (Input.GetKey("down"))
            down = true;
        if (Input.GetKeyDown(KeyCode.E) && !throwing_mode)
            take_in_bag = true;
        else if (throwing_mode)
            if(Input.GetKey(KeyCode.E))
                throw_NOW = true;
            else
                throw_NOW = false;
        if (normal_mode)
            //Gère le QAI, y'a des touches à presser donc on vérifie ici
            QAI.MayoUpdate();
    }
    private void KeyPressedToFalse()
    {
        take = false;
        launch = false;
        down = false;
        canJump = false;
        
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    void FixedUpdate()
    {

        if (Physics2D.Linecast(tagGroundR.position, tagGroundL.position))
        {
            isGround = true;
            isJumping = false;
        }
        else
        {
            isGround = false;
        }
        direction = Input.GetAxisRaw("Horizontal");
        Move(direction);

        oldDirection = direction;
        directionHasChanged = false;
        DetectorAndCoDirection();
        //Modules update

        //FixedUpdateThrowing();
        //Ici on switch entre les différents mode
        if (inside_buisson)
        {
            FixedUpdateBuisson();
        }
        else if(throwing_mode)
        {
            FixedUpdateThrowing();
        }
        else if(crawling_mode)
        {
            FixedUpdateCrawling();
        }
        else if(normal_mode)
        {
            FixedUpdateNormal(); 
        }

    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void FixedUpdateNormal()
    {
        if (canJump && isGround)
        {
            if (myBody.IsSleeping())
                myBody.WakeUp();
            Jump();
            canJump = false;
        }

        if (wall != null)
        {
            WallCollision.update(canJump);
        }
        else
        {
            AntiSlide();
        }
        ObjectCarrying.update(take, directed, launch);

        if (down && trans_buisson != null)
        {
            //Changer de perspective avec le buisson (on a les données)
            //cad, changer le sprite (avec l'animation) de Mayo, changer la vitesse et le collider
            ToBuisson();
        }
        
        KeyPressedToFalse();
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void FixedUpdateCrawling()
    {
        AntiSlide();
        if (canJump && isGround)
        {
            if (myBody.IsSleeping())
                myBody.WakeUp();

            //Retour à Mayo
            ToMayo();
            Jump();
            canJump = false;
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void FixedUpdateBuisson()
    {
        AntiSlide();
        if (canJump && isGround)
        {
            if (myBody.IsSleeping())
                myBody.WakeUp();

            //Retour à Mayo
            ToMayo();
            Jump();
            canJump = false;
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void FixedUpdateThrowing()
    {
        Vector3 z_axis = new Vector3(0, 0, 1);
        if(directed)
        {
            if (Input.GetKey(KeyCode.Z) && sight.localPosition.y < 1f) //Rotation vers le haut 
            {
                sight.RotateAround(point_to_throw.position, z_axis, sight_velocity);
            }
            else if (Input.GetKey(KeyCode.S) && sight.localPosition.y > -1f) //Rotation vers le bas
            {
                sight.RotateAround(point_to_throw.position, z_axis, -sight_velocity);
            }
            else
            {
                Debug.Log("Viseur bloqué"); //Avertir le joueur ou mettre une indication ?
            }
        }else
        {
            if (Input.GetKey(KeyCode.Z) && sight.localPosition.y < 1f) //Rotation vers le haut 
            {
                sight.RotateAround(point_to_throw.position, z_axis, -sight_velocity);
            }
            else if (Input.GetKey(KeyCode.S) && sight.localPosition.y > -1f) //Rotation vers le bas
            {
                sight.RotateAround(point_to_throw.position, z_axis, sight_velocity);
            }
            else
            {
                Debug.Log("Viseur bloqué"); //Avertir le joueur ou mettre une indication ?
            }
        }
        
        line_sight.SetPosition(0, point_to_throw.position);
        line_sight.SetPosition(1, sight.position);

        //item_to_throw
        if(item_to_throw != null && throw_NOW) //On lance l'objet et on revient en mode normal     
            charging = true;
        if(charging && throw_NOW ) //On charge...
        {
            if (throw_velocity < max_throw_velocity)
            {
                float re = (throw_velocity / max_throw_velocity);
                throw_velocity += charging_speed;
                Debug.Log("re : " + re);
                
                line_sight.endColor = line_sight.startColor = new Color(1, 0, 0, re);
            }
            else
            {
                if(line_sight.endColor.g != 1)
                    line_sight.endColor = line_sight.startColor = new Color(1, 1, 0, 1);
                else
                    line_sight.endColor = line_sight.startColor = new Color(1, 0, 0, 1);
                Debug.Log("chargement au max");
            }
                

        }
        else if(charging && !throw_NOW) //On lache la sauce (chargement fini)
        {
            item_to_throw.obj_rb.constraints = RigidbodyConstraints2D.None;
            if(throw_velocity < max_throw_velocity)
                item_to_throw.obj_rb.AddForce(sight.localPosition * throw_velocity);
            else
                item_to_throw.obj_rb.AddForce(sight.localPosition * max_throw_velocity);

            bag.removeItem(item_to_throw.place);
            QAI.removeUsedItem();
            throwing_mode = false;
            normal_mode = true;
            charging = false;
            throw_velocity = 0;
            line_sight.enabled = false;
        }

    }

    //------------------------------------------------------------------------------------------------------------------------------------
    public void Move(float hInput)
    {
        if (!myBody.IsAwake())
            myBody.WakeUp();
        if (hInput == 0)
        {
            current_speed = 0;
            return;
        }
        Vector2 moveVel = myBody.velocity;
        if (current_speed + acceleration < Mathf.Abs(hInput * max_speed))
            current_speed += acceleration;
        else
            current_speed = max_speed;
        if (hInput > 0)
        {
            directed = true;
            moveVel.x = current_speed;
        }
        else if (hInput < 0)
        {
            directed = false;
            moveVel.x = -current_speed;
        }
        myBody.velocity = moveVel;
        isGrabbing = false;
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    public void Jump()
    {

        myBody.velocity = new Vector2(0f, 0f);
        myBody.AddForce(new Vector2(0f, jumpVelocity));
        isJumping = true;
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    public void SetPlayerCollider(bool b)
    {
        myCollider.enabled = b;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void DescendSlope(Vector2 velocity)
    {
        if (isGround)
        {
            velocity.y -= (angle / 10);
        }
        myBody.velocity = velocity;
    }
    public void AntiSlide()
    {
        WallCollision.has_jumped = false;
        if (!moving && isGround)
        {
            myBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            myBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }
    public void AntiBump()
    {

    }
    //------------------------------------------------------------------------------------------------------------------------------------

    public void DetectorAndCoDirection()//optimisable
    {
        Vector2 newDirectionWallDetector = wall_detector_transform.localPosition;
        Vector2 new_ray_start = ray_obj_start.localPosition;
        if ((newDirectionWallDetector.x < -0.27f) | (newDirectionWallDetector.x > -0.27f))
        {
            if (directed) //Mayo se tourne vers la droite
            {
                newDirectionWallDetector.x = -0.2059f;
                new_ray_start.x = ray_dist_action_x;
            }
            else //Mayo se tourne vers la gauche
            {
                newDirectionWallDetector.x = -0.28f;
                new_ray_start.x = -ray_dist_action_x;
            }

            ray_obj_start.localPosition = new_ray_start;
            wall_detector_transform.localPosition = newDirectionWallDetector;
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    //Setting up Modules

    public void SetWall(GameObject wall, SpriteRenderer wall_renderer, Transform wall_transform, bool direction_wall, bool grabable_wall)
    {
        this.wall = wall;
        this.wall_renderer = wall_renderer;
        this.wall_transform = wall_transform;
        this.direction_wall = direction_wall;
        this.grabable_wall = grabable_wall;
    }
    public void SetObjectLocked(GameObject object_locked, Transform object_locked_trans, Rigidbody2D object_locked_rb)
    {
        if (object_locked_rb != null)
        {
            this.object_locked = object_locked;
            this.object_locked_trans = object_locked_trans;
            this.object_locked_rb = object_locked_rb;
        }
        else
        {
            this.object_locked = null;
        }
    }
    public void SetBuisson(Transform tb, Rigidbody2D rb, bool nullornot)
    {
        if (!inside_buisson)
        {
            if (nullornot)
                trans_buisson = tb;
            else
                trans_buisson = null;
        }
        else
        {
            Debug.Log("impossible déjà dans un buisson");
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void ToBuisson()//changer le sprite(avec l'animation) de Mayo, changer la vitesse et le collider
    {
        myTrans.gameObject.layer = LayerMask.NameToLayer("Buisson");
        normal_mode = false;
        inside_buisson = true;
        max_speed = 2.5f;
        Vector3 vscale = myTrans.localScale;
        Vector2 cscale = myCollider.size;
        vscale.y = 0.6f;
        vscale.x = 0.6f;
        vscale.z = 0.03f;
        cscale.y = 0.9921848f;
        cscale.x = 0.8920884f;
        myTrans.localScale = vscale;
        myCollider.size = cscale;
    }

    private void ToCrawling()
    {
        normal_mode = false;
        max_speed = 2.5f;
        Vector3 vscale = myTrans.localScale;
        Vector2 cscale = myCollider.size;
        vscale.y = 0.6f;
        vscale.x = 0.6f;
        vscale.z = 0.03f;
        cscale.y = 0.9921848f;
        cscale.x = 0.8920884f;
        myTrans.localScale = vscale;
        myCollider.size = cscale;
    }

    private void ToMayo()
    {
        myTrans.gameObject.layer = LayerMask.NameToLayer("Player");
        inside_buisson = false;
        max_speed = 4f;
        Vector3 vscale = myTrans.localScale;
        Vector2 cscale = myCollider.size;
        vscale.y = 0.7f;
        vscale.x = 0.7f;
        vscale.z = 0f;
        cscale.y = 0.9921848f;
        cscale.x = 0.3713819f;
        myTrans.localScale = vscale;
        myCollider.size = cscale;

        SetBuisson(null, null, false);
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void SetCheckpoint(Vector3 checkpoint)
    {
        respawn_point = checkpoint;
    }

    public void Respawn()
    {
        myTrans.position = respawn_point;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void Lock(GameObject obj)
    {
        Vector3 v = obj.transform.position;
        //v.z = -3;
        if (!lock_trans.gameObject.activeInHierarchy)
            lock_trans.gameObject.SetActive(true);
        lock_trans.position = v;
        if(obj.layer == LayerMask.NameToLayer("Object"))
            locking = true;
    }
    public void Delock()
    {
       /* Vector3 v = lock_trans.position;
        //v.z = 100;
        lock_trans.position = v;*/
        if (lock_trans.gameObject.activeInHierarchy)
            lock_trans.gameObject.SetActive(false);
        locking = false;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void StartDialogue(GameObject pnj)
    {
        dialogue_mode = true;
        normal_mode = false;
        pnj.GetComponent<Character>().startDialogue();
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void StopDialogue()
    {
        dialogue_mode = false;
        normal_mode = true;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void LaunchThrowingMode(InventoryObject item, GameObject item_to_throw_go)
    {
        
        if (item.throwable)
        {
            Vector2 new_sight_pos = sight.localPosition;
            Vector2 new_ptt_pos = point_to_throw.localPosition;
            normal_mode = false;
            throwing_mode = true;
            item_to_throw = item;
            if (directed) //Règle la direction du viseur
            {
                new_sight_pos.x = 1.46f;
                new_ptt_pos.x = 0.199f;
            }
            else
            { 
                new_sight_pos.x = -1.46f;
                new_ptt_pos.x = -0.199f;
            }
            sight.localPosition = new_sight_pos;
            point_to_throw.localPosition = new_ptt_pos;

            item.game_object.SetActive(true);
            Physics2D.IgnoreCollision(item.obj_col, myCollider);
            item.obj_rb.constraints = RigidbodyConstraints2D.FreezeAll;
            item.obj_trans.position = point_to_throw.position;
            //line_sight.startColor = new Color(255, 255, 255, 90);

            line_sight.endColor = line_sight.startColor = new Color(1, 0, 0, 1);
            line_sight.enabled = true;
            //line_sight.UpdateGIMaterials();
        }
        else
        {
            Debug.Log("On ne peut pas lancer cet item");
        }
    }
     
    //------------------------------------------------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FallDetector")
        {
            transform.position = respawn_point;
        }
        else if (other.tag == "Checkpoint")
        {
            Debug.Log("chekpoint");
            respawn_point = other.transform.position;

        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.transform.gameObject.layer == LayerMask.NameToLayer("Buisson"))
        {
            if (!locking)
            {
                Lock(other.transform.gameObject);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Buisson"))
        {
            Delock();
        }
    }

}
