using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CameraMode
{
    Far,
    Close,
    FirstPerson,
    SoFar

}

public class CameraController3 : MonoBehaviour
{

    public float shakeAmount = 0.0f;
    public CameraMode mode;

    public LayerMask wallLayers;

    public Vector3 farTargetOffset;

    public float farRange;

    public float farRangeAction;

    public float soFarRange;

    public float soFarRangeAction;

    public Vector3 closeTargetOffset;

    public float closeRange;

    public float closeRangeLookUp;

    public Vector3 fpsTargetOffset;

    public Vector2 cameraCenter;

    public float cameraZoom = 1f;

    private float nearClip = 0.05f;

    public bool ignoreWalls;

    public Mandra human;

    public Movement move;

    private Camera camera;

    private Ragdoll ragdoll;

    private List<float> offsetSmoothingFast = new List<float>();

    private List<float> offsetSmoothingSlow = new List<float>();

    private float wallHold = 1000f;

    private float wallHoldTime;

    private float offset = -1f;

    public float creditsAdjust;

    public float headPivotAhead;

    private float pitchRange = 30f;

    private float oldPitchSign;

    private float pitchExpandTimer;

    private Vector3[] rayStarts = new Vector3[4];

    private float cameraTransitionSpeed = 1f;

    private float cameraTransitionPhase;

    private float startFov;

    private Vector3 startOffset;

    private Quaternion startRotation = Quaternion.identity;

    private Vector3 startHumanCameraPos;

    private bool isOnceSofar;

    public float shake;

    private void OnEnable()
    {
        this.camera = base.GetComponent<Camera>();
        this.ragdoll = this.human.transform.parent.GetComponent<Ragdoll>();
    }

    public void LateUpdate()
    {
        if (shake > 0)
        {
            cameraCenter = UnityEngine.Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * 1f;
        }
        else if (shake <= 0)
        {
            cameraCenter = new Vector2(0, 0);
        }
        Vector3 b;
        float y;
        float num;
        float num2;
        float num3;
        float num4;
        float nearClipPlane;
        switch (this.mode)
        {
            case CameraMode.Far:
                this.CalculateFarCam(out b, out y, out num, out num2, out num3, out num4, out nearClipPlane);
                break;
            case CameraMode.Close:
                this.CalculateCloseCam(out b, out y, out num, out num2, out num3, out num4, out nearClipPlane);
                break;
            case CameraMode.SoFar:
                this.CalculateSoFarCam(out b, out y, out num, out num2, out num3, out num4, out nearClipPlane);
                break;
            case CameraMode.FirstPerson:
                this.CalculateFirstPersonCam(out b, out y, out num, out num2, out num3, out num4, out nearClipPlane);
                break;
            default:
                this.CalculateCloseCam(out b, out y, out num, out num2, out num3, out num4, out nearClipPlane);
                break;
        }
        num2 /= this.cameraZoom;
        num2 = Mathf.Max(num2, num3);
        if (this.creditsAdjust > 0f)
        {
            num = Mathf.Lerp(num, 90f, this.creditsAdjust * 0.7f);
            num2 += this.creditsAdjust * 20f;
            num4 = Mathf.Lerp(num4, 40f, this.creditsAdjust);
        }
        Quaternion quaternion = Quaternion.Euler(num, y, 0f);
        Vector3 a = quaternion * Vector3.forward;
        Vector3 vector = this.ragdoll.partHead.transform.position + b;
        this.camera.nearClipPlane = nearClipPlane;
        this.camera.fieldOfView = num4;

        float num5 = (!this.ignoreWalls) ? this.CompensateForWallsNearPlane(vector, quaternion, num2, num3) : num2;

        this.wallHoldTime -= Time.deltaTime;
        if (this.wallHoldTime < 0f)
        {
            this.wallHold -= this.wallHoldTime / 20f;
        }
        if (num5 < num2 && num5 < this.wallHold)
        {
            this.wallHold = num5;
            this.wallHoldTime = 0.5f;
        }
        if (this.wallHold < num5)
        {
            num5 = this.wallHold;
        }
        if (this.offset <= 0f)
        {
            this.offset = num5;
        }
        float num6;
        if (num5 < this.offset)
        {
            if (num5 < num2)
            {
                num6 = 1f;
            }
            else
            {
                num6 = 0.5f;
            }
        }
        else
        {
            num6 = 0.25f;
        }
        this.offset = Mathf.Lerp(this.offset, num5, num6 * 5f * Time.deltaTime);
        this.offset = Mathf.MoveTowards(this.offset, num5, num6 * 30f * Time.deltaTime);
        this.ApplyCameraOffset();
        this.ApplyCamera(vector - a * this.offset, quaternion, num4);
    }

