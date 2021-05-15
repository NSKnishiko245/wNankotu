using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class NewStageSelect : MonoBehaviour
{
    private GameObject eventSystem;
    private Animator[] bookAnim;
    private Animator[] bookLAnim;
    private GameObject[] book;

    private int bookMax = 5;    // �{�̍ő吔
    private int stageMax = 43;  // �X�e�[�W�̍ő吔

    [SerializeField] private int bookNum = 0;    // �{�̔ԍ�
    private int bookSelectCntInit = 30;
    private int bookSelectCnt = 0;

    // �T�E���h
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private AudioSource DecSource;


    [SerializeField] private float sceneChangeTime; // �V�[���J�ڂ܂ł̎���
    private int sceneChangeCnt = 0;                 // �V�[���J�ڂ̃J�E���^
    private bool sceneChangeFlg = false;

    [SerializeField] private int pageIntervalInit;  // �y�[�W���߂����܂ł̑ҋ@���Ԃ̏����l
    private int pageInterval = 0;

    [SerializeField] private int operationCntInit;  // �V�[���J�ڂ��đ���\�ɂȂ�܂ł̎���
    private int operationCnt = 0;

    // ���_��
    private GameObject[] goldMedal;
    private GameObject[] silverMedal;
    private GameObject[] copperMedal;
    public struct Score
    {
        public bool isGold;
        public bool isSilver;
        public bool isCopper;
    }
    public static Score[] score;
    public static int[] silverCondition;

    // �V�[���̏��
    private enum STATUS
    {
        BOOK_SELECT,
        STAGE_SELECT,
    }
    private STATUS status;



    //==============================================================
    // ��������
    //==============================================================
    private void Awake()
    {
        pageInterval = pageIntervalInit;
        operationCnt = operationCntInit;

        eventSystem = GameObject.Find("EventSystem");

        bookAnim = new Animator[bookMax];
        bookLAnim = new Animator[bookMax];
        book = new GameObject[bookMax];
        for (int i = 0; i < bookMax; i++)
        {
            bookAnim[i] = GameObject.Find("BookModel" + (i + 1)).GetComponent<Animator>();
            bookLAnim[i] = GameObject.Find("book_L" + (i + 1)).GetComponent<Animator>();
            book[i] = GameObject.Find("BookModel" + (i + 1));
        }
        bookAnim[bookNum].SetBool("isUp", true);


        score = new Score[stageMax];
        silverCondition = new int[stageMax];
        goldMedal = new GameObject[stageMax];
        silverMedal = new GameObject[stageMax];
        copperMedal = new GameObject[stageMax];
        for (int i = 1; i < stageMax; i++)
        {
            goldMedal[i] = GameObject.Find("GoldImage" + i);
            silverMedal[i] = GameObject.Find("SilverImage" + i);
            copperMedal[i] = GameObject.Find("CopperImage" + i);

            if (score[i].isGold) goldMedal[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else goldMedal[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

            if (score[i].isSilver) silverMedal[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else silverMedal[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

            if (score[i].isCopper) copperMedal[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else copperMedal[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        }

        SilverConditionsSet();

        this.GetComponent<PostEffectController>().SetVigFlg(false);
    }

    //==============================================================
    // �X�V����
    //==============================================================
    private void Update()
    {
        switch (status)
        {
            //-----------------------------------
            // �{�I��
            //-----------------------------------
            case STATUS.BOOK_SELECT:

                if (bookSelectCnt == 0)
                {
                    // ���̖{��I��
                    if (Input.GetAxis("Horizontal") < 0)
                    {
                        if (bookNum > 0)
                        {
                            bookNum--;
                            bookSelectCnt = bookSelectCntInit;
                            // �I�𒆂̖{����ɏオ��
                            bookAnim[bookNum].SetBool("isUp", true);
                            bookAnim[bookNum + 1].SetBool("isUp", false);
                        }
                    }
                    // �E�̖{��I��
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        if (bookNum < bookMax - 1)
                        {
                            bookNum++;
                            bookSelectCnt = bookSelectCntInit;
                            // �I�𒆂̖{����ɏオ��
                            bookAnim[bookNum].SetBool("isUp", true);

                            bookAnim[bookNum - 1].SetBool("isUp", false);
                        }
                    }
                }
                else bookSelectCnt--;

                // �{������
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                {
                    // �I�𒆂̖{�����Ɉړ�
                    //bookAnim[bookNum].SetBool("isCome", true);
                    book[bookNum].transform.DOScale(new Vector3(4.43f, 4.23f, 5.6f), 1.0f);
                    book[bookNum].transform.DOLocalMove(new Vector3(-0.77f, -2.66f, 11.15f), 1.0f);
                    book[bookNum].transform.DOLocalRotate(new Vector3(88.8f, 180.0f, 0.0f), 1.0f);
                    status = STATUS.STAGE_SELECT;
                }
                break;

            //-----------------------------------
            // �X�e�[�W�I��
            //-----------------------------------
            case STATUS.STAGE_SELECT:
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

                break;

            default:
                break;
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
            DecSource.Play();

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

    // �⃁�_���̊l���������Z�b�g(�X�e�[�W��܂�����)
    private void SilverConditionsSet()
    {
        silverCondition[0] = 0;
        silverCondition[1] = 3;
        silverCondition[2] = 3;
        silverCondition[3] = 3;
        silverCondition[4] = 3;
        silverCondition[5] = 3;
        silverCondition[6] = 3;
        silverCondition[7] = 3;
        silverCondition[8] = 3;
        silverCondition[9] = 3;
        silverCondition[10] = 3;

        silverCondition[11] = 3;
        silverCondition[12] = 3;
        silverCondition[13] = 3;
        silverCondition[14] = 3;
        silverCondition[15] = 3;
        silverCondition[16] = 3;
        silverCondition[17] = 3;
        silverCondition[18] = 3;
        silverCondition[19] = 3;
        silverCondition[20] = 3;

        silverCondition[21] = 3;
        silverCondition[22] = 3;
        silverCondition[23] = 3;
        silverCondition[24] = 3;
        silverCondition[25] = 3;
        silverCondition[26] = 3;
        silverCondition[27] = 3;
        silverCondition[28] = 3;
        silverCondition[29] = 3;
        silverCondition[30] = 3;

        silverCondition[31] = 3;
        silverCondition[32] = 3;
        silverCondition[33] = 3;
        silverCondition[34] = 3;
        silverCondition[35] = 3;
        silverCondition[36] = 3;
        silverCondition[37] = 3;
        silverCondition[38] = 3;
        silverCondition[39] = 3;
        silverCondition[40] = 3;

        silverCondition[41] = 3;
        silverCondition[42] = 3;
    }
}
