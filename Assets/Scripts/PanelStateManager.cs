using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStateManager : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator;

    [SerializeField] private string outBoolName;
    [SerializeField] private bool transitionOnStart;

    public bool IsOut { get; private set; }

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
        IsOut = true;
    }
    public void TransitionOut()
    {
        panelAnimator.SetBool(outBoolName, false);
        IsOut = false;
    }
}