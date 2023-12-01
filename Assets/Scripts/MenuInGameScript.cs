using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MenuInGameScript : MonoBehaviour
{
    public GameObject menuButton;
    public GameObject panel;
    bool menuIsOpened;
    public GameController Controller;
    public GameObject helpMenu;
    void Start()
    {
        menuIsOpened = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Controller.gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (helpMenu.activeInHierarchy == false) 
                {
                    MenuOpenClose();
                }
                
            }
        }
        else
        {
            menuButton.SetActive(false);
        }
    }


    public void MenuOpenClose()
    {
        if(!menuIsOpened)
        {
            Time.timeScale = 0;
            panel.SetActive(true);
            menuIsOpened = true;
            menuButton.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            panel.SetActive(false);
            menuIsOpened = false;
            menuButton.SetActive(true);
        }
        

    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
