using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총알 충돌 감지 후 이펙트, 총알 사라짐, 사운드 효과
public class RemoveBullet : MonoBehaviour
{
    private readonly string bulletTag = "Bullet";
    public GameObject spark;
    private AudioSource source;
    public AudioClip hitClip;
    void Start()
    {
        source = GetComponent<AudioSource>();
        spark = Resources.Load("Weapon/FlareMobile") as GameObject;
        hitClip = Resources.Load("Sounds/bullet_hit_metal_enemy_4") as AudioClip;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == bulletTag)
        {
            col.gameObject.SetActive(false);
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            source.PlayOneShot(hitClip, 1f);
            var spk = Instantiate(spark, contact.point, rot);
            Destroy(spk, 1f);
            

        }
    }
}
