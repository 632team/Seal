using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeControl : MonoBehaviour
{
	
    public Transform player;
    public int speed = 5;
    public float rotSpeed = 4.0f;
    public float hp = 100;
    public Vector3 vc;
    public Slider HPStrip;

    private Vector3 targetDir;
    private int cnt = 0;

    //这里表示斯莱姆头顶发射的激光
    public LineRenderer laserRenderer_silaimu;
    //这里表示激光开始的位置
    public GameObject fire_position;

    //用于保存闪电坐标的链表
    private List<Vector3> _linePosList;
    //美术资源中进行调整
    public float detail = 0.01f;//增加后，线条数量会减少，每个线条会更长。
    public float displacement = 0.01f;//位移量，也就是线条数值方向偏移的最大值

    //这个变量表示攻击的频率
    public float curr_time;
    public float last_tme;

    private bool die;

    //表示停止运行游戏的变量
    public float stop_curr;
    public float stop_last;

    private AudioSource mAudioSource;
    [SerializeField] private AudioClip m_Attck;
	[SerializeField] private AudioClip m_lightning;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        HPStrip.value = HPStrip.maxValue = hp;
        // 声音组件
        mAudioSource = GetComponent<AudioSource>();
        _linePosList = new List<Vector3>();
        curr_time = last_tme = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
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


        if (Vector3.Distance(player.position, transform.position) > 25.0f)
        {

            if (++cnt > 100)
            {
                cnt = 0;
                targetDir = new Vector3(Random.value, transform.position.y, Random.value) - transform.position; // 计算目标相对于自己的朝向
            }

            float step = rotSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            if (laserRenderer_silaimu.enabled == true)
            {
                laserRenderer_silaimu.enabled = false;
            }

        }
        else if (Vector3.Distance(player.position, transform.position) <= 25.0f && Vector3.Distance(player.position, transform.position) > 4.0f)
        {
            targetDir = player.position - transform.position; // 计算目标相对于自己的朝向
            float step = rotSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (laserRenderer_silaimu.enabled == true)
            {
                laserRenderer_silaimu.enabled = false;
            }
        }
        else
        {
            //这里更新攻击的时间判定
            curr_time = Time.time;
            if (curr_time - last_tme >= 0.1f)
            {
                last_tme = curr_time;
                if (laserRenderer_silaimu.enabled == false)
                {
                    laserRenderer_silaimu.enabled = true;
                }
                //这里开始向人物释放闪电
                Vector3 temp_position = player.transform.position;
                temp_position.y += 1.0f;
                //new_shandian(fire_position.transform.position, temp_position);
                laserRenderer_silaimu.SetPositions(new Vector3[] { fire_position.transform.position, temp_position });
                mAudioSource.clip = m_lightning;
                mAudioSource.Play();
                //在这里进行对敌人的伤害
                player.GetComponent<NoLockView_Player>().TakeDamage(0.3f);
                if (!die && player.GetComponent<NoLockView_Player>().hp <= 0)
                {
                    die = true;
                    player.GetComponent<NoLockView_Player>().mAnim.SetTrigger("die");
                    player.GetComponent<NoLockView_Player>().die = true;
					GameObject.FindGameObjectWithTag("Player").GetComponent<NoLockView_Player>().end.SetActive(true);
                    stop_last = Time.time;
                }
            }
            else {
                if (laserRenderer_silaimu.enabled == true)
                {
                    laserRenderer_silaimu.enabled = false;
                }
            } 
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        hurt();
    }

    void hurt()
    {
        int atk = player.GetComponent<NoLockView_Player>().atk;
        mAudioSource.clip = m_Attck;
        mAudioSource.Play();
        hp -= atk;
        HPStrip.value = hp;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    //这里更新敌人的属性值
    public void new_attribute()
    {
        HPStrip.value = hp;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        new_attribute();

    }
}
