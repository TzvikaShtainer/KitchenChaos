using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    
    private Player _player;
    private float footStepsTimer;
    private float footStepsTimerMax = 0.1f;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        footStepsTimer -= Time.deltaTime;

        if (footStepsTimer <= 0)
        {
            footStepsTimer = footStepsTimerMax;

            if (_player.IsWalking())
            {
                float vol = 1f;
                SoundManager.Instance.PlayFootStepsSound(transform.position, vol);
            }
        }
    }
}
