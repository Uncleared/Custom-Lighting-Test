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

    float[] CalculateHighestAndTotalIntensity()
    {
        float totalIntensity = 0;
        float mostHighIntensity = 0;
        float highestRange = 0;
        // Go through each light
        foreach (LightInfo lightInfo in currentLights)
        {
            float intensity = lightInfo.CalculateRelativeIntensity(transform.position);

            totalIntensity += intensity;

            if (intensity > mostHighIntensity)
            {
                mostHighIntensity = intensity;
            }

            if (lightInfo.GetRange() > highestRange)
            {
                highestRange = lightInfo.GetRange();
            }
        }

        return new float[] {totalIntensity, mostHighIntensity, highestRange};
    }

    Color CalculateColor()
    {
        Color resultingColor = Color.black;

        float[] val = CalculateHighestAndTotalIntensity();
        float totalIntensity = val[0];
        float mostHighIntensity = val[1];
        

        // Gather light and calculate resulting color and brightness
        
     

        // No division by error
        if (totalIntensity > 0f)
        {
            foreach (LightInfo lightInfo in currentLights)
            {
                // Update their life time
                lightInfo.UpdateLife();

                float intensity = lightInfo.CalculateRelativeIntensity(transform.position);
                //LightInfo light = currentLights.Dequeue();
                resultingColor += lightInfo.GetColor() * (intensity / totalIntensity) * mostHighIntensity;
                //resultingColor += lightInfo.GetColor() * (intensity);

                debugVal = totalIntensity;
            }

            resultingColor.a = 1f;
        }
      
        return resultingColor;
    }

    void BounceLighting()
    {
        float[] val = CalculateHighestAndTotalIntensity();
        float totalIntensity = val[0];
        float mostHighIntensity = val[1];
        float range = val[2];

        bounceTimer = 0.2f;
        for (int i = 0; i < 5; i++)
        {
            Vector3 dir = Random.insideUnitSphere;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, 1))
            {
                if (hit.transform != transform)
                {
                    if (hit.transform.TryGetComponent(out SimpleLightable lightable))
                    {
                        lightable.AddLight(new LightInfo(this.color, range * 0.1f, mostHighIntensity * 0.1f, transform), transform);
                    }
                }
               
            }
        }
    }
    private float bounceTimer = 0.5f;
    // Update is called once per frame
    void Update()
    {
        bounceTimer -= Time.deltaTime;
        if (bounceTimer < 0f)
        {

            BounceLighting();
        }
        this.color = CalculateColor();

        mr.material.color = Color.Lerp(mr.material.color, this.color, Time.deltaTime * colorLerpSpeed);
    }
}
