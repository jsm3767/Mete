using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    public void Startgame()
    {
        SceneManager.LoadScene("Level");
    }

    public void StartPuzzleMode()
    {
        SceneManager.LoadScene("Level 1");

    }

    public void HowToPlay()
	{
		SceneManager.LoadScene ("HowToPlay");
	}

	public void Menu()
	{
		SceneManager.LoadScene ("Main");
	}

	public void Credits()
	{
		SceneManager.LoadScene ("Credits");
	}

    public void Quit()
    {
        Application.Quit();
    }
}
