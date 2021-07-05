using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelectManager : MonoBehaviour
{
    private GameObject eventSystem;
    [SerializeField] private GameObject bookL;
    private Animator bookLAnim;
    [SerializeField] private GameObject bookBack;
    private GameObject book;
    private GameObject bookUI;
    private Animator cameraAnim;
    [SerializeField] private int bookNum;
    [SerializeField] GameObject mist;
    private int bookMax = 6;    // �{�̍ő吔
    private int bookStageMax = 6;  // ���������̃X�e�[�W�̍ő吔
    public static int stageMax = 36;
    private int firstStageNum;
    private int endStageNum;
    int medalCompleteNum = 0;


    private int bookSelectCntInit = 30;
    private int bookSelectCnt = 0;
    private bool bookSelectFlg = false;
    private int bookRemoveCnt = 90;
    private bool stageEnterFlg = true;

    public static int selectPageNum = 1;
    public static bool selectPageMoveFlg = false;

    // �T�E���h
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private AudioSource DecSource;
    [SerializeField] private AudioSource BuSource;



    private int sceneChangeCnt = 60;                 // �V�[���J�ڂ̃J�E���^
    private bool sceneChangeFlg = false;

    enum COMMAND
    {
        EMPTY,
        BOOK_SELECT,
        STAGE,
    }
    COMMAND command;

    [SerializeField] private int pageIntervalInit;  // �y�[�W���߂����܂ł̑ҋ@���Ԃ̏����l
    private int pageInterval = 0;

    [SerializeField] private int operationCntInit;  // �V�[���J�ڂ��đ���\�ɂȂ�܂ł̎���
    private int operationCnt = 0;

    // ���_��
    public struct Score
    {
        public bool isGold;
        public bool isSilver;
        public bool isCopper;
    }
    public static Score[] score = new Score[37];
    public static int[] silverConditions = new int[37];
    public static bool[] enterExtraFlg = new bool[6];

    //==============================================================
    // ��������
    //==============================================================
    private void Awake()
    {
        //LoadClearDate();

        this.GetComponent<CreateStageSelect>().Create();

        StageSelectManager.selectPageMoveFlg = true;

        pageInterval = pageIntervalInit;
        operationCnt = operationCntInit;
        eventSystem = GameObject.Find("EventSystem");
        bookLAnim = bookL.GetComponent<Animator>();
        bookLAnim.SetBool("isAnim", true);
        bookBack.SetActive(false);
        book = GameObject.Find("BookModel");
        bookUI = GameObject.Find("BookCanvas");
        cameraAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
        mist = GameObject.Find("Mist");
        mist.SetActive(false);
        command = COMMAND.EMPTY;



        for (int i = 1; i < stageMax + 1; i++)
        {
            if (score[i].isCopper == true && score[i].isSilver == true && score[i].isGold == true)
            {
                medalCompleteNum++;
            }
            else break;
        }

        if (medalCompleteNum >= stageMax)
        {
            BookSelect.bonusFlg = true;
        }

        if(!BookSelect.bonusFlg)
        {
            GameObject.Find("BookModel7").SetActive(false);
        }

        //BookSelect.bookNum = bookNum - 1;
        for (int i = 0; i < bookMax; i++)
        {
            if (i == BookSelect.bookNum)
            {
                firstStageNum = BookSelect.bookNum * bookStageMax + 1;
                endStageNum = firstStageNum + 5;
                break;
            }
        }

        SilverConditionsSet();
        this.GetComponent<PostEffectController>().SetVigFlg(false);
        //SaveClearDate();
    }

    //==============================================================
    // �X�V����
    //==============================================================
    private void Update()
    {
            // �P�y�[�W�ڂ��߂���
            if (operationCnt == 1) eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();

            // ����\
            if (operationCnt == 0)
            {
                if (selectPageMoveFlg)
                {
                    if (pageInterval == 0)
                    {
                        // ���̃y�[�W�֐i��
                        eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();
                        pageInterval = pageIntervalInit;
                    }
                    else pageInterval--;

                    if (selectPageNum == eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum())
                    {
                        selectPageMoveFlg = false;
                    }
                }
                else
                {
                    // �{������Ƃ��͑���s�\�ɂ���
                    if (!eventSystem.GetComponent<IgnoreMouseInputModule>().GetAllBackFlg())
                    {
                        PageOperation(); // �y�[�W���߂��鑀��
                    }
                }
            }
            else operationCnt--;

            if (!selectPageMoveFlg)
            {
                ExtraConditions();  // �G�N�X�g���X�e�[�W�ɓ���邩�`�F�b�N
                StageSceneChange(); // �X�e�[�W��ʂւ̑J��
                BookSelectChange(); // �{�̑I����ʂւ̑J��
            }

            BookClearCheck(1, 5, 0);
            BookClearCheck(7, 11, 1);
            BookClearCheck(13, 17, 2);
            BookClearCheck(19, 23, 3);
            BookClearCheck(25, 29, 4);
            BookClearCheck(31, 35, 5);

            if (Input.GetKeyDown(KeyCode.T))
            {
                SaveClearDate();
            }
    }

    //==============================================================
    // ��Փx���ƂɃN���A�������`�F�b�N
    //==============================================================
    private void BookClearCheck(int startNum, int endNum, int bookNum)
    {
        int stageNum = endNum - startNum + 1;
        int clearCnt = 0;
        for (int cnt = startNum; cnt <= endNum; cnt++)
        {
            if (score[cnt].isCopper)
            {
                StageClearManager.m_isGetCopper[cnt] = true;
                clearCnt++;
            }
        }
        if (clearCnt == stageNum)
        {
            StageClearManager.StageClear[bookNum] = true;
        }
    }

    //==============================================================
    // �y�[�W���߂��鑀��
    //==============================================================
    private void PageOperation()
    {
        int stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum() + firstStageNum - 1;

        GameObject[] stageImage = GameObject.FindGameObjectsWithTag("Samne");
        for (int i = 0; i < 6; ++i)
        {
            stageImage[i].GetComponent<SamnaleMovie>().num = stageNum;
        }

        if (pageInterval == 0)
        {
            if (!FinshManager.escFlg)
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
        }
        else pageInterval--;
    }

    //==============================================================
    // �X�e�[�W��ʂւ̑J��
    //==============================================================
    private void StageSceneChange()
    {
        if (!FinshManager.escFlg)
        {
            if (command == COMMAND.EMPTY && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 0")))
            {

                // ���݂̃y�[�W�擾(�X�e�[�W�ԍ�)
                StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum() + firstStageNum - 1;

                // �V�[���J�ڊJ�n
                if (StageManager.stageNum > 0)
                {
                    if (stageEnterFlg)
                    {
                        DecSource.Play();

                        command = COMMAND.STAGE;
                        selectPageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();
                    }
                    else
                    {
                        BuSource.Play();
                    }
                }
            }
        }

        if (command == COMMAND.STAGE)
        {
            // �{�����
            eventSystem.GetComponent<IgnoreMouseInputModule>().AllBackPage();
            // �������Â�����
            this.GetComponent<PostEffectController>().SetVigFlg(true);

            // �{�����I�����
            if (eventSystem.GetComponent<IgnoreMouseInputModule>().GetBookCloseFlg())
            {
                // �J��
                SceneManager.LoadScene("Stage1Scene");
            }
        }
    }

    //==============================================================
    // �{�̑I����ʂւ̑J��
    //==============================================================
    private void BookSelectChange()
    {
        if (!FinshManager.escFlg)
        {
            if (command == COMMAND.EMPTY && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown("joystick button 1")))
            {
                DecSource.Play();

                // �{�����
                eventSystem.GetComponent<IgnoreMouseInputModule>().AllBackPage();
                mist.SetActive(false);
                command = COMMAND.BOOK_SELECT;
            }
        }

        // �{�����I�����
        if (eventSystem.GetComponent<IgnoreMouseInputModule>().GetBookCloseFlg() && command == COMMAND.BOOK_SELECT)
        {
            bookUI.SetActive(false);
            bookBack.SetActive(true);
            cameraAnim.SetBool("isAnim", true);

            // �{����ɏオ��
            if (bookRemoveCnt > 0)
            {
                Vector3 pos = new Vector3(0.0f, 0.5f, -0.5f);
                book.transform.position += pos;
                bookRemoveCnt--;
            }
            // �{���I�ɖ߂�
            if (bookRemoveCnt == 0)
            {
                book.transform.localPosition = new Vector3(-2.0f, 49.73f, -50.55f);
                book.transform.localEulerAngles = new Vector3(25.0f, 0.0f, -90.0f);
                book.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                book.transform.DOLocalMove(new Vector3((-2.0f + (BookSelect.bookNum * 0.75f)), 9.73f, -0.55f), 0.75f).OnComplete(() =>
                {
                    sceneChangeFlg = true;
                });
                bookRemoveCnt = -1;
            }

            // �V�[���J��
            if (sceneChangeFlg)
            {
                if (sceneChangeCnt == 0)
                {
                    selectPageNum = 1;
                    SceneManager.LoadScene("BookSelectScene");
                }
                else sceneChangeCnt--;
            }
        }
    }

    //==============================================================
    // �G�N�X�g���X�e�[�W�ɓ���邩�`�F�b�N
    //==============================================================
    private void ExtraConditions()
    {
        // �J���Ă���y�[�W���G�N�X�g���X�e�[�W�̎�
        if (eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum() + firstStageNum - 1 == endStageNum)
        {
            int cnt = 0;

            // �e�X�e�[�W�̃��_�����擾�ł��Ă��邩�`�F�b�N
            for (int i = firstStageNum; i <= endStageNum - 1; i++)
            {
                if (score[i].isCopper && score[i].isSilver && score[i].isGold) cnt++;
            }

            // �����𖞂����Ă��Ȃ����͓���Ȃ�
            if (cnt != bookStageMax - 1)
            {
                stageEnterFlg = false;
                mist.SetActive(true);
            }
            else
            {
                enterExtraFlg[BookSelect.bookNum] = true;
            }
        }
        // �J���Ă���y�[�W���G�N�X�g���X�e�[�W�ȊO�̎�
        else
        {
            stageEnterFlg = true;
            mist.SetActive(false);
        }

        if (enterExtraFlg[BookSelect.bookNum] == true)
        {
            stageEnterFlg = true;
            mist.SetActive(false);
        }
    }

    //==============================================================
    // �⃁�_���̊l���������Z�b�g(�X�e�[�W��܂�����)
    //==============================================================
    private void SilverConditionsSet()
    {
        // �e�L�X�g�t�@�C������^�C�g�������擾
        TextAsset textAsset = Resources.Load("Text/SilverConditions", typeof(TextAsset)) as TextAsset;
        //��s�Â��
        string[] SilverConditionsString = textAsset.text.Split('\n');

        silverConditions[0] = 0;

        for (int i = 1; i < stageMax + 1; i++)
        {
            silverConditions[i] = int.Parse(SilverConditionsString[i]);
        }
    }

    
    public static void SaveClearDate()
    {
        List<int> date = new List<int>();
        for (int i = 0; i < 37; i++)
        {
            if (score[i].isGold) date.Add(1);
            else date.Add(0);

            if (score[i].isSilver) date.Add(1);
            else date.Add(0);

            if (score[i].isCopper) date.Add(1);
            else date.Add(0);
        }
        CsvWrite cw = new CsvWrite();
        cw.SetFileInputFolderName();
        cw.WriteBarMapFromCsv(date, "clear");
    }

    public static void LoadClearDate()
    {
        List<int> clearDate = new List<int>(new int[37 * 3]);
        CsvWrite cw = new CsvWrite();
        cw.SetFileInputFolderName();
        cw.ReadBarMapFromCsv(clearDate, "clear");
        for (int i = 0; i < 37; i++)
        {
            if (clearDate[i * 3] != 0) score[i].isGold = true;
            else score[i].isGold = false;
            if (clearDate[i * 3 + 1] != 0) score[i].isSilver = true;
            else score[i].isSilver = false;
            if (clearDate[i * 3 + 2] != 0) score[i].isCopper = true;
            else score[i].isCopper = false;
        }
    }
}
