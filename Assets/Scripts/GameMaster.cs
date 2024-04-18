using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameMaster : MonoBehaviour
{
    public GameData saveData;

    #region Singleton
    public static GameMaster instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //hold reference to current players
    [HideInInspector] public PlayerData currentPlayer1;
    [HideInInspector] public PlayerData currentPlayer2;

    //hold a temp list of scores to be sorted for highscores
    public List<PlayerData> tempPlayers = new List<PlayerData>(10);

    //debug switches
    public bool debugButtons;
    public bool loadOnStart = true;

    //edit current players data like score and name
    public void Start()
    {
        if (loadOnStart)
        {
            LoadGame();
        }
        else
        {
            saveData = new GameData();
            CreateTempList();
        }
    }
    //create a temp list of all players, filled in with data from saveData
    public void CreateTempList()
    {
        //generate a blank list
        tempPlayers = new List<PlayerData>();

        //get the players form saveData and put them in the list
        for(int i = 0; i < saveData.playerNames.Length; i++)
        {
            //creat a player profile
            PlayerData newPlayer = new PlayerData();

            //input the information from the savedata to the new player
            newPlayer.playerName = saveData.playerNames[i];
            newPlayer.kills = saveData.kills[i];
            newPlayer.death = saveData.deaths[i];

            //calculate the kdr and input it
            if (newPlayer.death == 0) newPlayer.kdr = newPlayer.kills;
            else if (newPlayer.kills == 0) newPlayer.kdr = -newPlayer.death;
            else newPlayer.kdr = (float)newPlayer.kills / (float)newPlayer.death;

            //Add new player to list
            tempPlayers.Add(newPlayer);
        }
    }
    //add our current players to the list
    //sort the list from hightest to lowest scores
    public List<PlayerData> SortTempList(List<PlayerData> unSortedPlayers, bool addCurrentPlayers = false)
    {
        if (addCurrentPlayers)
        {
            //check if list already contains player 1
            if(tempPlayers.Find(p => p.playerName == currentPlayer1.playerName) == null)
            {
                tempPlayers.Add(currentPlayer1);
            }
            else //if the player already exists, then replace its score with your current score
            {
                PlayerData existingPlayer = tempPlayers.Find(p => p.playerName == currentPlayer1.playerName);
                existingPlayer.kills = currentPlayer1.kills;
                existingPlayer.death = currentPlayer1.death;
                
                //calculate the kdr and input it
                if (existingPlayer.death == 0) existingPlayer.kdr = existingPlayer.kills;
                else if (existingPlayer.kills == 0) existingPlayer.kdr = -existingPlayer.death;
                else existingPlayer.kdr = (float)existingPlayer.kills / (float)existingPlayer.death;
            }
            //check if list already contains player 2
            if (tempPlayers.Find(p => p.playerName == currentPlayer2.playerName) == null)
            {
                tempPlayers.Add(currentPlayer2);
            }
            else //if the player already exists, then replace its score with your current score
            {
                PlayerData existingPlayer = tempPlayers.Find(p => p.playerName == currentPlayer2.playerName);
                existingPlayer.kills = currentPlayer2.kills;
                existingPlayer.death = currentPlayer2.death;

                //calculate the kdr and input it
                if (existingPlayer.death == 0) existingPlayer.kdr = existingPlayer.kills;
                else if (existingPlayer.kills == 0) existingPlayer.kdr = -existingPlayer.death;
                else existingPlayer.kdr = (float)existingPlayer.kills / (float)existingPlayer.death;
            }

        }
        List<PlayerData> sortedPlayers = unSortedPlayers.OrderByDescending(p => p.kdr).ToList();
        return sortedPlayers;
    }


    //convert the list to simple data arrays
    //save the arrays to saveDatas
    public void SendHighScoresToSaveData(List<PlayerData> players)
    {
        for(int i = 0; i < 10; i++)
        {
            saveData.playerNames[i] = players[i].playerName;
            saveData.kills[i] = players[i].kills;
            saveData.deaths[i] = players[i].death;
        }
    }
    
    //save the game
    public void SaveGame()
    {
        SortTempList(tempPlayers, false);
        SendHighScoresToSaveData(tempPlayers);

        saveData.lastPlayerNames[0] = currentPlayer1.playerName;
        saveData.lastPlayerNames[1] = currentPlayer2.playerName;

        SaveSystem.instance.SaveGame(saveData);
    }

    public void LoadGame()
    {
        //attempt to get a saveData file from the computer
        saveData = SaveSystem.instance.LoadGame();
        if(saveData == null) //create a new file if none where found
        {
            saveData = new GameData();
            Debug.Log("No data was found, a new file was created instead");
        }

        currentPlayer1.playerName = saveData.lastPlayerNames[0];
        currentPlayer2.playerName = saveData.lastPlayerNames[1];
        CreateTempList();
    }

    #region debugging
    private void Update()
    {
        if (!debugButtons) return;

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (tempPlayers != null)
            {
                tempPlayers = SortTempList(tempPlayers, false);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomFillData();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ClearData();
        }

    }

    #region debugging functions
    void ClearData()
    {
        foreach (PlayerData player in tempPlayers)
        {
            player.playerName = "";
            player.kills = 0;
            player.death = 0;
            player.kdr = 0;
        }
    }
    void RandomFillData()
    {
        //create possible letters to randomise from
        string glyphs = "abcdefghijklmnopqrstuvwxyz";

        foreach (PlayerData player in tempPlayers)
        {
            //generate a random name for the temp player
            int charAmount = Random.Range(3, 10);
            player.playerName = "";
            for (int i = 0; i < charAmount; i++)
            {
                player.playerName += glyphs[Random.Range(0, glyphs.Length)];
            }
            //generate random Kills score
            player.kills = Random.Range(0, 20);
            //generate random deaths
            player.death = Random.Range(0, 20);

            //calculate the kdr and input it
            if (player.death == 0) player.kdr = player.kills;
            else if (player.kills == 0) player.kdr = -player.death;
            else player.kdr = (float)player.kills / (float)player.death;
        }

    }
    #endregion
    #endregion
    #region OldCode
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            saveData.AddScore(1);
            PrintScore();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            saveData.AddScore(-1);
            PrintScore();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveSystem.instance.SaveGame(saveData);
            Debug.Log("Saved Game");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            saveData = SaveSystem.instance.LoadGame();
            Debug.Log("Loaded data");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            saveData.ResetData();
            PrintScore();
        }
    }
    public void PrintScore()
    {
        Debug.Log("The current score is " + saveData.score);
    }*/
    #endregion
}
