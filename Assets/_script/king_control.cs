using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class king_control : MonoBehaviour {

    public Transform head;
    private float distanceArriveTarget = 35.0f;
    public Slider hpSlider;
    private float totalhp;
    public float hp = 1000;

	public bool appear = false;

	public GameObject shooterPos;
	public float force=500;
	public GameObject slime;
	private int frame_cnt = 0;

    //在这里获取游戏的结束界面和借结束里面的先关的图片
    //获取结束界面的UI
    public GameObject end;
    public Image image_ui;

    //这里表示斯莱姆头顶发射的激光
    public LineRenderer laserRenderer_silaimu_left;
    public LineRenderer laserRenderer_silaimu_right;
    //这里表示激光开始的位置
    public GameObject fire_position_left;
    public GameObject fire_position_right;

    //这个变量表示攻击的频率
    public float curr_time;
    public float last_tme;
    //这里表示主角的对象
    public GameObject player;

    private bool die = false;
    //表示停止运行游戏的变量
    public float stop_curr;
    public float stop_last;

    // Use this for initialization
    void Start () {
        totalhp = hp;
        curr_time = last_tme = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        //这里表示停止运行游戏
        if (die)
        {
            stop_curr = Time.time;
            if (stop_curr - stop_last >= 0.5)
            {
                //Time.timeScale = 0;
                return;
            }
        }
        
		if (!appear)
			return;
        Vector3 dir_copy = player.transform.position - transform.position;
        if (laserRenderer_silaimu_left.enabled == true)
        {
            laserRenderer_silaimu_left.enabled = false;
        }
        if (laserRenderer_silaimu_right.enabled == true)
        {
            laserRenderer_silaimu_right.enabled = false;
        }
        if (dir_copy.magnitude < 15.0f)
        {
            //这里更新攻击的时间判定
            curr_time = Time.time;
            if (curr_time - last_tme >= 0.1f)
            {
                last_tme = curr_time;
                if (laserRenderer_silaimu_left.enabled == false)
                {
                    laserRenderer_silaimu_left.enabled = true;
                }
                if (laserRenderer_silaimu_right.enabled == false)
                {
                    laserRenderer_silaimu_right.enabled = true;
                }
                //这里开始向人物释放闪电
                Vector3 temp_position = player.transform.position;
                temp_position.y += 1.0f;
                //new_shandian(fire_position.transform.position, temp_position);
                laserRenderer_silaimu_left.SetPositions(new Vector3[] { fire_position_left.transform.position, temp_position });
                laserRenderer_silaimu_right.SetPositions(new Vector3[] { fire_position_right.transform.position, temp_position });
                //在这里进行对敌人的伤害
                player.GetComponent<NoLockView_Player>().TakeDamage(0.3f);
                if (!die && player.GetComponent<NoLockView_Player>().hp <= 0)
                {
                    die = true;
                    player.GetComponent<NoLockView_Player>().mAnim.SetTrigger("die");
                    player.GetComponent<NoLockView_Player>().die = true;
                    end.SetActive(true);
                    stop_last = Time.time;
                }
            }
            else {
                if (laserRenderer_silaimu_left.enabled == true)
                {
                    laserRenderer_silaimu_left.enabled = false;
                }
                if (laserRenderer_silaimu_right.enabled == true)
                {
                    laserRenderer_silaimu_right.enabled = false;
                }
            }
        }
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < obj.Length; i++)
        {
            Vector3 dir = obj[i].transform.position - transform.position;
            if(dir.magnitude < distanceArriveTarget)
            {
                Vector3 targetPosition = obj[i].transform.position;
                targetPosition.y = head.position.y;
                head.LookAt(targetPosition);

				if (++frame_cnt % 20 == 0) {
                    GameObject[] obj_copy = GameObject.FindGameObjectsWithTag("Enemy");
                    if (obj_copy.Length < 50)
                    {
                        GameObject new_slime = Instantiate(slime, shooterPos.transform.position, Quaternion.identity);
                        new_slime.GetComponent<Rigidbody>().AddForce(force * shooterPos.transform.forward);
                    }
				}
                break;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = (float)hp / totalhp;
        if (hp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        //GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        //Destroy(effect, 1.5f);
        end_UI();
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        hurt();
    }

    void hurt()
    {
        int atk = GameObject.FindGameObjectWithTag("Player").GetComponent<NoLockView_Player>().atk;
        //mAudioSource.clip = m_Attck;
        //mAudioSource.Play();
        hp -= atk*10;
        hpSlider.value = (float)hp / totalhp;
        if (hp <= 0)
        {
            Destroy(gameObject);
            end_UI();
        }
    }

    //调用最后的结束界面
    public void end_UI()
    {
        Texture2D aa = (Texture2D)Resources.Load("sources/YOUWIN") as Texture2D;
        Sprite kk = Sprite.Create(aa, new Rect(0, 0, aa.width, aa.height), new Vector2(0.5f, 0.5f));
        image_ui.sprite = kk;
        end.SetActive(true);
        //这里找到场景里面的所有的敌人，直接消灭
        GameObject[] obj_des = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < obj_des.Length; i++)
        {
            Destroy(obj_des[i]);
        }
    }
}
