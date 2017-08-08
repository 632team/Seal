using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderController : MonoBehaviour {
	public Slider HPStrip;
	public int HP;
	// Use this for initialization
	void Start () {
		HPStrip.value = HPStrip.maxValue = HP;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.rotation = Camera.main.transform.rotation;
	}

	public void OnHit(int damage){
		HP -= damage;
		if (HP <= 0){
			//Destroy (this);
			return;
		}
		HPStrip.value = HP;
	}
	private void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Player")){
			OnHit (50);
			//Destroy (this);
		}
	}
}
