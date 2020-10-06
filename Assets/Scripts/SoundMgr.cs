using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour {

    public static SoundMgr Instance { get; set; }

    public float sfxVolumn = 1.0f;
    public bool isSfxMute = false;
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
    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (isSfxMute) return;
        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;

        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.minDistance = 10.0f;
        audioSource.maxDistance = 30.0f;
        audioSource.volume = sfxVolumn;
        audioSource.playOnAwake = false;
        audioSource.Play();
        Destroy(soundObj, sfx.length);
    }
}