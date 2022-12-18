using FunkyCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    private Light2D torchLight;
    private Color lightColor;

    private void Start()
    {
        torchLight = GetComponent<Light2D>();
        lightColor = torchLight.color;
        StartCoroutine(LightAnimation());
    }

   IEnumerator LightAnimation()
    {
        yield return new WaitForSeconds(Random.Range(0.10f, 0.20f));
        torchLight.color = new Color(lightColor.r, lightColor.g, lightColor.b, Random.Range(0.50f, 0.60f));
        StartCoroutine(LightAnimation());
    }
}
