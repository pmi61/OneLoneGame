using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public void startHost()
    {
        StartHost();
    }

    public void startClient()
    {
       StartClient();
    }

    public void disconnect()
    {
        StopClient();
        StopServer();
    }
    
}
