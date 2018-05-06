using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    private GameObject player;
    public float blowForce= 15.0f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(LayerMask.GetMask("Ignore Raycast"));
        
        RaycastHit2D[] hits = new RaycastHit2D[10];

        for (int index = 0; index < 10; index++) 
        {
            hits[index] = Physics2D.Raycast((Vector2)gameObject.transform.position + new Vector2(-1.25f + .25f*index,0.0f),
               (Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z) * Vector2.up), 
                100.0f, 
                ~LayerMask.GetMask("Ignore Raycast"));
        }

        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + ((Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z ) * Vector2.up) * 100.0f )  );
        
        if ( hits[0].transform.tag == "Player" ||
            hits[1].transform.tag == "Player" ||
            hits[2].transform.tag == "Player" ||
            hits[3].transform.tag == "Player" ||
            hits[4].transform.tag == "Player" ||
            hits[5].transform.tag == "Player" ||
            hits[6].transform.tag == "Player" ||
            hits[7].transform.tag == "Player" ||
            hits[8].transform.tag == "Player" ||
            hits[9].transform.tag == "Player" )
        {
            Debug.Log("hit");
            player.transform.GetComponent<Rigidbody2D>().AddForce(
                Quaternion.Euler(0, 0, gameObject.transform.rotation.z * (180 / Mathf.PI)) * (Vector2.up * blowForce),
                ForceMode2D.Force);
        }
    }
}
