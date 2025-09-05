using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스스로 z축으로 발사
public class BulletCtrl : MonoBehaviour
{
    private float speed = 3000.0f;
    private Rigidbody rb;
    private TrailRenderer trail;

    public float damage = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        //Destroy(this.gameObject, 3f);
    }

    private void Start()
    {
        damage = GameManager.instance.gameData.damage;
    }

    void UpdateSetUp()
    {
        damage = GameManager.instance.gameData.damage;
    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
        GameManager.OnItemChange += UpdateSetUp;
        Invoke("BulletDisable", 3f);
    }

    private void OnDisable()
    {
        trail.Clear();
        rb.Sleep();
        CancelInvoke("BulletDisable");
    }

    void BulletDisable()
    {
        gameObject.SetActive(false);
        
    }
}
