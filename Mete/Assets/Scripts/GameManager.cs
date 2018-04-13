using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    float songBPM;
    float secondsPerBeat;
    float secondsPerMeasure;

    [SerializeField] GameObject Chunk;
    GameObject spawn;

    Queue<GameObject> chunks;

    float timer;
    private int shake;

	// Use this for initialization
	void Start () {
        timer = 0;
        songBPM = 120;
        secondsPerBeat = 60.0f / songBPM;
        secondsPerMeasure = secondsPerBeat * 4;



        chunks = new Queue<GameObject>();
        Pulse(true);
        Pulse(true);
        Pulse(true);
        Pulse(true);
        Pulse();

        
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer > secondsPerMeasure)
        {
            timer -= secondsPerMeasure;
            Pulse();
        }
	}

    void Pulse(bool startChunk = false)
    {
        if (spawn)
        {
            spawn.GetComponent<Rigidbody>().useGravity = false;
            spawn.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            spawn.transform.position = this.transform.position;
        }


        this.transform.position = this.transform.position + new Vector3(12,0,0);
        if (spawn && !startChunk)
        {
            Random.seed = Random.Range(0, 100000);
            spawn = Instantiate(spawn.GetComponent<NextChunks>().next[(int)Random.Range(0, spawn.GetComponent<NextChunks>().next.Count)]);
        }
        else
        {
            spawn = Instantiate(Chunk);
        }
        spawn.transform.position = this.transform.position + new Vector3(0,-.5f * Physics.gravity.y * (secondsPerMeasure * secondsPerMeasure),0);
        spawn.GetComponent<Rigidbody>().useGravity = true;


        chunks.Enqueue(spawn);
        if (chunks.Count > 4)
        {
            GameObject fall = chunks.Dequeue();
            Rigidbody rb = fall.GetComponent<Rigidbody>();
            rb.useGravity = true;
            Destroy(fall, 4);
        }

        GameObject[] chunksOnScreen = GameObject.FindGameObjectsWithTag("Chunk");

        for(int i = 0; i < chunksOnScreen.Length; i++)
        {
            chunksOnScreen[i].GetComponent<ChunkPulse>().Pulse();
        }

        StartCoroutine(SlideCamera());
    }

    protected IEnumerator SlideCamera()
    {
        float t = 0.0f;
        float timer = secondsPerMeasure / 2.0f;

        Vector3 startPos = Camera.main.transform.position;
        Vector3 endPos = Camera.main.transform.position + new Vector3(12, 0, 0);

        while(t < timer)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, endPos, t/timer);
            t += Time.deltaTime;
            yield return 0;
        }

        yield break;
    }
}
