using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager {

    public AudioManager(GameFacade facade) : base(facade) { }

    private const string Path_Prefix = "Sounds/";
    public const string alertSound = "Alert";
    public const string arrowShootSound = "ArrowShoot";
    public const string bgFastSound = "Bg(fast)";
    public const string bgModerateSound = "Bg(moderate)";
    public const string buttonClickSound = "ButtonClick";
    public const string missSound = "Miss";
    public const string shootPersonSound = "ShootPerson";
    public const string timerSound = "Timer";

    private AudioSource bgAudioSource;
    private AudioSource comAudioSource;

    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        comAudioSource = audioSourceGO.AddComponent<AudioSource>();

        PlaySound(bgAudioSource, bgModerateSound, 0.5f, true);

    }

    /// <summary>
    /// 让指定的AudioSource播放指定声音.
    /// </summary>
    /// <param name="audioSource">指定AudioSource</param>
    /// <param name="audioClip">声音资源名</param>
    /// <param name="volume">控制声音大小</param>
    /// <param name="loop">是否循环，默认为不循环</param>
    private void PlaySound(AudioSource audioSource, string audioClip, float volume, bool loop = false)
    {
        audioSource.clip = Resources.Load<AudioClip>(Path_Prefix + audioClip);
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }

    //private AudioClip LoadSound(string soundName)
    //{
    //    return Resources.Load<AudioClip>(Path_Prefix + soundName);
    //}

    public void PlayBgSound(string soundName)
    {
        PlaySound(bgAudioSource, soundName, 0.5f, true);
    }

    public void PlayComSound(string soundName)
    {
        PlaySound(comAudioSource, soundName, 1f);
    }
}
