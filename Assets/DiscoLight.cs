using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiscoLight : MonoBehaviour
{
    SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(changeLighting());
    }

    // Update is called once per frame
    void Update()
    {

        
    }



    private void randomColor()
    {
        if(!Manager.instance.isSuperDance)
            sr.color = Random.ColorHSV();
    }

    IEnumerator changeLighting()
    {
        while (true)
        {
            float t = Random.Range(1f, 5f);
            yield return new WaitForSeconds(t);

            randomColor();
        }

    }
}
