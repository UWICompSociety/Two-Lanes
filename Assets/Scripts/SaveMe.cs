using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveMe : MonoBehaviour
{

    //public GameManager.instance GameManager.instance;
    public GameObject PauseMenu;
    public Text JewelCountText;
    public Button saveMeButton;
    public Button skipButton;
    // Use this for initialization
    void Start()
    {
        //int gemCount = PlayerPrefs.GetInt("JewelCurrency2") ;
        int gemCount = GameManager.instance.totalGems;
        JewelCountText.text = gemCount.ToString();
        saveMeButton.onClick.AddListener(() =>
        {
           // GameManager.instance.gems -= GameManager.instance.requiredGems;
            gemCount -= GameManager.instance.requiredGems;
            GameManager.instance.deathScore = GameManager.instance.score;
            PlayerPrefs.SetInt("JewelCurrency2", gemCount);
            GameObject[] circles = GameObject.FindGameObjectsWithTag("circle");
            foreach(GameObject circle in circles)
            {
                circle.SetActive(false);
            }


            PauseMenu.SetActive(true);
            PauseMenu.GetComponent<PauseManager>().saveMe = true;
            GameManager.instance.savedMe = true;
            gameObject.SetActive(false);
           
        });

        skipButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false); //hide the panel
            GameManager.instance.EndGame(); //end the game
        });
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            gameObject.SetActive(false); //hide the panel
            GameManager.instance.EndGame(); //end the game
        }
    }
}
	
	
