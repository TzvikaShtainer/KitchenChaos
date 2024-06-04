using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter, IKitchenObjectParent
{
    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlateRemoved;
    
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;

            if (plateSpawnAmount < spawnPlateTimerMax)
            {
                plateSpawnAmount++;
                
                OnPlateSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //player is empty handed
            if (plateSpawnAmount > 0)
            {
                //at least one plate here
                plateSpawnAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
