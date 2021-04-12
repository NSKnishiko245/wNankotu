using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private AudioSource selectBgmSource;
    [SerializeField] private AudioSource selectDecSource;
    private GameObject eventSystem;

    [SerializeField] private float sceneChangeTime;
    private int sceneChangeCnt = 0;
    private bool sceneChangeFlg = false;

    private void Awake()
    {
        eventSystem = GameObject.Find("EventSystem");
    }

    private void Update()
    {
        // ���݂̃y�[�W(�X�e�[�W�ԍ�)
        int stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetMoveNum();

        // �V�[���J��
        if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown("joystick button 0")))
        {
            selectDecSource.Play();
            sceneChangeFlg = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            sceneChangeFlg = false;
        }

        if (sceneChangeFlg)
        {
            sceneChangeCnt++;

            if (sceneChangeCnt > sceneChangeTime * 60)
            {
                SceneManager.LoadScene("Stage" + stageNum + "Scene");
                //Debug.Log("Stage" + stageNum + "Scene");
            }
        }
    }
}
