using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Brightness : MonoBehaviour
{
    SpriteRenderer sr;
    bool isSquidGameInvoked;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        isSquidGameInvoked = false;
    }

    // Update is called once per frame
    void Update()
    {
        // adjust brightness with O and P
        if (Input.GetKeyDown(KeyCode.O))
        {
            adjustBrightness("darker");
        } 
        else if (Input.GetKeyDown(KeyCode.P))
        {
            adjustBrightness("brighter");
        }
        if (Manager.instance.isSuperDance)
        {
            StartCoroutine(superDanceLighting());

        }
        //////////////////////////////

        // start change lightings for squidgame if squidgame is active   otherwise disable invoke
        if (Manager.instance.isSquidGame && !isSquidGameInvoked)
        {
            // switch background to green and red
            isSquidGameInvoked = true;
            InvokeRepeating(nameof(greenLightRedLight), 2f, 5f);




        }
        else if(!Manager.instance.isSquidGame && isSquidGameInvoked)
        {
            isSquidGameInvoked = false;
            CancelInvoke();
            sr.color = Color.white;
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

    IEnumerator superDanceLighting()
    {
        sr.color = Color.black;
        while(Manager.instance.isSuperDance)
        {
            yield return null;
        }
        sr.color = Color.white;
    }

    void greenLightRedLight()
    {
        Debug.Log("Change lighting squid");
        if (sr.color == Color.green)
        {
            sr.color = Color.red;
            Manager.instance.isRedPhaseOfSquidGame = true;
        }
        else 
        {
            sr.color = Color.green;
            Manager.instance.isRedPhaseOfSquidGame = false;
        }


    }
}
