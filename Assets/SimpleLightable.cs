using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleLightable : MonoBehaviour
{
    public MeshRenderer mr;

    public List<LightInfo> currentLights;
    //public Dictionary<Transform, float> lights;
    //public Dictionary<Transform, float> seenAmount;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();

        currentLights = new List<LightInfo>();
    }

    public void AddLight(LightInfo newLightInfo, Transform lightOwner)
    {
        if (!AlreadyLit(lightOwner))
        {
            currentLights.Add(newLightInfo);
        }
        else
        {
            foreach (LightInfo currentLightInfo in currentLights)
            {
                if (currentLightInfo.OwnedByTransform(lightOwner))
                {
                    currentLightInfo.Update(newLightInfo);
                }
            }
        }
    }

    public float colorLerpSpeed = 3f;
    public float fadeSpeed = 1f;
    public float debugVal = 0f;

    public Color color;

    public bool AlreadyLit(Transform lightTransform)
    {
        foreach(LightInfo lightInfo in currentLights)
        {
            if (lightInfo.OwnedByTransform(lightTransform))
            {
                return true;
            }
        }

        return false;
    }

    Color CalculateColor()
    {
        Color resultingColor = Color.black;

        float totalIntensity = 0;
        float mostHighIntensity = 0f;

        // Gather light and calculate resulting color and brightness
      
        // Go through each light
        foreach (LightInfo lightInfo in currentLights)
        {
            totalIntensity += lightInfo.CalculateRelativeIntensity(transform.position);

            float intensity = lightInfo.CalculateRelativeIntensity(transform.position);
            if (intensity > mostHighIntensity)
            {
                mostHighIntensity = intensity;
            }
        }

        foreach (LightInfo lightInfo in currentLights)
        {
            float intensity = lightInfo.CalculateRelativeIntensity(transform.position);
            //LightInfo light = currentLights.Dequeue();
            //resultingColor += light.color * (light.intensity / totalIntensity) * mostHighIntensity;
            resultingColor += lightInfo.GetColor() * (intensity);

            debugVal = totalIntensity;
        }

        return resultingColor;
    }
    // Update is called once per frame
    void Update()
    { 
        this.color = CalculateColor();

        mr.material.color = Color.Lerp(mr.material.color, this.color, Time.deltaTime * colorLerpSpeed);
    }
}
