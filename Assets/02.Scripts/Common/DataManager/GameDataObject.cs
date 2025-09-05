using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;


[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameDataSO", order = 0)]
public class GameDataObject : ScriptableObject
{
    public int killCount = 0;
    public float hp = 120f;
    public float damage = 25f;
    public float speed = 6f;
    public List<Item> equipItem = new List<Item>();
}
