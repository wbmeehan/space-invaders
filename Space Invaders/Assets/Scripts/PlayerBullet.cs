using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBullet : MonoBehaviour
{
    #region

    /* Invader explosion animation frame */
    public Sprite InvaderExplosion;

    /* Speed */
    public float Speed = 30;

    /* Rigid body */
    private Rigidbody2D _rigidBody;

    #endregion

    public void Start()
    {
       _rigidBody = GetComponent<Rigidbody2D>();
       _rigidBody.velocity = Vector2.up * Speed;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Shield":
                /* Player shot shield so destroy bullet and shield */
                Destroy(collider.gameObject);
                Destroy(gameObject);
                break;
            case "Wall":
                /* Player shot wallet so destroy bullet*/
                Destroy(gameObject);
                break;
        }

        string lastSevenCharactersOfTag = collider.tag.Substring(Math.Max(0, collider.tag.Length - 7));
        if (string.Compare("Invader", lastSevenCharactersOfTag) == 0 &&
            collider.GetComponent<SpriteRenderer>().sprite != InvaderExplosion)
        {
            int colourTagLength = collider.tag.Length - 7;
            SoundManager.Instance.PlayAudioClip(
                    SoundManager.Instance.InvaderKilled);
            switch (collider.tag.Substring(0, colourTagLength))
            {
                case "Blue":
                    /* Award 10 points for blue invader kill */
                    IncrementScore(10);
                    break;
                case "Orange":
                    /* Award 20 points for orange invader kill */
                    IncrementScore(20);
                    break;
                case "Green":
                    /* Award 30 points for green invader kill */
                    IncrementScore(30);
                    break;
                case "Purple":
                    /* Award 40 points for purple invader kill */
                    IncrementScore(40);
                    break;
            }

            /* Destroy invader */
            collider.GetComponent<SpriteRenderer>().sprite = InvaderExplosion;
            Destroy(gameObject);
            Destroy(collider.gameObject, 0.5f);
            SoundManager.Instance.PlayAudioClip(
                   SoundManager.Instance.InvaderKilled);
        }
    }

    public void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Increment score display.
    /// </summary>
    /// <param name="pointsAwarded">Points increment.</param>
    public void IncrementScore(int pointsAwarded)
    {
        var scoreDisplay = GameObject.Find("Score").GetComponent<Text>();
        int score = int.Parse(scoreDisplay.text);
        score += pointsAwarded;
        scoreDisplay.text = score.ToString();
    }
}
