using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    // �T�E���h
    [SerializeField] private AudioSource titleBgmSource;
    [SerializeField] private AudioSource titleStartSource;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private float sceneChangeTime; // �V�[���J�ڂ܂ł̎���
    private int sceneChangeCnt = 0;                // �V�[���J�ڂ̃J�E���^
    private bool sceneChangeFlg = false;            // true:�V�[���J�ڊJ�n
    private bool sceneChangeFirstFlg = true;        // �V�[���J�ڊJ�n��ɂP�x�����ʂ鏈��

    private void Awake()
    {
        ScoreReset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            sceneChangeFlg = true;

            //mainCamera.transform.DOLocalMove(new Vector3(0, -6.35f, -10.33f), 5.0f);
            //mainCamera.transform.DORotate(new Vector3(-27.255f, 0f, 0f), 5.0f);
        }

        // �V�[���J�ڊJ�n
        if (sceneChangeFlg)
        {
            // �V�[���J�ڊJ�n��ɂP�x�����ʂ鏈��
            if (sceneChangeFirstFlg)
            {
                titleBgmSource.Stop();
                titleStartSource.Play();
                sceneChangeFirstFlg = false;
            }

            // ��莞�Ԍo�߂���ƑJ�ڂ���
            if (sceneChangeCnt > sceneChangeTime * 60)
            {
                SceneManager.LoadScene("SelectScene");
            }
            sceneChangeCnt++;
        }
    }

    private void ScoreReset()
    {
        for (int i = 0; i < 11; i++)
        {
            StageSelectManager.score[i].isGold = false;
            StageSelectManager.score[i].isSilver = false;
            StageSelectManager.score[i].isCopper = false;
        }
    }
}