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
            newPlayer.Loose = saveData.Loose[i];
            newPlayer.Win = saveData.Win[i];

            //calculate the kdr and input it
            if (newPlayer.Win == 0) newPlayer.Draw = newPlayer.Loose;
            else if (newPlayer.Loose == 0) newPlayer.Draw = -newPlayer.Win;
            else newPlayer.Draw = (float)newPlayer.Loose / (float)newPlayer.Win;

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
                existingPlayer.Loose = currentPlayer1.Loose;
                existingPlayer.Win = currentPlayer1.Win;
                
                //calculate the kdr and input it
                if (existingPlayer.Win == 0) existingPlayer.Draw = existingPlayer.Loose;
                else if (existingPlayer.Loose == 0) existingPlayer.Draw = -existingPlayer.Win;
                else existingPlayer.Draw = (float)existingPlayer.Loose / (float)existingPlayer.Win;
            }
            //check if list already contains player 2
            if (tempPlayers.Find(p => p.playerName == currentPlayer2.playerName) == null)
            {
                tempPlayers.Add(currentPlayer2);
            }
            else //if the player already exists, then replace its score with your current score
            {
                PlayerData existingPlayer = tempPlayers.Find(p => p.playerName == currentPlayer2.playerName);
                existingPlayer.Loose = currentPlayer2.Loose;
                existingPlayer.Win = currentPlayer2.Win;

                //calculate the kdr and input it
                if (existingPlayer.Win == 0) existingPlayer.Draw = existingPlayer.Loose;
                else if (existingPlayer.Loose == 0) existingPlayer.Draw = -existingPlayer.Win;
                else existingPlayer.Draw = (float)existingPlayer.Loose / (float)existingPlayer.Win;
            }

        }
        List<PlayerData> sortedPlayers = unSortedPlayers.OrderByDescending(p => p.Draw).ToList();
        return sortedPlayers;
    }


    //convert the list to simple data arrays
    //save the arrays to saveDatas
    public void SendHighScoresToSaveData(List<PlayerData> players)
    {
        for(int i = 0; i < 10; i++)
        {
            saveData.playerNames[i] = players[i].playerName;
            saveData.Loose[i] = players[i].Loose;
            saveData.Win[i] = players[i].Win;
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
            player.Loose = 0;
            player.Win = 0;
            player.Draw = 0;
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
            player.Loose = Random.Range(0, 20);
            //generate random deaths
            player.Win = Random.Range(0, 20);

            //calculate the kdr and input it
            if (player.Win == 0) player.Draw = player.Loose;
            else if (player.Loose == 0) player.Draw = -player.Win;
            else player.Draw = (float)player.Loose / (float)player.Win;
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
