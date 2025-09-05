using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ѿ� �浹 ���� �� ����Ʈ, �Ѿ� �����, ���� ȿ��
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
