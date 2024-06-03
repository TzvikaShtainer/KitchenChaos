using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual1 : MonoBehaviour
{
    private const string CUT = "Cut";
    
    [SerializeField] private CuttingCounter cuttingCounter;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, EventArgs e)
    {
        _animator.SetTrigger(CUT);
    }
}
