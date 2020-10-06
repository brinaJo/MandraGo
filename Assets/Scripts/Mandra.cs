using System;
using System.Collections.Generic;
using System.Collections;
using RootMotion.Dynamics;
using UnityEngine.UI;
using UnityEngine;
using Rewired;

public class Mandra : MandraBase
{
    public struct AnimState
    {
        public Vector3 moveDirection; // the forward speed
        public bool jump; // should the character be jumping?
        public bool crouch; // should the character be crouching?
        public bool onGround; // is the character grounded
        public bool isStrafing; // should the character always rotate to face the move direction or strafe?
        public float yVelocity; // y velocity of the character
    }
    public float rotSpeed;

    private float prevCameraYawAngle;

    public float cameraPitchAngle;

    public float verticalInputAngle;

    public float cameraYawAngle;

    public float targetPitchAngle;

    public float targetYawAngle;

    public Tutorial tutorial;

    public Image punchIcon;

    public Ragdoll ragdoll;

    public BehaviourPuppet puppet;

    public Camera mandraCam;

    public AnimState animState = new AnimState();

    public Emotion emotion;

    public int player;

    public bool disableInput;

    public Vector3 walkDirection;

    private List<float> mouseInputX = new List<float>();

    private List<float> mouseInputY = new List<float>();

    public Vector3 jumpWalkDir;

    public float turnValue;

    public bool jumpPressed;

    public bool isJump;

    public bool mouseControl;

    public bool attackPressed;

    public bool isAttack3;

    public float turnSpeed;

    public bool tutorialIng;

    public bool resultIng;

    public bool canMove;

    public bool oakMove;

    public bool canJump;

    public bool isDamage = false;

    public bool canHackPunch;

    public float damageDelay;

    public float damageDelaytime = 1.26f;

    public bool onGround { get; private set; }

    public GameObject dark; //시야가림.

    public GameObject OutLine_Ice;

    public GameObject OutLine_Posion;

    public bool darkness;

    public float darktime; //시야가림 시간.

    public MandraState state;

    AttackMotion motion;

    private float jumpDelay;

    private float comboDelay;

    private float AttackDelay;

    private Animator animator;

    private Vector3 gravity;

    private Vector3 verticalVelocity;

    private float velocityY;

    private Vector3 normal, platformVelocity, platformAngularVelocity;

    public float platformFriction = 7f;                 // the acceleration of adapting the velocities of moving platforms

    public float groundStickyEffect = 4f;				// power of 'stick to ground' effect - prevents bumping down slopes.

    private RaycastHit hit;

    public Movement movement;//4.18

    private GameManager gamemanager;

    [Header("Jumping and Falling")]
    public float airSpeed = 6f;

    public float airControl = 2f;

    public float jumpPower = 12f;

    public float jumpRepeatDelayTime = 0f;

    private float jumpLeg, jumpEndTime, forwardMlp, groundDistance, lastAirTime, stickyForce;

    private Vector3 wallNormal = Vector3.up;

    private Transform gameOverPos;

    private float rotEnd;
    /// <summary>

    public Player Replayer;

