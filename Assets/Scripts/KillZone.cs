using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("COllision");
        //check which player to respawn
        if(collision.gameObject == GamePlayManager.instance.player1)
        {
            Debug.Log("found player1");
            //call the respawn function on this player
            GamePlayManager.instance.SpawnPlayer(1);
        }
        else if(collision.gameObject == GamePlayManager.instance.player2)
        {
            GamePlayManager.instance.SpawnPlayer(2);
        }
    }
}
