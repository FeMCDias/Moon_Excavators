using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Script : MonoBehaviour
{
    public static BGM_Script instance;
    // Script for the background music to play throughout the game, even when the scene changes
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
