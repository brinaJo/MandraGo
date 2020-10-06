using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_night : MonoBehaviour
{
    public enum NightState
    {
        Attack,
        None
    }
    public NightState state;

    public float damageforce = 100f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator StateChange(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.state = NightState.None;
    }
    void OnCollisionEnter(Collision col)
    {

        var _root = col.gameObject.transform;
        var pp = _root.GetComponentInChildren<Mandra>();
        while (_root != null)
        {
            pp = _root.GetComponentInChildren<Mandra>();

            if (pp != null)
                break;
            _root = _root.parent;
        }

        if (col.gameObject.tag == "Ponitail1" && pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack")
            && this.state != NightState.Attack)
        {

            if (this.gameObject.tag == "night_piece")
            {
                SoundPool.Instance.SetSound(SoundPool.Instance.ChessProbPool, ref SoundPool.Instance.indexChess, col.gameObject.transform);
                this.state = NightState.Attack;
                Vector3 dir = this.transform.position - pp.transform.position;
                dir = dir.normalized;
                dir.y = 0.08f;
                dir *= 6;
                this.gameObject.GetComponent<Rigidbody>().AddForce(dir * damageforce, ForceMode.Impulse);
                StartCoroutine(StateChange(0.25f));               
            }
        }
        else if (col.gameObject.tag == "Ponitail2" && pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack")
            && this.state != NightState.Attack)
        {
            if (this.gameObject.tag == "night_piece")
            {
                SoundPool.Instance.SetSound(SoundPool.Instance.ChessProbPool, ref SoundPool.Instance.indexChess, col.gameObject.transform);
                this.state = NightState.Attack;
                Vector3 dir = this.transform.position - pp.transform.position;
                dir = dir.normalized;

                dir.y = 0.08f;
                dir *= 6;
                this.gameObject.GetComponent<Rigidbody>().AddForce(dir * damageforce, ForceMode.Impulse);
                StartCoroutine(StateChange(0.25f));
            }
        }
           

    }
}
