using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullte : MonoBehaviour {

    public GameObject explosionEffectPrefab;
    private float distanceArriveTarget = 2.5f;
    private float Turret = 3.5f;
    private float king = 6.5f;
    //这里表示魔法球的攻击力量
    public int damage = 50;

    void Update()
    {
        //在这里判断击中斯莱姆
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < obj.Length; i++)
        {
            Vector3 dir = obj[i].transform.position - transform.position;
            if(dir.magnitude< distanceArriveTarget)
            {
                GameObject effect=GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
                Destroy(this.gameObject);
                Destroy(effect, 1);
                obj[i].GetComponent<SlimeControl>().TakeDamage(damage);
                if (obj[i].GetComponent<SlimeControl>().hp <= 0)
                {
                    Destroy(obj[i]);
                }
                return;
            }
        }

        //这里判断击中炮塔
        GameObject[] obj_turret = GameObject.FindGameObjectsWithTag("Turret");
        for (int i = 0; i < obj_turret.Length; i++)
        {
            Vector3 dir = obj_turret[i].transform.position - transform.position;
            if (dir.magnitude < Turret)
            {
                GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
                Destroy(this.gameObject);
                Destroy(effect, 1);
                obj_turret[i].GetComponent<Turret>().TakeDamage(damage);
                return;
            }
        }

        //在这个地方判断是击中国王
        GameObject[] obj_king = GameObject.FindGameObjectsWithTag("king");
        for (int i = 0; i < obj_king.Length; i++)
        {
            Vector3 dir = obj_king[i].transform.position - transform.position;
            if (dir.magnitude < king)
            {
                GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
                Destroy(this.gameObject);
                Destroy(effect, 1);
                obj_king[i].GetComponent<king_control>().TakeDamage(damage);
                return;
            }
        }
    }
}
