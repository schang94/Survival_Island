using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_Instance = null;
    [SerializeField] private GameObject bulletPref;
    private int maxbullet = 10;
    [SerializeField] private List<GameObject> bullets = new List<GameObject>();

    public List<Transform> spawnList;
    public GameObject zombiePref;
    public List<GameObject> zombiePool;
    public GameObject skelectPref;
    public List<GameObject> skelectPool;

    private int maxZombieCount = 10;
    private int maxSkeleton = 5;

    private void Awake()
    {
        if (p_Instance == null)
        {
            p_Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (p_Instance != this)
        {
            Destroy(this.gameObject);
        }

        CreateBullet();

        var spawnPos = GameObject.Find("SpawnPoints").gameObject;
        if (spawnPos != null)
            spawnPos.GetComponentsInChildren<Transform>(spawnList);
        spawnList.RemoveAt(0);

        StartCoroutine(CreateZombie());
        StartCoroutine(CreateSkeleton());
    }

    private void Start()
    {
        InvokeRepeating("ZombieSpawn", 0.02f, 3f);
        InvokeRepeating("SkeletonSpawn", 0.02f, 5f);
    }
    private void CreateBullet()
    {
        GameObject bulletObjects = new GameObject("bulletObjects");

        for (int i = 0; i < maxbullet; i++)
        {
            GameObject _bullet = Instantiate(bulletPref, bulletObjects.transform);
            _bullet.name = $"{i + 1}_bullet";
            _bullet.SetActive(false);
            bullets.Add(_bullet);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < bullets.Count;i++)
        {
            if (bullets[i].activeSelf == false)
            {
                return bullets[i];
            }
        }
        return null;
    }

    IEnumerator CreateZombie()
    {
        yield return new WaitForSeconds(0.5f);

        var ZombieGroup = new GameObject("ZombieGroup");

        for (int i = 0; i < maxZombieCount; i++)
        {
            var _zombie = Instantiate(zombiePref, ZombieGroup.transform);
            _zombie.name = $"zombie{i + 1}";
            _zombie.SetActive(false);
            zombiePool.Add(_zombie);
        }
    }
    IEnumerator CreateSkeleton()
    {
        yield return new WaitForSeconds(0.5f);

        var skeletonGroup = new GameObject("SkeletonGroup");

        for (int i = 0; i < maxZombieCount; i++)
        {
            var _skeleton = Instantiate(skelectPref, skeletonGroup.transform);
            _skeleton.name = $"skeleton{i + 1}";
            _skeleton.SetActive(false);
            skelectPool.Add(_skeleton);
        }
    }

    void ZombieSpawn()
    {
        foreach (var _zombie in zombiePool)
        {
            if (_zombie.activeSelf == false)
            {
                _zombie.transform.position = spawnList[Random.Range(0, spawnList.Count)].transform.position;
                _zombie.transform.rotation = spawnList[Random.Range(0, spawnList.Count)].transform.rotation;
                _zombie.gameObject.SetActive(true);
                break;
            }
        }
    }

    void SkeletonSpawn()
    {
        foreach (var _skeleton in skelectPool)
        {
            if (_skeleton.activeSelf == false)
            {
                _skeleton.transform.position = spawnList[Random.Range(0, spawnList.Count)].transform.position;
                _skeleton.transform.rotation = spawnList[Random.Range(0, spawnList.Count)].transform.rotation;
                _skeleton.gameObject.SetActive(true);
                break;
            }
        }
    }
}
