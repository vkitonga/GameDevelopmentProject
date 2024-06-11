using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject player1, player2;

    public enum Turn {Player1, Player2};
    public Turn currentTurn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(currentTurn == Turn.Player2) ChangeTurn(Turn.Player1);
            
            else if(currentTurn == Turn.Player1) ChangeTurn(Turn.Player2);
        }
    }

    public void ChangeTurn(Turn turn)
    {
        currentTurn = turn;
        if(currentTurn == Turn.Player2)
        {
            player1.SetActive(false);
            player2.SetActive(true);
        }
        else{
            player1.SetActive(true);
            player2.SetActive(false);
        }
    }
}
