using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOnScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 centerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
        this.transform.position = centerPos;

    }
}
