using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshAgent를 이용하여 플레이어가 추적 범위안에 들어오면
// 추적하고 공격 범위안에 들어오면 공격하는 로직 구현과 애니메이션 연동

// 추적범위 공격범위를 구하려면 거리를 구해야함. 플레이어와 좀비의 위치를 알아야함
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class ZombieCtrl : MonoBehaviour
{
    private readonly string traceTxt = "IsTrace";
    private readonly string attackTxt = "IsAttack";

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform zombieTr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private ZombieDamage z_damage;
    public float traceDist = 20.0f;
    public float attackDist = 3.0f;
    public float rotSpeed = 10f;

    void Start()
    {
        z_damage = GetComponent<ZombieDamage>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        zombieTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (z_damage == null) return;

        if (z_damage.isDie) return;

        float dist = Vector3.Distance(zombieTr.position, playerTr.position); //개발자는 보통 이거 사용
        //float dist = (playerTr.position - zombieTr.position).magnitude; //2d에서 많이 사용
        if (dist <= attackDist)
        {
            PlayerAttack();
        }
        else if (dist <= traceDist)
        {
            PlayerTrace();
        }
        else
        {
            PlayerIdle();
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
    private void PlayerIdle()
    {
        animator.SetBool(hashTrace, false);
        agent.isStopped = true;
    }

    private void PlayerTrace()
    {
        animator.SetBool(hashAttack, false);
        animator.SetBool(hashTrace, true);
        agent.isStopped = false;
        agent.destination = playerTr.position;
    }

    private void PlayerAttack()
    {
        animator.SetBool(hashAttack, true);
        agent.isStopped = true;
        Vector3 attackTarget = (playerTr.position - zombieTr.position).normalized; // 공격 대상과 거리
                                                                                   //플레이어(타겟) - 좀비(자기자신)의 위치를 빼면 방향이 나온다.
                                                                                   //maganitude : 크기
                                                                                   //normalized : 방향
                                                                                   //zombieTr.rotation = Quaternion.LookRotation(zombieTr, attackTarget); // 좀비가 플레이어를 바라보게 회전

        Quaternion rot = Quaternion.LookRotation(attackTarget);
        zombieTr.rotation = Quaternion.Slerp(zombieTr.rotation, rot, Time.deltaTime * rotSpeed);
        // Quaternion.Slerp(곡면보간 함수) : 부드럽게 회전 (자기자신회전값, 타켓회전, 시간만큼 회전)
    }
    public void PlayerDie()
    {
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }
}
