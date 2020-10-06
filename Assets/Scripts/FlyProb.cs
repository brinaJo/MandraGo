using UnityEngine;
using System.Collections;


public class FlyProb : MonoBehaviour
{
    public enum ProbState
    {
        Fly,
        NotFly
    }
    private Vector3 gravity;

    public Vector3 prevPos;

    public ProbState state;

    public float collforce = 200f;

    public float speedMultiply = 1f;

    public Transform target1;
    public Transform target2;
    private GameObject target11;
    private GameObject target22;

    public Vector3 targetPos;
    private Vector3 StartPos;
    private Vector3 EndPos;

    private float vx;       //x축 방향 속도
    private float vy;       //y축 방향 속도
    private float vz;       //z축 방향 속도
    private float g = 9.8f;        //y축 방향의 중력 가속도

    private float dat;              //도착점 도달 시간
    private float mh;               //최고점 - 시작점(높이)
    private float dh;               //도착점 높이

    private float t = 0.1f;         //진행시간
    public float Max_Y = 4.0f;     //최고점 노ㅓㅍ이
    private float mht = 1.0f;       //최고점 도달 시간

    public bool isFly = true;
    private bool isNotPoni1 = false;
    private bool isNotPoni2 = false;

    public int whoOwn = -1;
    private Mandra mandra;


    // Use this for initialization
    void Start()
    {
        target11 = GameObject.Find("Player 0");
        target22 = GameObject.Find("Player 1");
        //Init(targetPos, Max_Y);
        gravity = new Vector3(0.0f, 9.8f, 0.0f);
        prevPos = transform.position;
        this.state = ProbState.NotFly;
        whoOwn = -1;

    }

