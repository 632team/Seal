using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour {

    public Slider hpSlider;
    public float hp = 150;
    private float totalHp;
    private float distanceArriveTarget = 15.0f;
	private bool die = false;

    //多少秒攻击一次
    public float attackRateTime = 1;
    private float timer = 0;
    public GameObject attackWeaponPrefab;//子弹
    public Transform firePosition;
    public Transform head;


    //获取结束界面的UI
    public GameObject end;

    //判断是否使用激光
    public bool useLaser = false;

    public float damageRate = 70;

    public LineRenderer laserRenderer;

    //表示停止运行游戏的变量
    public float stop_curr;
    public float stop_last;

    // Use this for initialization
    void Start () {
        totalHp = hp;
        timer = attackRateTime;
    }
	
	// Update is called once per frame
	void Update () {
        //这里表示停止运行游戏
        if (die)
        {
            stop_curr = Time.time;
            if (stop_curr - stop_last >= 1)
            {
                //Time.timeScale = 0;
                return;
            }
        }

        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < obj.Length; i++)
        {
            Vector3 dir = obj[i].transform.position - transform.position;
            if (dir.magnitude < distanceArriveTarget)
            {
                Vector3 targetPosition = obj[i].transform.position;
                targetPosition.y = head.position.y;
                head.LookAt(targetPosition);
                if (useLaser==false)//进行子弹的攻击
                {
                    timer += Time.deltaTime;
                    //检测到敌人，在这里进行攻击敌人
                    if (timer >= attackRateTime)
                    {
						this.GetComponent<AudioSource> ().Play ();
                        timer = 0;
                        Attack(obj[i]);
                        obj[i].GetComponent<NoLockView_Player>().TakeDamage(1f);
                    }
                }
                else//进行激光的攻击
                {
                    Vector3 temp_position = obj[i].transform.position;

                    temp_position.y += (float)1.2;
					if (laserRenderer.enabled == false) {
						laserRenderer.enabled = true;
						this.GetComponent<AudioSource> ().Play ();
					}
                    laserRenderer.SetPositions(new Vector3[] { firePosition.position, temp_position });
                    obj[i].GetComponent<NoLockView_Player>().TakeDamage(0.3f);
                }

				obj [i].GetComponent<NoLockView_Player> ().HPStrip.value = obj [i].GetComponent<NoLockView_Player> ().hp;
				if (!die && obj [i].GetComponent<NoLockView_Player> ().hp <= 0) {
					die = true;
					obj [i].GetComponent<NoLockView_Player> ().mAnim.SetTrigger("die");
					obj [i].GetComponent<NoLockView_Player> ().die = true;
                    end.SetActive(true);
                    stop_last = Time.time;
                }
                return;
            }
            else
            {
                if (useLaser == true)
                {
                    laserRenderer.enabled = false;
					this.GetComponent<AudioSource> ().Stop();
                }
            }
        }
    }

    //攻击方法
    void Attack(GameObject obj)
    {
        GameObject bullet=GameObject.Instantiate(attackWeaponPrefab, firePosition.position, firePosition.rotation);
        bullet.GetComponent<Standard_bullet>().SetTarget(obj.transform);
    }

    

    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = (float)hp / totalHp;
        if (hp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        //GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        //Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }
}
