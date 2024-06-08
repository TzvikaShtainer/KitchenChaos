using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    [SerializeField] private RecipeListSO recipeListSo;
    
    [SerializeField] private List<RecipeSO> waitingRecipeSoList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int WaitingRecipeSoListMax = 4;
    private void Awake()
    {
        Instance = this;
        
        waitingRecipeSoList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSoList.Count < WaitingRecipeSoListMax)
            {
                RecipeSO waitingRecipeSO = recipeListSo.RecipeSOList[Random.Range(0, recipeListSo.RecipeSOList.Count)];
                
                waitingRecipeSoList.Add(waitingRecipeSO);
                
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSoList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSoList[i];

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
                    waitingRecipeSoList.RemoveAt(i);
                    
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    
                    return;
                }
            }
        }
        
        //no matches found
        Debug.Log("player didnt deliver a correct recipe");
    }

    public List<RecipeSO> GetWaitingRecipeSoList()
    {
        return waitingRecipeSoList;
    }
}
