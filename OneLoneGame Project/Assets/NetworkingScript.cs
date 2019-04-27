using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkingScript : NetworkBehaviour
{

    public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (!GameManager.instance.isGameRunning)
            GameManager.instance.InitGame();
        var playerPrefab = GetComponent<NetworkManager>().playerPrefab;
        var playerSpawnPos = GetComponent<NetworkManager>().spawnPrefabs[Random.Range(0, 1)].transform.position;
        var player = Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

}
