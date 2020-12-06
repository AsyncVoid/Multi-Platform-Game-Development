using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip iceCharge;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChargeSound() {
        audioSource.PlayOneShot(iceCharge, 1f);
    }
}
