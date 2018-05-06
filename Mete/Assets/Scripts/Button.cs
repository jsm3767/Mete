using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public List<GameObject> ObjectsToDisable;
    public List<AudioSource> soundsToPlay;
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

        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<AudioSource>().Play();
            foreach (GameObject g in ObjectsToDisable)
            {
                g.SetActive(false);
            }
            foreach (AudioSource a in soundsToPlay)
            {
                a.GetComponent<AudioSource>().PlayDelayed(.1f);
            }
        }
    }
}
