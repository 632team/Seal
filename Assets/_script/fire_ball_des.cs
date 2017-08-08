using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_ball_des : MonoBehaviour {

	//这个脚本用来销毁发射的火球
	void Start () {
        Destroy(this.gameObject, 2f);
	}
}
