using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    int frameCount;     // �t���[����
    float prevTime;     // �O��̌o�ߎ���
    Text fpsText;       // FPS�\���p�e�L�X�g

    void Start()
    {
        // FPS�Œ�
        Application.targetFrameRate = 60;

        fpsText = GameObject.Find("FPSText").GetComponent<Text>();

        frameCount = 0;
        prevTime = 0.0f;
    }

    void Update()
    {
        ++frameCount;

        // �o�ߎ��� - �O��̌o�ߎ���
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            // FPS���e�L�X�g�ɃZ�b�g
            float fps = frameCount / time;
            fpsText.text = "fps:" + fps.ToString("f2");

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }
}
