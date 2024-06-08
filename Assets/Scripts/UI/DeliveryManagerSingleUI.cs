using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSo)
    {
        recipeNameText.text = recipeSo.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) 
                continue;
            
            Destroy(child.gameObject);
        }
        
        foreach (KitchenObjectSO kitchenObjectSO in recipeSo.KitchenObjectSoList)
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate, iconContainer);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
