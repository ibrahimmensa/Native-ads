using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject panelMenu;
	public Admob admob;
	public PanelHandler panel;

	void Awake ()
	{
		panelMenu.SetActive (false); //hide menu in the start
	}

	public void ShowMenu ()
	{
		panelMenu.SetActive (true);
		admob.showNativeAd(panel.gameObject);
	}

	public void HideMenu ()
	{
		panelMenu.SetActive (false);
		admob.requestForNewNativeAd();
	}
}

