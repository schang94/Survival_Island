using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    private readonly string bulletTag = "Bullet";
    private readonly string skeletonTag = "SKELETON";
    private readonly string zombieTag = "ZOMBIE";
    [SerializeField] private GameObject expEffect; // ÀÌÆåÆ®
    [SerializeField] private AudioClip expClip;
    [SerializeField] private Mesh[] meshes;
    private Rigidbody rb; // ÆøÆÄ ±¸Çö
    private AudioSource source;
    private Texture[] textures;
    private MeshRenderer _renderer;
    private MeshFilter meshFilter;
    int hikCount = 0;
    private float radiuse = 10f;
    private bool isExp = false;
    public Shake shake;
    void Start()
    {
        //yield return new WaitForSeconds(1);
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("Textures");
        meshFilter = GetComponent<MeshFilter>();
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        
        StartCoroutine(GetShake());
    }

    IEnumerator GetShake()
    {
        while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName("MainScene").isLoaded)
        {
            yield return null;
        }
        shake = Camera.main.GetComponent<Shake>();
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bulletTag))
        {
            if (++hikCount == 3)
            {
                ExpolosionBarrel();
            }
        }
    }
    void ExpolosionBarrel()
    {
        var exp = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(exp, 1.5f);
        source.PlayOneShot(expClip, 1f);
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
        isExp = true;

        Collider[] cols = Physics.OverlapSphere(transform.position, radiuse, 1 << 9 | 1 << 7 | 1 << 8);

        foreach (Collider col in cols)
        {
            var _rb = col.GetComponent<Rigidbody>();
            _rb.mass = 1;
            _rb.AddExplosionForce(200f, transform.position, radiuse, 100f);
            ExpEnemyDie(col);
            
            StartCoroutine(shake.ShakeCamera(duration : 0.5f));
        }
    }

    public void ExpEnemyDie(Collider col)
    {
        //GameObject[] skelectons = GameObject.FindGameObjectsWithTag(skeletonTag);
        //GameObject[] zombies = GameObject.FindGameObjectsWithTag(zombieTag);
        if (col.CompareTag(skeletonTag))
            col.transform.SendMessage("SkeletonDie", SendMessageOptions.DontRequireReceiver);
        else if(col.CompareTag(zombieTag))
            col.transform.SendMessage("ZombieDie", SendMessageOptions.DontRequireReceiver);

        //for (int i = 0; i < skelectons.Length; i++)
        //{
        //    skelectons[i].SendMessage("SkeletonDie", SendMessageOptions.DontRequireReceiver);
        //}

        //for (int i = 0; i < zombies.Length; i++)
        //{
        //    zombies[i].SendMessage("ZombieDie", SendMessageOptions.DontRequireReceiver);
        //}
    }

}
