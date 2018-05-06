using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounciness = 30.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            { 
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, bounciness), ForceMode2D.Impulse);                GetComponentInChildren<AudioSource>().Play();
                GetComponentInChildren<AudioSource>().volume = 1.5f;
                GetComponentInChildren<AudioSource>().Play();
            }
        }
    }
}
