using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
        if (other.gameObject.tag == "circle")

            //Destroy (other.gameObject); //destroys the circle game object
            other.gameObject.SetActive(false);

	}
}
