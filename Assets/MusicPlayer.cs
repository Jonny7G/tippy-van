using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer musicPlayer;

    private void Awake()
    {
        if (musicPlayer != null)
            Destroy(gameObject);
        else
            musicPlayer = this;
    }
}