using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]private Player scriptPlayer;
    public TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Ball"))
        {
            if(name.Equals("GoalDetector1"))
            {
                 scriptPlayer.IncreasemyScore();
            turnManager.ChangeTurn();
            }
           else if(name.Equals("GoalDetector2"))
            {
                 scriptPlayer.IncreaseotherScore();
                turnManager.ChangeTurn();
            }
        }
        else
        {
            //scriptPlayer.IncreaseotherScore();
        }
     if(other.gameObject.tag.Equals("Ball"))
        {
            
           
        }
        else
        {
            //scriptPlayer.IncreasemyScore();
        }
    }
   
}
