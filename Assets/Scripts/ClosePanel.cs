using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClosePanel : MonoBehaviour {

    private Button btn;

	// Use this for initialization
	void Awake() {
        btn = GetComponent<Button>();
	}

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
	
}
