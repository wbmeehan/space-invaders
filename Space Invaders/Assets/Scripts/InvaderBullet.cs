using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderBullet : MonoBehaviour
{
    #region Fields

    /* Player explosion animation frame */
    public Sprite PlayerExplosion;
    /* Speed */
    public float Speed = 40;
    /* Rigid body */
    private Rigidbody2D _rigidBody;

    #endregion

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.velocity = Vector2.down * Speed;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Shield":
                /* Invader bullet hit a shield */
                /* Destroy bullet and shield */
                Destroy(collider.gameObject);
                Destroy(gameObject);
                break;
            case "Wall":
                /* Invader bullet hit a wall */
                /* Destroy bullet */
                Destroy(gameObject);
                break;
            case "Player":
               /* Invader bullet hit a player */
               /* Destroy bullet and player */
                Destroy(gameObject);
                Destroy(collider.gameObject, 0.5f);
                Player.IsExploding = true;
                SoundManager.Instance.PlayAudioClip(
                    SoundManager.Instance.PlayerExplosion);
                collider.GetComponent<SpriteRenderer>().sprite = PlayerExplosion;
                break;
        }
    }

    public void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }
}
