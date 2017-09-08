//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//


namespace UrUtils.Ads
{
    using System;
    using UnityEngine;
#if ADS_UNITY
    using UnityEngine.Advertisements;
#endif

    [CreateAssetMenu(fileName = "UnityAds", menuName = PathBase + "Unity Ads")]
    public class AdsUnity : AdsBase
    {
#pragma warning disable 414
        [SerializeField]
        string RewardedVideoID = "";
#pragma warning restore 414


        public override void ShowAd(Action<AdsResult> callback = null)
        {
#if ADS_UNITY
            if (Advertisement.IsReady())
            {
                var options = new ShowOptions
                {
                    resultCallback = (result) => OnAdFinished(result, callback)
                };
                Advertisement.Show(options);
            }
            else
                OnAdFinished(ShowResult.Failed, callback);
#endif
        }

        public override void ShowInterstitial(Action<AdsResult> callback = null)
        {
            Debug.LogError("AdsUnity.ShowInterstitial() not implemented");
        }

        public override void ShowRewardedAd(Action<AdsResult> callback = null, Action<bool> giveRewardCallback = null)
        {
#if ADS_UNITY
            if (Advertisement.IsReady())
            {
                var options = new ShowOptions
                {
                    resultCallback = (result) => OnRewardedAdFinished(result, callback, giveRewardCallback)
                };
                Advertisement.Show(RewardedVideoID, options);
            }
            else
                OnRewardedAdFinished(ShowResult.Failed, callback, giveRewardCallback);
#endif
        }

        public override void ShowBanner()
        {
            Debug.LogError("AdsUnity.ShowBanner() not implemented");
        }

        public override void HideBanner()
        {
            Debug.LogError("AdsUnity.HideBanner() not implemented");
        }

        public override void RemoveBanner()
        {
            Debug.LogError("AdsUnity.RemoveBanner() not implemented");
        }

#if ADS_UNITY
        void OnAdFinished(ShowResult adsResult, Action<AdsResult> callback)
        {
            if (callback != null)
            {
                var result = ParseResult(adsResult);
                callback(result);
            }
        }


        void OnRewardedAdFinished(ShowResult adsResult, Action<AdsResult> callback, Action<bool> giveRewardCallback)
        {
            OnAdFinished(adsResult, callback);

            if (giveRewardCallback != null)
                giveRewardCallback(adsResult == ShowResult.Finished);
        }

        AdsResult ParseResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Failed:
                    return AdsResult.Error;
                case ShowResult.Skipped:
                    return AdsResult.Skipped;
                case ShowResult.Finished:
                    return AdsResult.Watched;
                default:
                    Debug.LogErrorFormat("AdsUnity.ParseResult Unknown ShowResult: {0}", result);
                    return AdsResult.Error;
            }
        }
#endif
    }
}