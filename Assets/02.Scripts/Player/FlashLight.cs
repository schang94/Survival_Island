using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    private Light flashLight;
    private AudioSource flashSource;
    public AudioClip flashClip;
    void Start()
    {
        flashLight = GetComponent<Light>();
        flashSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.enabled = !flashLight.enabled;
            flashSource.PlayOneShot(flashClip);
        }
    }
}
