using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ���ʽ���ƮŰ�� WŰ�� ���ÿ� ������ �� �ִϸ��̼�(���� ����)�� ����ϴ� ��ũ��Ʈ
// �� �� �ϳ��� ���� �ִϸ��̼��� ���߰� ���� �ܴ��� �ִϸ��̼� ����� ����
public class PlayerHandAnimation : MonoBehaviour
{
    private readonly int hashZoom = Animator.StringToHash("isZoom");
    private readonly string running = "running";
    private readonly string runStop = "runStop";
    private readonly string fire = "fire";
    public bool isRun;
    public Animation anim;
    public Animator animator;
    public float speed;
    FireBullet fireBullet;
    void Start()
    {
        fireBullet = GetComponent<FireBullet>();
        anim = transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        //anim = GetComponentsInChildren<Animation>()[0];
        isRun = false;
    }

    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp;
    }
    void UpdateSetUp()
    {
        speed = GameManager.instance.gameData.speed;
    }
    void Update()
    {
        RunningAni();
        PlyerFire();

        if (Input.GetKey(KeyCode.Mouse1) && !isRun && !fireBullet.isReload)
        {
            animator.SetBool(hashZoom, true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool(hashZoom, false);
        }

        if (EventSystem.current.IsPointerOverGameObject()) return;
        //GameManager.instance.MouseCursorVisible();
    }

    public void PlyerFire()
    {
        if (Input.GetMouseButtonDown(0) && !isRun && !fireBullet.isReload)
        {
            anim.Play(fire);
        }
    }

    private void RunningAni()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            anim.Play(running);
            isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.W))
        {
            anim.Play(runStop);
            isRun = false;
        }
    }
}
