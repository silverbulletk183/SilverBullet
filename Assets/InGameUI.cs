using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
   public static InGameUI Instance {  get; private set; }

    [SerializeField] private TextMeshProUGUI txtRoundWinA;
    [SerializeField] private TextMeshProUGUI txtRoundWinB;
    [SerializeField] private TextMeshProUGUI txtTimer;

    private void Awake()
    {
        Instance = this;
    }

    internal void UpdateScoreUI(int oldValue, int newValue)
    {
        txtRoundWinA.text = "Team A: " + SilverBulletManager.Instance.teamAWins.Value;
        txtRoundWinB.text = "Team B: " + SilverBulletManager.Instance.teamBWins.Value;
    }

    internal void SetTimer(float previousValue, float newValue)
    {
        txtTimer.text = Mathf.CeilToInt(newValue).ToString();
    }
}
