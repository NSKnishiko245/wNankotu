using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    // �T�E���h
    [SerializeField] private AudioSource selectBgmSource;
    [SerializeField] private AudioSource selectDecSource;

    [SerializeField] private GameObject eventSystem;

    [SerializeField] private float sceneChangeTime; // �V�[���J�ڂ܂ł̎���
    private int sceneChangeCnt = 0;                 // �V�[���J�ڂ̃J�E���^
    private bool sceneChangeFlg = false;

    [SerializeField] private int pageIntervalInit;  // �y�[�W���߂����܂ł̑ҋ@���Ԃ̏����l
    private int pageInterval = 0;

    [SerializeField] private int operationCntInit;  // �V�[���J�ڂ��đ���\�ɂȂ�܂ł̎���
    private int operationCnt = 0;

    private GameObject[] goldImage = new GameObject[42];
    private GameObject[] silverImage = new GameObject[42];
    private GameObject[] copperImage = new GameObject[42];
    public struct Score
    {
        public bool isGold;
        public bool isSilver;
        public bool isCopper;
    }
    public static Score[] score = new Score[43];
    public static int[] silverConditions = new int[43];

    private void Awake()
    {
        pageInterval = pageIntervalInit;
        operationCnt = operationCntInit;

        for (int i = 1; i < 42; i++)
        {
            goldImage[i] = GameObject.Find("GoldImage" + i);
            silverImage[i] = GameObject.Find("SilverImage" + i);
            copperImage[i] = GameObject.Find("CopperImage" + i);

            if (score[i].isGold) goldImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else goldImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

            if (score[i].isSilver) silverImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else silverImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

            if (score[i].isCopper) copperImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else copperImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        }

        //ScoreReset();            
        SilverConditionsSet();

        this.GetComponent<PostEffectController>().SetVigFlg(false);
    }

    private void Update()
    {
        if (operationCnt == 1) eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();

        if (operationCnt == 0)
        {
            if (!eventSystem.GetComponent<IgnoreMouseInputModule>().GetAllBackFlg())
            {
                PageOperation();
            }
        }
        else operationCnt--;

        SceneChange();      // �V�[���J��

        if (!eventSystem.GetComponent<IgnoreMouseInputModule>().GetAllBackFlg())
        {
            //ScoreDisplay();
        }
    }

    // �y�[�W���߂���
    private void PageOperation()
    {
        if (pageInterval == 0)
        {
            // ���̃y�[�W�֐i��
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
            {
                eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();
                pageInterval = pageIntervalInit;
            }
            // �O�̃y�[�W�ɖ߂�
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
            {
                eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();
                pageInterval = pageIntervalInit;
            }
        }
        else pageInterval--;
    }

    // �V�[���J��
    private void SceneChange()
    {
        if (!sceneChangeFlg && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0")))
        {
            selectDecSource.Play();

            // ���݂̃y�[�W�擾(�X�e�[�W�ԍ�)
            StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();

            // �V�[���J�ڊJ�n
            if (StageManager.stageNum > 0)
            {
                // �{�����
                eventSystem.GetComponent<IgnoreMouseInputModule>().AllBackPage();
                // �������Â�����
                this.GetComponent<PostEffectController>().SetVigFlg(true);

                sceneChangeFlg = true;
            }
        }

        if (eventSystem.GetComponent<IgnoreMouseInputModule>().GetBookCloseFlg())
        {
            // �J��
            SceneManager.LoadScene("Stage1Scene");
        }
    }

    //private void ScoreDisplay()
    //{
    //    // ���݂̃y�[�W�擾(�X�e�[�W�ԍ�)
    //    StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();

    //    for (int i = 0; i < 42; i++)
    //    {
    //        if (score[StageManager.stageNum].isGold) goldImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //        else goldImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
    //    }

    //    for (int i = 0; i < 42; i++)
    //    {
    //        if (score[StageManager.stageNum].isSilver) silverImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //        else silverImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
    //    }

    //    for (int i = 0; i < 42; i++)
    //    {
    //        if (score[StageManager.stageNum].isCopper) copperImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //        else copperImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
    //    }
    //}

    // �⃁�_���̊l���������Z�b�g(�X�e�[�W��܂�����)
    private void SilverConditionsSet()
    {
        silverConditions[0] = 0;
        silverConditions[1] = 3;
        silverConditions[2] = 3;
        silverConditions[3] = 3;
        silverConditions[4] = 3;
        silverConditions[5] = 3;
        silverConditions[6] = 3;
        silverConditions[7] = 3;
        silverConditions[8] = 3;
        silverConditions[9] = 3;
        silverConditions[10] = 3;

        silverConditions[11] = 3;
        silverConditions[12] = 3;
        silverConditions[13] = 3;
        silverConditions[14] = 3;
        silverConditions[15] = 3;
        silverConditions[16] = 3;
        silverConditions[17] = 3;
        silverConditions[18] = 3;
        silverConditions[19] = 3;
        silverConditions[20] = 3;

        silverConditions[21] = 3;
        silverConditions[22] = 3;
        silverConditions[23] = 3;
        silverConditions[24] = 3;
        silverConditions[25] = 3;
        silverConditions[26] = 3;
        silverConditions[27] = 3;
        silverConditions[28] = 3;
        silverConditions[29] = 3;
        silverConditions[30] = 3;

        silverConditions[31] = 3;
        silverConditions[32] = 3;
        silverConditions[33] = 3;
        silverConditions[34] = 3;
        silverConditions[35] = 3;
        silverConditions[36] = 3;
        silverConditions[37] = 3;
        silverConditions[38] = 3;
        silverConditions[39] = 3;
        silverConditions[40] = 3;

        silverConditions[41] = 3;
        silverConditions[42] = 3;
    }
}