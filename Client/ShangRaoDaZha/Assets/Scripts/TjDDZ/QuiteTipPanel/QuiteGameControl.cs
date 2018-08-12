using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiteGameControl : MonoBehaviour {

    public GameObject TipModule;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            // if (QuitGame.Instance == null) UIManager.Instance.ShowUIPanel(UIPaths.QuitGame);
            TipModule.SetActive(!TipModule.activeSelf);
        }

    }
}
