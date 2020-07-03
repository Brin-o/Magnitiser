using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStop : MonoBehaviour
{
    AudioSource m_song;
    void Start()
    {
        m_song = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_song.volume == 0)
            m_song.Stop();
    }
}
