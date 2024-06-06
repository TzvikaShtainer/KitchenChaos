using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    
    private List<KitchenObjectSO> _kitchenObjectSoList;

    private void Awake()
    {
        _kitchenObjectSoList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo )
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSo))
            return false;
        
        if (_kitchenObjectSoList.Contains(kitchenObjectSo))
            return false;

        _kitchenObjectSoList.Add(kitchenObjectSo);
        return true;
    }
}
