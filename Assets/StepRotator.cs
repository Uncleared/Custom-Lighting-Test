using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StepRotator : MonoBehaviour
{
    public Vector3 rotateVector;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        timer += Random.value;
    }

    public float interval = 0.2f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0f;
            transform.Rotate(rotateVector);
        }
    }
}
