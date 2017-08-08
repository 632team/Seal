using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bgm : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		this.gameObject.GetComponent<AudioSource> ().Play ();
		SceneManager.LoadScene("Menu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
