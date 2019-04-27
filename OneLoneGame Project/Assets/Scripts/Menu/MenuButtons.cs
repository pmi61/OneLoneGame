using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MenuButtons : NetworkBehaviour {

	public void ExitButton()
    {
        Application.Quit();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("ExampleScene");
        //NetworkServer.Spawn();
        GameManager.instance.InitGame();
        //GameManager.instance.enabled = true;
        //GameManager.instance.isGameRunning = true;

    }
    public void toMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
