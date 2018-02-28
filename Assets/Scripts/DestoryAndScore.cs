using UnityEngine;
using System.Collections;

public class DestoryAndScore : MonoBehaviour {

    private Square square; 
	//private GameManager gameManager;
    public GameObject Square; //switch game object
    private Animator anim; //animator component
    public AudioClip collectClip;
  

	// Use this for initialization

    void Awake()
    {
        anim = Square.GetComponent<Animator>(); //reference animator component
        square = Square.GetComponent<Square>(); //reference square component
		//gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> (); //reference game manager script
      
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "circle") //check if game object is circle
        {

            //Check if color of circle and color of square match
            //If they match increase player score using function from game manager script
            //If not initiate game over in game manager script

           
            if (other.gameObject.GetComponent<Circle>().color == square.color)
            {
                anim.SetTrigger("Catch"); //Play catch Animation
                GameManager.instance.UpdateScore(1); //Update score
             
            }
            else
            {
                GameManager.instance.GameOver(); //end the game
            }

        }

    }
}