    public void StartProbCameraFar()
    {
        StartCoroutine(WaitFlyProbSeconds(3.0f));
    }
    IEnumerator WaitFlyProbSeconds(float seconds)
    {
        this.ignoreWalls = true;
        isOnceSofar = true;
        yield return new WaitForSeconds(seconds);
        this.mode = CameraMode.Far;
        isOnceSofar = false;
        this.ignoreWalls = false;
    }
    public void ApplyCameraOffset()
    {
        if (this.cameraCenter != Vector2.zero)
        {
            this.OffsetCenter(this.cameraCenter);
        }
        else
        {
            this.ResetCenterOffset();
        }
    }

    private void CalculateFarCam(out Vector3 targetOffset, out float yaw, out float pitch, out float camDist, out float minDist, out float fov, out float nearClip)
    {
        fov = 70f;
        nearClip = this.nearClip;
        yaw = this.human.cameraYawAngle;
        pitch = this.human.cameraPitchAngle * this.pitchRange / 80f + 15f;
        float num = Mathf.Sign(this.human.cameraPitchAngle);
        if (num != this.oldPitchSign)
        {
            this.pitchRange = 30f;
            this.pitchExpandTimer = 0f;
        }
        else if (this.move.movementSpeed < 0.5f && (this.human.verticalInputAngle > 70f || this.human.verticalInputAngle < -70f))
        {
            this.pitchExpandTimer += Time.fixedDeltaTime;
            this.pitchRange = Mathf.MoveTowards(this.pitchRange, 60f, 30f * Time.fixedDeltaTime * (0.4f + 0.6f * Mathf.Clamp01(this.pitchExpandTimer)));
        }
        else
        {
            float num2 = this.pitchRange;
            this.pitchRange = Mathf.MoveTowards(this.pitchRange, 30f, 30f * Time.fixedDeltaTime);
            if (num2 <= this.pitchRange)
            {
                this.pitchExpandTimer = 0f;
            }
        }
        this.oldPitchSign = num;
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        targetOffset = rotation * this.farTargetOffset;
        camDist = this.farRange;
        minDist = 0.25f;
    }
    private void CalculateSoFarCam(out Vector3 targetOffset, out float yaw, out float pitch, out float camDist, out float minDist, out float fov, out float nearClip)
    {
        fov = 70f;
        nearClip = this.nearClip;
        yaw = this.human.cameraYawAngle;
        pitch = this.human.cameraPitchAngle * this.pitchRange / 80f + 15f;
        float num = Mathf.Sign(this.human.cameraPitchAngle);
        if (num != this.oldPitchSign)
        {
            this.pitchRange = 30f;
            this.pitchExpandTimer = 0f;
        }
        else if (this.move.movementSpeed < 0.5f && (this.human.verticalInputAngle > 70f || this.human.verticalInputAngle < -70f))
        {
            this.pitchExpandTimer += Time.fixedDeltaTime;
            this.pitchRange = Mathf.MoveTowards(this.pitchRange, 60f, 30f * Time.fixedDeltaTime * (0.4f + 0.6f * Mathf.Clamp01(this.pitchExpandTimer)));
        }
        else
        {
            float num2 = this.pitchRange;
            this.pitchRange = Mathf.MoveTowards(this.pitchRange, 30f, 30f * Time.fixedDeltaTime);
            if (num2 <= this.pitchRange)
            {
                this.pitchExpandTimer = 0f;
            }
        }
        this.oldPitchSign = num;
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        targetOffset = rotation * this.farTargetOffset;
        camDist = this.soFarRange;
        minDist = 0.25f;
    }
    private void CalculateCloseCam(out Vector3 targetOffset, out float yaw, out float pitch, out float camDist, out float minDist, out float fov, out float nearClip)
    {
        fov = 70f;
        nearClip = this.nearClip;
        yaw = this.human.cameraYawAngle;
        pitch = this.human.cameraPitchAngle + 10f;
        if (pitch < 0f)
        {
            pitch *= 0.8f;
        }
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        targetOffset = rotation * this.closeTargetOffset;
        Vector3 vector = Quaternion.Euler(this.human.cameraPitchAngle, this.human.cameraYawAngle, 0f) * Vector3.forward;
        camDist = Mathf.Lerp(this.closeRange, this.closeRangeLookUp, vector.y);
        minDist = 0.25f;
    }

