using System;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardListEntry : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerTimeText;
    [SerializeField] private TextMeshProUGUI playerLootText;
    [SerializeField] private LeaderboardSystemSetter systemSetter;

    internal void SetData(Unity.Services.Leaderboards.Models.LeaderboardEntry leaderboardItems, int index = 0, int pageIndex = 0)
    {

        string sanitized = Convert.CutHashtagAndEnding(leaderboardItems.PlayerName);
        string unSanitized = sanitized.Replace("_", " ");

        // PLayer Name Position
        playerNameText.text = unSanitized;
        
        // Leaderboard Position
        indexText.text = index.ToString();
        //
        bool isSpeedLeaderboard = pageIndex == 0;
        //
        if (leaderboardItems.Metadata == null || leaderboardItems.Metadata.Length == 0) {
            if (isSpeedLeaderboard)
                playerTimeText.text = Convert.MStoTimeString(leaderboardItems.Score);
            else
                playerLootText.text = leaderboardItems.Score + "$";
        }
        else {
            ScoreMetadata scoreMetadata = JsonConvert.DeserializeObject<ScoreMetadata>(leaderboardItems.Metadata);

            // Always use metadata as source of truth
            playerTimeText.text = Convert.MStoTimeString((int)scoreMetadata.time);
            playerLootText.text = ((int)scoreMetadata.loot) + "$";

            systemSetter.SetAsSystem(scoreMetadata.systemID, scoreMetadata.versionString);
        }
        
                
        /*
        //Debug.Log("Converting ms "+leaderboardItems.Score + " = "+Convert.MStoTimeString(leaderboardItems.Score));
        playerTimeText.text = Convert.MStoTimeString(leaderboardItems.Score);

        //Debug.Log("Metadata string = "+leaderboardItems.Metadata);

        // Percent completed metadata handeling
        if (leaderboardItems.Metadata == null || leaderboardItems.Metadata.Length == 0)
            playerLootText.text = "XX%";
        else {
            ScoreMetadata scoreMetadata = JsonConvert.DeserializeObject<ScoreMetadata>(leaderboardItems.Metadata);
            playerLootText.text = ((int)scoreMetadata.loot).ToString()+"$";
            playerTimeText.text = Convert.MStoTimeString((int)scoreMetadata.time);

            systemSetter.SetAsSystem(scoreMetadata.systemID,scoreMetadata.versionString);
            //playerNameText.text += AddSystemText(); 
        }*/

    }

    // Adding system text with sprite Asset
    private string AddSystemText(int systemIndex)
    {
        Debug.Log(this.name + " set to system type "+systemIndex);
        switch (systemIndex) {
            case 0:
                return "  <sprite=0>";
            case 1:
                return "  <sprite=1>";
            case 2:
                return "  <sprite=2>";
        }
        return "";
    }
}
