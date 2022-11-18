using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float sfxVolumn = 1.0f;
    public bool isSfxMute = false; //음소거 할지 안할지
    public static SoundManager soundmanager;
    //싱글턴
    private void Awake()
    {
        if (soundmanager == null)
            soundmanager = this;
        else if (soundmanager != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (isSfxMute) return;
        GameObject soundObj = new GameObject("background_Sfx");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 80f;
        audioSource.volume = sfxVolumn;
        audioSource.Play();

        Destroy(soundObj, sfx.length);
    }
    public void PlaySfx(Vector3 pos, AudioClip sfx, bool loop)
    {
        if (isSfxMute) return;
        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 80f;
        audioSource.volume = sfxVolumn;
        audioSource.loop = loop;
        audioSource.Play();
    }
}