using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    #region Singleton
    private static AudioManager instance = null;
    public static AudioManager Instance {
        get { return instance; }
    }
    #endregion

    public enum SoundType
    {
        SoundEffect,
        Music,
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public SoundType soundType;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
        [HideInInspector]
        public AudioSource source;
    }
    public AudioMixer audioMixer;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup soundEffectsGroup;
    public Sound[] sounds;
    private Sound currentMusicClip;

    private void Awake()
    {
        #region instanceCode
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        #endregion

        //Asign a type of audiosource to each sound
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.soundType == SoundType.Music)
                s.source.outputAudioMixerGroup = musicGroup;
            else
                s.source.outputAudioMixerGroup = soundEffectsGroup;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }
        if(s.soundType == SoundType.Music)  // See if the sound is a music clip
        {
            if (currentMusicClip != null && currentMusicClip.source.isPlaying)   // If there was a previous music clip playing and stop it if true
                Stop(currentMusicClip.name);
            currentMusicClip = s;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }
        s.source.Stop();
    }

    public void StopSound()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }

    #region AudioSettingsFunctions
    /// <summary>
    /// Sets the game music volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetGameMusicVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", volume);
        else
            Debug.LogError("Missing audio mixer in Game Manager");
    }

    /// <summary>
    /// Sets the game sound effects volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetGameSoundEffectsVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SoundEffectsVolume", volume);
        else
            Debug.LogError("Missing audio mixer in Game Manager");
    }
    #endregion
}
