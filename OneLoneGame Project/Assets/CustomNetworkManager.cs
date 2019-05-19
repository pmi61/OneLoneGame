using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class CustomNetworkManager : NetworkManager
{

    public Button Disconnect;
    public void StartupHost()
    {
        setPort();
        NetworkManager.singleton.StartHost();
    }

    void setPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    public void JoinGame()
    {
        setIPAdress();
        setPort();
        NetworkManager.singleton.StartClient();
    }

    void setIPAdress()
    {
       // NetworkManager.singleton.networkAddress = "127.0.0.1";
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
           // SetupMenuSceneButtons();
            StartCoroutine(SetupMenuSceneButtons());
        }
        else
        {
            SetupOtherSceneButtons();
        }
    }

    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ConnectButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ConnectButton").GetComponent<Button>().onClick.AddListener(JoinGame);

    }

    void SetupOtherSceneButtons()
    {
      // Disconnect.GetComponent<Button>().onClick.RemoveAllListeners();
      //  GameObject.Find("Disconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }

}
