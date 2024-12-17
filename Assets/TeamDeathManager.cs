using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamDeathManager : NetworkBehaviour
{
    public static TeamDeathManager Instance {  get; private set; }

    private NetworkVariable<int> team1DeadCount = new NetworkVariable<int>(0);
    private NetworkVariable<int> team2DeadCount = new NetworkVariable<int>(0);

    private void Awake()
    {
        Instance = this;
    }
    
    public void ReportPlayerDeath(string teamId)

    {
        if (teamId == "A")
        {
            Debug.Log("team death a"+team1DeadCount.Value);
            team1DeadCount.Value++;
            CheckForLossCondition();
        }
        else if (teamId == "B")
        {
            Debug.Log("team death b" + team1DeadCount.Value);
            team2DeadCount.Value++;
            CheckForLossCondition();
        }
    }
    private void CheckForLossCondition()
    {
        
        if (team1DeadCount.Value >= SilverBulletManager.Instance.maxPlayer/2)
        {
            SilverBulletManager.Instance.EndRoundServerRpc("B");// Team 2 wins
        }
        else if (team2DeadCount.Value >= SilverBulletManager.Instance.maxPlayer / 2)
        {
            SilverBulletManager.Instance.EndRoundServerRpc("A"); // Team 1 wins
        }
        
    }
    public void ResetTeamDeath()
    {
        team1DeadCount.Value = 0;
        team2DeadCount.Value = 0;
    }
}
