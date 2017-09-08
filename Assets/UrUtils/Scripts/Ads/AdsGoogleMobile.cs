//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace UrUtils.Ads
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if ADS_ADMOB
    using GoogleMobileAds.Api;
#endif

    [CreateAssetMenu(fileName = "Google Mobile Ads", menuName = PathBase + "Google Mobile Ads")]
    public class AdsGoogleMobile : AdsBase
    {
#pragma warning disable 414
        [SerializeField]
        PlatformString BannerID = new PlatformString("ca-app-pub-*/*");
        [SerializeField]
        PlatformString InterstitialID = new PlatformString("ca-app-pub-*/*");

        [SerializeField, Space(5)]
        List<string> TestDevices = null;
#if ADS_ADMOB
        [SerializeField]
        AdPosition AdPosition = AdPosition.Bottom;
#endif
#pragma warning restore 414

#if ADS_ADMOB
        BannerView BannerView;
        InterstitialAd Interstitial;
#endif

        public override void ShowAd(Action<AdsResult> callback = null)
        {
            Debug.LogError("AdsGoogleMobile.ShowAd() not implemented");
        }

        public override void PrepareInterstitial()
        {
            base.PrepareInterstitial();

#if ADS_ADMOB
            Interstitial = new InterstitialAd(InterstitialID);
            var request = new AdRequest.Builder();
            if (!TestDevices.Empty())
            {
                request.AddTestDevice(AdRequest.TestDeviceSimulator);
                foreach (var device in TestDevices)
                    request.AddTestDevice(device);
            }
            Interstitial.LoadAd(request.Build());
#endif
        }

        public override void ShowInterstitial(Action<AdsResult> callback = null)
        {
#if ADS_ADMOB
            if (Interstitial == null)
                PrepareInterstitial();

            if (Interstitial != null)
            {
                if (Interstitial.IsLoaded())
                {
                    Interstitial.Show();
                    if (callback != null)
                        callback(AdsResult.Watched);
                    PrepareInterstitial();
                }
                else
                {
                    Debug.LogError("AdsGoogleMobile.ShowInterstitial - Interstitial is not loaded");
                    if (callback != null)
                        callback(AdsResult.Error);
                }
            }
            else
            {
                Debug.LogError("AdsGoogleMobile.ShowInterstitial() - Interstitial is null");
                if (callback != null)
                    callback(AdsResult.Error);
            }
#endif
        }

        public override void ShowRewardedAd(Action<AdsResult> callback = null, Action<bool> giveRewardCallback = null)
        {
            Debug.LogError("AdsGoogleMobile.ShowRewardedAd() not implemented");
        }


        public override void PrepareBanner()
        {
            base.PrepareBanner();

#if ADS_ADMOB
            var bannerView = new BannerView(BannerID, AdSize.Banner, AdPosition);
            var request = new AdRequest.Builder();
            if (!TestDevices.Empty())
            {
                request.AddTestDevice(AdRequest.TestDeviceSimulator);
                foreach (var device in TestDevices)
                    request.AddTestDevice(device);
            }
            bannerView.LoadAd(request.Build());
            BannerView = bannerView;
#endif
        }

        public override void ShowBanner()
        {
#if ADS_ADMOB
            if (BannerView == null)
                PrepareBanner();

            if (BannerView != null)
                BannerView.Show();
            else
                Debug.LogError("AdsGoogleMobile.ShowBanner() - Banner is null");
#endif
        }

        public override void HideBanner()
        {
#if ADS_ADMOB
            if (BannerView != null)
                BannerView.Hide();
#endif
        }

        public override void RemoveBanner()
        {
#if ADS_ADMOB
            if (BannerView != null)
            {
                BannerView.Hide();
                BannerView.Destroy();
                BannerView = null;
            }
#endif
        }
    }
}