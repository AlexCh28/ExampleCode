using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleLeftScoreUpdated))]
    int leftScore;
    [SyncVar(hook = nameof(HandleRightScoreUpdated))]
    int rightScore;

    public static event Action<int> ClientOnLeftScoreUpdated;
    public static event Action<int> ClientOnRightScoreUpdated;

    public override void OnStartServer()
    {
        Ball.OnGoal += ServerHandleGoal;
    }

    public override void OnStopServer()
    {
        Ball.OnGoal -= ServerHandleGoal;
    }

    void ServerHandleGoal(string side)
    {
        if (side == "Right") leftScore++;
        else rightScore++;
    }

    void HandleLeftScoreUpdated(int oldScore, int newScore)
    {
        ClientOnLeftScoreUpdated?.Invoke(newScore);
    }

    void HandleRightScoreUpdated(int oldScore, int newScore)
    {
        ClientOnRightScoreUpdated?.Invoke(newScore);
    }
}
