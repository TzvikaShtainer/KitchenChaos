using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State State;
    }
    
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArry;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArry;

    private State state;
    
    private FryingRecipeSO fryingRecipeSo;
    private float fryingTimer;
    
    private BurningRecipeSO burningRecipeSo;
    private float burningTimer;


    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:

                    break;

                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSo.fryingTimerMax
                    });
                    
                    if (fryingTimer > fryingRecipeSo.fryingTimerMax)
                    {
                        //fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSo.output, this);
                        
                        state = State.Fried;
                        burningTimer = 0;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            State = state
                        });
                        
                        burningRecipeSo = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }

                    break;

                case State.Fried:
                    burningTimer += Time.deltaTime;

                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        progressNormalized = (float)burningTimer / burningRecipeSo.burningTimerMax
                    });
                    
                    if (burningTimer > burningRecipeSo.burningTimerMax)
                    {
                        //burned
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSo.output, this);

                        state = State.Burned;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            State = state
                        });
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                        {
                            progressNormalized = 0f
                        });
                    }

                    break;
                case State.Burned:

                    
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //no kitchen obj here
            if (player.HasKitchenObject())
            {
                //player carrying something

                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //player carrying something that can be fry
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    
                    fryingRecipeSo = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        State = state
                    });
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSo.fryingTimerMax
                    });
                    
                }
            }
            else
            {
                //player carrying nothing
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                //player carrying something
                
            }
            else
            {
                //player carrying nothing
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    State = state
                });
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSo != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSo in fryingRecipeSOArry)
        {
            if (fryingRecipeSo.input == inputKitchenObjectSO)
            {
                return fryingRecipeSo;
            }
        }
        
        return null;
    }
    
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSo in burningRecipeSOArry)
        {
            if (burningRecipeSo.input == inputKitchenObjectSO)
            {
                return burningRecipeSo;
            }
        }
        
        return null;
    }
}
