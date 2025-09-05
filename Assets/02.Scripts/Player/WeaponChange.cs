using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 메쉬랜더러가 필요
public class WeaponChange : MonoBehaviour
{
    private readonly string drawTag = "draw";
    public SkinnedMeshRenderer Spas12;
    public MeshRenderer[] AK47;
    public MeshRenderer[] M4A1;
    public Animation anim;
    public bool isHaveM4M1 = false;
    public bool isHaveAK47 = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponChangeAni();
            KeyOne();
            isHaveM4M1 = false;
            isHaveAK47 = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponChangeAni();
            KeyTwo();
            isHaveM4M1 = false;
            isHaveAK47 = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponChangeAni();
            KeyThree();
            isHaveM4M1 = true;
            isHaveAK47 = false;
        }
    }

    void WeaponChangeAni()
    {
        
        anim.Play(drawTag);
    }
    void KeyOne()
    {
        for (int i = 0; i < AK47.Length; i++)
        {
            AK47[i].enabled = true;
        }
        Spas12.enabled = false;
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = false;
        }
    }

    void KeyTwo()
    {
        for (int i = 0; i < AK47.Length; i++)
        {
            AK47[i].enabled = false;
        }
        Spas12.enabled = true;
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = false;
        }
    }

    void KeyThree()
    {
        for (int i = 0; i < AK47.Length; i++)
        {
            AK47[i].enabled = false;
        }
        Spas12.enabled = false;
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = true;
        }
    }
}
