using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torch_flame : MonoBehaviour
{
    public float t=1.0f;
    private Light light;
    private int time_now;
    private float base_intensity;
    
    void Start ()
    {
        light = gameObject.GetComponent<Light>();
        time_now=(int)(Random.value*100);
        base_intensity=light.intensity;
        
    }
    void Update(){
        time_now++;
        time_now%=1000;

        light.intensity = base_intensity+ Mathf.Sin (time_now * 0.2f *t)*base_intensity*0.1f+ Mathf.Sin (time_now * 0.12f *t)*base_intensity*0.05f;
    }
}