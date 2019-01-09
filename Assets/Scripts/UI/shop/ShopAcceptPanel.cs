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

    [SerializeField] private IntReference totalScore;
    
    private Unlockable activeUnlock;
    
    public void SetProperties(Unlockable unlockable)
    {
        costText.SetText(unlockable.shopPanel.cost.ToString());
        carImage.sprite = unlockable.shopPanel.carImage;
        activeUnlock = unlockable;

        if (totalScore.value < unlockable.shopPanel.cost)
            buyButton.interactable = false;
    }
    public void ExecuteUnlock()
    {
        if (totalScore.value >= activeUnlock.shopPanel.cost)
        {
            activeUnlock.Unlock();
            ProgressData.instance.Unlocked(activeUnlock.shopPanel.cost);
        }
    }
}