using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    float songBPM;
    float secondsPerBeat;

    GameObject Chunk;

    float timer;
    private int shake;

	// Use this for initialization
	void Start () {
        timer = 0;
        songBPM = 60;
        secondsPerBeat = 60.0f / songBPM;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer > secondsPerBeat)
        {
            timer -= secondsPerBeat;
            Pulse();
        }
	}

    void Pulse()
    {
        Debug.Log("Boom");
    }
}
