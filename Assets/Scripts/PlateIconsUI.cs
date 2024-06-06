using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach (Transform child in transform)
        {
            if(child == iconTemplate) 
                continue;
            
            Destroy(child.gameObject);
        }
        
        foreach (KitchenObjectSO kitchenObjectSo in _plateKitchenObject.GetKitchenObjectSoList())
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSo);
        }
    }
}
