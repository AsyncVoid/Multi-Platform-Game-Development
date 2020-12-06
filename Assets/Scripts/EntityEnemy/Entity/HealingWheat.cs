using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingWheat : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Sway());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Sway() {
        int range = Random.Range(10, 25);
        yield return new WaitForSeconds(range);

        animator.SetTrigger("Sway");
        StartCoroutine(Sway());
    }
}
