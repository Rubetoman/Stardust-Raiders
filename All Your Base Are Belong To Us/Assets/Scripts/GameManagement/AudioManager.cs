using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    #region Singleton
    private static AudioManager _instance = null;
    public static AudioManager Instance {
        get {
            if (_instance == null)
            {
                Resources.Load("Prefab/GameManagement/AudioManager");
            }
            return _instance; }
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
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
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

    public void StopEverySound()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }

    public void StopSoundEffects()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying && s.soundType == SoundType.SoundEffect)
                s.source.Stop();
        }
    }

    public void StopMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying && s.soundType == SoundType.Music)
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

    #region AudioSourceFunctions

    /// <summary>
    /// Funtion to change a the AudioClip of a existing AudioSource. Yo can make the new one to loop and decide if wait for the previous song to end before changing.
    /// </summary>
    /// <param name="source"> The existing AudioSource. </param>
    /// <param name="newClip"> The new AudioClip to play. </param>
    /// <param name="loop"> If the new AudioClip will loop. </param>
    /// <param name="wait"> If wait for the AudioClip currently on the AudioSource to end before playing the new song. </param>
    /// <returns></returns>
    public IEnumerator ChangeAudioSourceClip(AudioSource source, AudioClip newClip, bool loop, bool wait)
    {
        if (wait)
            yield return new WaitForSeconds(source.clip.length - source.time);
        source.clip = newClip;
        source.loop = loop;
        if (!source.isPlaying)
            source.Play();
    }

    public bool IsClipPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return false;
        }
        if (s.source.isPlaying)
            return true;
        else
            return false;
    }
    #endregion
}
