using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Fields

    /* Is the player exploding? */
    public static bool IsExploding;

    /* Speed */
    public float Speed = 30;

    /* Player bullet */
    public GameObject PlayerBullet;

    /* Rigid body */
    private Rigidbody2D _rigidBody;

    /* Time the last bullet was fired */
    private float timeLastShot;

    public static Vector2 InitialPosition;

    #endregion

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (InitialPosition == Vector2.zero)
        {
            InitialPosition = transform.position;
        }
        else
        {
            transform.position = InitialPosition;
        }
    }

    public void FixedUpdate()
    {
        if (!IsExploding)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            Vector2 direction = new Vector2(moveHorizontal, 0);
            _rigidBody.velocity = direction * Speed;
        }
        else
        {
            /* If the player is exploding stop moving */
            _rigidBody.velocity = new Vector2(0, 0);
        }
    }

    public void Update()
    {
        /* If the player presses the spacebar, fire if the weapon has
         *  recharged */
        if (Input.GetKeyDown("space") && Recharged())
        {
            timeLastShot = Time.timeSinceLevelLoad;
            Instantiate(PlayerBullet, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayAudioClip(
                SoundManager.Instance.PlayerShooting);
        }
    }

    public void OnDestroy()
    {
        if (IsExploding)
        {
            GameManager.IsGameOver = true;
            IsExploding = false;
        }
        else
        {
            InitialPosition = transform.position;
        }
    }

    /// <summary>
    /// Determines if the player's weapon has recharged.
    /// </summary>
    /// <returns>Has the player's weapon recharged?</returns>
    private bool Recharged()
    {
        /* If 0.5 seconds has passed the weapon has recharged */
        return Time.timeSinceLevelLoad >= (timeLastShot + 0.25f);
    }
}
