using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    //Singleton pattern
    public static AudioManager instance;

    private void OnEnable()
    {
        Player.OnPlayerShoot += Play;
        Player.OnPlayerHit += Play;
    }

    private void OnDisable()
    {
        Player.OnPlayerShoot -= Play;
        Player.OnPlayerHit -= Play;
    }

    private void Awake()
    {
        //Singleton pattern
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
      
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme1");
    }

    public string Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {s} not found. Did you input the string name correctly?");
            return name;
        }
        s.source.Play();
        return name;
    }
}
