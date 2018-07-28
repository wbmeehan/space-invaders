using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    #region Fields

    /* Is the game over? */
    public static bool IsGameOver;
    /* Game over text */
    public Text GameOverText;
    /* Restart instructions text */
    public Text RestartText;

    #endregion

    public void Start()
    {
        GameOverText.enabled = false;
        RestartText.enabled = false;
        IsGameOver = false;
    }

    public void Update()
    {
        /* If it's gameover display the game over and restart instructions
         *  text */
        if (IsGameOver == true)
        {
            GameOverText.enabled = true;
            RestartText.enabled = true;
        }

        /* Reset the game if it is game over and the player presses the
         *  spacebar */
        if (Input.GetKeyDown("space") && IsGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnDestroy()
    {
        Invader.Direction = Vector2.right;
        Invader.VerticalDisplacement = new Vector2(0, 0);
    }
}
