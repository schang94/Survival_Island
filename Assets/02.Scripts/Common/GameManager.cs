using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.UI;

// �̱��� ����� ����Ͽ� ���� �Ŵ����� �����մϴ�.
// �� �¾�� 1. �¾ ��ġ, 2. �¾ �ð�, 3. �¾ ���� ������ ����
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱��� ���
    private readonly string zombieTag = "ZOMBIE";
    private readonly string skelTag = "SKELETON";
    // 1. �������� ��ü ��������
    // 2. �������� ���� ���� ����
    public GameObject zombiePrefab;
    public GameObject skeletonPrefab;
    private float timePrev; //����
    private float timePrev2; //���̷���
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
        if (instance == null) // �̱��� �ν��Ͻ��� ������
        { 
            instance = this; // ���� �ν��Ͻ��� ����
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� ������ ���� ������Ʈ �ı�
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
        // ���̶�Ű���� SpawnPoints ������Ʈ�� ã�� ã�� �ڽĵ��� Ʈ�������� ������
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
        // ������ �����Լ� (what, where, rotation)
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
        Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ���
        Cursor.visible = false; // Ŀ�� �Ⱥ��̰� ����
    }

    public void MouseCursorVisible()
    {
        Cursor.lockState = CursorLockMode.None; // Ŀ�� ��� ����
        Cursor.visible = true; // Ŀ�� ���̰� ����
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
