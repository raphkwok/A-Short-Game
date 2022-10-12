using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{

    public Material skybox;

    [Header("Time Settings")]
    public float dayTime = 60;
    [Header("Color settings")]
    public Transform sun;
    public Color32 dayColorT;
    public Color32 dayColorB;
    public Color32 nightColorT;
    public Color32 nightColorB;

    public Color32 fogColorDay;
    public Color32 fogColorNight;
    // Start is called before the first frame update
    void Start()
    {
        print(dayColorT);
        StartCoroutine(ChangeSkyboxColor());
        StartCoroutine(RotateSun());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ChangeSkyboxColor()
    {
        Color32 T = dayColorT;
        Color32 B = dayColorB;
        while (true)
        {
            T = Color32.Lerp(T, nightColorT, 0.1f / (dayTime / 4));
            B = Color32.Lerp(B, nightColorB, 0.1f / (dayTime / 4));
            skybox.SetColor("_SkyGradientTop", T);
            skybox.SetColor("_SkyGradientBottom", B);

            RenderSettings.fogColor = Color32.Lerp(fogColorDay, fogColorNight, 0.1f / (dayTime / 4));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator RotateSun()
    {
        while (true)
        {

            sun.transform.Rotate(Vector3.right, (360 / dayTime) * Time.deltaTime);
            yield return null;
        }
    }
}
