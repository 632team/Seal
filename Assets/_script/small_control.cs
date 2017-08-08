using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_control : MonoBehaviour {

    public GameObject small_p;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, transform.position) < 1.0f)
        {
            Destroy(small_p);
            if (tag == "small_hp")
            {
                KnapsackManager.Instance.StoreItem(2);
            }
            if (tag == "small_mp")
            {
                KnapsackManager.Instance.StoreItem(3);
            }
        }
    }
}
