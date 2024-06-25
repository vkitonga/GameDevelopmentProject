using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //you can only save simple data
    public string[] playerNames = new string[10];
    //store arrays of our score information MAKE DIFFERENT FOR YOUR INDIVIDUAL GAME
    public int[] Draw = new int[10];
    public int[] Loose = new int[10];
    public int[] Win = new int[10];
    //Game settings or information
    public float maxRoundTime = 120f;
    public int maxDraw = 10;

    public string[] lastPlayerNames = new string[2];


}
