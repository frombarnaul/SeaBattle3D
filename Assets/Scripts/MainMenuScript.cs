using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject StatsPanel;
    public GameObject MainMenuPanel;

    public TextMeshProUGUI StatWins;
    public TextMeshProUGUI StatGames;
    public TextMeshProUGUI StatWinRate;

    public void StartPlayerVScomp()
    {
        SceneManager.LoadScene(1);
        LoadingScreen.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void HideOrOpenStatsPanel()
    {
        if(StatsPanel.activeInHierarchy == true) 
        {
            StatsPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
        }
        else
        {
            StatsPanel.SetActive(true);
            MainMenuPanel.SetActive(false);
        }
    }

    public void Start()
    {
        if (File.Exists(Application.dataPath + "/stats.json"))
        {
            var stats = JsonUtility.FromJson<Stats>(File.ReadAllText(Application.dataPath + "/stats.json"));
            float wins = stats.winsCount;
            float games = stats.gamesCount;
            StatWins.text = wins.ToString();
            StatGames.text = games.ToString();

            
            StatWinRate.text = $"{Math.Round((wins / games) * 100f, 2)}%";
        }
    }
}
