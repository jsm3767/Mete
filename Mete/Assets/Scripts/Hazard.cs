using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c){
		Debug.Log ("Bonk!");

		if (c.tag == "Player") {
			gameManager.End ();
		}
	}

}
