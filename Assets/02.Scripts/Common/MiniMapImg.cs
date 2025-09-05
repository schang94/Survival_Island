using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapImg : MonoBehaviour
{
    private Image img;
    float timePrev;
    void Start()
    {
        img = GetComponent<Image>();
        timePrev = Time.time;
    }

    void Update()
    {
        if (Time.time - timePrev >= 0.3f)
        {
            img.enabled = !img.enabled;
            timePrev = Time.time;
        }
    }

}
