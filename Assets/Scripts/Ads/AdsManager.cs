using System;
using System.Threading.Tasks;
using Core;
using Core.Configs;
using Core.Signals;
using GoogleMobileAds.Api;
using UnityEngine;
using Zenject;

namespace Ads
{
    public class AdsManager : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private BannerView _bannerView;
        private RewardedAd _rewardedAd;
        private AdsConfig _config;
        private LoadLevelService _loadLevelService;

        public AdsManager(SignalBus signalBus, AdsConfig config, LoadLevelService loadLevelService)
        {
            _signalBus = signalBus;
            _config = config;
            _loadLevelService = loadLevelService;
        }

        public void Initialize()
        {
            Init();
            
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDeath);
            LoadRewardedAd();
            
            _loadLevelService.OnLoadScene += LoadBannerView;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDeath);
            _loadLevelService.OnLoadScene -= LoadBannerView;
            DestroyBannerView(_bannerView);
            DestroyRewardedAd(_rewardedAd);
        }

        private void Init()
        {
            MobileAds.Initialize(initstatus =>
            {
                if (initstatus == null)
                {
                    Debug.LogError("Google Mobile Ads initialization failed.");
                    return;
                }

                Debug.Log("Google Mobile Ads initialization complete.");
            });
        }

        private void DestroyBannerView(BannerView bannerView)
        {
            bannerView.Destroy();
        }

        private void DestroyRewardedAd(RewardedAd rewardedAd)
        {
            rewardedAd.Destroy();
        }

        private void OnPlayerDeath()
        {
            ShowRewardedAd();
        }

        private void RegisterReloadHandler(RewardedAd ad)
        {
            ad.OnAdFullScreenContentClosed += async () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");

                LoadRewardedAd();
                await Task.Delay(100);
                Time.timeScale = 0;
            };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);

                LoadRewardedAd();
            };
        }

        private void LoadBannerView()
        {
            int deviceWidth = MobileAds.Utils.GetDeviceSafeWidth();

            AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth);

            var adRequest = new AdRequest();
            _bannerView = new BannerView(_config.ADUnitIDBanner, adaptiveSize, AdPosition.Bottom);
            _bannerView.LoadAd(adRequest);
        }

        private void LoadRewardedAd()
        {
            var adRequest = new AdRequest();

            RewardedAd.Load(_config.ADUnitIDRewarded, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                _rewardedAd = ad;
                RegisterReloadHandler(ad);
            });
        }

        private void ShowRewardedAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) => { });
            }
        }
    }
}