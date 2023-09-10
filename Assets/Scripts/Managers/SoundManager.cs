using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonWithMonobehaviour<SoundManager>
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private List<AudioClipInfo> _audioClips = new List<AudioClipInfo>();

    public void PlaySound(GameAudioClipsEnum clipToPlay)
    {
        AudioClipInfo clipInfo = _audioClips.Find((audioInfo) => audioInfo.clipEnum == clipToPlay);
        _audioSource.clip = clipInfo.audioClipToPlay;
        _audioSource.Play();
    }
}

[System.Serializable]
public class AudioClipInfo
{
    public GameAudioClipsEnum clipEnum;
    public AudioClip audioClipToPlay;
}