    public void Init(Vector3 targetPos, float Max_Y, Vector3 Pos)
    {
        this.targetPos = targetPos;
        this.Max_Y = Max_Y;
        StartPos = Pos;

        dh = targetPos.y - StartPos.y;
        mh = Max_Y - StartPos.y;
        //g = 2 * mh / (mht * mht);
        vy = Mathf.Sqrt(2 * g * mh);

        float a = g;
        float b = -2 * vy;
        float c = 2 * dh;

        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        vx = -(StartPos.x - targetPos.x) / dat;
        vz = -(StartPos.z - targetPos.z) / dat;

        this.t = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        target1 = target11.transform;
        target2 = target22.transform;
        this.t += Time.deltaTime * speedMultiply;

        if (this.transform.position.y < -4)
        {
            Destroy(this.gameObject);
        }
        if (!isFly)
        {
            var _x = StartPos.x + vx * t;
            var _y = StartPos.y + vy * t - 0.5f * g * t * t;
            var _z = StartPos.z + vz * t;

            var npos = new Vector3(_x, _y, _z);

            if (_y < -1f)
            {
                resetFruit();
                return;
            }
            this.transform.position = npos;
        }
        

    }
    void resetFruit()
    {
        gravity = new Vector3(0.0f, 9.8f, 0.0f);
        prevPos = transform.position;
        this.state = ProbState.NotFly;
        whoOwn = -1;
        StartPos = new Vector3(0, 0, 0);
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

        int ran = Random.Range(0, 100);
        if (col.gameObject.tag == "Ponitail1" && pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack") 
            && this.state != ProbState.Fly)
        {
            whoOwn = 0;
            if (this.gameObject.tag == "chess_prob" || this.gameObject.tag == "night_piece")
                SoundPool.Instance.SetSound(SoundPool.Instance.ChessProbPool, ref SoundPool.Instance.indexChess, col.gameObject.transform);
            else
                SoundPool.Instance.SetSound(SoundPool.Instance.characObjPool, ref SoundPool.Instance.indexCharacObj, col.gameObject.transform);
            if (this.gameObject.tag == "tutorial_prob")
            {
                pp.GetComponent<Mandra>().tutorial.canAttackComplete = true;
            }
            if (this.gameObject.tag == "flask")
            {
                this.gameObject.GetComponentInParent<FlaskColl>().isFlaskBroken = true;
            }
            if (ran < 80)
            {
                targetPos = target2.position;

                var _root2 = target11.gameObject.transform;
                var pp2 = _root2.GetComponentInChildren<CameraController3>();
                while (_root != null)
                {
                    pp2 = _root2.GetComponentInChildren<CameraController3>();

                    if (pp2 != null)
                        break;
                    _root2 = _root2.parent;
                }

                if (!isNotPoni2)
                {
                    pp2.GetComponent<CameraController3>().mode = CameraMode.SoFar;

                    pp2.GetComponent<CameraController3>().StartProbCameraFar();
                    Init(targetPos, Max_Y, this.transform.position);
                }
                isFly = false;

                this.state = ProbState.Fly;
                isNotPoni1 = true;
            }
            else
            {
                targetPos = target2.position;

                float rx = Random.Range(0, 5);
                float rz = Random.Range(0, 4);
                var a = new Vector3(rx, targetPos.y, rz);
                this.targetPos = a;

                var _root2 = target11.gameObject.transform;
                var pp2 = _root2.GetComponentInChildren<CameraController3>();
                while (_root != null)
                {
                    pp2 = _root2.GetComponentInChildren<CameraController3>();

                    if (pp2 != null)
                        break;
                    _root2 = _root2.parent;
                }

                if (!isNotPoni2)
                {
                    pp2.GetComponent<CameraController3>().mode = CameraMode.SoFar;

                    pp2.GetComponent<CameraController3>().StartProbCameraFar();
                    Init(targetPos, Max_Y, this.transform.position);
                }
                isFly = false;

                this.state = ProbState.Fly;
                isNotPoni1 = true;

            }
        }

        else if (col.gameObject.tag == "Ponitail2" && pp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.LeafAttack")
            && this.state != ProbState.Fly)
        {
            whoOwn = 1;
            if (this.gameObject.tag == "chess_prob" || this.gameObject.tag == "night_piece")
                SoundPool.Instance.SetSound(SoundPool.Instance.ChessProbPool, ref SoundPool.Instance.indexChess, col.gameObject.transform);
            else
                SoundPool.Instance.SetSound(SoundPool.Instance.characObjPool, ref SoundPool.Instance.indexCharacObj, col.gameObject.transform);
            if (this.gameObject.tag == "tutorial_prob")
            {
                pp.GetComponent<Mandra>().tutorial.canAttackComplete = true;
            }
            if (this.gameObject.tag == "flask")
            {
                this.gameObject.GetComponentInParent<FlaskColl>().isFlaskBroken = true;
            }
            if (ran < 80)
            {
                targetPos = target1.position;

                var _root2 = target22.gameObject.transform;
                var pp2 = _root2.GetComponentInChildren<CameraController3>();
                while (_root != null)
                {
                    pp2 = _root2.GetComponentInChildren<CameraController3>();

                    if (pp2 != null)
                        break;
                    _root2 = _root2.parent;
                }

                if (!isNotPoni1)
                {
                    pp2.GetComponent<CameraController3>().mode = CameraMode.SoFar;

                    pp2.GetComponent<CameraController3>().StartProbCameraFar();
                    Init(targetPos, Max_Y, this.transform.position);
                    Debug.Log("SoFar");
                }
                isFly = false;

                this.state = ProbState.Fly;
                isNotPoni2 = true;
            }
            else
            {
                targetPos = target1.position;

                float rx = Random.Range(0, 5);
                float rz = Random.Range(0, 4);
                var a = new Vector3(rx, targetPos.y, rz);
                this.targetPos = a;

                var _root2 = target22.gameObject.transform;
                var pp2 = _root2.GetComponentInChildren<CameraController3>();
                while (_root != null)
                {
                    pp2 = _root2.GetComponentInChildren<CameraController3>();

                    if (pp2 != null)
                        break;
                    _root2 = _root2.parent;
                }

                if (!isNotPoni2)
                {
                    pp2.GetComponent<CameraController3>().mode = CameraMode.SoFar;

                    pp2.GetComponent<CameraController3>().StartProbCameraFar();
                    Init(targetPos, Max_Y, this.transform.position);
                    Debug.Log("SoFar");
                }
                isFly = false;

                this.state = ProbState.Fly;
                isNotPoni1 = true;
            }

        }

        if (col.gameObject.tag == "ground")
        {
            if(this.gameObject.tag == "king")
            {
                this.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
            }
            if(this.gameObject.tag == "queen")
            {
                this.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
            }
            if(this.gameObject.tag == "treasureBox")
            {
                if(this.gameObject.GetComponentInParent<Treasure>().open == true && this.gameObject.GetComponentInParent<Treasure>().damageDelay < 0f)
                    this.gameObject.GetComponentInParent<Treasure>().CoinEnable();
            }
            if (this.gameObject.tag == "flask" && this.gameObject.GetComponentInParent<FlaskColl>().isFlaskBroken == true)
            {
                Transform pos = this.gameObject.transform;
                this.gameObject.GetComponentInParent<FlaskColl>().FlaskBrokenInit(pos);
                SoundPool.Instance.SetSound(SoundPool.Instance.FlaskBrokePool, ref SoundPool.Instance.indexFlask, this.gameObject.transform);
                Destroy(this.gameObject);
            }
            whoOwn = -1;
            isFly = true;
            this.state = ProbState.NotFly;
            isNotPoni1 = false;
            isNotPoni2 = false;
        }
    }

}
