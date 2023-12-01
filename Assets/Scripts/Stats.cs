using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

[System.Serializable]
public class Stats
{
    public int gamesCount;
    public int winsCount;

    public void AddGame()
    {
        var stats = new Stats();
        if (File.Exists(Application.dataPath + "/stats.json"))
        {
            stats = JsonUtility.FromJson<Stats>(File.ReadAllText(Application.dataPath + "/stats.json"));
            stats.gamesCount++;
            File.WriteAllText(Application.dataPath + "/stats.json", JsonUtility.ToJson(stats));
            Debug.Log("STATKA GAMES = " + stats.gamesCount);
        }
        else
        {
            stats.gamesCount = 1;
            stats.winsCount = 0;
            File.WriteAllText(Application.dataPath + "/stats.json", JsonUtility.ToJson(stats));
        }
    }

    public void AddWin()
    {
        var stats = new Stats();
        if (File.Exists(Application.dataPath + "/stats.json"))
        {
            stats = JsonUtility.FromJson<Stats>(File.ReadAllText(Application.dataPath + "/stats.json"));
            stats.winsCount++;
            File.WriteAllText(Application.dataPath + "/stats.json", JsonUtility.ToJson(stats));
            Debug.Log("STATKA WINS = " + stats.winsCount);
        }
        else
        {
            stats.gamesCount = 1;
            stats.winsCount = 0;
            File.WriteAllText(Application.dataPath + "/stats.json", JsonUtility.ToJson(stats));
        }
    }
}


