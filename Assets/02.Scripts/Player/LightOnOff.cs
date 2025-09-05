using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    private readonly string player = "Player";
    private Light _light;
    private AudioSource source;
    public AudioClip _lightOnSound;
    public AudioClip _lightOffSound;

    void Start()
    {
        _light = GetComponent<Light>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(player))
        {
            _light.enabled = true;
            source.PlayOneShot(_lightOnSound);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag(player))
        {
            _light.enabled = false;
            source.PlayOneShot(_lightOffSound);
        }
    }
    private IEnumerator LightOff()
    {
        yield return new WaitForSeconds(3f);
        _light.enabled = false;
        source.PlayOneShot(_lightOffSound);

    }


}
