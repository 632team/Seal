using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class talkFangju : MonoBehaviour {
	public Flowchart talkFlowchart;
	public string onCollision;
    public bool flag = false;
    private float distanceArriveTarget = 0.9f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //用于判断主角是否碰到武器店的商人
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < obj.Length; i++)
        {
            Vector3 dir = obj[i].transform.position - transform.position;
            if (dir.magnitude < distanceArriveTarget)
            {
                if (!flag)
                {
                    flag = true;
                    Block target = talkFlowchart.FindBlock(onCollision);
                    talkFlowchart.ExecuteBlock(target);
                    KnapsackManager.Instance.StoreItem(4);
                    KnapsackManager.Instance.StoreItem(5);
                    KnapsackManager.Instance.StoreItem(6);
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
	//private void OnCollisionEnter(Collision other){
	//	if (other.gameObject.CompareTag("Player")){
	//		Block target = talkFlowchart.FindBlock (onCollision);
	//		talkFlowchart.ExecuteBlock (target);
	//	}
	//}
}
