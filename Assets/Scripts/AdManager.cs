using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour {

    private string adId = "1071838";
    private string rewardZone = "rewardedVideo";
	private string pictureZone = "pictureZone";
    public static AdManager instance = null;
    public GameObject watchAdBtn;
    public GameObject gemsText;
    public GameObject NoAdPanel;
   

	
	void Awake(){

        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        if (Advertisement.isSupported) {
			Advertisement.Initialize(adId,false);
            
		}


	}
	
	public void ShowAd(){

		ShowOptions options = new ShowOptions ();
		options.resultCallback = AdCalBackHandler;

		if (Advertisement.IsReady ()) {
			Debug.Log ("Ad Showing");
			Advertisement.Show (rewardZone,options);

		} else {
            NoAdPanel.SetActive(true);
			//do something
		}
	}



	void AdCalBackHandler(ShowResult result){

		switch (result) {
		case ShowResult.Finished:
                watchAdBtn.SetActive(false);
                int gems = PlayerPrefs.GetInt("JewelCurrency2");
                gems += 5;
                PlayerPrefs.SetInt("JewelCurrency2", gems);
                gemsText.GetComponent<Text>().text = gems.ToString();
                Debug.Log("Ad shown");
                break;

		}

	}

	/*public bool isAdActive(){
		return Advertisement.IsReady (zone);

	}*/
}
