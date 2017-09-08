//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//


namespace UrUtils.Ads
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;


    public class AdsManager : Singleton<AdsManager>, IAds
    {
        [SerializeField]
        List<AdsBase> AllSystems = null;
        [SerializeField]
        AdsBase AdsSystem = null;
        [SerializeField]
        AdsBase InterstitialsSystem = null;
        [SerializeField]
        AdsBase AdsBannerSystem = null;


        #region Behaviours
        protected new void Awake()
        {
            base.Awake();

            foreach (var ads in AllSystems)
                ads.Init();

            if (InterstitialsSystem != null)
                InterstitialsSystem.PrepareInterstitial();
            if (AdsBannerSystem != null)
                AdsBannerSystem.PrepareBanner();
        }
        #endregion


        public void ShowAd(Action<AdsResult> callback = null)
        {
            Debug.Log("AdsManager.ShowAd called");

            if (AdsSystem != null)
            {
                AdsSystem.ShowAd(callback);
            }
            else
            {
                if (callback != null)
                    callback(AdsResult.Error);
            }

        }

        public void ShowInterstitial(Action<AdsResult> callback = null)
        {
            Debug.Log("AdsManager.ShowIntersitial called");

            if (InterstitialsSystem != null)
            {
                InterstitialsSystem.ShowInterstitial(callback);
            }
            else
            {
                if (callback != null)
                    callback(AdsResult.Error);
            }

        }

        public void ShowRewardedAd(Action<AdsResult> callback = null, Action<bool> giveRewardCallback = null)
        {
            Debug.Log("AdsManager.ShowRewardedAd called");

            if (AdsSystem != null)
            {
                AdsSystem.ShowRewardedAd(callback, giveRewardCallback);
            }
            else
            {
                if (callback != null)
                    callback(AdsResult.Error);
                if (giveRewardCallback != null)
                    giveRewardCallback(false);
            }

        }

        public void ShowBanner()
        {
            if (AdsBannerSystem != null)
                AdsBannerSystem.ShowBanner();
            else
                Debug.LogError("Utils.Ads.AdsManager.AdsBannerSystem is null");
        }

        public void HideBanner()
        {
            if (AdsBannerSystem != null)
                AdsBannerSystem.HideBanner();
            else
                Debug.LogError("Utils.Ads.AdsManager.AdsBannerSystem is null");
        }

        public void RemoveBanner()
        {
            if (AdsBannerSystem != null)
                AdsBannerSystem.RemoveBanner();
            else
                Debug.LogError("Utils.Ads.AdsManager.AdsBannerSystem is null");
        }
    }
}