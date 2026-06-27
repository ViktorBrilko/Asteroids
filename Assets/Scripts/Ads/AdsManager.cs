using System;
using Core;
using Core.Configs;
using Core.Signals;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;
using Zenject;

namespace Ads
{
    public class AdsManager : IInitializable, IDisposable
    {
        private readonly AdsConfig _config;
        private readonly LoadLevelService _loadLevelService;
        private readonly SignalBus _signalBus;
        private BannerView _bannerView;
        private RewardedAd _rewardedAd;

        public AdsManager(SignalBus signalBus, AdsConfig config, LoadLevelService loadLevelService)
        {
            _signalBus = signalBus;
            _config = config;
            _loadLevelService = loadLevelService;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDeath);
            _loadLevelService.OnLoadScene -= LoadBannerView;
            DestroyBannerView(_bannerView);
            DestroyRewardedAd(_rewardedAd);
        }

        public void Initialize()
        {
            Init();

            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDeath);
            LoadRewardedAd();

            _loadLevelService.OnLoadScene += LoadBannerView;
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
                await UniTask.Delay(100);
               _signalBus.Fire(new PauseGameSignal(true));
            };
            ad.OnAdFullScreenContentFailed += error =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);

                LoadRewardedAd();
            };
        }

        private void LoadBannerView()
        {
            var deviceWidth = MobileAds.Utils.GetDeviceSafeWidth();

            var adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth);

            var adRequest = new AdRequest();
            _bannerView = new BannerView(_config.AdUnitIdBanner, adaptiveSize, AdPosition.Bottom);
            _bannerView.LoadAd(adRequest);
        }

        private void LoadRewardedAd()
        {
            var adRequest = new AdRequest();

            RewardedAd.Load(_config.AdUnitIdRewarded, adRequest, (ad, error) =>
            {
                if (error != null || ad == null) return;

                _rewardedAd = ad;
                RegisterReloadHandler(ad);
            });
        }

        private void ShowRewardedAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd()) _rewardedAd.Show(reward => { });
        }
    }
}