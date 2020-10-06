using UnityEngine;
using System.Collections;
public class MandraAnimCtrl : MonoBehaviour {
   
    private Mandra mandra;

    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        this.mandra = base.gameObject.GetComponent<Mandra>();
        this.animator = this.gameObject.GetComponent<Animator>();

        //StartCoroutine(this.MandraAction());
    }
	
    private void Update()
    {

        MandraAction();
        animator.SetBool("OnGround", mandra.animState.onGround);
        animator.SetBool("IsStrafing", mandra.animState.isStrafing);

        if (!mandra.animState.onGround)
        {
            animator.SetFloat("Jump", mandra.animState.yVelocity);
        }
    }
    //IEnumerator MandraAction()
    //{

    //    while (this.mandra.state != MandraState.Dead)
    //    {
    //        switch(this.mandra.state)
    //        {
    //            case MandraState.Idle:
    //            {
    //                animator.SetBool("IsTrace", false);
    //                animator.SetBool("IsWalkJump", false);
    //                Debug.Log("Idle" + mandra.player);
    //            }
    //            break;
    //            case MandraState.Walk:
    //            {
    //                if (mandra.animState.onGround)
    //                {
    //                    animator.SetBool("IsTrace", true);
    //                    Debug.Log("walk");

    //                }
    //            }
    //            break;
    //            case MandraState.Attack:
    //            {
    //                animator.SetTrigger("IsAttack3");
    //                Debug.Log("Attack");
    //            }
    //            break;
    //            case MandraState.Damage:
    //            {
    //                 mandra.isDamage = true;
    //                 Debug.Log("Damage");
    //            }
    //                break;
    //            case MandraState.Jump:
    //            {
    //                Debug.Log("Jump");
    //                animator.SetBool("IsWalkJump", true);
    //            }
    //            break;
    //        }
    //        yield return null;
    //    }
    //}
    void MandraAction()
    {
        
        switch (this.mandra.state)
        {
            case MandraState.Idle:
                {
                    animator.SetBool("IsTrace", false);
                    animator.SetBool("IsWalkJump", false);
                }
                break;
            case MandraState.Walk:
                {
                    if (mandra.animState.onGround)
                    {
                        animator.SetBool("IsTrace", true);
                    }
                }
                break;
            case MandraState.Attack:
                {
                    //animator.SetTrigger("IsAttack3");
                    Debug.Log("Attack" + mandra.player);
                    //SoundMgr.Instance.PlaySfx(this.transform.position, AttackClip);
                }
                break;
            case MandraState.Damage:
                {
                    Debug.Log("MandraDamage");
                }
                break;
            case MandraState.Jump:
                {
                    Debug.Log("Jump");
                    animator.SetBool("IsWalkJump", true);
                }
                break;
        }
    }
}
