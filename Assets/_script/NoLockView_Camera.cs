using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NoLockView_Camera : MonoBehaviour {

	//观察目标  
	public Transform Target;  
	//观察距离  
	public float Distance = 5F;  
	//旋转速度  
	private float SpeedX=240;  
	private float SpeedY=120;  
	//角度限制  
	private float  MinLimitY = 5;  
	private float  MaxLimitY = 180;  

	//旋转角度  
	private float mX = 0.0F;  
	private float mY = 0.0F;  

	//鼠标缩放距离最值  
	private float MaxDistance=10;  
	private float MinDistance=1.5F;  
	//鼠标缩放速率  
	private float ZoomSpeed=2F;  

	//是否启用差值  
	public bool isNeedDamping=true;  
	//速度  
	public float Damping=2.5F;

    //用于整个背包显示的公共变量
    public GameObject KnapsackMenu;
	public GameObject escMenu;

    //转移特效
    public GameObject Portal;

    //人物的对象
    public GameObject Brave;
    //记录转移之前的时间
    private float lasttime = 0;
    //用于判断是否移动人物的变量
    private bool brave_flags = false;
    //用于移动后判断特效的消失
    private bool portal_flags = false;
    //用于控制进度条显示
    public Slider slider;
    public GameObject panel;
    //用于判断是否移动人物的变量，这个是king区域
    public bool brave_flags_copy = false;
    //用于判断移动后特效消失的变量，这个是king区域
    public bool portal_flags_copy = false;


    void Start ()   
	{  
		Target = GameObject.FindWithTag ("Player").transform;
		//初始化旋转角度  
		mX=transform.eulerAngles.x;  
		mY=transform.eulerAngles.y + 20;  
	}  

	void LateUpdate ()   
	{  

		if (Brave.GetComponent<NoLockView_Player> ().die)
			return;
		//鼠标右键旋转  
		if(Target!=null && Input.GetMouseButton(1))  
		{  
			//获取鼠标输入  
			mX += Input.GetAxis("Mouse X") * SpeedX * 0.02F;  
			mY -= Input.GetAxis("Mouse Y") * SpeedY * 0.02F;  
			//范围限制  
			mY = ClampAngle(mY,MinLimitY,MaxLimitY);  
		}  

		//鼠标滚轮缩放  

		Distance-=Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;  
		Distance=Mathf.Clamp(Distance,MinDistance,MaxDistance);  

		//重新计算位置和角度  
		Quaternion mRotation = Quaternion.Euler(mY, mX, 0);  
		Vector3 mPosition = mRotation * new Vector3(0.0F, 1.0F, -Distance) + Target.position;  

		//设置相机的角度和位置  
		if(isNeedDamping){  
			//球形插值  
			transform.rotation = Quaternion.Lerp(transform.rotation,mRotation, Time.deltaTime*Damping);   
			//线性插值  
			transform.position = Vector3.Lerp(transform.position,mPosition, Time.deltaTime*Damping);   
		}else{  
			transform.rotation = mRotation;  
			transform.position = mPosition;  
		}  
		//将玩家转到和相机对应的位置上  
		if(Target.GetComponent<NoLockView_Player>().State==NoLockView_Player.PlayerState.Walk)  
		{  
			Target.eulerAngles=new Vector3(0,mX,0);  
		}

        //将背包界面显示出来
        if (Input.GetKeyDown("i"))
        {
            Time.timeScale = 0;
            KnapsackMenu.SetActive(true);
        }

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Time.timeScale = 0;
			escMenu.SetActive(true);
		}

        //在这里将人物转移
        if (Input.GetKeyDown("p"))
        {
            lasttime = Time.time;
            brave_flags = true;
            Portal = GameObject.Instantiate(Portal);
            Portal.transform.localPosition = Brave.transform.position;
            Portal.SetActive(true);
            panel.SetActive(true);
			// 播放转移音效
			GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().mAudioSource.clip =
				GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().m_transport;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().mAudioSource.Play ();
        }
        if (brave_flags)
        {
            slider.value = (Time.time - lasttime) / 3;
            if (Time.time - lasttime >= 3)
            {
                slider.value = 1;
                panel.SetActive(false);
                move_brave();
            }
        }
        if (portal_flags)
        {
            if (Time.time - lasttime >= 2)
            {
                portal_flags = false;
                Portal.SetActive(false);
                //Destroy(Portal);
            }
        }

        //下面的代码是将人物瞬间移动到King区域
        if (Input.GetKeyDown("o"))
        {
            lasttime = Time.time;
            brave_flags_copy = true;
            Portal = GameObject.Instantiate(Portal);
            Portal.transform.localPosition = Brave.transform.position;
            Portal.SetActive(true);
            panel.SetActive(true);
			// 播放转移音效
			GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().mAudioSource.clip =
				GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().m_transport;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().mAudioSource.Play ();
        }
        if (brave_flags_copy)
        {
            slider.value = (Time.time - lasttime) / 3;
            if (Time.time - lasttime >= 3)
            {
                slider.value = 1;
                panel.SetActive(false);
                move_brave_copy();
            }
        }
        if (portal_flags_copy)
        {
            if (Time.time - lasttime >= 2)
            {
                portal_flags_copy = false;
                //Destroy(Portal);
                Portal.SetActive(false);
            }
        }

    }  

	private float  ClampAngle (float angle,float min,float max)   
	{  
		if (angle < -360) angle += 360;  
		if (angle >  360) angle -= 360;  
		return Mathf.Clamp (angle, min, max);  
	}

    //背包界面上面的按钮，回到游戏
    public void OnResume()
    {
        Time.timeScale = 1f;
        KnapsackMenu.SetActive(false);
		escMenu.SetActive(false);
    }

    //移动人物的函数
    private void move_brave()
    {
        float x = (float)-50;
        float y = (float)2.5;
        float z = (float)-50.3;
        Vector3 brave_t = new Vector3(x, y, z);
        Brave.transform.localPosition = brave_t;
        float ry = (float)10.261;
        Brave.transform.rotation= Quaternion.Euler(0, ry, 0);
        Portal.transform.localPosition = brave_t;
        //这里移动一次了将判断变量变为false
        brave_flags = false;
        portal_flags = true;
        lasttime = Time.time;
    }

    //这里将人物移动到King区域
    private void move_brave_copy()
    {
		float x = (float)87.3f;
        float y = (float)0.2f;
		float z = (float)203.3f;
        Vector3 brave_t = new Vector3(x, y, z);
        Brave.transform.localPosition = brave_t;
        float ry = (float)10.261;
        Brave.transform.rotation = Quaternion.Euler(0, ry, 0);
        Portal.transform.localPosition = brave_t;
        //这里移动一次了将判断变量变为false
        brave_flags_copy = false;
        portal_flags_copy = true;
        lasttime = Time.time;
    }


    //结束界面的重玩按钮时间
    public void restart()
    {
		Time.timeScale = 1f;
        SceneManager.LoadScene("world");
		//GameObject.FindGameObjectWithTag ("Player").GetComponent<NoLockView_Player> ().die = false;
    }
    //结束界面的菜单界面
    public void muen()
    {
		Time.timeScale = 1f;
		GameObject.FindGameObjectWithTag ("menubgm").GetComponent<AudioSource> ().Play ();
		GameObject.FindGameObjectWithTag ("bgm").GetComponent<AudioSource> ().Stop ();
		SceneManager.LoadScene("Menu");
    }
}

public class JFConst
{

	public static  bool TouchBegin()
	{
		if(Input.GetMouseButtonDown(0))
		{
			return true;
		}
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			return true;
		}
		return false;
	}

	public static bool TouchEnd()
	{
		if(Input.GetMouseButtonUp(0))
		{
			return true;
		}
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			return true;
		}
		return false;
	}

	public static bool TouchIng()
	{
		if(Input.GetMouseButton(0))
		{
			return true;
		}else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			return true;
		}
		return false;
	}
}