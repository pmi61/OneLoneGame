using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

	public void ExitButton()
    {
        Application.Quit();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("ExampleScene");
        GameManager.instance.InitGame();
        //GameManager.instance.enabled = true;
        //GameManager.instance.isGameRunning = true;

    }
    public void toMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
