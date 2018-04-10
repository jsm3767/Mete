using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChunkLoading : MonoBehaviour {

    public static ChunkLoading Instance;
    public static SceneManager SceneManagerInstance;
    //[SerializeField] GameObject ChunkOne;
    //[SerializeField] GameObject ChunkTwo;
    //[SerializeField] GameObject ChunkThree;
    //[SerializeField] GameObject ChunkFour;
    //[SerializeField] GameObject ChunkFive;
    //[SerializeField] GameObject ChunkSix;


    // Use this for initialization
    void Awake () {
		if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
       LoadScene(1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }

    void UnloadScene(Scene toUnload)
    {
        if (toUnload.name != "Main")
            StartCoroutine(UnloadSceneCoruitine(toUnload));

    }

    IEnumerator UnloadSceneCoruitine(Scene sceneToUnload)
    {
        while (!sceneToUnload.isLoaded)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(sceneToUnload.buildIndex);
    }

    public void CreateNewChunk(List<GameObject> Platforms)
    {
        UnityEngine.SceneManagement.Scene rootScene = Platforms[0].scene;
        GameObject newChunk = new GameObject();

        foreach(GameObject a in Platforms)
        {
            a.transform.SetParent(newChunk.transform, false);
            a.transform.SetAsLastSibling();
        }
        UnloadScene(rootScene);
    }
}
