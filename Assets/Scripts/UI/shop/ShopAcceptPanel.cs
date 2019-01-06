using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopAcceptPanel : MonoBehaviour
{
   
    [SerializeField] private ShopData shopData;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image carImage;
    [SerializeField] private Button buyButton;

    [SerializeField] private IntVariable totalScore;

    private Unlockable activeUnlock;

    public void SetProperties(Unlockable unlockable)
    {
        costText.SetText(unlockable.shopPanel.cost.ToString());
        carImage.sprite = unlockable.shopPanel.carImage;
        activeUnlock = unlockable;

        if (totalScore.Value < unlockable.shopPanel.cost)
            buyButton.interactable = false;
    }
    public void ExecuteUnlock()
    {
        if (totalScore.Value >= activeUnlock.shopPanel.cost)
        {
            totalScore.Value -= activeUnlock.shopPanel.cost;
            activeUnlock.Unlock();
        }
    }
}