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
    private bool sceneChangeFlg = false;            // true:�V�[���J�ڊJ�n
    //private bool sceneChangeFirstFlg = true;        // �V�[���J�ڊJ�n��ɂP�x�����ʂ鏈��

    private int pageInterval;                       // �y�[�W���߂����܂ł̑ҋ@����
    [SerializeField] private int pageIntervalInit;  // �y�[�W���߂����܂ł̑ҋ@���Ԃ̏����l

    private GameObject[] goldImage;
    private GameObject[] silverImage;
    private GameObject[] copperImage;
    public struct Score
    {
        public bool isGold;
        public bool isSilver;
        public bool isCopper;
    }
    public static Score[] score = new Score[11];
    public static int[] silverConditions = new int[11];

    private void Awake()
    {
        pageInterval = pageIntervalInit;

        goldImage = GameObject.FindGameObjectsWithTag("GoldImage");
        silverImage = GameObject.FindGameObjectsWithTag("SilverImage");
        copperImage = GameObject.FindGameObjectsWithTag("CopperImage");

        //ScoreReset();            
        SilverConditionsSet();

        this.GetComponent<PostEffectController>().SetVigFlg(false);
    }

    private void Update()
    {
        PageOperation();    // �y�[�W���߂���
        SceneChange();      // �V�[���J��
        ScoreDisplay();
    }

    // �y�[�W���߂���
    private void PageOperation()
    {
        if (pageInterval == 0)
        {
            // ���̃y�[�W�֐i��
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick button 5"))
            {
                eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();
                pageInterval = pageIntervalInit;
            }
            // �O�̃y�[�W�ɖ߂�
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick button 4"))
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
        // �V�[���J�ڊJ�n
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            selectDecSource.Play();

            // ���݂̃y�[�W�擾(�X�e�[�W�ԍ�)
            StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();
            // �X�e�[�W�ɑJ��
            if (StageManager.stageNum > 0) sceneChangeFlg = true;
        }

        if (sceneChangeFlg)
        {
            this.GetComponent<PostEffectController>().SetVigFlg(true);
            // ��莞�Ԍo�߂���ƑJ�ڂ���
            if (sceneChangeCnt > sceneChangeTime)
            {
                SceneManager.LoadScene("Stage1Scene");
            }
            sceneChangeCnt++;
        }
    }

    private void ScoreDisplay()
    {
        // ���݂̃y�[�W�擾(�X�e�[�W�ԍ�)
        StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();

        for (int i = 0; i < goldImage.Length; i++)
        {
            if (score[StageManager.stageNum].isGold) goldImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else goldImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }

        for (int i = 0; i < silverImage.Length; i++)
        {
            if (score[StageManager.stageNum].isSilver) silverImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else silverImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }

        for (int i = 0; i < copperImage.Length; i++)
        {
            if (score[StageManager.stageNum].isCopper) copperImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else copperImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    // �⃁�_���̊l���������Z�b�g(�X�e�[�W��܂�����)
    private void SilverConditionsSet()
    {
        silverConditions[0] = 3;
        silverConditions[1] = 5;
        silverConditions[2] = 20;
        silverConditions[3] = 20;
        silverConditions[4] = 20;
        silverConditions[5] = 20;
        silverConditions[6] = 20;
        silverConditions[7] = 20;
        silverConditions[8] = 20;
        silverConditions[9] = 20;
        silverConditions[10] = 20;
    }
}