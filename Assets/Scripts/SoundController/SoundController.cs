﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using DG.Tweening;

public class SoundController : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
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
            Debug.LogError("Unable to find a sound with the name " + name + " on the gameobject " + gameObject.name);

        else if (s.source.isPlaying)
        { s.source.Stop(); s.source.Play(); }

        else
            s.source.Play();


    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            Debug.LogError("Unable to find a sound with the name " + name + " on the gameobject " + gameObject.name);
        else
            s.source.Stop();
    }

    public void PlayVaried(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            Debug.LogError("Unable to find a sound with the name " + name + " on the gameobject " + gameObject.name);

        else if (s.source.isPlaying)
        {
            s.source.Stop();
            float rngRange = UnityEngine.Random.Range(0.5f, 1.5f);
            s.pitch = rngRange;
            s.source.Play();
        }

        else
            s.source.Play();


    }
}
