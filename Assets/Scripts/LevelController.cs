using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController Current;
    public bool gameActive = false;

    public GameObject startMenu, gameOverMenu, finishMenu;
    public Text scoreText, finishScoreText;

    int currentLevel;
    int score;
    
    // Start is called before the first frame update
    void Start()
    {
        Current = this;
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        if (SceneManager.GetActiveScene().name != "Level " + currentLevel)
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        PlayerController.Current.ChangeSpeed(PlayerController.Current.runningSpeed);
        startMenu.SetActive(false);
        PlayerController.Current.animator.SetBool("running", true);
        gameActive = true;
        Time.timeScale = 1.0f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level " + (currentLevel + 1));
    }

    public void GameOver()
    {

        gameOverMenu.SetActive(true);
        gameActive = false;
    }

    public void FinishGame()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        finishScoreText.text = PlayerController.Current._moneyScoreText.text;
        finishMenu.SetActive(true);
        gameActive=false;
    }

}
