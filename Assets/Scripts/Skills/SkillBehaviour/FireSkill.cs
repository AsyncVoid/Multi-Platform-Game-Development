using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip startFire;
    public AudioClip moveFire;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playFireSound() {
        audioSource.PlayOneShot(startFire, 0.5f);
    }

    public void playFireMoving() {
        audioSource.PlayOneShot(moveFire, 0.7f);
    }
}
