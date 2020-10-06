using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    public static SoundPool Instance { get; set; }

    public int indexCharacObj;
    public int indexCharacCharac;
    public int indexCharacDamage;
    public int indexBanana;
    public int indexIceWind;
    public int indexJumppad;
    public int indexTresure;
    public int indexHack;
    public int indexStone;
    public int indexBalack;
    public int indexDestroyStair;

    public int indexOak;
    public int indexWhaleWater;
    public int indexFlask;
    public int indexNotview;
    public int indexChess;


    public float sfxVolumn = 1.0f;
    public bool isSfxMute = false;

    public GameObject Music;

    public AudioClip charac_Obj;
    public AudioClip obj_damage;
    public AudioClip charac_damage;

    public AudioClip banana_slide;
    public AudioClip ice_wind;
    public AudioClip jumpPad;

    public AudioClip treasureOpen;
    public AudioClip oak_damage;
    public AudioClip whaleWater;

    public AudioClip flaskBroken;
    public AudioClip hackPunch;
    public AudioClip notView;
    public AudioClip stoneExplo;
    public AudioClip blackHoll;

    public AudioClip chessProb;
    public AudioClip destroyStairs;

    public GameObject[] characObjPool;
    public GameObject[] objDamagePool;
    public GameObject[] characDamagePool;
    public GameObject[] BananaPool;
    public GameObject[] IceWindPool;
    public GameObject[] JumppadPool;
    public GameObject[] TresurePool;
    public GameObject[] HackPool;
    public GameObject[] StoneExPool;
    public GameObject[] BalackPool;
    public GameObject[] DestroyStairPool;

    public GameObject[] OakDamPool;
    public GameObject[] WhaleWaterPool;
    public GameObject[] FlaskBrokePool;
    public GameObject[] NotViewPool;

    public GameObject[] ChessProbPool;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
	void Start ()
    {
        characObjPool = SoundInit(Music, 10, this.transform,charac_Obj);
        objDamagePool = SoundInit(Music, 5, this.transform, obj_damage);
        characDamagePool = SoundInit(Music, 5, this.transform, charac_damage);
        BananaPool = SoundInit(Music, 3, this.transform, banana_slide);
        IceWindPool = SoundInit(Music, 3, this.transform, ice_wind);
        JumppadPool = SoundInit(Music, 3, this.transform, jumpPad);
        TresurePool = SoundInit(Music, 3, this.transform, treasureOpen);
        HackPool = SoundInit(Music, 3, this.transform, hackPunch);
        StoneExPool = SoundInit(Music, 3, this.transform, stoneExplo);
        BalackPool = SoundInit(Music, 3, this.transform, blackHoll);
        DestroyStairPool =SoundInit(Music, 3, this.transform, destroyStairs);
        

        OakDamPool = SoundInit(Music, 3, this.transform, oak_damage);
        WhaleWaterPool = SoundInit(Music, 1, this.transform, whaleWater);
        FlaskBrokePool = SoundInit(Music, 5, this.transform, flaskBroken);
        NotViewPool = SoundInit(Music, 3, this.transform, notView);

        ChessProbPool = SoundInit(Music, 10, this.transform, chessProb);
    }

    private GameObject[] SoundInit(GameObject obj, int cacheSize, Transform pos, AudioClip sfx)
    {
        GameObject[] go = new GameObject[cacheSize];
        for (int i = 0; i < cacheSize; ++i)
        {
            go[i] = Instantiate(obj, pos.position, pos.rotation);
            go[i].SetActive(false);

            go[i].transform.position = transform.position;
            go[i].transform.parent = this.transform;
            AudioSource audioSource = go[i].AddComponent<AudioSource>();
            audioSource.clip = sfx;
            audioSource.minDistance = 10.0f;
            audioSource.maxDistance = 30.0f;
            audioSource.volume = sfxVolumn;
        }
        return go;
    }

    public void SetSound(GameObject[] Obj, ref int index, Transform pos)
    {
        Obj[index].transform.position = pos.position;
        Obj[index].transform.rotation = pos.rotation;
        Obj[index].SetActive(true);
        Obj[index].GetComponent<AudioSource>().Play();
        StartCoroutine(DisableSound(Obj[index], Obj[index].GetComponent<AudioSource>().clip.length));
        ++index;
        index %= Obj.Length;

    }
    public void DisableSoundEffect(GameObject[] Obj, ref int index)
    {
        StartCoroutine(DisableSound(Obj[index], 0));
    }
    IEnumerator DisableSound(GameObject Obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Obj.SetActive(false);
    }

    void Update ()
    {
		
	}
}
