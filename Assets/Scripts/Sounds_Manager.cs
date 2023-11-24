using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds_Manager : MonoBehaviour
{
    public static Sounds_Manager instance;

    AudioSource audioData;
    public AudioClip jumpSound;
    public AudioClip slideSound;
    public AudioClip deathSound;

    public int volume = 10;

    // Ajoutez une variable pour suivre le dernier son joué
    private AudioClip lastPlayedSound;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il existe déjà une instance de Sounds_Manager dans cette scène...");
            return;
        }
        instance = this;

        audioData = GetComponent<AudioSource>();
    }

    public void playSound(AudioClip sound)
    {
        // Vérifiez si le son est différent du dernier son joué
        if ((sound != lastPlayedSound || !audioData.isPlaying) && lastPlayedSound != deathSound)
        {
            audioData.PlayOneShot(sound, volume);
            lastPlayedSound = sound;
        }
        else
        {
            Debug.Log("Le son ne peut pas être joué actuellement.");
        }
    }

    public void playJumpSound()
    {
        playSound(jumpSound);
    }

    public void playSlideSound()
    {
        playSound(slideSound);
    }

    public void playDeadSound()
    {
        playSound(deathSound);
    }
}
