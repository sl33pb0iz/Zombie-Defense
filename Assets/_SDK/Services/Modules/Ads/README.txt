------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- 
------- ------- ------- ------- ------- G2 Ads  ------- ------- ------- ------- ------- 
------- ------- ------- ------- ----- 20/03/2017 ------ ------- ------- ------- -------
------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- 

******* I/ Overview
G2.Sdk.Services.Ads have 4 type of ads:
- MoreGame
- BubbleAd
- Interstitial
- Banner

WARNING:	- Everything you need to know in DemoScene
			- Please backup your project before use this library.
			- When MoreGame or Popup Ads show up, if you want to hide banner ad or pause your game (timescale = 0), this is your job.			
			- Write your app's api key to text field. If your ads does not have api key, please contact PhuongTM.
			- If you want to test this app, please choose 'Is Sandbox'.
			- If you call 'Show method' before 'Load method', it will be load, but not show after load done.

TIP:

+ Back button
*** If Scene have MoreGame and BubbleAd
void Update(){
	if(Input.GetKey(KeyCode.Escape)){
		if(moreGameController.OnBack() == false && bubbleAdController.OnBack() == false && AdsManager.Instance.OnBack() == false){
			// Do something
		}else{
			// Do nothing
		}	
	}
}

*** If Scene have BubbleAd
void Update(){
	if(Input.GetKey(KeyCode.Escape)){
		if(bubbleAdController.OnBack() == false && AdsManager.Instance.OnBack() == false){
			// Do something
		}else{
			// Do nothing
		}	
	}
}

*** If Scene have MoreGame
void Update(){
	if(Input.GetKey(KeyCode.Escape)){
		if(moreGameController.OnBack() == false && AdsManager.Instance.OnBack() == false){
			// Do something
		}else{
			// Do nothing
		}	
	}
}

*** If Scene do not have MoreGame 
void Update(){
	if(Input.GetKey(KeyCode.Escape)){
		if(AdsManager.Instance.OnBack() == false){
			// Do something
		}else{
			// Do nothing
		}
	}
}




******* II/ MoreGame

*** 1. Setup
- Drag MoreGameLandscape/MoreGamePortrait prefab (in G2Sdk/Services/Modules/Ads/Prefabs) to your Scene.
- Change coin's sprite in Inspector.

*** 2. How to use
You can use 6 methods:
- void RegisterListener(Action onLoaded, Action<int, string> onFailedToLoad,
            Action onOpened, Action onClosed, Action onLeavingApplication, Action<int> onGetReward); // Call first
- void LoadMoreGames(int offset = 0, int limit = 0, bool isRefresh = false);
- bool ShowMoreGames();
- bool HideMoreGames();
- bool IsMoreGamesShowing();
- bool IsMoreGamesLoading();
- bool OnBack();

*** 3. Tip & Warning
- Maximum is 10.
- MoreGame Data cache expires in 15 minutes.




******* III/ Interstitial ad & Reward Ad
- Auto Load after close

*** 1. Setup
- Drag AdsManager prefab (in G2Sdk/Services/Modules/Ads/Prefabs) to your first Scene.
- Change coin's sprite in Inspector.
- In CanvasAds, remove GameObject, which is not fit to your screen orientation.
- Drag gameobject full and not full to AdsManager's inspector.
- In AdsMob and AdsUnity, add more information;



*** 2. How to use
a/ Interstitial
You can use 6 methods:
- void RegisterInterstitialListener(Action onOpened, Action onClosed,
            Action onLeavingApplication, Action<int> onGetReward);
- void RegisterBannerListener(Action onOpened, Action onClosed, Action onLeavingApplication);
- void LoadInterstitial(int adsId = 1);
- bool ShowInterstitial(int adsId = 1);
- bool HideInterstitial(int adsId = 1);
- bool IsInterstitialLoaded(int adsId = 1);
- bool OnBack();


b/ Banner
You can use 4 methods:
- bool ShowBanner(int bannerId = 1);
- bool HideBanner(int bannerId = 1);
- int GetBannerWidth();
- int GetBannerHeight();



*** 3. Tip & Warning
- If Unity Ads Service was enable in your project, please disable this service and use SDK.Ads's library.
- Android: When app was installed, ad will not show.




******* II/ Bubble

*** 1. Setup
- Drag BubbleAd prefab (in G2Sdk/Services/Modules/Ads/Prefabs) to your Scene
- Change coin's sprite in Inspector.

*** 2. How to use
You can use 6 methods:
- void RegisterListener(Action onLoaded, Action<int, string> onFailedToLoad,
            Action onOpened, Action onClosed, Action onLeavingApplication); // Call first
- void LoadBubbleAd(int offset = 0, int limit = 0, bool isRefresh = false);
- bool ShowBubbleAd();
- bool HideBubbleAd();
- bool IsBubbleAdShowing();
- bool IsBubbleAdLoading();
- bool OnBack();

*** 3. Tip & Warning
- Maximum is 3.
- MoreGame Data cache expires in 15 minutes.