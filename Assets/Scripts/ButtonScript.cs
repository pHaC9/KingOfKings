using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip sound;

    public void IveBeenClicked()
    {
        audioSource.PlayOneShot(sound);
        SceneManager.LoadScene("Gameplay");
    }
}
