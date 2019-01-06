using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    [SerializeField] private GameEvent OnVehicleChange;
    [SerializeField] private VehicleVariable equipedVehicle;
    [SerializeField] private Unlockable[] allActiveUnlockables;

    private string savePath;
    private Unlockable activeVehicle;
    private AllUnlocksData data;
    private void Start()
    {
        /*
         * try's to pull data from save system and if the savepath doesnt exist, it treats it as a fresh install. 
         */

        savePath = System.IO.Path.Combine(Application.persistentDataPath, "UnlocksData.txt");
        bool firstSession = false;
        try
        {
            SavingSystem.LoadProgress(out data, savePath);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex + " TREATING SESSION AS FRESH INSTALL");
            firstSession = true;
            data = new AllUnlocksData();
            data.allUnlockables = new UnlockableData[allActiveUnlockables.Length];
            for(int i = 0; i < allActiveUnlockables.Length; i++)
            {
                data.allUnlockables[i] = allActiveUnlockables[i].myData;
                if (allActiveUnlockables[i].myData.vehicle == Vehicles.red)
                {
                    activeVehicle = allActiveUnlockables[i];
                    equipedVehicle.value = Vehicles.red;
                    OnVehicleChange.Raise();
                }
            }
            SavingSystem.SaveProgress(data, savePath);
        }
        if(!firstSession)
            LoadUnlockables();
    }
    private void LoadUnlockables()
    {
        for(int i = 0; i < data.allUnlockables.Length; i++)
        {
            for(int k = 0; k < allActiveUnlockables.Length; k++)
            {
                if (allActiveUnlockables[k].myData.vehicle == data.allUnlockables[i].vehicle)
                {
                    allActiveUnlockables[k].myData = data.allUnlockables[i];
                    allActiveUnlockables[k].UpdateState();
                }
            }
        }
    }
    public void ResetData()
    {
        for(int i = 0; i < allActiveUnlockables.Length; i++)
        {
            allActiveUnlockables[i].myData.ResetStates();
            allActiveUnlockables[i].UpdateState();
        }
    }
    public void EquipVehicle(Unlockable vehicle)
    {
        if(activeVehicle!=null)
            activeVehicle.Unequip();

        activeVehicle = vehicle;
        SavingSystem.SaveProgress(data, savePath);
        equipedVehicle.value = vehicle.myData.vehicle;
        OnVehicleChange.Raise();
    }
    public void SaveUnlock(UnlockableData unlockable)
    {
        unlockable.locked = false;
        SavingSystem.SaveProgress(data, savePath);
    }
}

public struct AllUnlocksData //is serialized and saved
{
    public UnlockableData[] allUnlockables;
}

[System.Serializable]
public class UnlockableData 
{
    public Vehicles vehicle;
    public bool locked;
    public bool equiped;

    public void ResetStates()
    {
        if (!(vehicle == Vehicles.red))
        {
            locked = true;
            equiped = false;
        }
        else if(vehicle==Vehicles.red)
        {
            locked = false;
            equiped = true;
        }
    }
}
public enum Vehicles { red,lightBlue,orange,purple,darkBlue,green} //this is whats used to identify unlocks and also set the player's animator.