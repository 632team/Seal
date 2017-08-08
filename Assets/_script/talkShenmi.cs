using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class talkShenmi : MonoBehaviour {
	public Flowchart talkFlowchart;
	public string onCollision;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Player")){
			Block target = talkFlowchart.FindBlock (onCollision);
			talkFlowchart.ExecuteBlock (target);
		}
	}
}
