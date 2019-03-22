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

    }
    public void toMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
