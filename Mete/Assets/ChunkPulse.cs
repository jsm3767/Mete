using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPulse : MonoBehaviour {


    public List<GameObject> platforms;
	// Use this for initialization
	void Start () {
        platforms = new List<GameObject>();
		for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).tag == "DisappearOnbeat" || gameObject.transform.GetChild(i).tag == "AppearOnBeat")
                platforms.Add(gameObject.transform.GetChild(i).gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Pulse()
    {
        foreach(GameObject a in platforms)
        {
            a.gameObject.SetActive(!a.gameObject.active);
        }
    }
}
