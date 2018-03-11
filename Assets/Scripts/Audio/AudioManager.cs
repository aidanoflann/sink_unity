using UnityEngine.Audio;
using System;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0f, 3f)]
    public float pitch;

    public AudioSource source;

    public bool loop;
}

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

	void Awake () {
        DontDestroyOnLoad(this.gameObject);

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
	
	public void Play(string soundName)
    {
        Sound s = Array.Find<Sound>(this.sounds, sound => sound.name == soundName);
        s.source.Play();
    }
}
