using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour {

    // 震动标志位
    private bool isshakeCamera = false;
    // 震动幅度
    public float shakeLevel = 3f;
    // 震动时间
    public float setShakeTime = 2.0f;
    // 震动的FPS
    public float shakeFps = 45f;
    private float fps;
    private float shakeTime = 0.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;
    public Camera selfCamera;

    //这里表示king出现的闪电特效
    //这里表示斯莱姆头顶发射的激光
    public LineRenderer laserRenderer1;
    public LineRenderer laserRenderer2;
    //表示闪电特效的位置
    public Transform start1;
    public Transform end1;
    public Transform start2;
    public Transform end2;

    //用于保存闪电坐标的链表
    private List<Vector3> _linePosList;
    private List<Vector3> _linePosList2;
    //美术资源中进行调整
    public float detail = 100;//增加后，线条数量会减少，每个线条会更长。
    public float displacement = 3000;//位移量，也就是线条数值方向偏移的最大值

    //国王出现的爆炸的特效
    public GameObject king_fire;


    //这个代表国王的变量
    public GameObject king;

    void Awake()
    {
        selfCamera = GetComponent<Camera>();
    }
    // Use this for initialization
    void Start()
    {
        shakeTime = setShakeTime;
        fps = shakeFps;
        frameTime = 0.03f;
        shakeDelta = 0.005f;
        _linePosList = new List<Vector3>();
        _linePosList2= new List<Vector3>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isshakeCamera)
        {
            if (shakeTime > 0)
            {
                shakeTime -= Time.deltaTime;
                if (shakeTime <= 0)
                {
                    selfCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                    isshakeCamera = false;
                    shakeTime = setShakeTime;
                    fps = shakeFps;
                    frameTime = 0.03f;
                    shakeDelta = 0.005f;
                    //这里出现爆照的特效
                    Vector3 temp = king.transform.position;
                    Vector3 temp_copy = king.transform.position;
                    temp.y += 1.0f;
                    GameObject effect = GameObject.Instantiate(king_fire, temp, king.transform.rotation);
                    Destroy(effect, 2.0f);
                    king.SetActive(true);
                    king.GetComponent<king_control>().appear = true;
                    if (laserRenderer1.enabled == true)
                    {
                        laserRenderer1.enabled = false;
                    }
                    if (laserRenderer2.enabled == true)
                    {
                        laserRenderer2.enabled = false;
                    }
                    
                }
                else
                {
                    frameTime += Time.deltaTime;
                    if (frameTime > 1.0 / fps)
                    {
                        frameTime = 0;
                        selfCamera.rect = new Rect(shakeDelta * (-1.0f + shakeLevel * Random.value), shakeDelta * (-1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
                    }
                    //在这个地方激发king出现的闪电
                    if (laserRenderer1.enabled == false)
                    {
                        laserRenderer1.enabled = true;
                    }
                    if (laserRenderer2.enabled == false)
                    {
                        laserRenderer2.enabled = true;
                    }
                    new_shandian(start1.position, end1.position);
                    new_shandian2(start2.position, end2.position);
                }
            }
        }
    }
    public void shake()
    {
        isshakeCamera = true;
    }

    
    //在这进行线条的尺度调整
    //收集顶点，中点分形法插值抖动
    //代表第一个闪电的一些函数
    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
    {
        if (displace < detail)
        {
            _linePosList.Add(startPos);
        }
        else
        {
            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;
            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;
            Vector3 midPos = new Vector3(midX, midY, midZ);
            CollectLinPos(startPos, midPos, displace / 2);
            CollectLinPos(midPos, destPos, displace / 2);
        }
    }

    //这里表示对闪电的更新
    private void new_shandian(Vector3 startPos, Vector3 endPos)
    {
            _linePosList.Clear();
            CollectLinPos(startPos, endPos, displacement);
            _linePosList.Add(endPos);
            laserRenderer1.SetVertexCount(_linePosList.Count);
            for (int i = 0, n = _linePosList.Count; i < n; i++)
            {
                laserRenderer1.SetPosition(i, _linePosList[i]);
            }
    }
    //代表第二个闪电的一些函数
    private void CollectLinPos2(Vector3 startPos, Vector3 destPos, float displace)
    {
        if (displace < detail)
        {
            _linePosList2.Add(startPos);
        }
        else
        {
            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;
            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;
            Vector3 midPos = new Vector3(midX, midY, midZ);
            CollectLinPos2(startPos, midPos, displace / 2);
            CollectLinPos2(midPos, destPos, displace / 2);
        }
    }

    //这里表示对闪电的更新
    private void new_shandian2(Vector3 startPos, Vector3 endPos)
    {
        _linePosList2.Clear();
        CollectLinPos2(startPos, endPos, displacement);
        _linePosList2.Add(endPos);
        laserRenderer2.SetVertexCount(_linePosList2.Count);
        for (int i = 0, n = _linePosList2.Count; i < n; i++)
        {
            laserRenderer2.SetPosition(i, _linePosList2[i]);
        }
    }

}
