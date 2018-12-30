using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameBehaviour : MonoBehaviour
{
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }
#endif
}
