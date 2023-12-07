using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    // Utilisation du modèle de conception singleton
    public static SoundsManager instance { get; private set; }

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip slideSound;
    public AudioClip deathSound;

    [Range(0, 1)] public float volume = 1.0f;  // Utilisation de Range pour définir une plage valide pour le volume

    private AudioClip lastPlayedSound;

    private void Awake()
    {
        // Assure qu'il n'y a qu'une seule instance de SoundsManager dans la scène
        if (instance != null)
        {
            Debug.LogWarning("Il existe déjà une instance de SoundsManager dans cette scène...");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound(AudioClip sound)
    {
        // Vérifie si le son est différent du dernier son joué
        if (sound != lastPlayedSound || !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sound, volume);
            lastPlayedSound = sound;
        }
    }

    // Méthodes spécifiques pour jouer des sons particuliers
    public void PlayJumpSound() => PlaySound(jumpSound);
    public void PlaySlideSound() => PlaySound(slideSound);
    public void PlayDeathSound() => PlaySound(deathSound);
}
