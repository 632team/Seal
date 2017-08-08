using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class big_control : MonoBehaviour {

    public GameObject big_p;
    //表示消失时的特效
    public GameObject disappear;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, transform.position) < 2.5f)
        {
            Destroy(big_p);
            //这里显示消失的特效
            GameObject effect = GameObject.Instantiate(disappear, transform.position, transform.rotation);
            effect.transform.localScale= new Vector3(0.8f, 0.8f, 0.8f);
            Destroy(effect, 2f);

            //这里增加人物的相应血条或蓝条
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (tag == "bighp")
            {
                player.GetComponent<NoLockView_Player>().hp = 30;
                KnapsackManager.Instance.StoreItem(2);
                KnapsackManager.Instance.StoreItem(2);
                KnapsackManager.Instance.StoreItem(2);
                KnapsackManager.Instance.StoreItem(2);
            }
            if (tag == "bigmp")
            {
                player.GetComponent<NoLockView_Player>().mp = 20;
                KnapsackManager.Instance.StoreItem(3);
                KnapsackManager.Instance.StoreItem(3);
                KnapsackManager.Instance.StoreItem(3);
                KnapsackManager.Instance.StoreItem(3);
            }
            player.GetComponent<NoLockView_Player>().new_Attributes();
        }
    }
}
