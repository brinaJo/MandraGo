using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour
{
    public MandraSegment partHead;

    public MandraSegment partChest;

    public MandraSegment partWaist;

    public MandraSegment partHips;

    public MandraSegment partLeftArm;

    public MandraSegment partLeftForearm;

    public MandraSegment partLeftHand;

    public MandraSegment partLeftThigh;

    public MandraSegment partLeftLeg;

    //public MandraSegment partLeftFoot;

    public MandraSegment partRightArm;

    public MandraSegment partRightForearm;

    public MandraSegment partRightHand;

    public MandraSegment partRightThigh;

    public MandraSegment partRightLeg;

    //public MandraSegment partRightFoot;

   // public MandraSegment partBall;

    public MandraSegment partLeafLoot;

    public MandraSegment partLeafLoot2;

    public MandraSegment partLeaf1;

    public MandraSegment partLeaf2;

    public MandraSegment partLeaf3;

    public MandraSegment partLeaf4;

    //public MandraSegment[] sensor;

    public SkinnedMeshRenderer meshRenderer;

    [NonSerialized]
    public Rigidbody[] rigidbodies;

    private Vector3[] velocities;

    public float weight;

    public float mass;

    public float handLength;

    private bool initialized;

    private bool isFallSpeedLimited;

    public bool skipLimiting;

    public float fallSpeed = 5;


  

    public Vector3 momentum
    {
        get
        {
            Vector3 vector = Vector3.zero;
            for (int i = 0; i < this.rigidbodies.Length; i++)
            {
                Rigidbody rigidbody = this.rigidbodies[i];
                vector += rigidbody.velocity * rigidbody.mass;
            }
            return vector;
        }
    }

    public Vector3 velocity
    {
        get
        {
            return this.momentum / this.mass;
        }
    }

    private void OnEnable()
    {
        if (this.initialized)
        {
            return;
        }
        this.initialized = true;
        this.meshRenderer = base.GetComponentInChildren<SkinnedMeshRenderer>();
        this.CollectSegments();
        this.SetupColliders();
        this.handLength = (this.partLeftArm.transform.position - this.partLeftForearm.transform.position).magnitude + (this.partLeftForearm.transform.position - this.partLeftHand.transform.position).magnitude;
    }

    private void CollectSegments()
    {
        Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
        Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
        this.rigidbodies = base.GetComponentsInChildren<Rigidbody>();
        this.velocities = new Vector3[this.rigidbodies.Length];
        this.mass = 0f;
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            dictionary[componentsInChildren[i].name.ToLower()] = componentsInChildren[i];
            Rigidbody component = componentsInChildren[i].GetComponent<Rigidbody>();
            if (component != null)
            {
                component.maxAngularVelocity = 10f;
                this.mass += component.mass;
            }
        }
        this.weight = this.mass * 9.81f;
        this.partHead = this.FindSegment(dictionary, "Head");
        this.partChest = this.FindSegment(dictionary, "Chest");
        this.partWaist = this.FindSegment(dictionary, "Waist");
        this.partHips = this.FindSegment(dictionary, "Hips");
        this.partLeftArm = this.FindSegment(dictionary, "LeftArm");
        this.partLeftForearm = this.FindSegment(dictionary, "LeftForearm");
        this.partLeftHand = this.FindSegment(dictionary, "LeftHand");
        this.partLeftThigh = this.FindSegment(dictionary, "LeftThigh");
        this.partLeftLeg = this.FindSegment(dictionary, "LeftLeg");
        //this.partLeftFoot = this.FindSegment(dictionary, "leftFoot");
        this.partRightArm = this.FindSegment(dictionary, "RightArm");
        this.partRightForearm = this.FindSegment(dictionary, "RightForearm");
        this.partRightHand = this.FindSegment(dictionary, "RightHand");
        this.partRightThigh = this.FindSegment(dictionary, "RightThigh");
        this.partRightLeg = this.FindSegment(dictionary, "RightLeg");
        //this.partRightFoot = this.FindSegment(dictionary, "rightFoot");
        this.partLeafLoot = this.FindSegment(dictionary, "Ponytail1");
        this.partLeafLoot2 = this.FindSegment(dictionary, "Ponytail11");
        this.partLeaf1 = this.FindSegment(dictionary, "Ponytail12");
        this.partLeaf2 = this.FindSegment(dictionary, "Ponytail13");
        this.partLeaf3 = this.FindSegment(dictionary, "Ponytail14");
        this.partLeaf4 = this.FindSegment(dictionary, "Ponytail15");
        //this.partBall = this.FindSegment(dictionary, "ball");
    }

    private void SetupColliders()
    {
        Physics.IgnoreCollision(this.partChest.collider, this.partHead.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partLeftArm.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partLeftForearm.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partRightArm.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partRightForearm.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partWaist.collider);

        Physics.IgnoreCollision(this.partHips.collider, this.partChest.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partWaist.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partLeftThigh.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partLeftLeg.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partRightThigh.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partRightLeg.collider);

        Physics.IgnoreCollision(this.partLeftForearm.collider, this.partLeftArm.collider);
        Physics.IgnoreCollision(this.partLeftForearm.collider, this.partLeftHand.collider);

        Physics.IgnoreCollision(this.partLeftArm.collider, this.partLeftHand.collider);

        Physics.IgnoreCollision(this.partRightForearm.collider, this.partRightArm.collider);
        Physics.IgnoreCollision(this.partRightForearm.collider, this.partRightHand.collider);

        Physics.IgnoreCollision(this.partRightArm.collider, this.partRightHand.collider);

        Physics.IgnoreCollision(this.partLeftThigh.collider, this.partLeftLeg.collider);
        Physics.IgnoreCollision(this.partRightThigh.collider, this.partRightLeg.collider);

        Physics.IgnoreCollision(this.partHead.collider, this.partLeafLoot.collider);
        Physics.IgnoreCollision(this.partHead.collider, this.partLeafLoot2.collider);

        Physics.IgnoreCollision(this.partLeafLoot.collider, this.partLeafLoot2.collider);

        Physics.IgnoreCollision(this.partLeafLoot2.collider, this.partLeaf1.collider);
        Physics.IgnoreCollision(this.partLeaf1.collider, this.partLeaf2.collider);
        Physics.IgnoreCollision(this.partLeaf2.collider, this.partLeaf3.collider);
        Physics.IgnoreCollision(this.partLeaf3.collider, this.partLeaf4.collider);

        Physics.IgnoreCollision(this.partChest.collider, this.partLeaf1.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partLeaf2.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partLeaf3.collider);
        Physics.IgnoreCollision(this.partChest.collider, this.partLeaf4.collider);

        Physics.IgnoreCollision(this.partHips.collider, this.partLeaf1.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partLeaf2.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partLeaf3.collider);
        Physics.IgnoreCollision(this.partHips.collider, this.partLeaf4.collider);

        Physics.IgnoreCollision(this.partLeftThigh.collider, this.partLeaf1.collider);
        Physics.IgnoreCollision(this.partLeftThigh.collider, this.partLeaf2.collider);
        Physics.IgnoreCollision(this.partLeftThigh.collider, this.partLeaf3.collider);
        Physics.IgnoreCollision(this.partLeftThigh.collider, this.partLeaf4.collider);

        Physics.IgnoreCollision(this.partRightThigh.collider, this.partLeaf1.collider);
        Physics.IgnoreCollision(this.partRightThigh.collider, this.partLeaf2.collider);
        Physics.IgnoreCollision(this.partRightThigh.collider, this.partLeaf3.collider);
        Physics.IgnoreCollision(this.partRightThigh.collider, this.partLeaf4.collider);

    }

    //private void LimitFallSpeed()
    //{
    //    bool flag = Game.instance.state != GameState.PlayingLevel;
    //    if (this.isFallSpeedLimited != flag)
    //    {
    //        this.isFallSpeedLimited = flag;
    //        if (flag)
    //        {
    //            this.SetDrag(0.1f);
    //        }
    //        else
    //        {
    //            this.SetDrag(0.05f);
    //        }
    //    }
    //}

    private MandraSegment FindSegment(Dictionary<string, Transform> children, string name)
    {
        MandraSegment MandraSegment = new MandraSegment();
        MandraSegment.transform = children[name.ToLower()];
        MandraSegment.collider = MandraSegment.transform.GetComponent<Collider>();
        MandraSegment.rigidbody = MandraSegment.transform.GetComponent<Rigidbody>();
        MandraSegment.startupRotation = MandraSegment.transform.localRotation;
        return MandraSegment;
    }

    public void SetDrag(float drag)
    {
        for (int i = 0; i < this.rigidbodies.Length; i++)
        {
            this.rigidbodies[i].drag = drag;
        }
    }

    public void AddForceSegments()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].useGravity = false;
            rigidbodies[i].AddForce(Physics.gravity * rigidbodies[i].mass/fallSpeed);
        }
        this.transform.parent = this.transform.parent.parent;
    }
    public void ResetGravitySegments()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].useGravity = true;
        }
    }

    public void AddKinemetic()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {

            rigidbodies[i].velocity = Vector3.zero;
            rigidbodies[i].angularVelocity = Vector3.zero;
            //rigidbodies[i].isKinematic = true;
        }
        StartCoroutine(ResetKinemetic());
    }

    IEnumerator ResetKinemetic()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < rigidbodies.Length; i++)
        {

            //rigidbodies[i].isKinematic = false;
        }
    }
    public void FixedUpdate()
    {
        //this.LimitFallSpeed();
        if (this.skipLimiting)
        {
            this.skipLimiting = false;
            return;
        }
        for (int i = 0; i < this.rigidbodies.Length; i++)
        {
            Vector3 vector = this.velocities[i];
            Vector3 vector2 = this.rigidbodies[i].velocity;
            Vector3 vector3 = vector2 - vector;
            if (Vector3.Dot(vector, vector3) < 0f)
            {
                Vector3 normalized = vector.normalized;
                float magnitude = vector.magnitude;
                float value = -Vector3.Dot(normalized, vector3);
                float d = Mathf.Clamp(value, 0f, magnitude);
                vector3 += normalized * d;
            }
            float num = 1000f * Time.deltaTime;
            if (vector3.magnitude > num)
            {
                Vector3 b = Vector3.ClampMagnitude(vector3, num);
                vector2 -= vector3 - b;
                this.rigidbodies[i].velocity = vector2;
            }
            this.velocities[i] = vector2;
        }
    }

    

}
