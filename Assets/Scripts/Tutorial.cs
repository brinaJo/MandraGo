using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Tutorial : MonoBehaviour
{
    public bool tutorialIng;

    public GameObject[] text;
    public Mandra mandra;

    public GameObject attack_Prob;
    public GameObject tutorial_character;
    public GameObject tutorial_mold;
    public GameObject[] tutorial_pad;

    public bool canMove;
    public bool canRot;
    public bool canJump;
    public bool canAttack;
    
    public bool canMoveComplete;
    public bool canJumpComplete;
    public bool canRotComplete;
    public bool canAttackComplete;
    public bool canTutorialComplete;

    private IEnumerator coroutine;

    public bool isMoveOnce;
    public bool isJumpOnce;
    public bool isRotOnce;
    public bool isAttackOnce;

    void Start()
    {
        text[0].SetActive(true);
        StartCoroutine(TextView(5.0f, 1));
    }

    void Update()
    {
        if (!isMoveOnce)
        {
            if (canMoveComplete && text[1] == null)
            {
                StartCoroutine(TextView(0.5f, 2));
                StopCoroutine(coroutine);
                tutorial_pad[0].SetActive(true);
                tutorial_pad[1].SetActive(false);
                isMoveOnce = true;
            }
        }
        if (!isJumpOnce)
        {
            if (canJumpComplete && text[2] == null)
            {
                StopCoroutine(coroutine);
                tutorial_pad[0].SetActive(true);
                tutorial_pad[2].SetActive(false);
                StartCoroutine(TextView(0.0f, 3));
                StartCoroutine(TextView(2.3f, 4));
                isJumpOnce = true;
            }
        }
        if (!isRotOnce)
        {
            if (canRotComplete && text[4] == null)
            {
                StopCoroutine(coroutine);
                tutorial_pad[0].SetActive(true);
                tutorial_pad[3].SetActive(false);
                StartCoroutine(TextView(0.0f, 5));
                StartCoroutine(TextView(2.3f, 6));
                isRotOnce = true;
            }
        }
        if (!isAttackOnce)
        {
            if (canAttackComplete && text[6] == null)
            {
                StopCoroutine(coroutine);
                tutorial_pad[0].SetActive(true);
                tutorial_pad[4].SetActive(false);
                StartCoroutine(TextView(0.0f, 7));
                StartCoroutine(TextView(2.3f, 8));
                StartCoroutine(TextView(7.5f, 9));
                StartCoroutine(TextView(13.5f, 10));
                StartCoroutine(TextView(17.0f, 11));
                isAttackOnce = true;
            }
        }
        if(text[11] == null)
        {

            canTutorialComplete = true;
        }
    }
    IEnumerator BlingBling(int a,bool returnnum)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            tutorial_pad[a].SetActive(true);
            tutorial_pad[0].SetActive(false);
            yield return new WaitForSeconds(0.1f);
            tutorial_pad[a].SetActive(false);
            tutorial_pad[0].SetActive(true);
        }

    }
    IEnumerator TextView(float seconds,int i)
    {
        yield return new WaitForSeconds(seconds);
        text[i].SetActive(true);
        yield return new WaitForSeconds(0.8f);
        if (i == 1)
        {
            canMove = true;
            coroutine = BlingBling(1,false);
            StartCoroutine(coroutine);
        }
        if (i == 2)
        {
            canJump = true;
            coroutine = BlingBling(2, false);
            StartCoroutine(coroutine);
        }
        if (i == 4)
        {
            canRot = true;
            coroutine = BlingBling(3, false);
            StartCoroutine(coroutine);
        }
        if (i == 6)
        {
            canAttack = true;
            attack_Prob.SetActive(true);
            coroutine = BlingBling(4, false);
            StartCoroutine(coroutine);
        }
        if(i == 10)
        {
            tutorial_mold.GetComponent<Image>().DOFade(0, 0.4f);
            tutorial_character.GetComponent<Image>().DOFade(0, 0.4f);
            tutorial_pad[0].GetComponent<Image>().DOFade(0, 0.4f);
        }
    }
}
