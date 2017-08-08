using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            int index = Random.Range(0, 10);
            KnapsackManager.Instance.StoreItem(index);
        }
    }
}

