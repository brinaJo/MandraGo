using UnityEngine;
using System.Collections;
using RootMotion.Dynamics;


public class Respawn : MonoBehaviour
{
    public enum Players { Player1, Player2 }//5.22
    public Players player;//5.22
    public int MapNum;
    public GameObject Player;
    public BehaviourPuppet puppet;
    public PuppetMaster master;
    public string idleAnimation;
    private UIctrl uictrl;//5.22    
    public bool isRespawn;//5.22
    private Score score;//6.9
    public Ragdoll ragdoll;
    public bool isOnce;

    public GameObject[] RespawnPos;
    void Start()
    {
        uictrl = GameObject.Find("UIController").GetComponent<UIctrl>();
        score = FindObjectOfType<Score>();//6.9

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.y < -3.5f && MapNum == 1)
        {
            if (!isOnce)
            {
                DeadPlayer(0.3f);
                isRespawn = true;//5.22

            }
        }
        else if (Player.transform.position.y < -20.0f && MapNum == 2)
        {
            DeadPlayer(0.0f);
            isRespawn = true;//5.22

        }
        else if (Player.transform.position.y < -0.5f || puppet.puppetMaster.state == PuppetMaster.State.Dead)
        {
            puppet.puppetMaster.state = PuppetMaster.State.Dead;
            Player.GetComponent<Mandra>().state = MandraState.Dead;
            Player.GetComponent<Mandra>().SpawnAt(this.transform, Vector3.zero);


        }
      
        if(puppet.puppetMaster.state == PuppetMaster.State.Dead)
        {
            StartCoroutine(DeadCheck());
        }
    }

    public void DeadPlayer(float time)
    {
        if (isRespawn)
        {
            if(MapNum!=1)
                ragdoll.AddKinemetic();
            isOnce = true;
            StartCoroutine(ReSpawnPos(time));
        }
    }

    IEnumerator DeadCheck()
    {
        yield return new WaitForSeconds(5.0f);
        ReSpawnPos(0.0f);
    }
    IEnumerator ReSpawnPos(float time)
    {
        yield return new WaitForSeconds(time);
        if (isOnce)
        {
            if (MapNum == 1)
                ragdoll.AddKinemetic();
            int num = Random.Range(0, RespawnPos.Length);
            ragdoll.ResetGravitySegments();
            puppet.puppetMaster.state = PuppetMaster.State.Alive;
            Player.GetComponent<CapsuleCollider>().enabled = true;
            Player.GetComponent<Animator>().SetFloat("Jump", 1.3f);
            Player.GetComponent<Animator>().SetTrigger("goIdle");
            Player.GetComponent<Mandra>().state = MandraState.Idle;
            Player.GetComponent<Movement>().movementSpeed = 1f;
            puppet.Reset(RespawnPos[num].transform.position, this.transform.rotation);

            if (isRespawn)//5.22
            {
                ScoreGet();//5.22

                isRespawn = false;
                if(MapNum == 1)
                    isOnce = false;
            }
        }
    }

    public void ScoreGet()//5.22
    {
        switch (player)
        {
            case Players.Player1:
                if (score.Player_2_Kill < 15f && !uictrl.isRestart)
                    score.Player_2_Kill++;
                isRespawn = false;
                break;

            case Players.Player2:
                if (score.Player_1_Kill < 15f && !uictrl.isRestart)
                    score.Player_1_Kill++;
                isRespawn = false;
                break;
        }
    }
}
