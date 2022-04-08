using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessModeCamera : MonoBehaviour
{
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.up * Speed) * Time.deltaTime;
    }
}
