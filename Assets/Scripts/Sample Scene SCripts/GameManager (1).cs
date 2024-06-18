using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //player scores
    public int player1, player2;

    //game state
    public bool gameOver = false;

    //timer
    float startTime;
    float currentTime;

    //ui display
    public TMP_Text score, timer;

    //ball
    public Rigidbody ball;

    //spawn point
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //score goal
    public void Goal(int playerNum)
    {
        if(playerNum == 1)
        {
            player1++;
        }
        else
        {
            player2++;
        }
        score.text = player1 + " | " + player2;

        StartCoroutine(ResetBall());
        
    }
    //timer coroutine

    //ball respawn coroutine
    public IEnumerator ResetBall()
    {
        ball.constraints = RigidbodyConstraints.FreezeAll;
        ball.useGravity = false;
        ball.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        ball.transform.position = spawnPoint.position;
        ball.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        ball.constraints = RigidbodyConstraints.None;
        ball.useGravity = true;

        yield return null;
    }

    //game over
}
