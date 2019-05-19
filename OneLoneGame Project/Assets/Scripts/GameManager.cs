using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;

    //private GameObject canvas;
    private GameObject menus;
    private GameObject UI;
    /* переменные для меню */
    private bool isInMenu = false;

    public bool IsInMenu
    {
        get
        {
            return isInMenu;
        }
    }

    public bool isGameRunning;

    /* главный источник освещения */
    public TimeController TimeControl;

    private void Start()
    {
        menus = GameObject.Find("ESCMenu");
        UI = GameObject.Find("Menus");
        isInMenu = false;
    }

    private void Awake()
    {
        Debug.Log("GameManaer Awake");

        // Проверяем существование instance
        if (instance == null)
        {
            // Если не существует, инициализируем
            instance = this;

            Debug.Log("Create instance of GameManager");
        }
        // Если instance уже существует, и он не ссылается на текущий экземпляр:
        else if (instance != this)
        {
            // Унитожаем его. Это соответствует концепции синглетного класса,
            // которая значит, что во всём приложении может быть только один экземпляр такого класса
            Destroy(gameObject);

            Debug.Log("Destroy another instance of GameManager");
        }

        DontDestroyOnLoad(gameObject);
       if (isServer)
       InitGame();
    }

     public void InitGame()
    {
        if (!isServer) return;
        Debug.Log("In GameManager : InitGame()");

        isGameRunning = true;
       

        if (menus == null)
        {
            Debug.Log("Can't find Death Screen");
        }
        TimeControl = GetComponent<TimeController>();
      
    }

    public void GameOver()
    {
        if (!isLocalPlayer)
            return;
        Debug.Log("In GameManager : GameOver()");

        isInMenu = true;
        menus.transform.Find("DeathScreen").gameObject.SetActive(true);
        UI.GetComponent<Canvas>().enabled = false;
        menus.GetComponent<Canvas>().enabled = true;

        isGameRunning = false;
        
        // Disable GameManager.
        enabled = false;
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

    public void OnESC()
    {
        menus.gameObject.SetActive(!IsInMenu);
        UI.GetComponent<Canvas>().enabled = !UI.GetComponent<Canvas>().enabled;
        isInMenu = !isInMenu;
        menus.GetComponent<Canvas>().enabled = !menus.GetComponent<Canvas>().enabled;

    }
    
    public void OnContinue()
    {
        UI.GetComponent<Canvas>().enabled = !UI.GetComponent<Canvas>().enabled;
        menus.gameObject.SetActive(false);
        isInMenu = false;
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }
}

