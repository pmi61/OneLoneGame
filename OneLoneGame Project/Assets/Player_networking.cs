using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Player_networking : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private GameObject GM;
    public override void OnStartLocalPlayer()
    {
       // base.OnStartLocalPlayer();
       
      var GO = GameObject.Find("GameManager");
        GO.GetComponent<TimeController>().timeText = TMP;
        transform.Find("UI").gameObject.SetActive(true);
    }
    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
    }
    public override void OnStartServer()
    {
            if (isServer)
            {
          //  NetworkServer.Spawn(GM);
           // DontDestroyOnLoad(GM);
                //  NetworkServer.Spawn(GameManager.instance)
            }
    }
}
