﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoomGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateWall(10, 5, Vector3.left, Vector3.left * 5f);
    }



    void GenerateWall(int width, int height, Vector3 forwardVector, Vector3 origin)
    {
        GameObject parent = new GameObject();
        parent.transform.name = "Wall";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                Vector3 rightVector = Vector3.Cross(forwardVector, Vector3.up);
                Vector3 calculatedPos = origin + rightVector * x + Vector3.up * y;
                cube.transform.position = calculatedPos;
                cube.AddComponent<SimpleLightable>();
                cube.transform.parent = parent.transform;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
