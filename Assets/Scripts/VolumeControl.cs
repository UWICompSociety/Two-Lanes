using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {
	
    private Button btn;
	//public GameObject GameController;
	//private AudioSource audio;
	public Sprite volumeSprite;
	public Sprite muteSprite;
	
    // Use this for initialization
	void Start () {
		btn = GetComponent<Button> ();
		btn.onClick.AddListener(() =>changeVolume());
		//audio = GameController.GetComponent<AudioSource> ();

		if (AudioListener.volume > 0f) {
			btn.GetComponent<Image>().sprite = volumeSprite;

		} else {
			btn.GetComponent<Image>().sprite = muteSprite;
		}

        if (PlayerPrefs.GetInt("volume") == 1) //if volume was set as mute
        {
            muteVolume();
        }
        else
        {
            enableVolume();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void changeVolume(){
		if (AudioListener.volume > 0f) {    //if sound is enabled
            muteVolume();
            PlayerPrefs.SetInt("volume", 1); //1-mute 0-volume
        } else {
            enableVolume();
            PlayerPrefs.SetInt("volume", 0);
		}
	}

    private void muteVolume()
    {
        AudioListener.volume = 0f; //mute the volume
        btn.GetComponent<Image>().sprite = muteSprite; //change volume button to mute sign
    }

    private void enableVolume()
    {
        AudioListener.volume = 1f;   //enable sound
        btn.GetComponent<Image>().sprite = volumeSprite; //change volume button to volume sprite
    }
}
