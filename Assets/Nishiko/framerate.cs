using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class framerate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60; //FPS��60�ɐݒ� 
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
