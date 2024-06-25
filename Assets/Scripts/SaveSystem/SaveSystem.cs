using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    string filePath;

    public string saveFileName;

    public static SaveSystem instance;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + saveFileName + ".saveData";

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveGame(GameData saveData)
    {
        // find the file path and create a file
        FileStream dataStream = new FileStream(filePath, FileMode.Create);

        //get the binary formatter ready
        BinaryFormatter converter = new BinaryFormatter();

        //convert the data and send it the file
        converter.Serialize(dataStream, saveData);

        dataStream.Close();

    }
    public GameData LoadGame()
    {
        //check if the file already exists
        if (File.Exists(filePath))
        {
            //if so get the existing file and return it
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            GameData saveData = converter.Deserialize(dataStream) as GameData;

            dataStream.Close();
            return saveData;
        }
        else
        {
            Debug.LogError("Save file not found in " + filePath);
            return null;
        }

        //if the file does not exist, return an error message and cancel the function
    }
    
}
