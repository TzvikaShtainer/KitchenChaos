using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSo;
    
    [SerializeField] private List<RecipeSO> WaitingRecipeSoList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int WaitingRecipeSoListMax = 4;
    private void Awake()
    {
        Instance = this;
        
        WaitingRecipeSoList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (WaitingRecipeSoList.Count < WaitingRecipeSoListMax)
            {
                RecipeSO waitingRecipeSO = recipeListSo.RecipeSOList[Random.Range(0, recipeListSo.RecipeSOList.Count)];
                WaitingRecipeSoList.Add(waitingRecipeSO);
                Debug.Log(waitingRecipeSO.name);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < WaitingRecipeSoList.Count; i++)
        {
            RecipeSO waitingRecipeSO = WaitingRecipeSoList[i];

            if (waitingRecipeSO.KitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectSoList().Count)
            {
                //has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSoList)
                {
                    //cycling through all ingredients in the Recipe
                    bool ingredientFound = false;
                    
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSoList())
                    {
                        //cycling through all ingredients in the Plate

                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //ingredient Matches
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        //this recipe ingredient was not found on that plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    //player deliver correct recipe
                    Debug.Log("player deliver correct recipe");
                    WaitingRecipeSoList.RemoveAt(i);
                    return;
                }
            }
        }
        
        //no matches found
        Debug.Log("player didnt deliver a correct recipe");
    }
}
