using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightInfo
{
    private Color color;
    private float range;
    private  Vector3 pos;
    private Transform transform;
    private float intensity;


    public LightInfo(Color color, float range, float intensity, Vector3 pos)
    {
        this.color = color;
        this.range = range;
        this.pos = pos;
        this.intensity = intensity;
    }

    public LightInfo(Color color, float range, float intensity, Transform transform)
    {
        this.color = color;
        this.range = range;
        this.transform = transform;
        this.intensity = intensity;
    }

    public float CalculateRelativeIntensity(Vector3 pos)
    {
        Vector3 lightPos;
        if (transform != null)
        {
            lightPos = transform.position;
        }
        else
        {
            lightPos = pos;
        }
        float calculatedIntensity = this.intensity * (this.range / Vector3.Distance(pos, lightPos));
        return calculatedIntensity;
    }

    public bool OwnedByTransform(Transform transform)
    {
        if (this.transform == null)
        {
            return false;
        }

        return this.transform.Equals(transform);
    }

    public void Update(LightInfo lightInfo)
    {
        this.range = lightInfo.range;
        this.intensity = lightInfo.intensity;
        this.color = lightInfo.color;
    }
    public Color GetColor()
    {
        return color;
    }
    //public override bool Equals(object obj)
    //{
    //    if (!(obj is LightInfo))
    //    {
    //        return false;
    //    }

    //    LightInfo other = (LightInfo) obj;
    //    if (other.pos != this.pos)
    //    {
    //        return false;
    //    }
    //    return true;
    //}
}

public class SimplePointlight : MonoBehaviour
{
    public int samples = 100;
    public Color color;
    public float range = 5f;
    private int layerMask = 1;

    public float intensity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    public LightInfo GetLightInfo()
    {
        return new LightInfo(color, range, intensity, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < samples; i++)
        {
            Vector3 dir = Random.insideUnitSphere;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.TryGetComponent(out SimpleLightable lightable))
                {
                    lightable.AddLight(new LightInfo(color, range, intensity, transform), hit.transform);
                }
            }
        }
    }
}
