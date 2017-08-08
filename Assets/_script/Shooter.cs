using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject shooterPos;
    public float force=1000;
    public Rigidbody fire_ball;
	// Update is called once per frame
	void LateUpdate () {
		GameObject brave = GameObject.FindWithTag ("Player");
		NoLockView_Player br = brave.GetComponent<NoLockView_Player> ();

		// 魔法值充足才能放魔法
		if (br.mp > 0 && Input.GetKeyDown("q"))
        {
            Rigidbody new_fire_ball = Instantiate(fire_ball, shooterPos.transform.position, Quaternion.identity);
			// 播放魔法球攻击音效
			GameObject.FindGameObjectWithTag("SkillAudio").GetComponent<AudioSource>().clip =
				GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().m_fireBall;
			GameObject.FindGameObjectWithTag("SkillAudio").GetComponent<AudioSource>().Play ();
			br.mp -= 1;
			br.MPStrip.value = br.mp;
            new_fire_ball.AddForce(force * shooterPos.transform.forward);
        }
    }
}
