using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static int Level = 1;
    public static int Score;
    public static bool IsGameOver;
    // Use this for initialization 

    public Text LevelText;


    public static GameManager Instance { get; set; }
    void Start () {
        if (Instance == null)
        {
            Instance = this;
            Invoke("HideLevelText", 2);
            try
            {
                GameObject.Find("LevelText").GetComponent<Text>().text = string.Format("Level {0}", Level);
            }
            catch (NullReferenceException) {; }
        }
        else
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        var scoreDisplay = GameObject.Find("Score").GetComponent<Text>();
        scoreDisplay.text = GameManager.Score.ToString();


        /* Reset the game if it is game over and the player presses the
 *  spacebar */

        if (IsGameOver && SceneManager.GetActiveScene().name != "GameOver")
        {
            SceneManager.LoadScene("GameOver");
        }

        if (SceneManager.GetActiveScene().name == "GameOver" && Input.GetKeyDown("space"))
        {
            GameManager.Score = 0;
            GameManager.Level = 1;
            Invader.Direction = Vector2.right;
            Invader.VerticalDisplacement = new Vector2(0, 0);
            Player.InitialPosition = Vector2.zero;
            IsGameOver = false;
            SceneManager.LoadScene("Level1");
           try
            {
                GameObject.Find("LevelText").GetComponent<Text>().text = string.Format("Level {0}", Level);
            }
            catch (NullReferenceException) {; }
        }
    }

    private void HideLevelText()
    {
        if (LevelText != null)
        {
            LevelText.enabled = false;
        }
    }


    public void LoadLevel()
    {
        ResetInvaders();
        Level++;
        string nextLevel = string.Format("Level{0}", (Level <= 2)?Level:2);
        SceneManager.LoadScene(nextLevel);
        string levelTitle = string.Format("Level{0}", Level);
        if (LevelText != null)
        {
            LevelText.text = levelTitle;
        }
        Invoke("HideLevelText", 2);
    }

    private void ResetInvaders()
    {
        Invader.Direction = Vector2.right;
        Invader.VerticalDisplacement = new Vector2(0, 0);
    }

}
