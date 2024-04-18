using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccessor : MonoBehaviour
{
    void Start()
    {
        // Accessing non-static data
        NonStaticData playerData = FindObjectOfType<NonStaticData>();
        if (playerData != null)
        {
            Debug.Log("Health: " + playerData.speed);
            Debug.Log("Speed: " + playerData.speed);
        }

        // Accessing static data
        Debug.Log("Score: " + StaticData.score);
        Debug.Log("Is Game Paused: " + StaticData.isGamePaused);
    }
}
