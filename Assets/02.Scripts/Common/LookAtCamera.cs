using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform Camtr;
    private Transform Canvastr;
    void Start()
    {
        Camtr = Camera.main.transform;
        Canvastr = GetComponent<Transform>();
    }

    void Update()
    {
        Canvastr.LookAt(Camtr);
    }
}
