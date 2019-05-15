using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_ID : NetworkBehaviour
{
    [SyncVar]
    public string nickname;
    private NetworkInstanceId playerNetID;
    private Transform myTransform;


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        GetNetworkIdentity();
        SetIdentity();
    }

    private void Awake()
    {
        myTransform = transform;
    }


    [Client]
    void GetNetworkIdentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }
    
    void SetIdentity()
    {
        if(!isLocalPlayer)
        {
            myTransform.name = nickname;
        }
        else
        {
            myTransform.name = MakeUniqueIdentity();
        }
    }

    string MakeUniqueIdentity()
    {
        string uniqueName = "Player " + playerNetID.ToString();
        return uniqueName;
    }

    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        nickname = name;
    }

    private void Update()
    {
        if (myTransform.name == "" || myTransform.name == "Character(Clone)")
        {
            SetIdentity();
        }
        var a = transform.Find("Nickname");
        a = a.transform.Find("Text");
        a.gameObject.GetComponent<Text>().text = myTransform.name;
    }

}
