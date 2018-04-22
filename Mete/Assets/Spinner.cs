using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    private bool direction = false;
    // Use this for initialization
    void Start()
    {
        if( Random.Range(0.0f,1.0f) > .5f)
        {
            direction = !direction;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(direction)
        { 
            transform.Rotate(new Vector3(0, 0, 1.0f ));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -1.0f));

        }
    }
}
