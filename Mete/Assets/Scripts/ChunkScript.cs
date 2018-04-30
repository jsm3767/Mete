using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        List<GameObject> children = new List<GameObject>();
      for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        ChunkLoading.Instance.CreateNewChunk(children);
        Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
