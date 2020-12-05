using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private TextMesh damageInd;

    // Start is called before the first frame update
    void Awake()
    {
        damageInd = GetComponent<TextMesh>();
    }

    void Start() {
        StartCoroutine(DestroySelf());
        transform.position += new Vector3(Random.Range(0.0f, 1.0f), 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime;
    }

    public void SetText(string text) {
        damageInd.text = text;
    }

    public void HealColour() {
        damageInd.color = Color.green;
    }

    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
