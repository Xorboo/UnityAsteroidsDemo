//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#elif UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif


public class SocialManager : Singleton<SocialManager>
{

    #region Behaviours
    void Start()
    {
        Debug.LogFormat("SocialManager.Start authenticated={0}", IsAuthenticated);
        if (IsAuthenticated)
        {
            OnAuthenticate(true);
        }
        else
        {
#if UNITY_ANDROID
            var config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
#endif
            Social.localUser.Authenticate(success =>
            {
                Debug.LogFormat("Social.localUser.Authenticate success={0} authenticated={1}", success, IsAuthenticated);
                OnAuthenticate(success);
            });
        }
    }
    #endregion


    bool IsAuthenticated { get { return Social.localUser.authenticated; } }


    #region Leaderboards
    public void ShowLeaderboard(string leaderboardID)
    {
        if (IsAuthenticated)
        {
            OpenLeaderboard(leaderboardID);
        }
        else
        {
            Social.localUser.Authenticate(success =>
            {
                //success = Social.localUser.authenticated;
                if (success)
                    OpenLeaderboard(leaderboardID);
                else
                    Debug.LogWarning("SocialManager.ShowLeaderboard - authentication failure, can't open leaderboard");
            });
        }
    }

    void OpenLeaderboard(string leaderboardID)
    {
#if UNITY_EDITOR
        Debug.Log("SocialManager.ShowLeaderboard call (no logic for editor)");
#elif UNITY_IOS
        Social.ShowLeaderboardUI();
#elif UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
#else
        Debug.LogWarningFormat("SocialManager.ShowLeaderboard not implemented on '{0}'", Application.platform);
#endif
    }
    #endregion


    #region Achievements
    public void TryReportProgress(string achievementId)
    {
        if (IsAuthenticated)
            ReportProgress(achievementId);
    }

    static void ReportProgress(string achievementId)
    {
        Social.ReportProgress(achievementId, 100.0, result =>
        {
            Debug.LogFormat("SocialManager.ReportProgress({0}) result: {1}", achievementId, result);
        });
    }

    public void TryReportScore(string leaderboardId, long score)
    {
        if (IsAuthenticated)
            ReportScore(leaderboardId, score);
    }

    static void ReportScore(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success =>
        {
            if (success)
                Debug.LogFormat("SocialManager.ReportScore({0}, {1}) Successfully!", leaderboardId, score);
            else
                Debug.LogWarningFormat("SocialManager.ReportScore({0}, {1}) Failed!", leaderboardId, score);
        });
    }

    static void OnAchievementsWasLoaded(IAchievement[] achievements)
    {
        var count = achievements.Length;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("SocialManager.OnAchievementsWasLoaded() Got {0} achievements", count);
        for (var i = 0; i < count; ++i)
        {
            var achievement = achievements[i];
            stringBuilder.AppendFormat("\n id={0} completed={1} hidden={2} lastReportedDate={3} percentCompleted={4}",
                achievement.id,
                achievement.completed,
                achievement.hidden,
                achievement.lastReportedDate,
                achievement.percentCompleted);
        }
        Debug.Log(stringBuilder);
    }
    #endregion


    static void OnAuthenticate(bool success)
    {
        Debug.LogFormat("SocialManager.OnAuthenticate, success={0}", success);
        if (success)
        {
#if UNITY_IOS
            GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
            Social.LoadAchievements(OnAchievementsWasLoaded);
        }
    }
}