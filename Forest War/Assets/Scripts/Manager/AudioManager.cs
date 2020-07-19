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

    private void PlaySound(AudioSource audioSource, string audioClip, float volume, bool loop = false)
    {
        audioSource.clip = Resources.Load<AudioClip>(Path_Prefix + audioClip);
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayBgSound(string soundName)
    {
        PlaySound(bgAudioSource, soundName, 0.5f, true);
    }

    public void PlayComSound(string soundName)
    {
        PlaySound(comAudioSource, soundName, 1f);
    }
}
