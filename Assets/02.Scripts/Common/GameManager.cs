using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.UI;

// 싱글톤 기법을 사용하여 게임 매니저를 구현합니다.
// 적 태어나기 1. 태어날 위치, 2. 태어날 시간, 3. 태어날 적의 종류를 설정
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 기법
    private readonly string zombieTag = "ZOMBIE";
    private readonly string skelTag = "SKELETON";
    // 1. 무차별한 객체 생성방지
    // 2. 전역에서 쉽게 접근 가능
    public GameObject zombiePrefab;
    public GameObject skeletonPrefab;
    private float timePrev; //좀비
    private float timePrev2; //스켈레톤
    public List<Transform> spwanList;

    private int maxZombieCount = 10;
    private int maxSkeleton = 5;

    public Text killTxt;

    PlayerDamage playerDamage;
    public GameObject inventory;
    DataManager dataManager;
    public GameDataObject gameData;
    //public GameData gameData;

    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    public GameObject slotList;
    public GameObject[] ItemObjects;

    private void Awake()
    {
        if (instance == null) // 싱글톤 인스턴스가 없으면
        { 
            instance = this; // 현재 인스턴스를 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 현재 오브젝트 파괴
        }

        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();

    }
    void Start()
    {
        MouseCursorVisible();
        if (instance != null)
            killTxt = GameObject.Find("Panel_Kill").transform.GetChild(0).GetComponent<Text>();
        else
            killTxt = null;

        timePrev = Time.time;
        timePrev2 = Time.time;
        Transform[] spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        // 하이라키에서 SpawnPoints 오브젝트를 찾고 찾은 자식들의 트랜스폼을 가져옴
        if (spawnPoints != null)
            spwanList = new List<Transform>(spawnPoints);

        spwanList.RemoveAt(0);
        playerDamage = GameObject.FindWithTag("Player").GetComponent<PlayerDamage>();
        DataLoad();
    }

    void DataLoad()
    {
        GameData data = new();
        data = dataManager.Load();
        gameData.hp = data.hp;
        gameData.speed = data.speed;
        gameData.damage = data.damage;
        gameData.killCount = data.killCount;
        gameData.equipItem = data.equipItem;

        if (gameData.equipItem.Count > 0)
            InventorySetUp();

        killTxt.text = $"KILL : <color=#ff0000>{gameData.killCount:000}</color>";

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }

    void DataSave()
    {
        dataManager.Save(gameData);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }

    void InventorySetUp()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();

        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            for (int j = 1; j < slots.Length; j++)
            {
                int idx = (int)gameData.equipItem[i].itemtype;
                if (slots[j].childCount > 0) continue;

                ItemObjects[idx].GetComponent<Transform>().SetParent(slots[j]);

                break;
            }
        }
    }

    public void AddItem(Item item)
    {
        if (gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Add(item);

        switch(item.itemtype)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;
                    break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed * item.value;
                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;
                break;
            case Item.ItemType.GRENADE:
                break;
        }

        OnItemChange();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }

    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);

        switch (item.itemtype)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp / (1f + item.value);
                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed / (1f + item.value);
                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage / (1f + item.value);
                break;
            case Item.ItemType.GRENADE:
                break;
        }

        OnItemChange();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }

    void Update()
    {
        if (playerDamage.isDie) return;
        //if (Time.time - timePrev >= 3f)
        //{
        //    timePrev = Time.time;
        //    int zombieCount = GameObject.FindGameObjectsWithTag(zombieTag).Length;
        //    if (zombieCount < maxZombieCount)
        //        CreateZobie();
        //}
        //if (Time.time - timePrev2 >= 5f)
        //{
        //    timePrev2 = Time.time;
        //    int skeletonCount = GameObject.FindGameObjectsWithTag(skelTag).Length;
        //    if (skeletonCount < maxSkeleton)
        //        CreateSkeleton();
        //}
    }
    void CreateZobie()
    {
        int idx = Random.Range(0, spwanList.Count);
        Instantiate(zombiePrefab, spwanList[idx].position, spwanList[idx].rotation);
        // 프리팹 생성함수 (what, where, rotation)
    }
    void CreateSkeleton()
    {
        int idx = Random.Range(0, spwanList.Count);
        Instantiate(skeletonPrefab, spwanList[idx].position, spwanList[idx].rotation);
    }

    public void UpdateKillCount(int killCount)
    {
        gameData.killCount += killCount;
        killTxt.text = $"KILL : <color=#ff0000>{gameData.killCount:000}</color>";
    }

    public void MouseCursorDisable()
    {
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
        Cursor.visible = false; // 커서 안보이게 설정
    }

    public void MouseCursorVisible()
    {
        Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
        Cursor.visible = true; // 커서 보이게 설정
    }

    private bool isPaused;
    public void OnPauseClick()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();

        foreach (var script in scripts)
            script.enabled = !isPaused;
    }

    public void OnInventoryClick(bool isInven)
    {
        if (isInven != isPaused) OnPauseClick();
        var canvasGroup = inventory.transform.GetComponent<CanvasGroup>();
        canvasGroup.alpha = isInven ? 1.0f : 0.0f;
        canvasGroup.blocksRaycasts = isInven;
        canvasGroup.interactable = isInven;

    }

    private void OnApplicationQuit()
    {
        DataSave();
    }
}
