using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    public float height = 0f;
    public float f = 5f;

    private float offset = 0f;
    // Start is called before the first frame update
    void Start()
    {
        offset = Random.value;
        origin = transform.position;
    }

    private Vector3 origin;
    // Update is called once per frame
    void Update()
    {
        transform.position = origin + new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup * f + offset) * height + height, 0f);
    }
}
