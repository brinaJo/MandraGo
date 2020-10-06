using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCtrl : MonoBehaviour
{
    public enum Type { Block, Stair };
    public Type type;
    public bool isFall;
    private float FallTime;
    private ChessMapCtrl chessmap;
    public float DangerTime;
    private bool isDanger;

    public bool playSound;
    // Use this for initialization
    void Start()
    {
        chessmap = FindObjectOfType<ChessMapCtrl>();
        DangerTime = 5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (isFall)
        {
            if (!isDanger && type == Type.Block)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.16f, transform.position.z);
                GameObject danger = Instantiate(chessmap.Danger, pos, Quaternion.identity) as GameObject;
                danger.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                danger.transform.parent = this.transform;
                isDanger = true;
            }
            DangerTime -= 1f * Time.deltaTime;
            if (DangerTime <= 0f)
            {
                if (!playSound)
                {
                    SoundPool.Instance.SetSound(SoundPool.Instance.DestroyStairPool, ref SoundPool.Instance.indexDestroyStair, this.transform);
                    playSound = true;
                }
                FallTime += 1f * Time.deltaTime;
                if (FallTime >= 4f)
                {
                    Destroy(gameObject);
                }
                transform.Translate(0f, 0f, -2f * Time.deltaTime);
            }
        }

    }
}
