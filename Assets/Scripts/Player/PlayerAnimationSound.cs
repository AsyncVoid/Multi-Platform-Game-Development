using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip walkClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void WalkSound()
    {

        int rng = Random.Range(0, 1);

        switch (rng)
        {
            case 0:
                audioSource.PlayOneShot(walkClip, 0.1f);
                break;
            case 1:
                audioSource.PlayOneShot(walkClip, 0.05f);
                break;
            default:
                break;
        }
    }
}
