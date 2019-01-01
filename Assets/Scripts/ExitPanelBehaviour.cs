using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanelBehaviour : MonoBehaviour
{
    [SerializeField] private PanelStateManager myPanel;
    [SerializeField] private BoolReference gameActive;

    public void Update()
    {
        if (!gameActive.value)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!myPanel.IsOut)
                    myPanel.TransitionIn();
                else
                    myPanel.TransitionOut();
            }
        }
    }
}