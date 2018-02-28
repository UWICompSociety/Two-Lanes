using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {


    public Text ScoreText;
	public Text ScoreLabel;
	public Text GemsText;
    public Text HighScoreText;
    public Text HighScorePromptText;
    public Text HighScoreLabelText;
    public Text ModeText;
  //  private GameManager gameManager;
    public GameObject Switches;
    public GameObject Lanes;
    public GameObject ScoreCanvas;

	public Color oldHighScoreColor = Color.white;
	public Color newHighScoreColor = Color.yellow;

	public string modeType;
	private string highScore = "_HighScore";
    private string medley_prefs_str = "Medley";

    public bool isNewHighScore = false;

    private int score;

    public GameObject AdButton;

    // Use this for initialization

    void Awake()
    {
     //   gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); //reference game manager script
    }
	void Start () {
		modeType = GameManager.instance.gameMode.ToString (); //Gets current game mode as string

        Switches.SetActive(false); //hide switches
        Lanes.SetActive(false); //hide lanes
        ScoreCanvas.SetActive(false); //hide score
        RemoveCircles(); //remove left over circles

        if(!GameManager.instance.isMedley)
        {
            if (modeType == "REGULAR")
                ModeText.text = "REGULAR";
            else if (modeType == "DOUBLESWITCH")
                ModeText.text = "DOUBLE SWITCH";
            else if (modeType == "MYSTERY")
                ModeText.text = "MYSTERY";
            else if (modeType == "THREELANES")
                ModeText.text = "THREE LANES";
        }else
        {
            ModeText.text = "MEDLEY";
        }

        if (!GameManager.instance.canShowAd)
            AdButton.SetActive(false);
        else
            AdButton.SetActive(true);
     
        
        score = GameManager.instance.score; //get score
        ScoreText.text = score.ToString();
		//GemsText.text = ScoreText.text;

		//int currency = PlayerPrefs.GetInt ("JewelCurrency2"); //gets currency from saved file
        int currency = GameManager.instance.totalGems;

        GemsText.text = currency.ToString();

		PlayerPrefs.SetInt ("JewelCurrency2",currency); //stores it back to saved file

  
        int prevHighScore;
        if (!GameManager.instance.isMedley)
            prevHighScore = PlayerPrefs.GetInt(modeType + highScore); //check the highscore from mode
        else
            prevHighScore = PlayerPrefs.GetInt(medley_prefs_str + highScore); //check medley high score

        if (prevHighScore == 0)
        {
            isNewHighScore = true;
            HighScoreText.text = score.ToString();
        }
            
        else
            HighScoreText.text = prevHighScore.ToString();

		if (score > prevHighScore) { //if the score is a new high score

            isNewHighScore = true;
            HighScorePromptText.gameObject.SetActive(true);
            HighScoreText.gameObject.SetActive(false);
            HighScoreLabelText.gameObject.SetActive(false);

            if(!GameManager.instance.isMedley)
			    PlayerPrefs.SetInt (modeType + highScore, score);
            else
                PlayerPrefs.SetInt(medley_prefs_str + highScore, score);
			HighScoreText.text = score.ToString ();
		} else {
			ScoreLabel.text = "SCORE";
			ScoreLabel.color = oldHighScoreColor;
		}

        CheckAchievement();

        if (isNewHighScore)
            PlayGamesManager.instance.postScoreToLeaderboard(score, GetLeaderBoardByModeType(modeType));
        else
            PlayGamesManager.instance.postScoreToLeaderboard(prevHighScore, GetLeaderBoardByModeType(modeType));
    }

    string GetLeaderBoardByModeType(string gameMode)
    {
        if (!GameManager.instance.isMedley)
        { 
            if (gameMode == "REGULAR")
            {
                return PlayGamesConstants.leaderboard_regular;
            } else if (gameMode == "DOUBLESWITCH")
            {
                return PlayGamesConstants.leaderboard_doubles;
            }
            else if (gameMode == "MYSTERY")
            {
                return PlayGamesConstants.leaderboard_mystery;
            }
            else if (gameMode == "THREELANES")
            {
                return PlayGamesConstants.leaderboard_3_lanes;

            }
        }
        else
        {
           
            return PlayGamesConstants.leaderboard_medley;
        }
        return null;
    }
	
	// Update is called once per frame
	void Update () {
         if(Input.GetKeyUp(KeyCode.Escape))
         {
             GameManager.instance.MainMenu(); //load main menu
         }
         if (Input.GetKeyUp(KeyCode.Space))
             GameManager.instance.RestartGame(); //restart game

	}

    void RemoveCircles()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("circle");
        foreach(GameObject circle in circles)
        {
            Destroy(circle); 
        }
    }

    public void ShowAd()
    {
        AdManager.instance.ShowAd();
    }

    void CheckAchievement()
    {
        if (score >= 50)
            PlayGamesManager.instance.recordAchievement(PlayGamesConstants.achievement_50_points);
        if (score >= 100)
            PlayGamesManager.instance.recordAchievement(PlayGamesConstants.achievement_100_points);
        if (score >= 200)
            PlayGamesManager.instance.recordAchievement(PlayGamesConstants.achievement_200_points);
        if (score >= 300)
            PlayGamesManager.instance.recordAchievement(PlayGamesConstants.achievement_300_points);
        if (score >= 400)
            PlayGamesManager.instance.recordAchievement(PlayGamesConstants.achievement_400_points);
    }

   
}
