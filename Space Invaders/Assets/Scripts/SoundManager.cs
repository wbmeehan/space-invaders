using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields

    /* Audio clips */
    public AudioClip InvaderKilled;
    public AudioClip PlayerShooting;
    public AudioClip PlayerExplosion;

    /* Audio source */
    private AudioSource _audioSource;

    #endregion

    public static SoundManager Instance { get; set; }

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Play an audio clip.
    /// </summary>
    /// <param name="audioClip">Audio clip to be played.</param>
   public void PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
}
