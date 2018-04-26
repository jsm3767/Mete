using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x,gameObject.transform.position.y), Vector2.up);
        if(hit.transform.tag == "Player")
        {
            Debug.Log("hit");
            hit.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1.0f), ForceMode2D.Force);
        }
    }
}
