using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Tiny.Singleton<SoundManager>
{
    public List<AudioClip> cutClips;
    public List<AudioClip> jumpClips;
    public AudioSource audioSource;

    private bool isSoundEnable = true;

    private void Start() 
    {
        UserDataManager.Instance.OnDataChanged -= OnDataChanged;
        UserDataManager.Instance.OnDataChanged += OnDataChanged;
    }

    private void OnDataChanged()
    {
        isSoundEnable = UserDataManager.Instance.UserData.soundOption;
    }

    public void PlayRandomCutClip()
    {
        if (!isSoundEnable) return;

        var index = Random.Range(0, cutClips.Count);
        audioSource.PlayOneShot(cutClips[index]);
    }

    public void PlayRandomJumpClip()
    {
        if (!isSoundEnable) return;

        var index = Random.Range(0, jumpClips.Count);
        audioSource.PlayOneShot(jumpClips[index]);
    }
}
