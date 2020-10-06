using UnityEngine;
using System.Collections;
using RootMotion.Dynamics;

namespace RootMotion.Demos
{

    public class MandraDamage : MonoBehaviour
    {
        public PuppetMaster puppetMaster;

        public Mandra mandra;

        public int damageLayer;

        public int PlayerLayer;

        public float unpin = 10f;

        public float force = 10f;

        public float damageforce = 100.0f;

        private int countProb;

        public EffectPool pool;
        private int index = 0;
        private int usIndex = 0;

        private bool isOneDamage;

        void Start()
        {
            pool = GameObject.Find("EffectPool").GetComponent<EffectPool>();
        }
    
        
        void OnCollisionEnter(Collision collision)
        {
            var m = collision.gameObject.GetComponent<MuscleCollisionBroadcaster>();


            if (m != null)
            {
                //m.Hit(unpin, (collision.transform.forward - this.transform.forward) * force, collision.transform.forward - this.transform.forward);
            }
            if (collision.rigidbody == null)
                return;

            if (this.mandra.state == MandraState.Dead)
                return;

            var _root = this.gameObject.transform;
            var pp = _root.GetComponentInChildren<Mandra>();
            while (_root != null)
            {
                pp = _root.GetComponentInChildren<Mandra>();

                if (pp != null)
                    break;
                _root = _root.parent;
            }

            var col_root = collision.gameObject.transform;
            var col_pp = col_root.GetComponentInChildren<Mandra>();
            while (col_root != null)
            {
                col_pp = col_root.GetComponentInChildren<Mandra>();

                if (col_pp != null)
                    break;
                col_root = col_root.parent;
            }

            if (collision.gameObject.layer == damageLayer && collision.gameObject.GetComponent<FlyProb>().state == FlyProb.ProbState.Fly
                && pp.gameObject.GetComponentInChildren<Mandra>().isDamage != true)
            {
                Debug.Log(collision.gameObject.GetComponent<FlyProb>().whoOwn);
                if (this.mandra.player == collision.gameObject.GetComponent<FlyProb>().whoOwn)
                {
                    return;
                }
                if (collision.gameObject.GetComponent<FlyProb>().whoOwn == -1)
                {
                    return;
                }
                if (collision.gameObject.tag == "OakBox")
                {
                    this.mandra.oakMove = true;

                    this.mandra.OutLine_Posion.SetActive(true);
                    SoundPool.Instance.SetSound(SoundPool.Instance.OakDamPool, ref SoundPool.Instance.indexOak, this.mandra.transform);
                }
                Vector3 dir = collision.transform.position - this.transform.position;
                dir.y = 0.1f;
                dir = dir.normalized;
                dir *= 10f;
                this.mandra.GetComponent<Rigidbody>().AddForce(dir * 5, ForceMode.Impulse);
                this.mandra.state = MandraState.Damage;


                this.mandra.isDamage = true;
                this.mandra.GetComponent<Animator>().SetBool("IsDamage", true);
                Debug.Log("Damage");
                this.mandra.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_1") as Texture;
                this.mandra.emotion.ScalePlusEmotion();
                Vector3 ppos = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 0.2f, collision.gameObject.transform.position.z);
                collision.gameObject.transform.position = ppos;

                pool.SetEffect(pool.HitEffectPool, ref index, collision.gameObject.transform);

                SoundPool.Instance.SetSound(SoundPool.Instance.objDamagePool, ref SoundPool.Instance.indexCharacDamage, SoundPool.Instance.transform);
            }
            
            if (collision.gameObject.tag == "P1Punch")
            {
                Debug.Log("P1Punch");
            }

