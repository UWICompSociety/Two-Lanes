using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {

    private Rigidbody2D rb; //rigidbody component
    public float moveSpeed = 4f;
    public ColorState color;
    public Sprite redCircle;
    public Sprite blueCircle;
	public Sprite specialCircle;
    public bool isJewel = false;
    private SpriteRenderer spriteRenderer;
    public bool canSwitch = false; //whether circle can switch color or not
    private Transform switchMargin;
    private Transform mysterySwitchMargin;
    private ColorState[] colors = { ColorState.BLUE, ColorState.RED };

    
    // Use this for initialization

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //reference sprite renderer
        rb = GetComponent<Rigidbody2D>(); //reference rigidbody component
        switchMargin = GameObject.FindGameObjectWithTag("Switch").transform;
        mysterySwitchMargin = GameObject.Find("MysterySwitchMargin").transform;
    }
	

    void Update()
    {
        /*if(canSwitch) //cheks if color of cicle should be switch for double switch game mode
        {
            if (transform.position.y <= switchMargin.position.y+2.38f) //if the circle is approximatley halfway
            {
                AlternateColor();
                
            }
            
        }*/

        if(color == ColorState.SPECIAL)
        {
            if (transform.position.y < mysterySwitchMargin.position.y) //if the circle is approximatley halfway
            {
               
                setColor(colors[Random.Range(0, 2)]);

            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        transform.position += new Vector3(0f, -1f, 0f) * moveSpeed * Time.deltaTime; //moves the circle downwards
	
    }

    public void setColor(ColorState newColor)
    {
        color = newColor;
        if (color == ColorState.BLUE)
            spriteRenderer.sprite = blueCircle; //changes the sprite to a blue sprite
        else if (color == ColorState.RED)
            spriteRenderer.sprite = redCircle; //changes the sprite to a red sprite
        else if (color == ColorState.SPECIAL)
            spriteRenderer.sprite = specialCircle; //changes the sprite to the special sprite
      


    }

    public void AlternateColor()
    {
        if (color == ColorState.BLUE)
            setColor(ColorState.RED);  //change color to red
        else if (color == ColorState.RED)
            setColor(ColorState.BLUE); //change color to blue
        canSwitch = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Switch"))
        {
            if(canSwitch)
                AlternateColor();
        }
    }



}
