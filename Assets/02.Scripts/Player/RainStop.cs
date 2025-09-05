using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainStop : MonoBehaviour
{
    public GameObject rainPrefab;
    public GameObject rainObj;

    private readonly string player = "Player";
    void Start()
    {
        rainObj = Instantiate(rainPrefab);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(player))
        {
            Destroy(rainObj);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag(player))
        {
            rainObj = Instantiate(rainPrefab);
            rainObj.transform.position = new Vector3(-250f, 0, 0);
        }
    }
    
}
