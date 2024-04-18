using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manage game state, scores, and respawns during gameplay. Only one GamePlayManager should be in the scene
/// reference this script to access gameplay objects like UI
/// </summary>
public class GamePlayManager : MonoBehaviour
{
    #region Variables
    //store gameplay states
    public enum State { Intro, Gameplay, Pause, Ending }

    [Header("Game Settings")]
    public State gameState = State.Intro;
    //store score data
    public int player1Score, player2Score;

    public int maxScore;
    public float gameDuration;

    //store respawns positions
    public Transform[] respawnPositions;
    private int lastSpawnP1, lastSpawnP2;

    [Header("Player Settings")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    //store player object references
    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;

    

    //store player data
    public string player1Name, player2Name;

    [Header("UI Settings")]
    //store UI references
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;
    public TMP_Text timerText;
    public TMP_Text messageText;

    
    [Header("Other Components")]
    //store a timer
    public Timer gameTimer;


    #endregion

    #region Singleton
    public static GamePlayManager instance; //create a static reference to ourselves

    //assign this object to the reference
    private void Awake()
    {
        //if (instance != null) Destroy(gameObject); //optional for if you only ever want 1 singleton
        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == State.Gameplay)
        {
            DisplayTimer();
        }
    }

    //Score update function - increase one players score, check if game is over, update UI
    public void UpdateScore(int playerNumber, int amount)
    {
        //cancel the function if we are not in gameplay
        if (gameState != State.Gameplay) return; 

        //if playerNumber is 1, increase player1 score, otherwise increase player 2
        if (playerNumber == 1) player1Score += amount;
        else player2Score += amount;

        //ternary operator can be used for bool statements to put logic on one clean line
        //playerNumber = 1 ? player1Score ++ : player2Score ++ 1;

        //check if either players score is more than the max score. If so, call End game function
        if(player1Score >= maxScore || player2Score >= maxScore && gameState != State.Ending)
        {
            EndGame();
            gameState = State.Ending;
        }
        //update the score UI
        player1ScoreText.text = player1Name + " : " + player1Score;
        player2ScoreText.text = player2Name + " : " + player2Score;
    }

    //respawns - find player to respawn, deactivate controls, decrease lives*, 
    //disable player, move to spawn point, reset data, reactivate player, check for end of game if relevant
    public void SpawnPlayer(int playerNumber)
    {
        //Pick which player to spawn
        GameObject currentPlayer;
        if (playerNumber == 1) currentPlayer = player1;
        else currentPlayer = player2;

        //disable any scripts
        currentPlayer.GetComponent<CCMovement>().enabled = false;
        //add all the scripts you want to disable here.

        //call the reactivation of the player
        StartCoroutine(FinishSpawn(currentPlayer));

    }
    private IEnumerator FinishSpawn(GameObject currentPlayer)
    {
        //wait incase animations need to play
        yield return new WaitForSeconds(2f);

        //pick a random number to spawn from
        int spawnIndex = Random.Range(0, respawnPositions.Length);

        //check if the spawn index matches either previous spawn, if so, reroll
        while(spawnIndex == lastSpawnP1 || spawnIndex == lastSpawnP2)
        {
            spawnIndex = Random.Range(0, respawnPositions.Length);
        }

        //move player to spawn point
        currentPlayer.transform.position = respawnPositions[spawnIndex].position;
        //store new last position
        if (currentPlayer == player1) lastSpawnP1 = spawnIndex;
        else lastSpawnP2 = spawnIndex;

        yield return new WaitForSeconds(0.5f);
        //reactivate all scripts and reset all data
        currentPlayer.GetComponent<CCMovement>().enabled = true;
        //add all the other scripts you need to activate or reset e.g.
        //health reset, animation state changed, weapons reactivated etc
    }

    //Display the timer
    void DisplayTimer()
    {
        timerText.text = gameTimer.GetFormattedTime();
    }

    //run intro sequence
    void SetupGame()
    {
        //set game state
        gameState = State.Intro;

        //Spawn players for the first time
        int spawnIndex = Random.Range(0, respawnPositions.Length);
        player1 = Instantiate(player1Prefab, respawnPositions[spawnIndex].position, respawnPositions[spawnIndex].rotation);
        lastSpawnP1 = spawnIndex;

        //repeat for player 2
        spawnIndex = Random.Range(0, respawnPositions.Length);

        int attempts = 0;
        while (spawnIndex == lastSpawnP1 && attempts < 3)
        {
            Random.Range(0, respawnPositions.Length);
            attempts++;
        }

        player2 = Instantiate(player2Prefab, respawnPositions[spawnIndex].position, respawnPositions[spawnIndex].rotation);
        lastSpawnP2 = spawnIndex;


        //player names displayed in UI
        player1ScoreText.text = player1Name + " : " + 0;
        player2ScoreText.text = player2Name + " : " + 0;

        //hide timer
        timerText.text = "";

        //display start message
        messageText.text = "Get Ready";
        //run the intro sequence coroutine
        StartCoroutine(IntroSequence());
    }
    //intro coroutine
    private IEnumerator IntroSequence()
    {
        //show the timer message
        int timer = 4;
        
        //count down from three (displaying time as we go)
        while(timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer -= 1;
            messageText.text = "Starting in " + timer;
        }
        //Activate game play
        messageText.text = "Go!";
        yield return new WaitForSeconds(1);
        //hide messages
        messageText.text = "";

        //run gameplay timer
        gameTimer.countDown = true;
        gameTimer.maxTime = gameDuration;
        gameTimer.StartTimer();

        gameState = State.Gameplay;
    }

    

    //End game - freeze players, tally scores, display results or move to next scene
    public void EndGame()
    {
        //FreezePlayers
        player1.GetComponent<CCMovement>().enabled = false;
        player2.GetComponent<CCMovement>().enabled = false;
        //deactivate other scripts when ready

        //change state
        gameState = State.Ending;

        //update UI;
        player1ScoreText.text = "";
        player2ScoreText.text = "";
        timerText.text = "";

        //display winner
        string winningPlayer;
        if (player1Score > player2Score) winningPlayer = player1Name;
        else winningPlayer = player2Name;

        messageText.text = winningPlayer + " Wins!" + "\n" + player1Name + " : " + player1Score + "\n" + player2Name + " : " + player2Score;
    }
}
