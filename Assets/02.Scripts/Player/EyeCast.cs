using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCast : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Transform tr;
    public CrossHair crossHair;
    void Start()
    {
        tr = transform;
        crossHair = GameObject.Find("Image_Aim").GetComponent<CrossHair>();
    }

    void Update()
    {
        ray = new Ray(tr.position, tr.forward); // 현재 위치에서 앞 방향으로 레이 생성
        //Debug.DrawRay(tr.position, tr.forward * 20f, Color.red); // 레이 시각화

        if (Physics.Raycast(ray, out hit, 20f, 1 << 7 | 1 << 8))
        {
            //Debug.Log($"Ray : {hit.collider.name}");
            crossHair.isGaze = true;
        }
        else
        {
            crossHair.isGaze = false;
        }
    }
}
