using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
  public static TurnManager instance;    
    public GameObject player1, player2;

    public enum Turn {Player1, Player2};
    public Turn currentTurn;

  

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ChangeTurn();
            //if(currentTurn == Turn.Player2) ChangeTurn();
            
            //else if(currentTurn == Turn.Player1) ChangeTurn();
        }
    }

    public void ChangeTurn()
    {
        Debug.Log("Attempt To Change");
        if(currentTurn == Turn.Player2) currentTurn = Turn.Player1;
            
        else if(currentTurn == Turn.Player1) currentTurn = Turn.Player2;
        
        if(currentTurn == Turn.Player2)
        {
            Debug.Log("Activate 2");
            player1.SetActive(false);
            player2.SetActive(true);
        }
        else{
             Debug.Log("Activate 1");
            player1.SetActive(true);
            player2.SetActive(false);
        }
    }
}
