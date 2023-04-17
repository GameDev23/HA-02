using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool isSuperDance;
    public static Manager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        instance.isSuperDance = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
