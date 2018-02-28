using UnityEngine;
using System.Collections;

public class PanelEnter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "circle")
            GetComponent<Animator>().SetTrigger("CircleEnter");
    }
}
