using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetTextToRefrence : MonoBehaviour
{
    [SerializeField] private IntReference number;

    [SerializeField] private TMP_Text myText;
    
    public void SetText()
    {
        myText.SetText(number.value.ToString());
    }
}