    private void CalculateFirstPersonCam(out Vector3 targetOffset, out float yaw, out float pitch, out float camDist, out float minDist, out float fov, out float nearClip)
    {
        fov = 70f;
        nearClip = this.nearClip;
        yaw = this.human.cameraYawAngle;
        pitch = this.human.cameraPitchAngle + 10f;
        if (pitch < 0f)
        {
            pitch *= 0.8f;
        }
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        targetOffset = rotation * this.fpsTargetOffset;
        camDist = 0f;
        minDist = 0f;
    }

    private float CompensateForWallsNearPlane(Vector3 targetPos, Quaternion lookRot, float desiredDist, float minDist)
    {
        Vector3 a = lookRot * Vector3.forward;
        float nearClipPlane = this.camera.nearClipPlane;
        Vector3 position = this.camera.transform.position;
        Quaternion rotation = this.camera.transform.rotation;
        this.camera.transform.rotation = lookRot;
        this.camera.transform.position = targetPos - a * (this.camera.nearClipPlane + minDist);
        this.rayStarts[0] = this.camera.ViewportToWorldPoint(new Vector3(0f, 0f, nearClipPlane));
        this.rayStarts[1] = this.camera.ViewportToWorldPoint(new Vector3(0f, 1f, nearClipPlane));
        this.rayStarts[2] = this.camera.ViewportToWorldPoint(new Vector3(1f, 0f, nearClipPlane));
        this.rayStarts[3] = this.camera.ViewportToWorldPoint(new Vector3(1f, 1f, nearClipPlane));
        float num = desiredDist - nearClipPlane;
        for (int i = 0; i < this.rayStarts.Length; i++)
        {
            Ray ray = new Ray(this.rayStarts[i], -a);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, num, this.wallLayers) && raycastHit.distance < num)
            {
                num = raycastHit.distance;
            }
        }
        num += nearClipPlane + minDist;
        if (num < minDist * 2f)
        {
            num = minDist * 2f;
        }
        return num;
    }

    private void OffsetCenter(Vector2 center)
    {
        this.camera.ResetProjectionMatrix();
        Matrix4x4 projectionMatrix = this.camera.projectionMatrix;
        projectionMatrix[0, 2] = center.x;
        projectionMatrix[1, 2] = center.y;
        this.camera.projectionMatrix = projectionMatrix;
    }

    public void ResetCenterOffset()
    {
        this.camera.ResetProjectionMatrix();
    }

    public void TransitionFromCurrent(float duration)
    {
        if (duration == 0f)
        {
            throw new ArgumentException("duration can't be 0", "duration");
        }
        this.cameraTransitionPhase = 0f;
        this.cameraTransitionSpeed = 1f / duration;
        this.startFov = this.camera.fieldOfView;
        this.startOffset = this.camera.transform.position - this.human.transform.position;
        this.startRotation = this.camera.transform.rotation;
        this.startHumanCameraPos = this.camera.WorldToViewportPoint(this.human.transform.position);
    }

    public void ApplyCamera(Vector3 position, Quaternion rotation, float fov)
    {
        this.cameraTransitionPhase += this.cameraTransitionSpeed * Time.deltaTime;
        if (this.cameraTransitionPhase >= 1f)
        {
            base.transform.position = position;
            base.transform.rotation = rotation;
            this.camera.fieldOfView = fov;
        }
        else
        {
            float num = Ease.easeInOutSine(0f, 1f, Mathf.Clamp01(this.cameraTransitionPhase));
            base.transform.rotation = rotation;
            base.transform.position = position;
            this.camera.fieldOfView = fov;
            Vector3 b = this.camera.WorldToViewportPoint(this.human.transform.position);
            Vector3 position2 = Vector3.Lerp(this.startHumanCameraPos, b, this.cameraTransitionPhase);
            base.transform.rotation = Quaternion.Lerp(this.startRotation, rotation, this.cameraTransitionPhase);
            this.camera.fieldOfView = Mathf.Lerp(this.startFov, fov, this.cameraTransitionPhase);
            Vector3 b2 = this.camera.ViewportToWorldPoint(position2);
            base.transform.position += this.human.transform.position - b2;
        }
    }
}
