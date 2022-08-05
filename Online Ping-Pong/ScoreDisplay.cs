using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] Text leftText, rightText;

    void Start()
    {
        ScoreBoard.ClientOnLeftScoreUpdated += HandleLeftScoreUpdate;
        ScoreBoard.ClientOnRightScoreUpdated += HandleRightScoreUpdate;
    }

    void OnDestroy()
    {
        ScoreBoard.ClientOnLeftScoreUpdated -= HandleLeftScoreUpdate;
        ScoreBoard.ClientOnRightScoreUpdated -= HandleRightScoreUpdate;
    }

    void HandleRightScoreUpdate(int score)
    {
        rightText.text = score.ToString();
    }

    void HandleLeftScoreUpdate(int score)
    {
        leftText.text = score.ToString();
    }
}
