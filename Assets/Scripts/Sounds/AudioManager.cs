using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        bool play = true;
        foreach (Sound prev in sounds)
        {
            if (prev.music)
            {
                if (prev.source.isPlaying)
                {
                    if (prev.name != name)
                    {
                        Stop(prev.name);
                        play = true;
                        break;
                    }
                    else
                    {
                        play = false;
                        break;
                    }
                }
                else
                {
                    play = true;
                }
          
            }
        }
        if (play)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound not found");
                return;
            }
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }
        s.source.Stop();
    }

    public void musicVolumeChange(float newValue)
    {
        foreach(Sound s in sounds)
        {
            if(s.music)
            {
                s.source.volume = newValue;
            }     
        }
    }

    public void effectsVolumeChange(float newValue)
    {
        foreach (Sound s in sounds)
        {
            if (!s.music)
            {
                s.source.volume = newValue;
            }
        }
    }
}
