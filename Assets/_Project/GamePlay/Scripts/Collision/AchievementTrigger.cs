using UnityEngine;

public class AchievementTrigger : BaseTrigger
{

    [SerializeField] private string _achievementName = "";


    public override void OnTriggerEnter(Collider collider)
    {
#if !DISABLESTEAMWORKS
        if (!string.IsNullOrEmpty(_achievementName))
        {
            Steamworks.SteamUserStats.SetAchievement(_achievementName);
            Steamworks.SteamUserStats.StoreStats();
        }
#endif

        base.OnTriggerEnter(collider);
    }
}
