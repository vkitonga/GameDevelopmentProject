using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTicker : MonoBehaviour
{
    public int playerNumber;
    public int amount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //increase the score using the gameplaymanager
            GamePlayManager.instance.UpdateScore(playerNumber, amount);
        }
    }
}
