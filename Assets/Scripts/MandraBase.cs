using UnityEngine;
using System.Collections;

public abstract class MandraBase : MonoBehaviour {
    [Tooltip("지정되면 Physics.gravity 대신이 변형의 문자에서 방향을 중력 벡터로 사용합니다. Physics.gravity.magnitude는 중력 벡터의 크기로 사용됩니다.")]
    public Transform gravityTarget;

    [Tooltip("'Individual Gravity'가 선택되지 않았을때 중력 x2")]
    [SerializeField]
    protected float gravityMultiplier = 2f; // 증력x2

    [SerializeField]
    protected float airborneThreshold = 0.6f; // Height from ground after which the character is considered airborne
    [SerializeField]
    float slopeStartAngle = 50f; // The start angle of velocity dampering on slopes
    [SerializeField]
    float slopeEndAngle = 85f; // The end angle of velocity dampering on slopes
    [SerializeField]
    float spherecastRadius = 0.1f; // The radius of sperecasting
    [SerializeField]
    LayerMask groundLayers; // The walkable layers

    private PhysicMaterial zeroFrictionMaterial;

    private PhysicMaterial highFrictionMaterial;

    protected Rigidbody r;

    protected const float half = 0.5f;

    protected float originalHeight;

    protected Vector3 originalCenter;

    protected CapsuleCollider capsule;

   // public abstract void Move(Vector3 deltaPosition, Quaternion deltaRotation);

    protected Vector3 GetGravity()
    {
        //if(gravityTarget != null)
        //{
        //    return (gravityTarget.position - transform.position).normalized * Physics.gravity.magnitude;
        //}
        return Physics.gravity;
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        capsule = GetComponent<Collider>() as CapsuleCollider;

        r = GetComponent<Rigidbody>();

        originalHeight = capsule.height;
        originalCenter = capsule.center;

        zeroFrictionMaterial = new PhysicMaterial();
        zeroFrictionMaterial.dynamicFriction = 0f;
        zeroFrictionMaterial.staticFriction = 0f;
        zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        zeroFrictionMaterial.bounciness = 0f;
        zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;

        highFrictionMaterial = new PhysicMaterial();

        //회전 고정
        r.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
	}

    protected virtual RaycastHit GetSpherecastHit()
    {
        Vector3 up = transform.up;
        Ray ray = new Ray(r.position + up * airborneThreshold, -up);        //시작점 , 방향
        Debug.DrawRay(r.position + up * airborneThreshold, -up, Color.green);
        RaycastHit h = new RaycastHit();
        h.point = transform.position - transform.transform.up * airborneThreshold;
        h.normal = transform.up;

        Physics.SphereCast(ray, spherecastRadius, out h, airborneThreshold * 2f, groundLayers); // 레이저 발사위치, 반경, 방향, 거리, 마스크

        return h;
    }
	
    public float GetAngleFromForward(Vector3 worldDirection)
    {
        Vector3 local = transform.InverseTransformDirection(worldDirection);
        return Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
    }

    protected void RigidbodyRotateAround(Vector3 point, Vector3 axis, float angle)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 d = transform.position - point;
        r.MovePosition(point + rotation * d);
        r.MoveRotation(rotation * transform.rotation);
    }

    protected void ScaleCapsule(float mlp)
    {
        if(capsule.height != originalHeight * mlp)
        {
            capsule.height = Mathf.MoveTowards(capsule.height, originalHeight * mlp, Time.deltaTime * 4);
            capsule.center = Vector3.MoveTowards(capsule.center, originalCenter * mlp, Time.deltaTime * 2);
        }
    }

    protected void HighFriction()
    {
        capsule.material = highFrictionMaterial;
    }

    protected void ZeroFriction()
    {
        capsule.material = zeroFrictionMaterial;
    }

    protected float GetSlopeDamper(Vector3 velocity, Vector3 groundNormal)
    {
        float angle = 90f - Vector3.Angle(velocity, groundNormal);
        angle -= slopeStartAngle;
        float range = slopeEndAngle - slopeStartAngle;
        return 1f - Mathf.Clamp(angle / range, 0f, 1f);
    }
}
