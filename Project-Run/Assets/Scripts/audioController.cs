﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class audioController : MonoBehaviour
{
    public AudioSource audioSource;
    AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
        Debug.Log("Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
