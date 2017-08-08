using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class NoLockView_Player : MonoBehaviour {

	/*自由视角下的角色控制*/  

	public GameObject end;

	//玩家的行走、跳跃速度  
	public float walkSpeed = 4.0F; 
	public float jumpSpeed = 7.0f;
	//重力  
	public float Gravity = 30.0f;  
	//玩家方向
	Vector3 mDir = Vector3.zero; 
	//玩家攻击力
	public int atk = 10;
    //玩家的防御力
    public float defence = 10;
	public float hp = 30;
	public float mp = 20;
	public Slider HPStrip, MPStrip;
	private bool jump = false;
	private float lasty = 0.0f;
	//角色控制器  
	private CharacterController mController;  
	public bool die = false;
	//动画组件  
	public Animator mAnim;  
	//声音组件
	private bool walk = true;
	private int walkcnt = 0;
	public AudioSource mAudioSource;
	[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
	[SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
	[SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
	public AudioClip m_fireBall;
	public AudioClip m_transport;
	public AudioClip m_wind;
	public AudioClip m_f;

    //这里表示的是人物的大招
    public GameObject Trick;

    //这个变量代表另外一个技能
    public GameObject shooterPos;
    public Rigidbody fire_tornato;
    public float force = 300;

    //龙卷风播放
    private int last_cnt = -100, cur_cnt = 0;
	//背景音乐控制
	// 1村庄，2野区，3boss，4死亡，5胜利
	private int scence = 0;
    
    //用于判断人物是否装备的剑
    public bool flag_equipment = false;
    //用于对应装备栏的东西
    public GameObject equipment;

    //这几个变量代表装备栏上的变量的值
    public Slider HP_Slider;
    public Slider MP_Slider;
    public Text atk_text;
    public Text defense_text;

    //这两个变量用来表示单手剑和盾牌
    public GameObject sword;
    public GameObject sheild;

    //***********************************************
    //这个地方是修改的代码
    //这个表示判断是否无敌的变量
    public bool Invincible = false;
    //这个变量表示能3次无敌
    public float count_Invincible = 3;
    //用户辅助无敌的两个变量
    public float last_time;
    public float curr_time;
    //这个变量表示无敌的特效
    public GameObject InvinciblePrefab;
    //这里表示无敌的进度条
    public Slider Invincible_slider;
    //容纳无敌进度调的panel
    public GameObject Invincible_panel;
    //这里表示人物血条的减少
    public void TakeDamage(float damage)
    {
        if (Invincible == true)
        {
        }
        else
        {
            hp -= damage;
            new_Attributes();
        }
    }

    //***********************************************

    [HideInInspector]  
	//玩家状态，默认为Idle  
	public PlayerState State=PlayerState.Idle;  
	public static Flowchart flowchartManager;
	public Flowchart talkFlowchart;
	Rigidbody playerRigidbody;
	void Awake(){
		playerRigidbody = GetComponent<Rigidbody> ();
		flowchartManager = GameObject.Find ("对话控制器").GetComponent <Flowchart> ();
	}


	//判断是否正在对话
	public static bool talking{
		get{
			return flowchartManager.GetBooleanVariable("对话中");
		}
	}

	//定义玩家的状态枚举  
	public enum PlayerState  
	{  
		Idle,  
		Walk  
	}  

	void Start ()   
	{  
		GameObject.FindGameObjectWithTag ("menubgm").GetComponent<AudioSource> ().Stop ();

		//获取角色控制器  
		mController=GetComponent<CharacterController>();  
		//获取动画组件  
		mAnim=GetComponentInChildren<Animator>();  
		// 声音组件
		mAudioSource = GetComponent<AudioSource>();
		HPStrip.value = HPStrip.maxValue = hp;
		MPStrip.value = MPStrip.maxValue = mp;
        HP_Slider.value = HP_Slider.maxValue = hp;
        MP_Slider.value = MP_Slider.maxValue = mp;
		die = false;
    }  


	void Update ()   
	{  
		if (!die) {
			// 背景音乐
			float x = transform.position.x;
			float z = transform.position.z;
			if (x <= 0 && z <= 0) {
				if (scence != 1) {
					GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load ("Audio/country", typeof(AudioClip));
					GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().Play ();
					scence = 1;
				}
			} else if (GameObject.FindGameObjectWithTag ("king") != null) {
				if (scence != 3) {
					GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load ("Audio/boss", typeof(AudioClip));
					GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().Play ();
					scence = 3;
				}
			} else {
				if (scence != 2) {
					GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load ("Audio/out", typeof(AudioClip));
					GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().Play ();
					scence = 2;
				}
			}

			MoveManager ();
		}
		else {
			if (scence != 4) {
				GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load ("Audio/death", typeof(AudioClip));
				GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().loop = false;
				GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().Play ();
				scence = 4;
			}
		}
	}  

    //更新属性函数
    public void new_Attributes()
    {
        //更新角色的属性的相关值
        HPStrip.value = hp;
        MPStrip.value = mp;
        HP_Slider.value = hp;
        MP_Slider.value = mp;
        atk_text.text = atk.ToString();
        defense_text.text = defence.ToString();
    }
    //玩家移动控制  
    void MoveManager()  
	{
        //这里更新无敌状态的相关的变量
        if (Invincible == true)
        {
            curr_time = Time.time;
            Invincible_slider.value = (curr_time - last_time) / 10;
            if (curr_time - last_time >= 10)
            {
                Invincible = false;
                InvinciblePrefab.SetActive(false);
                Invincible_panel.SetActive(false);
				GameObject.FindGameObjectWithTag ("invincible").GetComponent<AudioSource> ().Stop ();
            }
        }


        new_Attributes();
        if (!talking){
			float mul = 1.0f; // 移动速度倍数
			if(mController.isGrounded || jump)  
			{  
				float z = Input.GetAxis("Vertical");
				float x = Input.GetAxis ("Horizontal");
				mDir = new Vector3(x, lasty, z);

				if (z > 0.0f) {  
					mAnim.SetFloat ("Y", 2);
					mul = 1.5f;
					State = PlayerState.Walk;  
				} 
				if(z < -0.0f)  
				{  
					mAnim.SetFloat ("Y", -2);
					State=PlayerState.Walk;  
				}  
				if(x < -0.0f)  
				{  
					mAnim.SetFloat ("X", -2);
					mul = 1.0f;
					State=PlayerState.Walk;  
				}  
				if(x > 0.0f)  
				{  
					mAnim.SetFloat ("X", 2);
					mul = 1.0f;
					State=PlayerState.Walk;  
				}  
				//角色的Idle动画  
				if (x == 0 && z == 0) {  
					mAnim.SetFloat ("X", 0);
					mAnim.SetFloat ("Y", 0);
					State = PlayerState.Idle;  
					walk = false;
				} else {
					walk = true;
				}

                //***************************************************************************
                //这个地方是修改的代码
                //这个地方开启无敌
                if (Input.GetKeyDown("u"))
                {
                    if (count_Invincible > 0)
                    {
                        Invincible_panel.SetActive(true);
                        InvinciblePrefab.SetActive(true);
                        curr_time = last_time = Time.time;
                        Invincible = true;
                        count_Invincible--;
						GameObject.FindGameObjectWithTag ("invincible").GetComponent<AudioSource> ().Play ();
                    }
                }
                //***************************************************************************

                // 击退技能
                if (Input.GetKeyDown("f")) {
					mAnim.SetTrigger ("power");
                    Vector3 temp = transform.position;
                    temp.y += 0.7f;
                    GameObject effect = GameObject.Instantiate(Trick, temp, transform.rotation);
                    Destroy(effect, 1.5f);
                    Collider[] colliders = Physics.OverlapSphere(temp, 100.0f);
                    foreach (Collider hit in colliders)
                    {
                        if (hit.GetComponent<Rigidbody>())
                            hit.GetComponent<Rigidbody>().AddExplosionForce(4000.0f, temp, 100.0f, 3.0F);
                        //在这里判断是否是斯莱姆
                        if(hit.tag== "Enemy")
                        {
                            hit.GetComponent<SlimeControl>().TakeDamage(50);
                            if (hit.GetComponent<SlimeControl>().hp <= 0)
                            {
                                Destroy(hit.transform.root.gameObject);
                            }
                        }
                        //在这里判断是否是king
                        if (hit.tag == "Turret")
                        {
                            hit.GetComponent<Turret>().TakeDamage(50);
                        }
                        //在这判断是否是炮塔
                        if (hit.tag == "king")
                        {
                            hit.GetComponent<king_control>().TakeDamage(50);
                        }
                    }
					// 击退音效
					GameObject.FindGameObjectWithTag("SkillAudio").GetComponent<AudioSource>().clip = m_f;
					GameObject.FindGameObjectWithTag("SkillAudio").GetComponent<AudioSource>().Play ();
                }

                ++cur_cnt;
                if (cur_cnt - last_cnt == 36)
                {
                    Rigidbody new_fire_tornato = Instantiate(fire_tornato, shooterPos.transform.position, Quaternion.identity);
                    new_fire_tornato.transform.Rotate(-90, 0, -90);
                    new_fire_tornato.AddForce(force * shooterPos.transform.forward);
					// 龙卷风音效
					GameObject.FindGameObjectWithTag("SkillAudio").GetComponent<AudioSource>().clip = m_wind;
					GameObject.FindGameObjectWithTag("SkillAudio").GetComponent<AudioSource>().Play ();
                }

                if (Input.GetKeyDown("e"))
                {
                    mAnim.SetTrigger("skill");
                    last_cnt = cur_cnt;
                }

				if (Input.GetMouseButton (0)) {
					int cur = mAnim.GetInteger ("attack");
					if (cur != 2) {
						mAnim.SetInteger ("attack", cur + 1);
					}
					walk = false;
				} else {
					mAnim.SetInteger ("attack", 0);
				}

				if (Input.GetKeyDown (KeyCode.Space) && !jump) {
					jump = true;
					mDir.y = jumpSpeed;
					lasty = jumpSpeed;
					mul = 1.2f;
					mAnim.SetBool ("jump", true);
					// 播放跳跃
					mAudioSource.clip = m_JumpSound;
					mAudioSource.Play();
				} else {
					lasty -= Gravity * Time.deltaTime;
					if (jump && mController.isGrounded) {
						mAnim.SetBool ("jump", false);
						jump = false;
						mAudioSource.clip = m_LandSound;
						mAudioSource.Play();
					}
				}
				if (walk && !jump) {
					if (walkcnt * mul > 10)
						walkcnt = 0;
					playWalkSound ();
					++walkcnt;
				} else {
					walkcnt = 0;
				}
			}  
			//考虑重力因素  
			mDir=transform.TransformDirection(mDir);
			mDir += Vector3.down * Gravity * Time.deltaTime;
			mController.Move(mDir * Time.deltaTime * walkSpeed * mul);  
		}else{
			playerRigidbody.Sleep ();
		}  
	}

	void playWalkSound() {
		if (walkcnt != 0)
			return;
		int n = Random.Range(1, 4);
		mAudioSource.clip = m_FootstepSounds[n];
		mAudioSource.PlayOneShot(mAudioSource.clip);
		// move picked sound to index 0 so it's not picked next time
		m_FootstepSounds[n] = m_FootstepSounds[0];
		m_FootstepSounds[0] = mAudioSource.clip;
	}

    //这里为物品的消耗函数
    public void Consumption()
    {
        Transform[] Grids = equipment.GetComponent<equipmentUI>().Grids;
        for(int i = 5; i <= 7; i++)
        {
            if (Grids[i].childCount == 0)
            {
                continue;
            }
            else
            {
                Item item = ItemModel.GetItem(Grids[i].name);
                switch (item.Name)
                {
                    case "治愈药水":  //这里代表增加HP
                        Consumable consumable1 = item as Consumable;
                        hp += consumable1.BackHp;
                        if (hp > HPStrip.maxValue)
                        {
                            hp = HPStrip.maxValue;
                        }
                        break;
                    case "魔法药水": //这里代表增加MP
                        Consumable consumable2 = item as Consumable;
                        mp += consumable2.BackMp;
                        if (mp > MPStrip.maxValue)
                        {
                            mp = MPStrip.maxValue;
                        }
                        break;
                    default:
                        break;
                }
                //在这里将消耗的物品删除掉
                Destroy(Grids[i].GetChild(0).gameObject);
            }
        }
        new_Attributes();
    }

    //这里为物品的装备函数
    public void fun_equipment()
    {
        defence = 10;
        atk = 10;
        Transform[] Grids = equipment.GetComponent<equipmentUI>().Grids;
        for (int i = 0; i <= 4; i++)
        {
            if (Grids[i].childCount == 0)
            {
                continue;
            }
            else
            {
                Item item = ItemModel.GetItem(Grids[i].name);
                switch (item.ItemType)
                {
                    case "Armor":  //这里代表防御
                        Armor armor = item as Armor;
                        defence += armor.Defend;
                        break;
                    case "Weapon": //这里代表攻击
                        Weapon weapon = item as Weapon;
                        atk += weapon.Damage;
                        if (weapon.Name == "单手剑")
                        {
                            sword.SetActive(true);
                        }
                        if (weapon.Name == "盾牌")
                        {
                            sheild.SetActive(true);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        new_Attributes();
    }
}
