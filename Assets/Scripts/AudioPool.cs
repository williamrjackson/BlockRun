using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary> 
/// Play audio clips without managing AudioSources and without the clicks and pops of interruptions.
/// </summary>
public class AudioPool : MonoBehaviour
{
    [Tooltip("Available pool slots. Must accommodate expected simultaneously-playing clips.")]
    public int poolCount = 10;
    [Tooltip("Audio Mixer Group. Each pool AudioSource will be routed to this output.")]
    public AudioMixerGroup audioMixerGroup;

    private List<AudioSource> sources = new List<AudioSource>();
    private int poolIndex = 0;

    /// Singleton behavior
    public static AudioPool instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Initialize list of GameObjects w/ AudioSource components
    void Start()
    {
        for (int i = 0; i < poolCount; i++)
        {
            // Creat object
            GameObject newGO = new GameObject();
            // Name it
            newGO.name = "AudioPoolObject_" + i.ToString();
            // Child it
            newGO.transform.parent = transform;
            // Add AudioSource
            AudioSource newSource = newGO.AddComponent<AudioSource>();
            // Set output
            newSource.outputAudioMixerGroup = audioMixerGroup;
            // Add to pool
            sources.Add(newSource);
        }
    }

    // Play oneshot.
    public void PlayOneShot(AudioClip clip)
    {
        PlayPitchOneShot(clip, 1f);
    }
    // Play oneshot. Allows setting a pitch, as well as prevents interrupted playback.
    public void PlayPitchOneShot(AudioClip clip, float pitch)
    {
        // Get next available audio source
        poolIndex = (poolIndex + 1) % poolCount;
        // Set it's pitch
        sources[poolIndex].pitch = pitch;
        // Play oneshot
        sources[poolIndex].PlayOneShot(clip);
    }
    // This should allow for more "musical" pitch adjustments. Up/Down 1 octave.
    public void PlayPitchOneShot(AudioClip clip, int pitch)
    {
        float fAppliedPitchScale = 1;
        if (pitch > 0)
        {
            fAppliedPitchScale = Wrj.Utils.Remap((float)pitch, 0, 12f, 1f, 2f);
        }
        else
        {
            fAppliedPitchScale = Wrj.Utils.Remap((float)pitch, 0, -12f, 1f, .5f);
        }
        PlayPitchOneShot(clip, fAppliedPitchScale);
    }
}
