using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 마우스 왼쪽 버튼을 누르면 총알 발사
// 뭐가 필요? 1.FirePos 발사 위치, 2.총알 발사 프리팹, 3.오디오소스 클립
public class FireBullet : MonoBehaviour
{
    private readonly string zombieTag = "ZOMBIE";
    private readonly string skelectonTag = "SKELETON";
    public Transform firePos;
    //public GameObject bullet;
    private AudioSource audioSource;
    public AudioClip audioClip;
    private PlayerHandAnimation playerHandAnimation;
    [Header("Reload")]
    public readonly float reloadTime = 1.5f;
    public readonly int maxBullet = 10;
    public int currentBullet = 10;
    public bool isReload = false;
    private float timePrev;

    public ParticleSystem muzzleFlash; 
    public ParticleSystem cartridge; 
    public Animation ani;
    private WeaponChange weaponChange;
    private readonly string pump = "pump2";

    private int zombieLayer;
    private int skelectonLayer;
    private int barrelLayer;
    private int mapLayer;

    private int layerMask;

    private bool isFire = false;
    private float nextFire;
    public float authFire = 0.15f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ani = transform.GetChild(0).GetChild(0).GetComponent<Animation>();
        muzzleFlash = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        cartridge = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<ParticleSystem>();
        playerHandAnimation = GetComponent<PlayerHandAnimation>();
        weaponChange = GetComponent<WeaponChange>();
        timePrev = Time.time;


        zombieLayer = LayerMask.NameToLayer("Zombie");
        skelectonLayer = LayerMask.NameToLayer("Skeleton");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        mapLayer = LayerMask.NameToLayer("MAP");
        layerMask = 1 << zombieLayer | 1 << skelectonLayer | 1 << barrelLayer | 1 << mapLayer;
    }

    
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.green);

        if (EventSystem.current.IsPointerOverGameObject()) return;

        RaycastHit hit;

        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20f, layerMask))
        {
            isFire = hit.collider.CompareTag(zombieTag) || hit.collider.CompareTag(skelectonTag);
        }
        else
        {
            isFire = false;
        }

        if (!isReload && isFire)
        {
            if (Time.time - nextFire > authFire)
            {
                Fire();
                nextFire = Time.time;
            }
        }

        //GameManager.instance.MouseCursorVisible();
        if (Input.GetMouseButton(0) && (weaponChange.isHaveM4M1 || weaponChange.isHaveAK47))
        {
            if (Time.time - timePrev > 0.1f)
            {
                Fire();
                timePrev = Time.time;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        if (isReload || playerHandAnimation.isRun) return;
        
        //Instantiate(bullet, firePos.position, firePos.rotation);
        GameObject _bullet = PoolingManager.p_Instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }
        audioSource.PlayOneShot(audioClip, 1.0f);
        muzzleFlash.Play();
        cartridge.Play();
        isReload = (--currentBullet % maxBullet == 0);

        if (isReload)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        ani.Play(pump);
        yield return new WaitForSeconds(reloadTime);
        currentBullet = maxBullet;
        isReload = false;
    }
}
