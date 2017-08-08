using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLand : MonoBehaviour {

	public GameObject land1, land2;
	private int length = 10;

	// Use this for initialization
	void Start () {
		for (int i = -100; i < 250; i += length) {
			for (int j = -100; j < 290; j += length) {
				if (i > 0 && j > 0) {
					Instantiate(land2, new Vector3(i, 0, j), Quaternion.Euler(0, 0, 0));//生成物体
				}
				else {
					Instantiate(land1, new Vector3(i, 0, j), Quaternion.Euler(0, 0, 0));//生成物体
				}
			}
		}
	}
}
