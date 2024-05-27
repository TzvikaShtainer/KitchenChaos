using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSo;
    }
}
