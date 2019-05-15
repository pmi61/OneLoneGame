using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
public class Player_networking : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private GameObject GM;

    [SyncVar]
    public string nickname;


    public void setNickname(string name)
    {
        nickname = name;
    }
    

    public override void OnStartLocalPlayer()
    {
         base.OnStartLocalPlayer();
        //CmdSetPlayerName(nickname);
        if (isClient)
        {
            var GO = GameObject.Find("GameManager");
            GO.GetComponent<TimeController>().timeText = TMP;
            transform.Find("UI").gameObject.SetActive(true);
            transform.Find("Main Camera").gameObject.SetActive(true);
        }

    }
    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
    }
   
}
