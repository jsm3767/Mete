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
        
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        for (int index = 0; index < 10; index++) 
        {
            RaycastHit2D temp = Physics2D.Raycast((Vector2)gameObject.transform.position + new Vector2(-1.25f + .25f * index, 0.0f),
               (Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z) * Vector2.up),
                100.0f,
                ~LayerMask.GetMask("Ignore Raycast"));
            if(temp)
            { 
                hits.Add(temp);
            }

        }

        bool hitPlayer = false;
        foreach( RaycastHit2D r in hits)
        {
            if (r.transform.tag == "Player")
                hitPlayer = true;
        }

        //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + ((Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z ) * Vector2.up) * 100.0f )  );
        
        if ( hitPlayer)
        {
            Debug.Log("hit");
            player.transform.GetComponent<Rigidbody2D>().AddForce(
                Quaternion.Euler(0, 0, gameObject.transform.rotation.z * (180 / Mathf.PI)) * (Vector2.up * blowForce),
                ForceMode2D.Force);
        }
    }
}
