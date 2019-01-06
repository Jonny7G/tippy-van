using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class Unlockable : MonoBehaviour
{
    public ShopData shopData;
    public UnlockableData myData;
    public ShopPanelProperties shopPanel;

    [SerializeField] private TMP_Text costText;
    [SerializeField] private ShopAcceptPanel acceptPanel;
    [SerializeField] private UnityEvent openAcceptPanel;
    [Space(2)]
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color boughtColor;
    [SerializeField] private Color equipedColor;
    private Image myImage;
    
    private void Awake()
    {
        myImage = GetComponent<Image>();
    }
    
    public void UpdateState()
    {
        if (myData.equiped)
        {
            Equip();
        }
        else
        {
            Unequip();
        }
        if (myData.locked)
            EnterLockState();
        else if(!myData.equiped&&!myData.locked)
            EnterUnlockState();
    }
    public void ButtonPushed() //called by this objects button
    {
        if (myData.locked)
        {
            acceptPanel.SetProperties(this);
            openAcceptPanel?.Invoke();
        }
        else if (!myData.equiped)
            Equip();
    }
    public void Unlock()
    {
        shopData.SaveUnlock(myData);
        EnterUnlockState();
    }
    public void EnterUnlockState()
    {
        HideCost();
        myImage.color = boughtColor;
    }
    public void EnterLockState()
    {
        ShowCost();
        myImage.color = lockedColor;
    }
    public void ShowCost()
    {
        costText.gameObject.SetActive(true);
    }
    public void Equip()
    {
        if(costText.gameObject.activeSelf)
            HideCost();

        myImage.color = equipedColor;
        myData.equiped = true;
        shopData.EquipVehicle(this);
    }
    public void HideCost()
    {
        costText.gameObject.SetActive(false);
    }
    public void Unequip()
    {
        myData.equiped = false;

        myImage.color = boughtColor;
    }
}
[System.Serializable]
public struct ShopPanelProperties
{
    public Sprite carImage;
    [HideInInspector]
    public UnlockableData data;
    public int cost;
}