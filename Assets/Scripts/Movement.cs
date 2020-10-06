using System;
using UnityEngine;
public class Movement : MonoBehaviour
{

    public static Movement instance;

    public float maxMovementSpeed;
    public float movementSpeed;
    public float jumpFower = 5f;

    private Vector3 inputDirection;

    private Mandra mandra;
    private Rigidbody rigid;

    public float slideSpeed = 2f;
    private bool isSliding = false;
    private Vector3 slideForward;
    private float slideTimer = 0.0f;
    public float slideTimerMax = 0.3f;
    private Vector3 BananaBeforePos;
    private GameObject Banana;

    public Camera camera;
    // Use this for initialization
    void Start()
    {
        this.mandra = base.gameObject.GetComponent<Mandra>();
        rigid = GetComponent<Rigidbody>();
        movementSpeed = maxMovementSpeed;
    }

    private void Update()
    {
        this.inputDirection = this.mandra.walkDirection;
        if (!isSliding)
        {
            if (this.inputDirection == Vector3.zero && (this.mandra.state != MandraState.Idle || this.mandra.state != MandraState.Attack || this.mandra.state != MandraState.Jump))
            {
                this.mandra.state = MandraState.Idle;
            }
            else if (this.inputDirection != Vector3.zero && this.mandra.state != MandraState.Attack)
            {
                this.transform.Translate(this.mandra.walkDirection * movementSpeed * Time.deltaTime, Space.Self);
                if (this.mandra.state != MandraState.Jump)
                    this.mandra.state = MandraState.Walk;
            }
        }
        if (isSliding)
        {
            movementSpeed = slideSpeed;
            slideTimer += Time.deltaTime;
            
            this.transform.Translate(slideForward * movementSpeed * Time.deltaTime, Space.Self);
            Banana.transform.parent.position = new Vector3(this.transform.position.x, Banana.transform.parent.position.y, this.transform.position.z);
            
        }
        if (slideTimer > slideTimerMax)
        {
            isSliding = false;
            if(Banana != null)
                Destroy(Banana.transform.parent.gameObject);
        }
    }
    
    void OnTriggerEnter(Collider call)
    {
        if(call.gameObject.tag == "BananaSkin")
        {
            Banana = call.gameObject;
            slideTimer = 0.0f;
            isSliding = true;
            //if(mandra.walkDirection.z == -1)
            //{
            //    slideForward = camera.transform.TransformDirection(Vector3.back);
            //}
            //else
            //{

            //    slideForward = camera.transform.TransformDirection(Vector3.forward);
            //}
            slideForward = this.mandra.transform.forward;
            BananaBeforePos = call.gameObject.transform.position;
            SoundPool.Instance.SetSound(SoundPool.Instance.BananaPool, ref SoundPool.Instance.indexBanana, call.gameObject.transform);
        }
        if (call.gameObject.tag == "Jump_pos")
        {
            Vector3 Up = new Vector3(3f, jumpFower, -4f);

            rigid.AddForce(Up, ForceMode.Impulse);
            SoundPool.Instance.SetSound(SoundPool.Instance.JumppadPool, ref SoundPool.Instance.indexJumppad, call.gameObject.transform);

        }

        else if (call.gameObject.tag == "Jump_pos2")
        {
            Vector3 Up = new Vector3(-3f, jumpFower, -4f);

            rigid.AddForce(Up, ForceMode.Impulse);
            SoundPool.Instance.SetSound(SoundPool.Instance.JumppadPool, ref SoundPool.Instance.indexJumppad, call.gameObject.transform);

        }
    }

}
