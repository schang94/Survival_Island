using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. �Ѿ˿� �¾��� �� ���� ��ġ�� ����Ʈ ����
// 2. �Ѿ˿� �¾��� �� �Ѿ��� ������� ���� ��ġ�� ���� ����
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
            //������      ���(col.transform.position)      ȸ��X
            Destroy(hitEff, 1.5f);
            source.PlayOneShot(clip, 1.0f);
        }
    }
}
