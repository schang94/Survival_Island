using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera;
    public bool shakeRotate = false;
    private Vector3 originPos;
    private Quaternion oringinRot = Quaternion.identity;

    void Start()
    {
        shakeCamera = transform;
        originPos = shakeCamera.transform.position;
        oringinRot = shakeCamera.transform.rotation;
    }

    public IEnumerator ShakeCamera(float duration = 0.05f, float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        float shakeTime = 0f;
        while (duration > shakeTime)
        {
            Vector3 shakePos = Random.insideUnitSphere;
            shakeCamera.transform.position = shakePos * magnitudePos;

            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time * magnitudeRot, 0f));
                shakeCamera.transform.rotation = Quaternion.Euler(shakeRot);
            }
            shakeTime += Time.deltaTime;
            yield return null;
        }
        shakeCamera.transform.position = originPos;
        shakeCamera.transform.rotation = oringinRot;
    }
}
