using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standard_bullet : MonoBehaviour {

    //表示炮塔子弹的攻击力量和速度
    public float damage = 5;
    public float speed = 20;
    private Transform target;
    public GameObject explosionEffectPrefab;
    private float distanceArriveTarget = 2.5f;

    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            Die();
            return;
        }
        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Vector3 dir = target.position - transform.position;
        if (dir.magnitude < distanceArriveTarget)
        {
            //可以在这里进行写人物的减血操作
            GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
            Destroy(effect, 1);
            return;
        }
    }
    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(effect, 1);
    }
}
