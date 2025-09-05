using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class SkeletonCtrl : MonoBehaviour
{
    private readonly string playerTxt = "Player";
    private readonly string traceTxt = "IsTrace";
    private readonly string attackTxt = "IsAttack";
     // 동적할당과 동시에 문자열을 읽어서 정수로 변환
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform skeletonTr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private SkeletonDamage skeletonDamage;
    private float traceDist = 20f;
    private float attackDist = 3f;
    void Start()
    {
        skeletonDamage = GetComponent<SkeletonDamage>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skeletonTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag(playerTxt).transform;
    }

    void Update()
    {
        if (skeletonDamage == null) return;
        if (skeletonDamage.isDie) return;
       
        float dist = Vector3.Distance(skeletonTr.position, playerTr.position);

        if (dist <= attackDist)
        {
            animator.SetBool(hashAttack, true);
            agent.isStopped = true;
            
        }
        else if (dist <= traceDist)
        {
            animator.SetBool(hashAttack, false);
            animator.SetBool(hashTrace, true);
            agent.isStopped = false;
            agent.destination = playerTr.position;
        }
        else
        {
            animator.SetBool(hashTrace, false);
            agent.isStopped = true;
        }
    }

    private void OnEnable()
    {
        PlayerDamage.OnPlyaerDie += PlayerDie;
    }

    private void OnDisable()
    {
        PlayerDamage.OnPlyaerDie -= PlayerDie;
    }

    public void PlayerDie()
    {
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }
}
