using UnityEngine;
using System.Collections;

public class AISwitch : MonoBehaviour {


    public Square square; //square component
    public GameObject StartCanvas;
	public GameObject pointer;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = pointer.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (StartCanvas.activeInHierarchy) //check if main menu is still active
        {
            if (other.gameObject.tag == "circle") //check if game object is a circle
            {
               
                if (other.gameObject.GetComponent<Circle>().color != square.color) //if colors dont match
                {
					anim.SetTrigger ("Tap");
                    square.SwitchColor(); //switch the color of the square
                }
            }
        }
      

    }
}
