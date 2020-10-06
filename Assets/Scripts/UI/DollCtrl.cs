using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DollCtrl : MonoBehaviour
{

    private enum State { Road1, Road2 };
    private State state;
    private NavMeshAgent navemesh;
    public GameObject Dolls;
    public GameObject Doll;
    public Transform Start1;
    public Transform Start2;
    public Transform Destination1;
    public Transform Destination2;

    // Use this for initialization
    void Start()
    {
        navemesh = GetComponent<NavMeshAgent>();
        state = State.Road1;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Road1:
                navemesh.destination = Destination1.transform.position;
                break;
            case State.Road2:
                navemesh.destination = Destination2.transform.position;
                break;
        }

        Doll.transform.Rotate(0f, 0f, 250f * Time.deltaTime);
        Doll.transform.position = transform.position + Vector3.up * 0.7f;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Destination1")
        {
            StartCoroutine("Wait1");

        }

        if (other.gameObject.name == "Destination2")
        {
            StartCoroutine("Wait2");
        }
    }

    IEnumerator Wait1()//8.16
    {
        yield return new WaitForSeconds(2f);
        navemesh.Stop();
        Dolls.SetActive(false);
        Dolls.transform.position = Start2.transform.position;
        state = State.Road2;
        Dolls.SetActive(true);
        StopCoroutine("Wait1");
        navemesh.Resume();
    }

    IEnumerator Wait2()//8.16
    {
        yield return new WaitForSeconds(2f);
        navemesh.Stop();
        Dolls.SetActive(false);
        Dolls.transform.position = Start1.transform.position;
        state = State.Road1;
        Dolls.SetActive(true);
        StopCoroutine("Wait2");
        navemesh.Resume();
    }
}
