using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public List<GameObject> ObjectsToDisable;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectsToDisable = (new List<GameObject>(GameObject.FindGameObjectsWithTag("Disable")));

        GetComponent<AudioSource>().Play();

        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<AudioSource>().Play();
            foreach (GameObject g in ObjectsToDisable)
            {
                g.SetActive(false);
            }

        }
    }
}
