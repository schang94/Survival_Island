using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange : MonoBehaviour
{
    public Light whiteLight;
    public Light yellowLight;
    public Light blueLight;
    public AudioSource[] sources;
    public AudioClip clip;
    void Start()
    {
        whiteLight = GetComponentsInChildren<Light>()[0];
        yellowLight = GetComponentsInChildren<Light>()[1];
        blueLight = GetComponentsInChildren<Light>()[2];
        sources = GetComponentsInChildren<AudioSource>();
        TurnOnLight();
    }

    void TurnOnLight()
    {
        StartCoroutine(LightOnOff());
    }

    IEnumerator LightOnOff()
    {
        whiteLight.enabled = true;
        yellowLight.enabled = false;
        blueLight.enabled = false;
        sources[0].PlayOneShot(clip);
        yield return new WaitForSeconds(3f);
        
        whiteLight.enabled = false;
        yellowLight.enabled = true;
        blueLight.enabled = false;
        sources[1].PlayOneShot(clip);

        yield return new WaitForSeconds(3f);

        whiteLight.enabled = false;
        yellowLight.enabled = false;
        blueLight.enabled = true;
        sources[2].PlayOneShot(clip);

        yield return new WaitForSeconds(3f);

        TurnOnLight();
    }
}
