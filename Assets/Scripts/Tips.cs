using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    WaveController waveController; 
    void Start()
    {
        waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
    }

    void Update()
    {
        if(waveController.wave == 1)
        {
            Destroy(this.gameObject);
        }
    }
}
