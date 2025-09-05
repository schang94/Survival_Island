using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 총알에 맞았을 때 맞은 위치에 이펙트 생성
// 2. 총알에 맞았을 때 총알은 사라지고 맞은 위치에 사운드 생성
public class Container : MonoBehaviour
{
    private readonly string bulletTag = "Bullet";
    public GameObject hitEffectPrefab;
    public AudioSource source;
    public AudioClip clip;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            var hitEff = Instantiate(hitEffectPrefab, col.contacts[0].point, Quaternion.identity);
            //무엇을      어디에(col.transform.position)      회전X
            Destroy(hitEff, 1.5f);
            source.PlayOneShot(clip, 1.0f);
        }
    }
}
