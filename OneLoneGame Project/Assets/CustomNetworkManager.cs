using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public void startHost()
    {
        base.StartHost();
    }

    public void startClient()
    {
        base.StartClient();
    }

    public void disconnect()
    {
        base.StopClient();
        base.StopServer();
    }
    
}
