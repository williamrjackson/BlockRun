using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float targetVolume = 1f;
    [SerializeField]
    private float fadeInDuration = 3f;
    [SerializeField]
    private float fadeOutDuration = 2f;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        FadeIn();
        GameManager.Instance.OnGameOver += FadeOut;
    }

    public void FadeOut()
    {
        Wrj.Utils.MapToCurve.Linear.FadeAudio(audioSource, 0, fadeOutDuration);
    }

    public void FadeIn()
    {
        Wrj.Utils.MapToCurve.Linear.FadeAudio(audioSource, targetVolume, fadeInDuration);
    }
}
