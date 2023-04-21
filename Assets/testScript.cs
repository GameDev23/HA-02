using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class testScript : MonoBehaviour
{
    private Vector2 worldPosition;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (worldPosition - (Vector2)transform.position).normalized;

        transform.right = direction;

        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("Direction is " + direction);
        }
    }
}
