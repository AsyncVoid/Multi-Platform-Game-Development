using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip iceCharge;
    public AudioClip iceBreak;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChargeSound() {
        audioSource.PlayOneShot(iceCharge, 1f);
    }

    public void BreakSound() {
        Debug.Log("No");
        audioSource.PlayOneShot(iceBreak, 0.6f);
    }
}
