using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landelement_control : MonoBehaviour {

	// Use this for initialization
	public GameObject king;
    public GameObject shakecamera;
    //这里记录摄像头抖动的时间
    public float curr_time;
    public float last_time;
    public bool flags;
    public bool flags_copy;

	void Start () {
        flags = false;
    }
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 2, 0));
		if (Vector3.Distance (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, transform.position) < 3.5f) {

            shakecamera.GetComponent<ShakeCamera>().shake();
			GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load ("Audio/quake", typeof(AudioClip));
			GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().Play ();

           Destroy(GameObject.FindGameObjectWithTag("landElement"));
		}
	}
}
