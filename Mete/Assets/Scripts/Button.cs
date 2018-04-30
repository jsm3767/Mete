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
        Debug.Log("entered");

        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            foreach (GameObject g in ObjectsToDisable)
            {
                Debug.Log("deactivating");

                g.SetActive(false);
            }
        }
    }
}
