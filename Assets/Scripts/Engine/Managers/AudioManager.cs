using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private static AudioManager m_Instance;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.Loop;
        }

        m_Instance = this;

        Play("MainMusic");
    }

    public static void Play (string name)
    {
        Sound s = Array.Find(m_Instance.sounds, sound => sound.name == name);
        if (s == null)
            return; 
        s.source.Play();
    }
    public static void Stop(string name)
    {
        Sound s = Array.Find(m_Instance.sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }
}
