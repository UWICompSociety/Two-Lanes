using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public Transform[] spawnSpots;
    
    public GameObject circlePrefab;
	
    public GameObject exitPrompt;
	public GameObject PauseMenuCanvas;
    public GameObject StartCanvas;
    public GameObject GameCanvas;
    public GameObject GameOverCanvas;
	public GameObject fingers;
    public Transform switchMargin;
	public GameObject nextAnim;
	public GameObject prevAnim;

    private ColorState[] colors = { ColorState.BLUE, ColorState.RED, ColorState.SPECIAL };

    public bool gameOver = false;
    public bool possibleSwitch = false;
    public bool gameStarted = false;

    public Text ScoreText;
    public Text GemsText;

    public float gameStartDelay = 1f;
    public float spawnDelay = 3f;
    public float updateDifficultyTime = 10;
    private float updateDifficultyTimer = 0f;
    private float spawnTimer = 0f;
    public float hiderTime = 0f;
    private float gameModeSwitchTimer = 0f;
    public float gameModeSwitchTime = 10f;
    private float gameModeTimer = 0f;
    public float gameModeTime = 6f;

    public int score = 0;
    private static int numTries = 1;
    private int switchChance = 0;
    private float tempCircleSpeed;


    public bool inGameMode = false;

    public float tempSpawnDelay;
    public float maxSpawnSpeed = 6f;
    public float minSpawnTime = 0.75f;
    public float circleSpeed = 1.5f;

    public GameMode gameMode = GameMode.REGULAR;
	
	public ParticleSystem switchHider;
    public bool spawnPause = false;
    public float spawnPauseDelay = 2f;

    private AudioSource audio;

    public bool isMedley = false;

    public GameObject jewel;
    public bool spawnJewel = false;
    public float spawnJewelTime = 8f;
    private float spawnJewelTimer = 0f;

    public int jewels = 0;
	public ScrollSnapRect scrollSnap;

    public bool isPaused = false;
    private Vector3 tmpPos;
    private float originalCircleSpeed;
    public int requiredGems = 20;
    
    public int gems;
    public int totalGems = 0;
    
    public GameObject saveMePanel;
    public bool savedMe = false;
    public int deathScore;
    public int prevGems;
    public static GameManager instance = null;
    public bool canShowAd = false;
	

   

	// Use this for initialization


    void Awake()
    {
        audio = GetComponent<AudioSource>();
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
         //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
           Destroy(gameObject); 
      //  audio.Play();
    }

   
   

	void Start () {
       
        tmpPos = switchMargin.position;
        originalCircleSpeed = circleSpeed;
		GemsText.text = PlayerPrefs.GetInt("JewelCurrency").ToString();

		

        StartCoroutine(SpawnCircles()); //spawn circles

        if (numTries > 1) //if user has played already
            StartGame();

        canShowAd = numTries % 3 == 0;
            



	}
	
	// Update is called once per frame
	void Update () {
		if (!gameStarted) {
			switch (scrollSnap._currentPage) {
				case 0:
					nextAnim.SetActive (true);
					gameMode = GameMode.REGULAR;
					isMedley = false;
					PlayerPrefs.SetInt ("GameMode",0);
					break;
				case 1:
					prevAnim.SetActive (true);
					nextAnim.SetActive (true);
					gameMode = GameMode.THREELANES;
					isMedley = false;
					PlayerPrefs.SetInt ("GameMode",1);
					break;
				case 2:
					prevAnim.SetActive (true);
					nextAnim.SetActive (true);
					gameMode = GameMode.MYSTERY;
					isMedley = false;
					PlayerPrefs.SetInt ("GameMode",2);
					break;
				case 3:
					prevAnim.SetActive (true);
					nextAnim.SetActive (true);
					gameMode = GameMode.DOUBLESWITCH;
					isMedley = false;
					PlayerPrefs.SetInt ("GameMode",3);
					break;
				case 4:
					prevAnim.SetActive (true);
					gameMode = GameMode.REGULAR;
					isMedley = true;
					PlayerPrefs.SetInt ("GameMode",4);
					break;
			}

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (exitPrompt.activeInHierarchy)
                    exitPrompt.SetActive(false);
                else
                    exitPrompt.SetActive(true);
            }


		}
        if (gameStarted)
        {
            if(!isPaused)
            {
               if(Input.GetKeyUp(KeyCode.Escape) && !GameOverCanvas.activeInHierarchy && !saveMePanel.activeInHierarchy)
                    PauseGame();
            }
            
               

               
            updateDifficultyTimer += Time.deltaTime;
            if (updateDifficultyTimer >= updateDifficultyTime)
                UpdateDifficulty(); //updates difficulty of the game

            spawnJewelTimer += Time.deltaTime;
            if (spawnJewelTimer >= spawnJewelTime)
                spawnJewel = true;

            if(isMedley)
            {
                if (!inGameMode)
                    gameModeSwitchTimer += Time.deltaTime;
                if (gameModeSwitchTimer >= gameModeSwitchTime)
                {
                    ChooseRandomGameMode();
                }

                if (inGameMode)
                {
                    gameModeTimer += Time.deltaTime;
                    if (gameModeTimer >= gameModeTime)
                    {
                        if (gameMode == GameMode.DOUBLESWITCH)
                        {
                            spawnDelay = tempSpawnDelay;
                            spawnPause = true;
                            //circleSpeed = tempCircleSpeed;
                        }

                        inGameMode = false;
                        gameModeTimer = 0f;
                        gameMode = GameMode.REGULAR;
                    }
                }
            }

            if(circleSpeed>=12)
            {
                Debug.Log("Reached maximum speed");
            }
           
        }
        

        if (StartCanvas.activeInHierarchy)
        {
            gameStarted = false;

            //if (Input.GetKeyUp(KeyCode.Escape)) //if user presses escape key
              //  Application.Quit(); //quit the game

            if (Input.GetKeyUp(KeyCode.Space)) //if user presses space key
                StartGame();

        }
    
    }


   

    IEnumerator SpawnCircles()
    {

        yield return new WaitForSeconds(gameStartDelay);
        while (!gameOver)
        {
           // Debug.Log("Spawning");
            if(spawnPause)
            {
                yield return new WaitForSeconds(spawnPauseDelay);
                spawnPause = false;
            }
            //spawns circle based on game mode
            if (gameMode == GameMode.REGULAR)
                RegularGameModeSpawn();
            else if (gameMode == GameMode.DOUBLESWITCH)
                DoubleSwitchGameModeSpwan();
            else if (gameMode == GameMode.MYSTERY)
                MysteryGameModeSpawn();
            else if (gameMode == GameMode.THREELANES)
                ThreeLaneGameModeSpawn();
            
			
            
            yield return new WaitForSeconds(spawnDelay);
        }

       
    }

    //spawns circle from three lanes
    //Player must make sure two squares match the color of the circle coming down the middle
    private void ThreeLaneGameModeSpawn()
    {
       // GameObject circle = Instantiate(circlePrefab, spawnSpots[Random.Range(0, spawnSpots.Length)].position, Quaternion.identity) as GameObject; //create a circle game object in one of three lanes
        GameObject circle = ObjectPool.instance.GetPooledObjectForType(circlePrefab.name);
        if(circle !=null)
        {
            circle.transform.position = spawnSpots[Random.Range(0, spawnSpots.Length)].position;
            circle.transform.rotation = Quaternion.identity;
            Circle circleScript = circle.GetComponent<Circle>();
            circleScript.setColor(colors[Random.Range(0, 2)]); //set circle to random color
            circleScript.moveSpeed = circleSpeed; //set the spped

        }
    

            
    }


    //Regular gameplay where circles only come down in two lanes
    private void RegularGameModeSpawn()
    {
        //GameObject circle = Instantiate(circlePrefab, spawnSpots[Random.Range(0, 2)].position, Quaternion.identity) as GameObject;

        GameObject circle = ObjectPool.instance.GetPooledObjectForType(circlePrefab.name);

        if(circle != null)
        {
            circle.transform.position = spawnSpots[Random.Range(0, 2)].position; //sets spawn position of circle
            circle.transform.rotation = Quaternion.identity;  //sets rotation to default rotation
            Circle circleScript = circle.GetComponent<Circle>();
            circleScript.setColor(colors[Random.Range(0, 2)]); //set circle to random color
            circleScript.moveSpeed = circleSpeed; //set speed of circle
        
        }
        
   


    
    }

    private void MysteryGameModeSpawn()
    {
      //  GameObject circle = Instantiate(circlePrefab, spawnSpots[Random.Range(0, 2)].position, Quaternion.identity) as GameObject;

        GameObject circle = ObjectPool.instance.GetPooledObjectForType(circlePrefab.name);

        if(circle != null)
        {
            circle.transform.position = spawnSpots[Random.Range(0, 2)].position; //sets spawn position of circle
            circle.transform.rotation = Quaternion.identity;  //sets rotation to default rotation
            Circle circleScript = circle.GetComponent<Circle>();
            circleScript.setColor(colors[Random.Range(0, 3)]); //set circle to random color
            circleScript.moveSpeed = circleSpeed; //set speed of circle
        }
        
   

  
    }

    private void DoubleSwitchGameModeSpwan()
    {
        //GameObject circle = Instantiate(circlePrefab, spawnSpots[0].position, Quaternion.identity) as GameObject;
        //GameObject circle2 = Instantiate(circlePrefab, spawnSpots[1].position, Quaternion.identity) as GameObject;


        GameObject circle = ObjectPool.instance.GetPooledObjectForType(circlePrefab.name);
        GameObject circle2 = ObjectPool.instance.GetPooledObjectForType(circlePrefab.name);

        if(circle!=null && circle2!=null)
        {
            circle.transform.position = spawnSpots[0].position;  //sets spawn position of circle
            circle.transform.rotation = Quaternion.identity;     //sets rotation to default rotation


            circle2.transform.position = spawnSpots[1].position;   //sets spawn position of circle
            circle2.transform.rotation = Quaternion.identity;      //sets rotation to default rotation
            
                
                
            Circle circleScript = circle.GetComponent<Circle>();
            circleScript.setColor(colors[Random.Range(0, 2)]); //set circle to random color


          //  tmpPos.y += Mathf.Abs((((circleSpeed - originalCircleSpeed) / originalCircleSpeed) * tmpPos.y)); //calculate switch position

           // Debug.Log(tmpPos.y.ToString());

            Circle circleScript2 = circle2.GetComponent<Circle>();
            circleScript2.setColor(colors[Random.Range(0, 2)]); //set circle to random color



            if (!(Camera.main.WorldToScreenPoint(tmpPos).y >= (Screen.height * 0.8)))
            {
                switchMargin.transform.position = tmpPos; //set position of switch margin

            }


            if (isMedley)
            {
                circleScript.moveSpeed = 6f;
                circleScript2.moveSpeed = 6f;
            }
            else
            {
                circleScript.moveSpeed = circleSpeed;
                circleScript2.moveSpeed = circleSpeed;

            }


            if (!isMedley)
            {
                switchChance = Random.Range(0, 10); //calculates chance of circles switching color
                if (switchChance >= 5)
                {
                   // Debug.Log("Switch " + switchChance.ToString());
                    circleScript.canSwitch = true;
                    circleScript2.canSwitch = true;
                }

            }
            else
            {
                circleScript.canSwitch = true;
                circleScript2.canSwitch = true;
            }
        }
        
        
        
       
       
    }


    public void StartGame()
    {
        audio.Play();
        if(numTries == 1) //if coming from main menu
        {
            GameObject[] circles = GameObject.FindGameObjectsWithTag("circle");
            foreach (GameObject circle in circles)
            {
               // Destroy(circle); //remove all circles that were left over on game board
                circle.SetActive(false);

            }
        }

        numTries++; //increment number of times player has played the game
        gameStarted = true;
        StartCanvas.SetActive(false); //hide main menu canvas
		fingers.SetActive(false); //hide fingers from main meny
        GameCanvas.SetActive(true); //show score canvas
        
       

		switch (PlayerPrefs.GetInt ("GameMode")) {
		case 0: gameMode = GameMode.REGULAR;
			isMedley = false;
			break;
		case 1:gameMode = GameMode.THREELANES;
			isMedley = false;
			break;
		case 2:gameMode = GameMode.MYSTERY;
			isMedley = false;
			break;
		case 3:gameMode = GameMode.DOUBLESWITCH;
			isMedley = false;
			break;
		case 4:gameMode = GameMode.REGULAR;
			isMedley = true;
			break;
		}

        if (gameMode == GameMode.DOUBLESWITCH)
        {
            spawnDelay = 1.2f;
            minSpawnTime = 0.5f;
            maxSpawnSpeed = 7.5f;
        }

        prevGems = PlayerPrefs.GetInt("JewelCurrency2");
    }



    public void UpdateScore(int points)
    {
        //Update Score
        if (gameStarted)
        {
            score += points;
            ScoreText.text = Mathf.RoundToInt(score).ToString();
        }

    }


    public void GameOver()
    {
   
        audio.Pause(); //pause the music
        if (gameStarted)
        {
            //check if player had used jewel to save
            if(!savedMe)
                gems = (int)((score/100f)*5);
            else
                gems  = (int)(((score-deathScore)/100f)*5);

            totalGems = gems + PlayerPrefs.GetInt("JewelCurrency2");
            //PlayerPrefs.SetInt("JewelCurrency2", totalGems);
            if (totalGems < requiredGems || score<20 || savedMe)
            {
                EndGame();
            }
            else
            {
                Time.timeScale = 0f; //pause game
                saveMePanel.SetActive(true); //activate save me panel
            
            }
            

        }
            
    }

    public void EndGame()
    {
        audio.Stop();
        gameOver = true;

        GameOverCanvas.SetActive(true);
    }


    void ChooseRandomGameMode()
    {
        GameMode[] gameModes = { GameMode.DOUBLESWITCH, GameMode.MYSTERY, GameMode.THREELANES };
        gameMode = gameModes[Random.Range(0, gameModes.Length)];
        if (gameMode == GameMode.DOUBLESWITCH && isMedley)
        {
            tempSpawnDelay = spawnDelay;
            spawnDelay = 1f;
            tempCircleSpeed = circleSpeed;
        }
           
        inGameMode =  true;
        gameModeSwitchTimer = 0f;
    }

    void OnApplicationPause()
    {
        if(gameStarted && !isPaused && !saveMePanel.activeInHierarchy && !gameOver)
            PauseGame();
    }

  

    void UpdateDifficulty()
    {
        //Update the difficulty of the game with time

        if (circleSpeed <= maxSpawnSpeed)
        {
            circleSpeed += 0.8f; //increase speed of circles

        }
          

        if (spawnDelay >= minSpawnTime)
            spawnDelay -= 0.1f; //decrease the time delay between spawning circles
        
        updateDifficultyTimer = 0f;
    }

		
	public void PauseGame()
	{
        isPaused = true;
        audio.Pause();
		Time.timeScale = 0;
		GameCanvas.SetActive (false);	// Disables Score Board
		PauseMenuCanvas.SetActive(true); //Show Pause Menu

	}

    //shows prompt to user to check whether he/she wants to quit the game
	public void ConfirmCloseAction()
	{
		exitPrompt.SetActive (true);
	}

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        audio.Play();
        GameCanvas.SetActive(true);
        PauseMenuCanvas.SetActive(false);
    }

	// Removes confirmation prompt
	public void ReturnToTitle()
	{
		exitPrompt.SetActive (false);
	}

	public void CloseGame()
	{
		Application.Quit (); //quit the game
	}

	public void MainMenu()
	{
        Time.timeScale = 1f;
		numTries = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload the scene
	}

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload the scene
    }
}
