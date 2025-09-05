using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieDamage : MonoBehaviour // 상속x => 컴포넌트 x
{
    private readonly string player = "Player";
    private readonly string jumpTag = "JUMPSUPPORT";
    private readonly string bulletTag = "Bullet";
    
    private readonly int hashJump = Animator.StringToHash("IsJump");
    private readonly int hashHit = Animator.StringToHash("IsHit");
    private readonly int hashDie = Animator.StringToHash("IsDie");
    private Rigidbody rb; // Is Kinematic을 설정하기 위해 필요
    private bool isJumping = false;
    private int hp;
    private int maxHp = 100;
    private Animator animator;
    private NavMeshAgent agent;
    public bool isDie = false;

    [Header("UI")]
    public Image hpBar;
    public Text hpText;
    public Canvas canvas;
    public BoxCollider attackCol;

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>(); // 캔버스가 하나밖에 없어서 가능
        //canvas = GetComponentsInChildren<Canvas>()[0]; // 여러개 있을 때
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        hp = maxHp;
        hpBar.color = Color.green;
        attackCol = transform.GetChild(19).GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(jumpTag) && !isJumping)
        {
            isJumping = true;
            animator.SetTrigger(hashJump);
            agent.speed = 0.1f;
        }


    }
    private void OnCollisionEnter(Collision col) // 콜백함수 : 스스로 호출, 호출하지 않아도 호출된다.
    {
        if (isDie) return;

        if (col.gameObject.CompareTag(player))
        {
            rb.isKinematic = true;
        }
        else if (col.gameObject.CompareTag(bulletTag))
        {
            animator.SetTrigger(hashHit);
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            hp -= (int)col.gameObject.GetComponent<BulletCtrl>().damage;
            hp = Mathf.Clamp(hp, 0, maxHp);
            hpBar.fillAmount = (float)hp / maxHp;
            if (hpBar.fillAmount <= 0.3f) hpBar.color = Color.red;
            else if (hpBar.fillAmount <= 0.5f) hpBar.color = Color.yellow;

             hpText.text = $"HP : <color=#FF0000>{hp}</color>";
        }

        if (hp <= 0)
        {
            ZombieDie();
        }
    }

    private void ZombieDie()
    {
        isDie = true;
        animator.SetTrigger(hashDie);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        //Destroy(gameObject, 5f);
        canvas.enabled = false;
        GameManager.instance.UpdateKillCount(1);

        StartCoroutine(PoolPush());
    }

    IEnumerator PoolPush()
    {
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
        isDie = false;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag(player))
        {
            rb.isKinematic = false;
        }
    }

    private void Update()
    {
        if (isJumping && agent.isOnOffMeshLink) // 오브메쉬 링크에 있을 때
        {
            //isJumping = false;
            //agent.speed = 4f;
            StartCoroutine(EnemyJump());
        }
    }

    IEnumerator EnemyJump()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0); // 0은 기본 인덱스 레이어
        yield return new WaitForSeconds(clipInfo.Length);
        isJumping = false;
        agent.speed = 4f;
    }

    public void EnableAttackCollider()
    {
        attackCol.enabled = true;
    }
    public void DisableAttackCollider()
    {
        attackCol.enabled = false;
    }
}
