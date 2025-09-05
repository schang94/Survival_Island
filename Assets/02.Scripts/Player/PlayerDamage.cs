using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerDamage : MonoBehaviour
{
    private readonly string punch = "PUNCH";
    private readonly string sword = "SWORD";

    float hp;
    float maxHp = 100f;

    public Image hpBar;
    public Text hpTxt;
    public GameObject dieImg;

    public bool isDie = false;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlyaerDie;

    void Start()
    {
        hp = maxHp;
        hpBar = GameObject.Find("Canvas_UI").transform.GetChild(0).GetChild(0).GetComponent<Image>();
        hpTxt = GameObject.Find("Canvas_UI").transform.GetChild(0).GetChild(1).GetComponent<Text>();
        hpBar.color = Color.blue;
        dieImg = GameObject.Find("Canvas_UI").transform.GetChild(4).gameObject;
    }

    private void OnEnable()
    {
        GameManager.OnItemChange += UpdatSetup;
    }
    void UpdatSetup()
    {
        hp += GameManager.instance.gameData.hp - maxHp;
        maxHp = GameManager.instance.gameData.hp;
    }
    private void OnTriggerEnter(Collider col)
    {
        // OnTriggerEnter에서 gameObject 생략 가능
        if (col.CompareTag(punch))
        {
            HitDamage(10);
        }
        else if (col.CompareTag(sword))
        {
            HitDamage(15);
        }
    }

    private void HitDamage(float damage)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHp);
        hpBar.fillAmount = (float)hp / maxHp;
        if (hpBar.fillAmount <= 0.3f) hpBar.color = Color.red;
        else if (hpBar.fillAmount <= 0.5f) hpBar.color = Color.yellow;
        hpTxt.text = $"HP : <color=#FF0000>{hp}</color>";

        if (hp <= 0)
        {
            isDie = true;
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        //dieImg.SetActive(true);
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>(); // 현재 오브젝트의 모든 스크립트를 가져오기
        foreach (var script in scripts)
        {
            script.enabled = false; // 모든 MonoBehaviour 스크립트 비활성화
        }
        OnPlyaerDie();
        // 플레이어 스크립트 비활성화
        Invoke("SceneMove", 5f);
    }

    void SceneMove()
    {
        SceneManager.LoadScene("EndScene");
    }

}
