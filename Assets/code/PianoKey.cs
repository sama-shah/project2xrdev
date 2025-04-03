using UnityEngine;

public class PianoKey : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on " + gameObject.name);
        }
    }

    void OnMouseDown()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
