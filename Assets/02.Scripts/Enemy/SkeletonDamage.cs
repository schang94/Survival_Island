using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SkeletonDamage : MonoBehaviour
{
    private readonly string player = "Player";
    private readonly string bullet = "Bullet";
    private readonly string jump = "JUMPSUPPORT";

    private readonly int hashHit = Animator.StringToHash("IsHit");
    private readonly int hashDie = Animator.StringToHash("IsDie");
    private readonly int hashJump = Animator.StringToHash("IsJump");

    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;
    private int hp;
    private int maxHp = 100;
    public bool isDie = false;
    private bool isJump = false;

    private Canvas canvas;
    private Text hpText;
    private Image hpBar;

    public BoxCollider attackCol;


    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        hp = maxHp;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        hpText = transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>();
        hpBar = transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>();
        hpBar.color = Color.green;

        attackCol = transform.GetChild(4).GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (isDie) return;

        if (col.gameObject.CompareTag(player))
        {
            rb.isKinematic = true;
        }
        else if (col.gameObject.CompareTag(bullet))
        {
            col.gameObject.SetActive(false);
            hp -= (int)col.gameObject.GetComponent<BulletCtrl>().damage;
            hp = Mathf.Clamp(hp, 0, maxHp);
            animator.SetTrigger(hashHit);
            hpBar.fillAmount = (float)hp / maxHp;
            if (hpBar.fillAmount <= 0.3f) hpBar.color = Color.red;
            else if (hpBar.fillAmount <= 0.5f) hpBar.color = Color.yellow;

            hpText.text = $"HP : <color=#FF0000>{hp}</color>";
        }

        if (hp <= 0)
        {
            SkeletonDie();
        }
    }

    private void SkeletonDie()
    {
        isDie = true;
        animator.SetTrigger(hashDie);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
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
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(jump))
        {
            animator.SetTrigger(hashJump);
            agent.speed = 0.1f;
            isJump = true;
        }
    }

    void Update()
    {
        if (isJump && agent.isOnOffMeshLink)
        {
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        AnimatorClipInfo[] animatorClipInfos = animator.GetCurrentAnimatorClipInfo(0);
        yield return new WaitForSeconds(animatorClipInfos.Length);
        agent.speed = 4f;
        isJump = false;
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
