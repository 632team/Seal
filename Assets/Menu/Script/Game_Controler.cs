using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Controler : MonoBehaviour {

    public GameObject setting;
    static string GameLevel;
    AsyncOperation async;
    public float tempProgress;
    public Slider slider;
    public Text text;
    public 
    void Start()
    {

        tempProgress = 0;
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            async = SceneManager.LoadSceneAsync(GameLevel);
            async.allowSceneActivation = false;
        }
    }
    void Update()
    {
        if (text && slider)
        {
            tempProgress = Mathf.Lerp(tempProgress,async.progress,Time.deltaTime*10);

            text.text = "勇者着陆进行中： " + ((int)(tempProgress / 9*10 * 100)).ToString() + "%";
            slider.value = tempProgress / 9*10;
            if (tempProgress/9*10 >= 0.995)
            {
				text.text = "勇者着陆完成\\(^o^)/~";
                slider.value = 100;
                async.allowSceneActivation = true;
            }
        }
    }

    public void OnSoundOff(bool isActive)
    {
        print(isActive);
    }
    public void OnSoundValueChanged(float value)
    {
        print(value);
    }
    public void return_menu()
    {
        SceneManager.LoadScene("Menu");
    }

    //最开始界面的几个按钮
    public void OnStartGame(string GameLevelName)
    {
        GameLevel = GameLevelName;
        SceneManager.LoadScene("Loading");
    }
    public void OnContinue()
    {

    }
    public void OnSetting()
    {
        SceneManager.LoadScene("Setting");
        //iTween.MoveTo(setting, setting.transform.position + new Vector3(340, 0.0f, 0.0f), 0.5f);
    }
	public void OnHelp()
	{
		SceneManager.LoadScene("Help");
	}
    public void OnExit()
    {
		Application.Quit();
    }
}
