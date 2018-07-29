using UnityEngine.Audio;
using System;
using UnityEngine;
using Assets.Utils;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0f, 3f)]
    public float pitch;

    //[HideInInspector]
    public AudioSource source;

    public bool loop;
}

public class AudioManager : SingletonBehaviour
{
    public Sound[] sounds;

	new void Awake () {
        base.Awake();

        for (int i = 0; i < this.sounds.Length; i++)
        {
            Sound s = this.sounds[i];
            s.source = this.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        this.Play("BaseMusic");
    }

    public void Play(string soundName, float? pitchOverride = null)
    {
        Sound s = Array.Find<Sound>(this.sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogErrorFormat("Could not find sound with name {0}. Skipping...", soundName);
        }

        // TODO: find nicer way of overriding pitch for the duration of the sound (this wont work with stacking)
        s.source.pitch = s.pitch;
        if (pitchOverride.HasValue)
        {
            s.source.pitch = pitchOverride.Value;
        }
        s.source.Play();
    }
}
