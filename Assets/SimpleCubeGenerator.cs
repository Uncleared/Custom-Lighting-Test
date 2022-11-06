using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCubeGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int z = 0; z < 5; z++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                   
                    if (z == 0 || z == 4 || x == 0 || x == 4 || y == 0 || y == 4)
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = transform.position + new Vector3(x, y, z);
                        cube.transform.parent = transform;
                        cube.AddComponent<SimpleLightable>();
                    }
                 
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
