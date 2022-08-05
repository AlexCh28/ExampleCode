using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongNetworkManager : NetworkManager
{
    [SerializeField] Transform leftPaddleSpawn, rightPaddleSpawn;
    [SerializeField] GameObject ballPrefab, scoreBoardPrefab;
    GameObject ball, scoreBoard;
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform start = numPlayers == 0 ? leftPaddleSpawn : rightPaddleSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        if (numPlayers == 2)
        {
            ball = Instantiate(ballPrefab);
            NetworkServer.Spawn(ball);
            scoreBoard = Instantiate(scoreBoardPrefab);
            NetworkServer.Spawn(scoreBoard);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        if (ball) NetworkServer.Destroy(ball);
        if (scoreBoard) NetworkServer.Destroy(scoreBoard);
    }
}
