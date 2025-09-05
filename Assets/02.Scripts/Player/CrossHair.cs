using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    private Transform tr;
    private float startTime = 0f;
    private float duration = 0.3f;
    private float minSize = 0.7f;
    private float maxSize = 1.2f;
    private Color originColor = Color.white;
    private Color gazeColor = Color.red;
    private Image crossHairImg;
    public bool isGaze = false; // 겨누고 있는지 확인
    void Start()
    {
        tr = GetComponent<Transform>();
        crossHairImg = GetComponent<Image>();
        startTime = Time.time;
        tr.localScale = Vector3.one * minSize;
        // Vector3.one은 x,y,z의 크기를 동일하게 설정
        crossHairImg.color = originColor;
    }

    void Update()
    {
        if (isGaze)
        {
            float t = (Time.time - startTime) / duration; // 시간 비율 계산
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, t); // 크기 변화 

            crossHairImg.color = gazeColor;
        }
        else
        {
            tr.localScale = Vector3.one * minSize;
            crossHairImg.color = originColor;
            startTime = Time.time;
        }
    }
}
