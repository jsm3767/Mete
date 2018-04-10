using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    float songBPM;
    float secondsPerBeat;

    [SerializeField] GameObject Chunk;

    Queue<GameObject> chunks;

    float timer;
    private int shake;

	// Use this for initialization
	void Start () {
        timer = 0;
        songBPM = 60;
        secondsPerBeat = 60.0f / songBPM;

        chunks = new Queue<GameObject>();
        Pulse();
        Pulse();
        Pulse();
        Pulse();
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
        this.transform.position = this.transform.position + new Vector3(12,0,0);
        GameObject spawn = Instantiate(Chunk);
        spawn.transform.position = this.transform.position;
        Debug.Log("Boom");


        chunks.Enqueue(spawn);
        if (chunks.Count > 4)
        {
            GameObject fall = chunks.Dequeue();
            Rigidbody rb = fall.GetComponent<Rigidbody>();
            rb.useGravity = true;
            Destroy(fall, 10);
        }
    }
}
