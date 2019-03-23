using UnityEngine;
using UnityEngine.SceneManagement;

class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //private GameObject canvas;
    private GameObject menus;

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
    }

    private void Update()
    {

    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.InitGame();
    }

    void InitGame()
    {
        Debug.Log("In GameManager : InitGame()");

        menus = GameObject.FindGameObjectWithTag("Menus");

        if (menus == null)
        {
            Debug.Log("Can't find Menus");
        }
    }

    public void GameOver()
    {
        Debug.Log("In GameManager : GameOver()");

        menus.transform.Find("DeathScreen").gameObject.SetActive(true);
        
        //Disable this GameManager.
        enabled = false;
    }
}

