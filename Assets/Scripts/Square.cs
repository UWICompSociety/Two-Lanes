using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {
	
    public Sprite redSprite;
    public Sprite blueSprite;
    private SpriteRenderer spriteRenderer;
    public ColorState color = ColorState.RED;
  //  private GameManager gameManager;
	public bool isLeftSquare = true;
    private Animator anim;
    public GameObject saveMePanel;
    public GameObject mainMenu;
	

    void Awake()
    {
        anim = GetComponent<Animator>(); //reference animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); //reference sprite renederer component
      //  gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); //reference game manager
  
    }
  

	void Update()
	{

        if (GameManager.instance.gameStarted && !GameManager.instance.isPaused && !saveMePanel.activeInHierarchy)
        {
            #if UNITY_EDITOR
                //Playing from the editor
                if (isLeftSquare) //checks if square is on the left
                {
                    if (Input.GetKeyUp(KeyCode.LeftArrow)) //if the user presses the left arrow key
                        SwitchColor(); //switch the color of the square
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.RightArrow)) //if the user presses the right arrow key
                        SwitchColor(); //switch the color of the square
                }
            #else

                //Playing with a mobile phone
                if (Input.touchCount > 0) //check if the there is a finger on the screen
                {
                    Touch[] touches = Input.touches;
                    foreach (Touch touch in touches)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            checkTouch(touch.position);
                        }
                    }

                }
            #endif

        }
        
	}


    void checkTouch(Vector3 pos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(pos); //coneverts touch positon froms screen space to world space
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        Collider2D hit = Physics2D.OverlapPoint(touchPos); //returns game object the user touched
     
        if(hit.gameObject == this.gameObject){ //if the game object is the square/switch this script is connected to
            SwitchColor(); //switch color
        }
    }
  

    void SetColor()
    {
        if (color == ColorState.BLUE)
            spriteRenderer.sprite = blueSprite; //sets the swquare to a blue square
        else
            spriteRenderer.sprite = redSprite; //sets the square to a red square
    }

    public void SwitchColor()
    {
        if (spriteRenderer.sprite == redSprite) //switch to blue square
        {
            spriteRenderer.sprite = blueSprite;
            color = ColorState.BLUE;
        }
        else  //switch to red square
        {
            spriteRenderer.sprite = redSprite;
            color = ColorState.RED;
        }
        
    }


}
