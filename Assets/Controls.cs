using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] GameObject discoLight;
    Camera cam;
    GameObject obj;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
        {
            float x = Random.Range(0.0f, 1.0f);
            float y = Random.Range(0.0f, 1.0f);

            Vector2 pos = cam.ViewportToWorldPoint(new Vector2(x, y));

            GameObject light = Instantiate(discoLight.gameObject,  pos, Quaternion.identity);
        }
    }
}
