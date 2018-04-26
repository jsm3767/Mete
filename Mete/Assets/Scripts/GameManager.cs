using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    float songBPM;
    float secondsPerBeat;
    float secondsPerMeasure;
    int majorBeat = 0;

    [SerializeField] GameObject Chunk;
    GameObject spawn;

    Queue<GameObject> chunks;

    float timer;
    private int shake;

	// Use this for initialization
	void Start () {
        timer = 0;
        songBPM = 90;
        secondsPerBeat = 60.0f / songBPM;
        secondsPerMeasure = secondsPerBeat * 2;
        chunks = new Queue<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            majorBeat = 0;
            Pulse(true);
        }

        
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

    void Pulse(bool startChunk = false)
    {
        Debug.Log("Timer:" + majorBeat);
        if (majorBeat == 0)
        {
            majorBeat = 2;
            if (spawn)
            {
                spawn.GetComponent<Rigidbody>().useGravity = false;
                spawn.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                spawn.transform.position = this.transform.position;
            }

            this.transform.position = this.transform.position + new Vector3(12,0,0);
            if (spawn && !startChunk)
            {
                Debug.Log("options"+ spawn.GetComponent<NextChunks>().next.Count);
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

            StartCoroutine(SlideCamera());
        }
        /* update chunks*/
        GameObject[] chunksOnScreen = GameObject.FindGameObjectsWithTag("Chunk");

        for(int i = 0; i < chunksOnScreen.Length; i++)
        {
            if(chunksOnScreen[i].GetComponent<ChunkPulse>())
                chunksOnScreen[i].GetComponent<ChunkPulse>().Pulse();
        }

        /* slide Camera */
        majorBeat -= 1;
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
