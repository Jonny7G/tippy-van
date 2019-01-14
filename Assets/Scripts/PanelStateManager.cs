using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStateManager : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator;

    [SerializeField] private string outBoolName;
    [SerializeField] private bool transitionOnStart;
    [SerializeField] private bool Unexitable;

    public bool IsOut { get; private set; }

    public static List<PanelStateManager> activePanels = new List<PanelStateManager>();
    private void Start()
    {
        activePanels.Clear();
        if (transitionOnStart)
        {
            TransitionIn();
        }
    }
    public static void CloseLastPanel(System.Action<bool> hasPanel)
    {
        if (activePanels.Count > 0)
        {
            hasPanel(!activePanels[activePanels.Count - 1].Unexitable);
            
            if (!activePanels[activePanels.Count - 1].Unexitable)
                activePanels[activePanels.Count - 1].TransitionOut();
        }
        else if(!GameState.GameActive)
            hasPanel(false);
    }
    public void TransitionIn()
    {
        activePanels.Add(this);
        panelAnimator.SetBool(outBoolName, true);
        IsOut = true;
    }
    public void TransitionOut()
    {
        activePanels.Remove(this);
        panelAnimator.SetBool(outBoolName, false);
        IsOut = false;
    }
}