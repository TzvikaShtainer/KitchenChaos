using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.State == StoveCounter.State.Fried || e.State == StoveCounter.State.Frying;

        if (playSound)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}
