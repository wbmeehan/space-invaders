using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Invader : MonoBehaviour
{
    #region Fields

    /* Direction */
    public static Vector2 Direction = Vector2.right;

    /* Displacement */
    public static Vector2 VerticalDisplacement;

    /* Animation frames */
    public Sprite Frame1;
    public Sprite Frame2;
    public Sprite PlayerExplosion;
    public Sprite InvaderExplosion;

    /* Speed */
    public float Speed = 6;

    /* Bullet */
    public GameObject AlienBullet;

    /* Sprite renderer */
    public SpriteRenderer SpriteRenderer;

    /* Number of invaders */
    private const int NumInvaders = 44;

    /* Death count */
    private static int _invaderDeathCount;

    /* Rigid body */
    private Rigidbody2D _rigidBody;

    /* Position */
    private Vector2 _initialPosition;

    /* Generated random firing interval for a single shot */
    private float _generatedFiringInterval;

    /* Range for random firing interval */
    private float _minFiringInterval = 1.0f;
    private float _maxFiringInterval = 4.0f;

    /* Has the invader made its first shot? */
    private bool _firstShot = true;

    /* Animation seconds/frame */
    private float _invaderFrameUpdateInterval = 0.5f;

    #endregion

    public void Start()
    {
        _invaderDeathCount = 0;
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.velocity = Vector2.right * Speed;
        _initialPosition = transform.position;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(UpdateAlienFrame());
    }

    public void OnCollisionEnter2D(Collision2D collider)
    {
        /* Check if collided into left wall */
        if (collider.gameObject.name == "LeftWall" && Direction == Vector2.left)
        {
            /* Turn right */
            Direction = Vector2.right;
            /* Move down */
            VerticalDisplacement += new Vector2(0, 1);
        }

        /* Check if collided into right wall */
        if (collider.gameObject.name == "RightWall" && Direction == Vector2.right)
        {
            /* Turn left */
            Direction = Vector2.left;
            /* Move down */
            VerticalDisplacement += new Vector2(0, 1);
        }

        /* Check if collided into bottom wall */
        if (collider.gameObject.name == "BottomWall")
        {
           SpriteRenderer.sprite = InvaderExplosion;
            SoundManager.Instance.PlayAudioClip(
               SoundManager.Instance.InvaderKilled);
            Destroy(gameObject, 0.5f);
        }

        /* Check if collided into player */
        if (collider.gameObject.name == "Player")
        {
            Destroy(gameObject);
            Destroy(collider.gameObject, 0.5f);
            SoundManager.Instance.PlayAudioClip(
                SoundManager.Instance.PlayerExplosion);
            collider.gameObject.GetComponent<SpriteRenderer>().sprite = PlayerExplosion;
        }
    }

    public void FixedUpdate()
    {
        /* Check if this is the first shot fired */
        if (_firstShot == true)
        {
            _firstShot = false;
            /* Generate the delay before the next shot */
            _generatedFiringInterval = _minFiringInterval +
                Random.Range(_minFiringInterval, _maxFiringInterval);
        }

        /* Check if generated firing delay has passed */
        if (Time.timeSinceLevelLoad > _generatedFiringInterval)
        {
            /* Generate the next delay */
            _generatedFiringInterval += _minFiringInterval +
                Random.Range(_minFiringInterval, _maxFiringInterval);
            /* Fire bullet */
            Instantiate(AlienBullet, transform.position, Quaternion.identity);
        }

        /* Update the velocity and position of the alien */
        _rigidBody.velocity = Direction * Speed;
        Vector2 position = transform.position;
        position.y = _initialPosition.y - VerticalDisplacement.y;
        transform.position = position;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        /* Check if collided with player */
        if (collider.gameObject.tag == "Player")
        {
            /* Play ship explosion audio clip */
            SoundManager.Instance.PlayAudioClip(SoundManager.Instance.PlayerExplosion);

            /* Display ship explosion frame */
            collider.GetComponent<SpriteRenderer>().sprite = PlayerExplosion;

            /* Destroy alien */
            GetComponent<SpriteRenderer>().sprite = InvaderExplosion;
            Destroy(gameObject, 0.5f);

            /* Destroy player */
            GetComponent<SpriteRenderer>().sprite = PlayerExplosion;
            Destroy(collider.gameObject, 0.5f);
            SoundManager.Instance.PlayAudioClip(
                   SoundManager.Instance.PlayerExplosion);
        }
    }

    public void OnDestroy()
    {
        /* Increment alien death count as alien has died */
        _invaderDeathCount++;
        /* Check if all the aliens are dead */
        if (_invaderDeathCount >= NumInvaders)
        {
            /* If all the aliens are dead signal game over */
            GameOverManager.IsGameOver = true;
        }
    }

    /// <summary>
    /// Updates alien animation frame.
    /// </summary>
    /// <returns>Yield instruction to wait for a number of seconds.</returns>
    public IEnumerator UpdateAlienFrame()
    {
        while (true)
        {
            if (SpriteRenderer.sprite == Frame1)
            {
                SpriteRenderer.sprite = Frame2;
            }
            else if (SpriteRenderer.sprite == Frame2)
            {
                SpriteRenderer.sprite = Frame1;
            }

            yield return new WaitForSeconds(_invaderFrameUpdateInterval);
        }
    }
}
