using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Brightness : MonoBehaviour
{
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            adjustBrightness("darker");
        } 
        else if (Input.GetKeyDown(KeyCode.P))
        {
            adjustBrightness("brighter");
        }
    }

    private void adjustBrightness(string mode)
    {
        float colorCode = sr.color.r;
        Debug.Log(colorCode);

        if(mode == "darker")
        {
            if (colorCode <= .1f) return;
            colorCode -= .1f;
            sr.color = new Color(colorCode, colorCode, colorCode);

        } 
        else if(mode == "brighter")
        {
            if(colorCode >= 1) return;
            colorCode += .1f;
            sr.color = new Color(colorCode, colorCode, colorCode);
        }
    }
}
