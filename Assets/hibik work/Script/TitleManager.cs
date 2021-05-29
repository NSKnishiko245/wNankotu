using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject mainCamera;
    private Animator canvasAnim;
    private Animator cameraAnim;
    private Animator pressAAnim;

    // �T�E���h
    [SerializeField] private AudioSource titleBgmSource;
    [SerializeField] private AudioSource titleStartSource;
    [SerializeField] private AudioSource commandSource;

    [SerializeField] private float sceneChangeTime; // �V�[���J�ڂ܂ł̎���
    private int sceneChangeCnt = 0;                // �V�[���J�ڂ̃J�E���^
    private bool sceneChangeFlg = false;            // true:�V�[���J�ڊJ�n
    private bool sceneChangeFirstFlg = true;        // �V�[���J�ڊJ�n��ɂP�x�����ʂ鏈��

    [SerializeField] private int canvasAnimStartCnt;
    [SerializeField] private int cameraAnimStartCnt;
    GameObject postprocess;
    private bool commandFlg = false;

    private void Awake()
    {
        canvasAnim = canvas.GetComponent<Animator>();
        cameraAnim = mainCamera.GetComponent<Animator>();
        pressAAnim = GameObject.Find("PressAImage").GetComponent<Animator>();
        postprocess = GameObject.Find("PostProcess");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            sceneChangeFlg = true;
            pressAAnim.SetFloat("speed", 5.0f);
        }

        // �G�N�X�g���X�e�[�W����R�}���h
        if (!commandFlg)
        {
            if (Input.GetKeyDown(KeyCode.C) || (Input.GetAxis("LTrigger") > 0 && Input.GetAxis("RTrigger") > 0))
            {
                Debug.Log("�G�N�X�g���X�e�[�W����R�}���h");
                commandSource.Play();

                for (int i = 0; i < 6; i++)
                {
                    StageSelectManager.enterExtraFlg[i] = true;
                }

                commandFlg = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.M) || (Input.GetAxis("LTrigger") > 0 && Input.GetAxis("RTrigger") > 0) && Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("���_���S����R�}���h");
            commandSource.Play();
            BookSelect.bonusFlg = true;

            for (int i = 0; i < 6; i++)
            {
                StageClearManager.StageClear[i] = true;
            }

            for (int i = 1; i < StageSelectManager.stageMax + 1; i++)
            {
                StageSelectManager.score[i].isCopper = true;
                StageSelectManager.score[i].isSilver = true;
                StageSelectManager.score[i].isGold = true;

                StageClearManager.m_isGetCopper[i] = true;
            }
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

            if (canvasAnimStartCnt == 0)
            {
                canvasAnim.SetBool("isAnim", true);
            }
            else canvasAnimStartCnt--;

            if (cameraAnimStartCnt == 0)
            {
                cameraAnim.SetBool("isAnim", true);
                postprocess.GetComponent<PostEffectController>().SetVigFlg(false);
            }
            else cameraAnimStartCnt--;


            // ��莞�Ԍo�߂���ƑJ�ڂ���
            if (sceneChangeCnt > sceneChangeTime)
            {
                SceneManager.LoadScene("BookSelectScene");
            }
            sceneChangeCnt++;
        }
    }
}
