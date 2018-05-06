using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {

    float songBPM;
    float secondsPerBeat;
    float secondsPerMeasure;
    int majorBeat = 0;

    [SerializeField] GameObject Chunk;
	GameObject player;
	GameObject spawn;
	GameObject last;

    Queue<GameObject> chunks;

	[SerializeField] private Text scoreText;
	[SerializeField] private Text highScoreText;

    float timer;
    private int shake;

	float score = 0;
	float highscore = 0;
	Vector3 startPosition;
	Vector3 currentPosition;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		startPosition = player.transform.position;

        timer = 0;
        if( SceneManager.GetActiveScene().name == "Level 1")
        {
            songBPM = 55;
        }
        else
        { 
            songBPM = 90;
        }
        secondsPerBeat = 60.0f / songBPM;
        secondsPerMeasure = secondsPerBeat * 2;
        chunks = new Queue<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            majorBeat = 0;
            Pulse(true);
        }

		if(PlayerPrefs.HasKey("playerscore"))
		{
			highscore = PlayerPrefs.GetFloat("playerscore");
		}
		else
		{
			PlayerPrefs.SetFloat("playerscore", highscore);
		}
    }

	public void End(){
		PlayerPrefs.SetFloat("playerscore", highscore);
		SceneManager.LoadScene("Main");
	}
	
	// Update is called once per frame
	void Update () {

		if (score > highscore) {
			highscore = score;
		}


		currentPosition = player.transform.position;
		score = currentPosition.x - startPosition.x;
		score = (int)score;
		scoreText.text = "SCORE: " + score;
		highScoreText.text = "HIGH: " + highscore;

		if (currentPosition.y < -2) {
			End ();
		}

        timer += Time.deltaTime;

        if(timer > secondsPerBeat)
        {
            timer -= secondsPerBeat;
            Pulse();
        }

		if(CrossPlatformInputManager.GetButtonDown("Cancel")){
			End ();
		}
	}

    void Pulse(bool startChunk = false)
    {
        if (majorBeat == 0)
        {
			//numbers
			majorBeat = 2;

			// going to spawn them much higher, and set the lock position to the left ogf the game object.  So basically the most recent spawn has to fall through two of these function calls
			// when we enter the method, we want last to be locked into place to the left of the game object right away.
			if (last) {
				last.GetComponent<Rigidbody>().useGravity = false;
				last.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
				last.transform.position = this.transform.position + new Vector3(-12,0,0);
				chunks.Enqueue (last);
			}


			this.transform.position = this.transform.position + new Vector3(12,0,0);

			// then set the new "last" equal to our current spawn
			if (spawn) {
				last = spawn;
			}

			

			// then update spawn
			if (spawn && !startChunk)
			{
				Random.seed = Random.Range(0, 100000);
				spawn = Instantiate(spawn.GetComponent<NextChunks>().next[(int)Random.Range(0, spawn.GetComponent<NextChunks>().next.Count)]);
			}
			else
			{
				spawn = Instantiate(Chunk);
			}
			if (startChunk) {
				spawn.transform.position = this.transform.position;
			} else {
				float doubleSeconds = secondsPerMeasure * 2;
				spawn.transform.position = this.transform.position + new Vector3(0,-.5f * Physics.gravity.y * (doubleSeconds * doubleSeconds),0);
				spawn.GetComponent<Rigidbody>().useGravity = true;
			}

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
