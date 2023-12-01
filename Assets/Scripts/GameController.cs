using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour

{
    public GameObject Player1;
    public GameObject Player2;
    public int Player1Points;
    public int Player2Points;
    public bool gameIsOver;
    public GameObject GameOverPanel;
    public TextMeshProUGUI WinnerName;
    AudioSource[] sounds;

    public void NextTurn(string PlayerName)
    {

        if (!gameIsOver)
        {
            if (PlayerName == "Player2")
            {
                Debug.Log("Ходит второй игрок");
                Player2.GetComponent<CPU>().Turn();
            }
            else
            {
                Debug.Log("Ходит первый игрок");
                Player1.GetComponent<Player>().Turn();
            }
        }
    }

    void Start()
    {
        var stats = new Stats();
        stats.AddGame();

        sounds = GetComponents<AudioSource>();
        sounds[2].Play();

        gameIsOver = false;
        if (Random.Range(0, 2) == 0)
        {
            NextTurn("Player1");
        }
        else
        {
            NextTurn("Player2");
        }

    }

    private void Update()
    {
        if (Player1Points >= 20 || Player2Points >= 20) gameIsOver = true;


        if (Input.GetKeyDown(KeyCode.M)) if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("ТЫ ПРОЗРЕЛ");
                GameObject[] enemyShips = GameObject.FindGameObjectsWithTag("team2");
                foreach(var enemyShip in enemyShips)
                {
                    enemyShip.GetComponentInChildren<MeshRenderer>().enabled = true;                    
                }
            }

    }

    public void GameOver()
    {
        Invoke(nameof(ShowWinner), 1);
    }

    void ShowWinner()
    {
        bool isWin = Player1Points >= 20;
        WinnerName.text = isWin ? "You win!" : "You loose";
        sounds[isWin ? 0 : 1].Play();
        if(isWin)
        {
            var stats = new Stats();
            stats.AddWin();
        }
        GameOverPanel.SetActive(true);
    }

}
