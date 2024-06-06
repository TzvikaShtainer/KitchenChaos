using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO KitchenObjectSo;
        public GameObject GameObject;
    }
    
    
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> _kitchenObjectSOGameObjectsList;

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in _kitchenObjectSOGameObjectsList)
        {
            kitchenObjectSoGameObject.GameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in _kitchenObjectSOGameObjectsList)
        {
            if (kitchenObjectSoGameObject.KitchenObjectSo == e.KitchenObjectSo)
            {
                kitchenObjectSoGameObject.GameObject.SetActive(true);
            }
        }
    }
}
