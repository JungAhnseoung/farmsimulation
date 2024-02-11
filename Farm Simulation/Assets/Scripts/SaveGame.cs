using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGame
{
    public Dictionary<string, SaveObject> saveObjectDict;
    
    public SaveGame()
    {
        saveObjectDict = new Dictionary<string, SaveObject>();
    }
}