            if (collision.gameObject.tag == "P2Punch" &&
                pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.RightAttack")
                && col_pp.state != MandraState.Damage && col_root.gameObject.GetComponentInChildren<Mandra>().isDamage == false)
            {
                Vector3 dir = col_pp.transform.position - pp.transform.position;
                dir = dir.normalized;
                dir *= 6;
                col_pp.GetComponent<Rigidbody>().AddForce(dir * damageforce, ForceMode.Impulse);
                col_pp.state = MandraState.Damage;
                col_root.gameObject.GetComponentInChildren<Mandra>().isDamage = true;
                Debug.Log("1이때렸다");

                col_root.gameObject.GetComponentInChildren<Animator>().SetBool("IsDamage", true);
                Debug.Log("Damage");
                col_root.gameObject.GetComponentInChildren<Mandra>().GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_1") as Texture;
                col_root.gameObject.GetComponentInChildren<Mandra>().emotion.ScalePlusEmotion();


                SoundPool.Instance.SetSound(SoundPool.Instance.objDamagePool, ref SoundPool.Instance.indexCharacDamage, SoundPool.Instance.transform);
            }
            if (collision.gameObject.tag == "P1Punch" &&
                pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.RightAttack")
                && col_pp.state != MandraState.Damage && col_root.gameObject.GetComponentInChildren<Mandra>().isDamage == false)
            {
                Vector3 dir = col_pp.transform.position - pp.transform.position;
                dir = dir.normalized;
                dir *= 6;
                col_pp.GetComponent<Rigidbody>().AddForce(dir * damageforce, ForceMode.Impulse);
                col_pp.state = MandraState.Damage;
                col_root.gameObject.GetComponentInChildren<Mandra>().isDamage = true;
                Debug.Log("0이때렸다");
                col_root.gameObject.GetComponentInChildren<Animator>().SetBool("IsDamage", true);
                Debug.Log("Damage");
                col_root.gameObject.GetComponentInChildren<Mandra>().GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_1") as Texture;
                col_root.gameObject.GetComponentInChildren<Mandra>().emotion.ScalePlusEmotion();



                SoundPool.Instance.SetSound(SoundPool.Instance.objDamagePool, ref SoundPool.Instance.indexCharacDamage, SoundPool.Instance.transform);
            }


            if (collision.gameObject.tag == "Ponitail1" &&
                pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack")
                && col_pp.state != MandraState.Damage && col_root.gameObject.GetComponentInChildren<Mandra>().isDamage == false)
            {
                Vector3 dir = col_pp.transform.position - pp.transform.position;
                dir = dir.normalized;
                dir *= 2;
                col_pp.GetComponent<Rigidbody>().AddForce(dir * damageforce, ForceMode.Impulse);
                col_pp.state = MandraState.Damage;
                col_root.gameObject.GetComponentInChildren<Mandra>().isDamage = true;
                Debug.Log("1이때렸다");

                col_root.gameObject.GetComponentInChildren<Animator>().SetBool("IsDamage", true);
                Debug.Log("Damage");
                col_root.gameObject.GetComponentInChildren<Mandra>().GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_1") as Texture;
                col_root.gameObject.GetComponentInChildren<Mandra>().emotion.ScalePlusEmotion();
                col_pp.transform.position = new Vector3(col_pp.gameObject.transform.position.x, col_pp.gameObject.transform.position.y + 0.5f, col_pp.gameObject.transform.position.z);
                pool.SetEffect(pool.GroupHitEffectPool,ref usIndex, col_pp.transform);


                SoundPool.Instance.SetSound(SoundPool.Instance.objDamagePool, ref SoundPool.Instance.indexCharacDamage, SoundPool.Instance.transform);
            }

            if (collision.gameObject.tag == "Ponitail2" &&
                pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack")
                && col_pp.state != MandraState.Damage && col_root.gameObject.GetComponentInChildren<Mandra>().isDamage == false)
            {
                Vector3 dir = col_pp.transform.position - pp.transform.position;
                dir = dir.normalized;
                dir *= 2;
                col_pp.GetComponent<Rigidbody>().AddForce(dir * damageforce, ForceMode.Impulse);
                col_pp.state = MandraState.Damage;
                col_root.gameObject.GetComponentInChildren<Mandra>().isDamage = true;
                Debug.Log("0이때렸다");
                col_root.gameObject.GetComponentInChildren<Animator>().SetBool("IsDamage", true);
                Debug.Log("Damage");
                col_root.gameObject.GetComponentInChildren<Mandra>().GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_1") as Texture;
                col_root.gameObject.GetComponentInChildren<Mandra>().emotion.ScalePlusEmotion();

                col_pp.transform.position = new Vector3(col_pp.gameObject.transform.position.x, col_pp.gameObject.transform.position.y + 0.5f, col_pp.gameObject.transform.position.z);

                pool.SetEffect(pool.GroupHitEffectPool, ref usIndex, col_pp.transform);


                SoundPool.Instance.SetSound(SoundPool.Instance.objDamagePool, ref SoundPool.Instance.indexCharacDamage, SoundPool.Instance.transform);
            }
           

            //if (collision.gameObject.tag == "Ponitail2" && this.mandra.player != collision.gameObject.GetComponent<Mandra>().player)
            //{
            //    Debug.Log("서로?");
            //}
        }
    }

}