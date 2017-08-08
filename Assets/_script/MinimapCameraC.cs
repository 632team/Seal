using UnityEngine;
using System.Collections;

public class MinimapCameraC : MonoBehaviour {
	
	public Transform target;
	
	void  Start (){
		if(!target){
			target = GameObject.FindWithTag ("Player").transform;
		}
		
	}
	
	void  Update (){
		if(target){
			transform.position = new Vector3(target.position.x ,transform.position.y ,target.position.z);
		}

	}
	
	void  FindTarget (){
		if(!target){
			Transform newTarget = GameObject.FindWithTag ("Player").transform;
			if(newTarget){
				target = newTarget;
			}
		}

	}
}