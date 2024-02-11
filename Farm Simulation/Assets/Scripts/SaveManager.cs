using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    public static List<Save> saveList;
    public static SaveGame saveGame;

    public void Awake()
    {
        saveList = new List<Save>();
    }

    public static void StoreScene()
    {
        foreach(Save save in saveList)
        {
            save.Store(SceneManager.GetActiveScene().name);
        }
    }

    public static void RecoverScene()
    {
        foreach(Save save in saveList)
        {
            save.Recover(SceneManager.GetActiveScene().name);
        }
    }

    public static void LoadFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Farm.dat"))
        {
            saveGame = new SaveGame();
            FileStream stream = File.Open(Application.persistentDataPath + "/Farm.dat", FileMode.Open);
            saveGame = (SaveGame)formatter.Deserialize(stream);

            for (int i = saveList.Count - 1; i > -1; i--)
            {
                if (saveGame.saveObjectDict.ContainsKey(saveList[i].SaveID)) saveList[i].Load(saveGame);
                else
                {
                    Component component = (Component)saveList[i];
                    Destroy(component.gameObject);
                }
            }
            stream.Close();
        }
        GameMenuManager.CloseGameMenu();
    }

    public void SaveFile()
    {
        saveGame = new SaveGame();
        foreach(Save save in saveList)
        {
            saveGame.saveObjectDict.Add(save.SaveID, save.Save());
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(Application.persistentDataPath + "/Farm.dat", FileMode.Create);
        formatter.Serialize(stream, saveGame);

        stream.Close();
        GameMenuManager.CloseGameMenu();
    }
}
