﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SesKontrol : MonoBehaviour {

    public static SesKontrol Instance { set; get; }
    public AudioSource source;
    public AudioClip[] allSounds;
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Menu");
    }
   
    public void PlaySound(int soundIndex)
    {
        AudioSource.PlayClipAtPoint(allSounds[soundIndex],transform.position);
    }
}
