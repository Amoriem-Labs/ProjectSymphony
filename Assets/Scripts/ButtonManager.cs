using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] buttonSounds;

    public void ButtonHover()
    {
        audioSource.PlayOneShot(buttonSounds[0]);
    }
    public void ConfirmButton()
    {
        audioSource.PlayOneShot(buttonSounds[1]);
    }
    public void CancelButton()
    {
        audioSource.PlayOneShot(buttonSounds[2]);
    }
    public void MapPullUp()
    {
        audioSource.PlayOneShot(buttonSounds[3]);
    }
}
