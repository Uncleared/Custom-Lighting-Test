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

    // If life runs out, light will just die
    private float life = 1f;

    private float defaultLife = 1f;
    private bool bounced = false;

    public LightInfo(Color color, float range, float intensity, Vector3 pos, bool bounced = false)
    {
        this.color = color;
        this.range = range;
        this.pos = pos;
        this.intensity = intensity;
        this.bounced = bounced;
    }

    public LightInfo(Color color, float range, float intensity, Transform transform, bool bounced = false)
    {
        this.color = color;
        this.range = range;
        this.transform = transform;
        this.intensity = intensity;
        this.bounced = bounced;
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

        float d = Vector3.Distance(pos, lightPos);
        //d = Mathf.Min(d, 0.05f);
        float calculatedIntensity = this.intensity * (this.range / (d * d));
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

        this.SetLife(defaultLife);
    }

    public void SetLife(float newLife)
    {
        this.life = newLife;
    }

    public void UpdateLife()
    {
        life -= Time.deltaTime;
        if (life < 0f)
        {
            intensity = 0f;
        }

        intensity = Mathf.Lerp(intensity, 0f, Time.deltaTime);
    }
    public Color GetColor()
    {
        return color;
    }

    public float GetRange()
    {
        return this.range;
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

    private MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        mr.material.EnableKeyword("_EmissionColor");
        mr.material.SetColor("_EmissionColor", color);
    }

    public LightInfo GetLightInfo()
    {
        return new LightInfo(color, range, intensity, transform.position);
    }

    void OnValidate()
    {
        if (mr == null)
        {
            mr = GetComponent<MeshRenderer>();
        }
        mr.material.SetColor("_EmissionColor", color);
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
                    lightable.AddLight(new LightInfo(color, range, intensity, transform), transform);
                }
            }
        }
    }
}
