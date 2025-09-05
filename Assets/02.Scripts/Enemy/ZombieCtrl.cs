using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshAgent�� �̿��Ͽ� �÷��̾ ���� �����ȿ� ������
// �����ϰ� ���� �����ȿ� ������ �����ϴ� ���� ������ �ִϸ��̼� ����

// �������� ���ݹ����� ���Ϸ��� �Ÿ��� ���ؾ���. �÷��̾�� ������ ��ġ�� �˾ƾ���
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

        float dist = Vector3.Distance(zombieTr.position, playerTr.position); //�����ڴ� ���� �̰� ���
        //float dist = (playerTr.position - zombieTr.position).magnitude; //2d���� ���� ���
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
        Vector3 attackTarget = (playerTr.position - zombieTr.position).normalized; // ���� ���� �Ÿ�
                                                                                   //�÷��̾�(Ÿ��) - ����(�ڱ��ڽ�)�� ��ġ�� ���� ������ ���´�.
                                                                                   //maganitude : ũ��
                                                                                   //normalized : ����
                                                                                   //zombieTr.rotation = Quaternion.LookRotation(zombieTr, attackTarget); // ���� �÷��̾ �ٶ󺸰� ȸ��

        Quaternion rot = Quaternion.LookRotation(attackTarget);
        zombieTr.rotation = Quaternion.Slerp(zombieTr.rotation, rot, Time.deltaTime * rotSpeed);
        // Quaternion.Slerp(��麸�� �Լ�) : �ε巴�� ȸ�� (�ڱ��ڽ�ȸ����, Ÿ��ȸ��, �ð���ŭ ȸ��)
    }
    public void PlayerDie()
    {
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }
}
