using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    private ClearCounter _clearCounter;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSo;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (_clearCounter != null)
        {
            _clearCounter.ClearKitchenObject();
        }
        
        _clearCounter = clearCounter;

        if (_clearCounter.HasKitchenObject())
        {
            Debug.Log("counter already has a kitchen object");
        }
        
        _clearCounter.SetKitchenObject(this);
        
        transform.parent = _clearCounter.GetKitchenObjectFollowTransform();
        transform.position = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}
