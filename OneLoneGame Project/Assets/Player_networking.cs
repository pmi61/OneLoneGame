using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_networking : NetworkBehaviour
{

    public override void OnStartLocalPlayer()
    {
       // base.OnStartLocalPlayer();
       if (isServer)
        {
          //  NetworkServer.Spawn(GameManager.instance)
        }
        transform.Find("UI").gameObject.SetActive(true);
    }
    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
    }
}
