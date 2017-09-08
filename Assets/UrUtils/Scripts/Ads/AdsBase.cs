//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//


namespace UrUtils.Ads
{
    using System;
    using UnityEngine;


    public abstract class AdsBase : ScriptableObject, IAds
    {
        protected const string PathBase = "Ads/";

        public virtual void Init() { }

        public abstract void ShowAd(Action<AdsResult> callback = null);

        public virtual void PrepareInterstitial() { }
        public abstract void ShowInterstitial(Action<AdsResult> callback = null);

        public abstract void ShowRewardedAd(Action<AdsResult> callback = null, Action<bool> giveRewardCallback = null);

        public virtual void PrepareBanner() { }
        public abstract void ShowBanner();
        public abstract void HideBanner();
        public abstract void RemoveBanner();
    }


    public interface IAds
    {
        void ShowAd(Action<AdsResult> callback = null);
        void ShowRewardedAd(Action<AdsResult> callback = null, Action<bool> giveRewardCallback = null);

        void ShowBanner();
        void HideBanner();
        void RemoveBanner();
    }

    public enum AdsResult
    {
        Error,
        Skipped,
        Watched
    }
}