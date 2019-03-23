using UnityEngine;
using UnityEngine.SceneManagement;

class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //private GameObject canvas;
    private GameObject menus;

    private GameObject deathScreen;

    public bool isGameRunning;

    private void Awake()
    {
        Debug.Log("GameManaer Awake");

        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;

            Debug.Log("Create instance of GameManager");
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

            Debug.Log("Destroy another instance of GameManager");
        }

        DontDestroyOnLoad(gameObject);

        InitGame();
    }


    void InitGame()
    {
        Debug.Log("In GameManager : InitGame()");

        isGameRunning = true;
        menus = GameObject.Find("Menus");

        if (menus == null)
        {
            Debug.Log("Can't find Death Screen");
        }
    }

    public void GameOver()
    {
        Debug.Log("In GameManager : GameOver()");

        //deathScreen.gameObject.SetActive(true);

        menus.GetComponent<Canvas>().enabled = true;

        isGameRunning = false;
        
        //Disable this GameManager.
        enabled = false;
    }
}

