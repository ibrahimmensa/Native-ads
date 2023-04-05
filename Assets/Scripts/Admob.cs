using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

[Serializable]
public class NativePanel
{
	public RawImage adIcon;
	public RawImage adChoices;
	public Text adHeadline;
	public Text adCallToAction;
	public Text adAdvertiser;
}

//Banner ad
public class Admob : MonoBehaviour
{
	private BannerView adBanner;
	private NativeAd adNative;
	private bool nativeLoaded = false;

	private string idApp, idBanner, idNative;

	public GameObject adNativePanel;
	public NativePanel nativePanel;
	[SerializeField] RawImage adIcon;
	[SerializeField] RawImage adChoices;
	[SerializeField] Text adHeadline;
	[SerializeField] Text adCallToAction;
	[SerializeField] Text adAdvertiser;


	void Awake ()
	{
		if(adNativePanel != null)
			adNativePanel.SetActive (false); //hide ad panel
	}

	void Start ()
	{
		idApp = "ca-app-pub-3940256099942544~3347511713";
		idBanner = "ca-app-pub-3940256099942544/6300978111";
		idNative = "ca-app-pub-3940256099942544/2247696110";

		MobileAds.Initialize(initStatus => {
			Debug.Log(initStatus.ToString());
		});

		RequestBannerAd ();
		RequestNativeAd ();
	}

	void Update ()
	{
		if (nativeLoaded && adNativePanel !=null && nativePanel != null) {
			nativeLoaded = false;

			Texture2D iconTexture = this.adNative.GetIconTexture ();
			Texture2D iconAdChoices = this.adNative.GetAdChoicesLogoTexture ();
			string headline = this.adNative.GetHeadlineText ();
			string cta = this.adNative.GetCallToActionText ();
			string advertiser = this.adNative.GetAdvertiserText ();
			nativePanel.adIcon.texture = iconTexture;
			nativePanel.adChoices.texture = iconAdChoices;
			nativePanel.adHeadline.text = headline;
			nativePanel.adAdvertiser.text = advertiser;
			nativePanel.adCallToAction.text = cta;

			//register gameobjects
			adNative.RegisterIconImageGameObject (nativePanel.adIcon.gameObject);
			adNative.RegisterAdChoicesLogoGameObject (nativePanel.adChoices.gameObject);
			adNative.RegisterHeadlineTextGameObject (nativePanel.adHeadline.gameObject);
			adNative.RegisterCallToActionGameObject (nativePanel.adCallToAction.gameObject);
			adNative.RegisterAdvertiserTextGameObject (nativePanel.adAdvertiser.gameObject);

			adNativePanel.SetActive (true); //show ad panel
		}
	}

	#region Banner Methods --------------------------------------------------

	public void RequestBannerAd ()
	{
		adBanner = new BannerView (idBanner, AdSize.Banner, AdPosition.Bottom);
		AdRequest request = AdRequestBuild ();
		adBanner.LoadAd (request);
	}

	public void DestroyBannerAd ()
	{
		if (adBanner != null)
			adBanner.Destroy ();
	}

	#endregion

	#region Native Ad Mehods ------------------------------------------------

	private void RequestNativeAd ()
	{
		AdLoader adLoader = new AdLoader.Builder (idNative).ForNativeAd ().Build ();
		adLoader.OnNativeAdLoaded += this.HandleOnUnifiedNativeAdLoaded;
		adLoader.LoadAd (AdRequestBuild ());
	}

	//events
	private void HandleOnUnifiedNativeAdLoaded (object sender, NativeAdEventArgs args)
	{
		this.adNative = args.nativeAd;
		nativeLoaded = true;
	}

	#endregion

	//------------------------------------------------------------------------
	AdRequest AdRequestBuild ()
	{
		return new AdRequest.Builder ().Build ();
	}

	void OnDestroy ()
	{
		DestroyBannerAd ();
	}

	public void showNativeAd(GameObject nativePanel)
	{
		this.adNativePanel = nativePanel;
		this.nativePanel = nativePanel.GetComponent<PanelHandler>().panelData;
	}

	public void requestForNewNativeAd()
	{
		RequestNativeAd();
		this.adNativePanel = null;
		this.nativePanel = null;
	}

}

