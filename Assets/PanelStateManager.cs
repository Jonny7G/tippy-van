using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStateManager : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator;

    [SerializeField] private string outBoolName;
    [SerializeField] private bool transitionOnStart;
    private void Start()
    {
        if (transitionOnStart)
        {
            TransitionIn();
        }
    }
    public void TransitionIn()
    {
        panelAnimator.SetBool(outBoolName, true);
    }
    public void TransitionOut()
    {
        panelAnimator.SetBool(outBoolName, false);
    }
}