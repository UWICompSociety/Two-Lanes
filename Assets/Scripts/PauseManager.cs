using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour {

	public Text CanvasTitle, CountDownLabel;
	public GameObject ResumeButton, HomeButton;
	public GameManager gameManager;

	private int timeKeep, countDown;
	private bool timeToResume;
    public bool saveMe = false;

	// Use this for initialization
	void Start () {
		timeToResume = false;
		timeKeep = 0;
		countDown = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(saveMe)
        {
            SetTimeToResume(true);
            saveMe = false;
        }
		if (timeToResume) {
           // Debug.Log("Resuming");
			// Checks if 1 sec has passed
			if (timeKeep < (int)Time.realtimeSinceStartup) {
				countDown--;
				timeKeep = (int)Time.realtimeSinceStartup;
				CountDownLabel.text = countDown.ToString ();
			}

			// Begins game on count down = zero
			if (countDown == 0) {
				timeToResume = false;
				// Return Panel back to default settings before closing
				CanvasTitle.text = "PAUSED";
				//CanvasTitle.fontSize = 20;
				CountDownLabel.text = "";
				ResumeButton.SetActive (true);
				HomeButton.SetActive (true);

				// Return time to normal
                gameManager.ResumeGame();
		
			}
		}


        if (Input.GetKeyUp(KeyCode.Escape))
            SetTimeToResume(true);
        
	}


	public void SetTimeToResume(bool shouldWe) {
		timeToResume = shouldWe;
		if (timeToResume) {		// If we are resuming, we set the timer...
			countDown = 4;
			timeKeep = (int)Time.realtimeSinceStartup;	// initialize time variable
			CanvasTitle.text = "Get Ready in";
			CanvasTitle.fontSize = 20;
			CountDownLabel.text = countDown.ToString ();
			ResumeButton.SetActive (false);
			HomeButton.SetActive (false);
		}
	}

	public void ReturnToHome() {
		Time.timeScale = 1;
        gameManager.MainMenu();
	}
}
