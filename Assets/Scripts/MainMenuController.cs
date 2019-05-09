using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public Text hS;

	// Use this for initialization
	void Start ()
    {
        HSFunction();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    void HSFunction()
    {
        hS.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