    private UIctrl uictrl;
    /// </summary>
    private float axisV;
    private float axisH;
    private float alpa;
    private void Awake()
    {
        base.tag = "Player";
        base.transform.name = "Player " + this.player;
        this.AttackDelay = 0f;
        this.animator = this.gameObject.GetComponent<Animator>();
        this.canMove = true;
        this.canJump = true;
        movement = this.gameObject.GetComponent<Movement>();
        this.ragdoll = this.transform.parent.GetComponent<Ragdoll>();
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    protected override void Start()
    {
        base.Start();
        onGround = true;
        animState.onGround = true;
        Replayer = ReInput.players.GetPlayer(this.player);
        uictrl = FindObjectOfType<UIctrl>();
        if(tutorialIng)
        {
            String name = "Tutorial_" + this.player;
            tutorial = GameObject.Find(name).GetComponent<Tutorial>();
        }
    }


    private void FixedUpdate()
    {

        gravity = GetGravity();
        verticalVelocity = V3Tools.ExtractVertical(r.velocity, gravity, 1f);
        velocityY = verticalVelocity.magnitude;
        if (gravityTarget != null)
        {
            r.useGravity = false;
            r.AddForce(gravity);
        }

        this.GroundCheck();
        if(animState.yVelocity<0.01f)
        {
            onGround = true;
        }
        if (this.state != MandraState.Dead && this.state != MandraState.GameEnd && !tutorialIng)
        {
            this.MoveInput();
            this.ReadInput();
            this.AttackInput();
            this.DamageCheck();
            this.DarkDamageCheck();
            if (onGround)
            {
                animState.jump = this.Jump();
            }
            else
            {
                r.AddForce(gravity * gravityMultiplier);
            }

        }
        else if (this.state != MandraState.Dead && this.state != MandraState.GameEnd && tutorialIng)
        {
            if (tutorial.canMove)
                if (this.MoveInput())
                {
                    tutorial.canMoveComplete = true;
                }
            if(tutorial.canRot)
                this.ReadInput();
            if(tutorial.canAttack)
                this.AttackInput();
            this.DamageCheck();
            this.DarkDamageCheck();
            if (onGround && tutorial.canJump)
            {
                // Jumping
                animState.jump = this.Jump();

            }
            else
            {
                r.AddForce(gravity * gravityMultiplier);
            }
        }
        else if(this.state == MandraState.GameEnd)
        {
            walkDirection = Vector3.zero;
            rotEnd += Time.deltaTime * 500f;
            if (rotEnd < 180f)
                this.transform.rotation = Quaternion.Euler(0f, rotEnd + cameraYawAngle, 0f);
        }
        else
        {
            this.MandraDead();
        }
        // Individual gravity


    }
    protected virtual void Update()
    {
        animState.onGround = onGround;
        animState.yVelocity = Mathf.Lerp(animState.yVelocity, velocityY, Time.deltaTime * 15f);

        if(oakMove)
        {
            StartCoroutine(OakCount(5.0f));
        }
    }
    IEnumerator OakCount(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        oakMove = false;
        this.OutLine_Posion.SetActive(false);
    }
    public void MandraDead()
    {
    }
    private void DamageCheck()
    {

        if (damageDelay > 0f && isDamage)
        {
            damageDelay -= Time.fixedDeltaTime;
        }
        else if (damageDelay < 0f)
        {
            damageDelay = damageDelaytime;
            isDamage = false;
            this.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV") as Texture;
            this.emotion.ScaleMinusEmotion();
        }
        else
            return;
    }

    private void ReadInput()
    {
        float mouseInputX = Replayer.GetAxis("Mouse X");
        float mouseInputY = Replayer.GetAxis("Mouse Y");

        if (oakMove)
        {
            mouseInputX = Replayer.GetAxis("Mouse X") * -1;
            mouseInputY = Replayer.GetAxis("Mouse Y");
        }

        //    Vector2 joyLook = this.controls.joyLook;
        //    Vector2 keyLook = this.controls.keyLook;
        //    if (joyLook.sqrMagnitude > keyLook.sqrMagnitude)
        //    {
        //        this.mouseControl = false;
        //    }
        //    if (keyLook.sqrMagnitude > joyLook.sqrMagnitude)
        //    {
        //        this.mouseControl = true;
        //    }
        if (this.mouseControl)
        {
            this.prevCameraYawAngle = this.cameraYawAngle;
            this.cameraYawAngle += Smoothing.SmoothValue(this.mouseInputX, mouseInputX * rotSpeed);
            this.cameraPitchAngle -= Smoothing.SmoothValue(this.mouseInputY, mouseInputY);
            this.cameraPitchAngle = Mathf.Clamp(this.cameraPitchAngle, -80f, 80f);
            int num = 0;
            float num2 = 0.25f;
            this.cameraPitchAngle = Mathf.Lerp(this.cameraPitchAngle, (float)num, num2 * 5f * Time.fixedDeltaTime);
            this.cameraPitchAngle = Mathf.MoveTowards(this.cameraPitchAngle, (float)num, num2 * 30f * Time.fixedDeltaTime);
            this.verticalInputAngle = this.cameraPitchAngle;
        }
        //    else
        //    {
        //        this.verticalInputAngle = -80f * this.controls.lookYnormalized;
        //        this.cameraYawAngle += joyLook.x * Time.deltaTime * 120f;
        //        if (this.verticalLookMode == VerticalLookMode.Relative)
        //        {
        //            this.cameraPitchAngle -= joyLook.y * Time.deltaTime * 120f * 2f;
        //            this.cameraPitchAngle = Mathf.Clamp(this.cameraPitchAngle, -80f, 80f);
        //        }
        //        else
        //        {
        //            float num3 = -80f * this.controls.lookYnormalized;
        //            bool flag = num3 * this.cameraPitchAngle < 0f || Mathf.Abs(num3) > Mathf.Abs(this.cameraPitchAngle);
        //            float num4 = (!this.leftGrab && !this.rightGrab) ? 0.25f : 0.0125f;
        //            float num5 = (!flag) ? num4 : 1f;
        //            this.cameraPitchAngle = Mathf.Lerp(this.cameraPitchAngle, num3, num5 * 5f * Time.fixedDeltaTime * this.controls.vScale);
        //            this.cameraPitchAngle = Mathf.MoveTowards(this.cameraPitchAngle, num3, num5 * 30f * Time.fixedDeltaTime * this.controls.vScale);
        //        }
        //    }
        this.targetPitchAngle = this.cameraPitchAngle;
        this.targetYawAngle = this.cameraYawAngle;
        this.transform.rotation = Quaternion.Euler(0f, this.cameraYawAngle, 0f);

        if (this.prevCameraYawAngle > this.cameraYawAngle)
        {
            if (tutorialIng)
                tutorial.canRotComplete = true;
            animator.SetBool("IsTurnLeft", true);

            animator.SetBool("IsTurnRight", false);
        }
        if (this.prevCameraYawAngle < this.cameraYawAngle)
        {
            if(tutorialIng)
                tutorial.canRotComplete = true;
            animator.SetBool("IsTurnRight", true);

            animator.SetBool("IsTurnLeft", false);
        }
        if(this.prevCameraYawAngle == this.cameraYawAngle)
        {
            animator.SetBool("IsTurnLeft", false);
            animator.SetBool("IsTurnRight", false);
        }
    }
    private bool MoveInput()
    {
        this.walkDirection = Vector3.zero;
        if (canMove)
        {
            if (Replayer.GetAxis("Move Horizontal") != 0f || Replayer.GetAxis("Move Vertical") != 0f)//(Input.GetAxis("P" + (this.player + 1) + " Vertical") != 0f || Input.GetAxis("P" + (this.player + 1) + " Horizontal") != 0f)
            {
                Vector3 a = Camera.main.transform.TransformDirection(Vector3.forward);
                a.y = 0f;
                a = a.normalized;
                Vector3 a2 = new Vector3(a.z, 0f, -a.x);
                axisV = Replayer.GetAxis("Move Vertical");//Input.GetAxis("P" + (this.player + 1) + " Vertical");
                axisH = Replayer.GetAxis("Move Horizontal");// Input.GetAxis("P" + (this.player + 1) + " Horizontal");

                walkDirection = (Vector3.right * axisH) + (Vector3.forward * axisV);
                if(oakMove)
                {
                    walkDirection = (Vector3.right * axisH) + (Vector3.back * axisV);
                }
                jumpWalkDir = new Vector3(0f, 0f, 0f);

                return true;
            }
        }
        return false;
    }
    protected virtual bool Jump()
    {
     
        if (!this.JumpInput()) return false;
        if (this.state != MandraState.Jump) return false;
        if (Time.time < lastAirTime + jumpRepeatDelayTime) return false;

        // Jump
        onGround = false;
        jumpEndTime = Time.time + 0.1f;

        jumpWalkDir = mandraCam.transform.rotation * walkDirection.normalized;
        Vector3 jumpVelocity = jumpWalkDir * airSpeed;
        r.velocity = jumpVelocity;
        r.velocity += transform.up * jumpPower;

        return true;
    }
    private bool JumpInput()
    {
        if (canJump && Replayer.GetButton("Jump"))//Input.GetButton(("P" + (this.player + 1) + " Jump")))
        {
            this.state = MandraState.Jump;
            if(tutorialIng)
                tutorial.canJumpComplete = true;
            return true;
        }
        return false;
    }

    private void AttackInput()
    {
        if (Replayer.GetButtonDown("Attack"))//Input.GetButtonDown("P" + (this.player + 1) + " Attack")) 
        {
            this.state = MandraState.Attack;
            if (canHackPunch)
            {
                animator.SetTrigger("IsHackPunch");
                canHackPunch = false;
                punchIcon.enabled = false;
                SoundPool.Instance.SetSound(SoundPool.Instance.HackPool, ref SoundPool.Instance.indexHack, this.transform);
            }
            else
            {
                animator.SetTrigger("IsAttack3");
            }

            canMove = false;
            this.AttackDelay = 1.0f;
        }


        if (animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack"))
        {
            canMove = true;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.RightAttack"))
        {
            canMove = true;
        }

        if (this.AttackDelay > 0f)
        {
            this.AttackDelay -= Time.fixedDeltaTime;

        }
        if (this.AttackDelay < 0f)
        {
            this.state = MandraState.Idle;
            this.AttackDelay = 0f;
        }

        //if (this.AttackDelay <= 0f)
        //{
        //    if (Input.GetButtonDown("P" + (this.player + 1) + " Attack"))
        //    {
        //        Debug.Log("맞긴하니?");
        //        this.state = MandraState.Attack;dkd
        //        this.attackPressed = true;
        //    }

        //    else if(this.AttackDelay <= 0f)
        //    {
        //        attackPressed = false;
        //    }

        //}

    }
    public void DarkDamageCheck()
    {
        if (darktime > 0f && darkness)
        {
            darktime -= Time.fixedDeltaTime;

            dark.SetActive(true);

        }

        else if (darktime < 0f)
        {
            darktime = 5.0f;
            darkness = false;
            dark.SetActive(false);
        }
        else
            return;
    }
    // Is the character grounded?
    private void GroundCheck()
    {
        Vector3 platformVelocityTarget = Vector3.zero;
        platformAngularVelocity = Vector3.zero;
        float stickyForceTarget = 0f;
        

        hit = GetSpherecastHit();

        //normal = hit.normal;
        normal = transform.up;

        //groundDistance = r.position.y - hit.point.y;
        groundDistance = Vector3.Project(r.position - hit.point, transform.up).magnitude;

        // 점프가 아니라면
        bool findGround = Time.time > jumpEndTime && velocityY < jumpPower * 0.5f;

        if (findGround)
        {
            bool g = onGround;
            onGround = false;

            // 캐릭터와 땅사이 거리
            float groundHeight = !g ? airborneThreshold * 0.5f : airborneThreshold;

            //Vector3 horizontalVelocity = r.velocity;
            Vector3 horizontalVelocity = V3Tools.ExtractHorizontal(r.velocity, gravity, 1f);

            float velocityF = horizontalVelocity.magnitude;

            if (groundDistance < groundHeight)
            {
                
                stickyForceTarget = groundStickyEffect * velocityF * groundHeight;

               
                if (hit.rigidbody != null)
                {
                    platformVelocityTarget = hit.rigidbody.GetPointVelocity(hit.point);
                    platformAngularVelocity = Vector3.Project(hit.rigidbody.angularVelocity, transform.up);
                }

                // Flag the character grounded
                onGround = true;
            }
            else
            {
                this.state = MandraState.Dead;
            }
        }
 
        

        platformVelocity = Vector3.Lerp(platformVelocity, platformVelocityTarget, Time.deltaTime * platformFriction);

        stickyForce = stickyForceTarget;//Mathf.Lerp(stickyForce, stickyForceTarget, Time.deltaTime * 5f);

       
        if (!onGround) lastAirTime = Time.time;
    }

    private void otherCollCheck()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack"))
        {

        }
    }
    
    public void SpawnAt(Transform spawnPoint, Vector3 offset)
    {
        this.state = MandraState.Dead;
        Vector3 a = this.KillHorizontalVelocity();
        int num = 2;
        Vector3 position = offset + spawnPoint.position - a * (float)num - Physics.gravity * (float)num * (float)num / 2f;
        //this.SetPosition(position);
        if (a.magnitude < 5f)
        {
           //this.AddRandomTorque(1f);
        }
    }
    public void SetPosition(Vector3 spawnPos)
    {
        base.transform.parent.position = spawnPos;
    }
    public Vector3 KillHorizontalVelocity()
    {
        Rigidbody[] rigidbodies = this.ragdoll.rigidbodies;
        Vector3 vector = this.ragdoll.velocity;
        Vector3 vector2 = vector;
        vector2.y = 0f;
        vector -= vector2;
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].useGravity = false;
            rigidbodies[i].AddForce(Physics.gravity * rigidbodies[i].mass / 5);
        }
        return vector;
    }
    public void AddRandomTorque(float multiplier)
    {
        Vector3 torque = UnityEngine.Random.onUnitSphere * 100f * multiplier;
        for (int i = 0; i < this.ragdoll.rigidbodies.Length; i++)
        {
            Rigidbody body = this.ragdoll.rigidbodies[i];
            body.SafeAddTorque(torque, ForceMode.VelocityChange);
        }
    }
    public void AddTorqueZero()
    {
        for (int i = 0; i < this.ragdoll.rigidbodies.Length; i++)
        {
            Rigidbody body = this.ragdoll.rigidbodies[i];
            body.AddTorque(new Vector3(0, 0, 0));
        }
    }
    public void VisiblePunchIcon()
    {
        punchIcon.enabled = true;
    }
    IEnumerator WaitForAnimation(Animator animator)
    {
        //while (true == animator.GetCurrentAnimatorStateInfo(0).IsName(name)) {
        while (animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack"))
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("ㄻㄴㅇㄻㄴ");
    }

    void OnTriggerStay(Collider call) //4.18
    {
        if (call.gameObject.tag == "Slow")
        {

            if (uictrl == null)
                return;
            if (!uictrl.isGameEnd)
            {
                if (gamemanager.isSlow)
                {
                    OutLine_Ice.SetActive(true);
                    Image outline = OutLine_Ice.GetComponent<Image>();
                    alpa +=  Time.deltaTime*0.3f;
                    outline.color = new Color(outline.color.r, outline.color.g, outline.color.b, alpa);
                    movement.movementSpeed = 0.1f;
                    this.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_2") as Texture;
                    emotion.ScalePlusEmotion();
                }
                else
                {
                    
                    movement.movementSpeed = movement.maxMovementSpeed;
                    if (damageDelay == damageDelaytime)
                    {
                        this.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV") as Texture;
                        emotion.ScaleMinusEmotion();
                    }
                    alpa -= Time.deltaTime * 0.3f;
                    Image outline = OutLine_Ice.GetComponent<Image>();
                    outline.color = new Color(outline.color.r, outline.color.g, outline.color.b, alpa);
                    if (alpa<0f)
                    OutLine_Ice.SetActive(false);
                }
            }
        }
    }
    void OnTriggerExit(Collider call)
    {
        if (call.gameObject.tag == "Slow")
        {

            movement.movementSpeed = movement.maxMovementSpeed;
            if (damageDelay == damageDelaytime)
            {
                this.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV") as Texture;
                emotion.ScaleMinusEmotion();
            }
        }
    }
}
