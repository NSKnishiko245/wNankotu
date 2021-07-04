using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class StageUIManager : MonoBehaviour
{
    private GameObject eventSystem;
    private GameObject stageManager;
    private GameObject tutorialUI;
    private GameObject editCanvas;
    private GameObject player;
    public GameObject frontEffectCamera;
    private GameObject Point;

    private GameObject bookL;
    private Animator bookLAnim;

    // ���j���[��UI
    private GameObject menuSelectGear;
    private GameObject menuRetryGear;
    private GameObject menuSelect;
    private GameObject menuRetry;
    private GameObject menuBoard;
    private GameObject menuStageNum;
    private GameObject menuOperation1;
    private GameObject menuOperation2;
    private GameObject copper;
    private GameObject silver;
    private GameObject gold;


    // �q���g��UI
    private GameObject hintBoard;
    private GameObject hintMovie;
    private GameObject hintUI;
    private GameObject tv;
    private GameObject hukidasi;
    private GameObject eIcon;

    public static int missCnt = 0;  // ���s������
    public static float hintCnt = 0.0f;
    private float hintDispTime = 5.0f;
    private float hintDeleteTime = 10.0f;
    private bool hintFlg = false;
    private bool hintFirstFlg = false;
    private bool hintUIFlg = false;
    public static int hintOpenCnt = 0;
    private int eIconTime = 0;
    private int eIconTimeInit = 20;
    private int eIconNum = 0;

    // �N���A��UI
    private GameObject clearSelectGear;
    private GameObject clearSelectGear2;
    private GameObject clearNextGear;
    private GameObject clearNextGear2;
    private GameObject clearSelect;
    private GameObject clearNext;


    [SerializeField] private Material[] material = new Material[6];

    // �T�E���h
    private AudioSource bgmSource;
    [SerializeField] private AudioSource resultSource;
    [SerializeField] private AudioSource selectDecSource;
    private int bgmNum;

    [SerializeField] private bool editFlg = false;  // true:�G�f�B�b�g�\��
    [SerializeField] private bool debugFlg = false;  // true:�f�o�b�O�e�L�X�g�\��

    [SerializeField] private int stageNum;   // �X�e�[�W�ԍ�

    // �f�o�b�N�p�e�L�X�g
    GameObject debug;
    Text stageNumText;
    Text hintTimeText;
    Text rotateNumText;
    Text silverMedalNumText;
    //InputField inputField;

    // �X�e�[�W�摜�p
    GameObject stageImage;

    // �V�[���J�ڂ܂ł̎���
    private int sceneChangeCnt;
    private string changeSceneName;

    // �v���C���[�ƃX�e�[�W��\������܂ł̎���
    [SerializeField] private int stageDisplayCntInit;
    private int stageDisplayCnt = 90;

    // �N���A�R�}���h������\�ɂȂ�܂ł̎���
    [SerializeField] private int clearCommandOperationCntInit;
    private int clearCommandOperationCnt = 0;

    // ���S���Ă��烊�g���C����܂ł̎���
    [SerializeField] private int gameOverCntInit;
    private int gameOverCnt;

    public static int menuOperationCnt = 240;
    private int menuBufferCntInit = 75;
    private int menuBufferCnt;
    private bool menuBufferFlg = true;

    private int startPageCnt = 45;  // �J�n���̃y�[�W���߂����܂ł̎���
    private int endBookCnt;    // �I�����̖{������܂ł̎���

    private bool statusFirstFlg = true;
    private bool menuCommandFirstFlg = true;
    private bool clearCommandFirstFlg = true;
    private bool goldMedalFlg = false;
    private bool inputFlg = false;

    private bool stageDisplayFlg = true;
    public static bool nextPossibleFlg = true;

    // �V�[���̏��
    private enum STATUS
    {
        START,
        PLAY,
        MENU,
        HINT,
        CLEAR,
        COMMAND_DECISION,
    }
    private STATUS status;

    // �I�𒆂̃R�}���h
    private enum COMMAND
    {
        STAGE_SELECT,
        RETRY,
        NEXT,
    }
    private COMMAND command;



    //==============================================================
    // ��������
    //==============================================================
    private void Awake()
    {
        // �X�e�[�W�ԍ��擾
        stageNum = StageManager.stageNum;

        // �X�e�[�W�̉摜���擾
        stageImage = GameObject.Find("StageImage");
        Sprite sprite = Resources.Load<Sprite>("Sprite/Stage/" + StageManager.stageNum);
        stageImage.GetComponent<Image>().sprite = sprite;

        // �X�e�[�W�ԍ���UI�̐ݒ�
        Image tensPlaceImage = GameObject.Find("TensPlaceImage").GetComponent<Image>();
        sprite = Resources.Load<Sprite>("Sprite/Number/" + (stageNum / 10));
        tensPlaceImage.sprite = sprite;

        Image onesPlaceImage = GameObject.Find("OnesPlaceImage").GetComponent<Image>();
        sprite = Resources.Load<Sprite>("Sprite/Number/" + (stageNum % 10));
        onesPlaceImage.sprite = sprite;

        eventSystem = GameObject.Find("EventSystem");
        stageManager = GameObject.Find("stageManager");
        tutorialUI = GameObject.Find("TutorialUI");
        editCanvas = GameObject.Find("EditCanvas");
        player = GameObject.Find("Player");
        bookL = GameObject.Find("book_L");
        menuSelectGear = GameObject.Find("SelectGearImage");
        menuRetryGear = GameObject.Find("RetryGearImage");
        menuBoard = GameObject.Find("MenuBoard");
        menuStageNum = GameObject.Find("StageNum");
        menuOperation1 = GameObject.Find("OperationImage1");
        menuOperation2 = GameObject.Find("OperationImage2");
        clearSelectGear = GameObject.Find("C_SelectGearImage");
        clearSelectGear2 = GameObject.Find("SelectUnderGearImage");
        clearNextGear = GameObject.Find("NextGearImage");
        clearNextGear2 = GameObject.Find("NextUnderGearImage");
        menuSelect = GameObject.Find("StageSelect");
        menuRetry = GameObject.Find("Retry");
        clearSelect = GameObject.Find("C_StageSelect");
        clearNext = GameObject.Find("NextStage");
        hintBoard = GameObject.Find("HintBoard");
        hintMovie = GameObject.Find("SamnaleMovie");
        tv = GameObject.Find("UiTv");
        hukidasi = GameObject.Find("HukidasiImage");
        hintUI = GameObject.Find("UICanvas");
        copper = GameObject.Find("CopperImage");
        silver = GameObject.Find("SilverImage");
        gold = GameObject.Find("GoldImage");
        eIcon = GameObject.Find("Eicon");

        tv.transform.localScale = new Vector3(0, 0, 0);
        hukidasi.transform.localScale = new Vector3(0, 0, 0);

        // ���_�����擾���Ă�����F���t��
        if (StageSelectManager.score[stageNum].isGold)
        {
            gold.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else gold.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

        if (StageSelectManager.score[stageNum].isSilver)
        {
            silver.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else silver.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

        if (StageSelectManager.score[stageNum].isCopper)
        {
            copper.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else copper.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);



        GameObject.Find("book_L2").GetComponent<Renderer>().material = material[BookSelect.bookNum];
        GameObject.Find("book_R2").GetComponent<Renderer>().material = material[BookSelect.bookNum];

        hintMovie.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Movie/Hint/stage_" + stageNum + "_hint");

        // �X�e�[�W�P�̓`���[�g���A����BGM
        if (StageManager.stageNum == 1) bgmNum = 0;
        // ����ȊO�̓X�e�[�W���Ƃ�BGM
        else bgmNum = BookSelect.bookNum + 1;

        // �l�N�X�g�ȊO��I�����Ă�����BGM���Z�b�g���Ȃ���
        if (StageBgm.bgmFlg)
        {
            bgmSource = GameObject.Find("StageBGM").GetComponent<AudioSource>();
            AudioClip audio = Resources.Load("Sound/bgm/mainBGM_" + bgmNum, typeof(AudioClip)) as AudioClip;
            bgmSource.clip = audio;
            bgmSource.time = 0.0f;
            bgmSource.Play();
            //StageBgm.bgmFlg = false;
        }

        if (stageNum != 1)
        {
            tutorialUI.SetActive(false);
            GameObject tutorialTV = GameObject.Find("TVCanvas");
            tutorialTV.SetActive(false);
        }
        // �G�f�B�b�g���\��
        if (!editFlg) editCanvas.SetActive(false);

        StageDisplay(false);

        // �ϐ�������
        status = STATUS.START;
        stageDisplayCnt = stageDisplayCntInit;
        clearCommandOperationCnt = clearCommandOperationCntInit;
        gameOverCnt = gameOverCntInit;
        menuBufferCnt = menuBufferCntInit;

        bookLAnim = bookL.GetComponent<Animator>();
        bookLAnim.SetBool("isAnim", true);

        if (debugFlg)
        {
            stageNumText = GameObject.Find("StageNumText").GetComponent<Text>();
            stageNumText.text = "�X�e�[�W�ԍ�:" + StageManager.stageNum;

            hintTimeText = GameObject.Find("HintTimeText").GetComponent<Text>();

            rotateNumText = GameObject.Find("RotateNumText").GetComponent<Text>();

            silverMedalNumText = GameObject.Find("SilverMedalNumText").GetComponent<Text>();
            silverMedalNumText.text = "�⃁�_���̉�:" + StageSelectManager.silverConditions[StageManager.stageNum];

            //inputField = GameObject.Find("InputField").GetComponent<InputField>();
        }
        else
        {
            debug = GameObject.Find("DebugCanvas");
            debug.SetActive(false);
        }
    }

    //==============================================================
    // �X�V����
    //==============================================================
    private void Update()
    {

        STATUS tempStatus = status;

        switch (status)
        {
            //-----------------------------------
            // �J�n��
            //-----------------------------------
            case STATUS.START:
                if (startPageCnt == 0)
                {
                    eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();
                    status = STATUS.PLAY;
                }
                else startPageCnt--;
                break;

            //-----------------------------------
            // �v���C��
            //-----------------------------------
            case STATUS.PLAY:

                if (stageNum == 1)
                {
                    Point = GameObject.FindGameObjectWithTag("Point");

                }
                // �J�E���g���O�ɂȂ�ƃv���C���[�ƃX�e�[�W��\������
                if (stageDisplayCnt == 0)
                {
                    if (StageBgm.bgmFlg) StageBgm.bgmFlg = false;
                    StageDisplay(true);
                    stageImage.SetActive(false);
                    if (stageNum == 1) tutorialUI.SetActive(true);
                }
                else stageDisplayCnt--;

                // �v���C���[���������烊�g���C
                if (stageManager.GetComponent<StageManager>().IsGameOver)
                {
                    if (gameOverCnt == 0)
                    {
                        if (stageNum == 1) tutorialUI.SetActive(false);
                        stageManager.SetActive(false);
                        changeSceneName = "Stage1Scene";
                        status = STATUS.COMMAND_DECISION;
                        command = COMMAND.RETRY;
                        sceneChangeCnt = 120;
                        endBookCnt = 0;
                    }
                    else gameOverCnt--;
                }

                if (menuBufferCnt == 0)
                {
                    menuBufferFlg = false;
                    menuBufferCnt = menuBufferCntInit;
                }
                else menuBufferCnt--;

                // ���j���[���J��
                if (menuOperationCnt == 0)
                {
                    if (stageNum == 1) { }
                    else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown("joystick button 7"))
                    {
                        if (!menuBufferFlg)
                        {
                            if (!stageManager.GetComponent<StageManager>().isMove)
                            {
                                menuBufferFlg = true;
                                status = STATUS.MENU;
                                stageImage.SetActive(true);

                                // �y�[�W��i�߂�
                                eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();

                                // �`���[�g���A����\��
                                if (stageNum == 1)
                                {
                                    Point.SetActive(false);
                                    tutorialUI.SetActive(false);
                                }

                                stageManager.GetComponent<StageManager>().SetModeGoalEffect(0);
                                stageManager.GetComponent<StageManager>().SetModeGoalEffect(2);
                            }
                        }
                    }
                }
                else menuOperationCnt--;

                // �q���g���J��
                if (hintFlg)
                {
                    if (menuOperationCnt == 0)
                    {
                        if (stageNum == 1) { }
                        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 7"))
                        {
                            if (!menuBufferFlg)
                            {
                                if (!stageManager.GetComponent<StageManager>().isMove)
                                {
                                    menuBufferFlg = true;
                                    status = STATUS.HINT;
                                    stageImage.SetActive(true);
                                    hintOpenCnt++;

                                    // �y�[�W��i�߂�
                                    eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();

                                    // �`���[�g���A����\��
                                    if (stageNum == 1)
                                    {
                                        Point.SetActive(false);
                                        tutorialUI.SetActive(false);
                                    }

                                    stageManager.GetComponent<StageManager>().SetModeGoalEffect(0);
                                    stageManager.GetComponent<StageManager>().SetModeGoalEffect(2);
                                }
                            }
                        }
                    }
                    else menuOperationCnt--;
                }

                // �X�e�[�W�N���A
                if (stageManager.GetComponent<StageManager>().IsGameClear || Input.GetKeyDown(KeyCode.C))
                {
                    status = STATUS.CLEAR;
                }
                if (goldMedalFlg)
                {
                    stageManager.GetComponent<StageManager>().SetModeGoalEffect(4);
                }
                break;

            //-----------------------------------
            // ���j���[�\����
            //-----------------------------------
            case STATUS.MENU:
                if (statusFirstFlg)
                {
                    StageDisplay(false);
                    statusFirstFlg = false;

                    menuRetry.SetActive(true);
                    menuSelect.SetActive(true);
                    menuBoard.SetActive(true);
                    menuStageNum.SetActive(true);
                    menuOperation1.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menuOperation2.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    hintBoard.SetActive(false);
                    hintMovie.SetActive(false);
                    copper.SetActive(true);
                    silver.SetActive(true);
                    gold.SetActive(true);
                }

                // ���j���[�R�}���h�X�V����
                MenuCommandOperation();

                if (menuBufferCnt == 0)
                {
                    menuBufferFlg = false;
                    menuBufferCnt = menuBufferCntInit;
                }
                else menuBufferCnt--;

                // �R�}���h����
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 0"))
                {
                    status = STATUS.COMMAND_DECISION;
                    selectDecSource.Play();
                    sceneChangeCnt = 120;
                    endBookCnt = 0;
                }

                // ���j���[�����
                if (!menuBufferFlg)
                {
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown("joystick button 7"))
                    {
                        status = STATUS.PLAY;
                        menuBufferFlg = true;

                        // �y�[�W��߂�
                        eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();

                        // �X�e�[�W��\������܂ł̎��Ԃ��Z�b�g
                        stageDisplayCnt = stageDisplayCntInit;

                        stageManager.GetComponent<StageManager>().FixPlayerPos();
                        stageManager.GetComponent<StageManager>().SetModeGoalEffect(3);

                        // �`���[�g���A���\��
                        if (stageNum == 1)
                        {
                            Point.SetActive(true);
                            tutorialUI.SetActive(true);
                        }
                    }
                }
                break;

            //-----------------------------------
            // �q���g�\����
            //-----------------------------------
            case STATUS.HINT:
                if (statusFirstFlg)
                {
                    StageDisplay(false);
                    statusFirstFlg = false;

                    menuRetry.SetActive(false);
                    menuSelect.SetActive(false);
                    menuBoard.SetActive(false);
                    menuStageNum.SetActive(false);
                    menuOperation1.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menuOperation2.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    copper.SetActive(false);
                    silver.SetActive(false);
                    gold.SetActive(false);
                    hintBoard.SetActive(true);
                    hintMovie.SetActive(true);
                }

                if (menuBufferCnt == 0)
                {
                    menuBufferFlg = false;
                    menuBufferCnt = menuBufferCntInit;
                }
                else menuBufferCnt--;

                // �q���g�����
                if (!menuBufferFlg)
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 7"))
                    {
                        status = STATUS.PLAY;
                        menuBufferFlg = true;

                        // �y�[�W��߂�
                        eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();

                        // �X�e�[�W��\������܂ł̎��Ԃ��Z�b�g
                        stageDisplayCnt = stageDisplayCntInit;

                        stageManager.GetComponent<StageManager>().FixPlayerPos();
                        stageManager.GetComponent<StageManager>().SetModeGoalEffect(3);

                        // �`���[�g���A���\��
                        if (stageNum == 1)
                        {
                            Point.SetActive(true);
                            tutorialUI.SetActive(true);
                        }
                    }
                }
                break;

            //-----------------------------------
            // �N���A��
            //-----------------------------------
            case STATUS.CLEAR:
                if (statusFirstFlg)
                {
                    // �X�R�A�A�j���[�V�����J�n
                    this.GetComponent<ScoreAnimation>().StartFlgOn();

                    // �`���[�g���A����\��
                    if (stageNum == 1) tutorialUI.SetActive(false);

                    AudioSource bgm = GameObject.Find("StageBGM").GetComponent<AudioSource>();
                    bgm.Stop();
                    resultSource.Play();

                    // �����_���擾
                    StageSelectManager.score[StageManager.stageNum].isCopper = true;

                    // �⃁�_���擾
                    SilverMedalConditions();

                    // �����_���擾
                    if (goldMedalFlg)
                    {
                        StageSelectManager.score[StageManager.stageNum].isGold = true;
                        this.GetComponent<ScoreAnimation>().GoldFlgOn();
                    }

                    // ���_���̎擾�󋵂�ۑ�
                    MedalDataSave();

                    // �l�N�X�g�X�e�[�W���I���\������
                    if (StageManager.stageNum % 6 == 5)
                    {
                        if (StageSelectManager.enterExtraFlg[BookSelect.bookNum] == true) nextPossibleFlg = true;
                        else nextPossibleFlg = false;
                    }
                    else nextPossibleFlg = true;
                    if (StageManager.stageNum % 6 == 0) nextPossibleFlg = false;

                    if (nextPossibleFlg) command = COMMAND.NEXT;
                    else command = COMMAND.STAGE_SELECT;

                    statusFirstFlg = false;
                }

                if (clearCommandOperationCnt == 0)
                {
                    if (this.GetComponent<ScoreAnimation>().GetOperationFlg())
                    {
                        // �N���A�R�}���h�X�V����
                        ClearCommandOperation();

                        // �R�}���h����
                        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 0"))
                        {
                            stageManager.GetComponent<StageManager>().SetModeGoalEffect(0);
                            stageManager.GetComponent<StageManager>().SetModeGoalEffect(2);
                            status = STATUS.COMMAND_DECISION;
                            selectDecSource.Play();
                            this.GetComponent<ScoreAnimation>().EndFlgOn();
                            sceneChangeCnt = 180;
                            endBookCnt = 90;
                        }
                    }
                }
                else clearCommandOperationCnt--;

                break;

            //-----------------------------------
            // �R�}���h�����
            //-----------------------------------
            case STATUS.COMMAND_DECISION:
                if (statusFirstFlg)
                {
                    // �����^���̉΂�����
                    this.GetComponent<PostEffectController>().SetFireFlg(false);
                    if (command == COMMAND.NEXT) StageSelectManager.selectPageNum++;

                    statusFirstFlg = false;
                }

                // �{�̃��f�������
                if (endBookCnt == 0) eventSystem.GetComponent<IgnoreMouseInputModule>().AllBackPage();
                else endBookCnt--;

                // ��莞�Ԍo�߂���ƑJ�ڂ���
                if (sceneChangeCnt == 0)
                {
                    // �l�N�X�g�ȊO��I�����Ă�����BGM���Z�b�g���Ȃ����āA���s�񐔂�����������
                    if (command != COMMAND.RETRY)
                    {
                        StageBgm.bgmFlg = true;
                        missCnt = 0;
                        hintCnt = 0.0f;
                        hintOpenCnt = 0;
                        menuOperationCnt = 240;
                    }
                    // �l�N�X�g��I�����Ă����玸�s�񐔂����Z
                    else
                    {
                        missCnt++;
                        menuOperationCnt = 60;
                    }

                    SceneManager.LoadScene(changeSceneName);
                }
                else sceneChangeCnt--;
                break;

            default:
                break;
        }

        if (tempStatus != status) statusFirstFlg = true;

        // �f�o�b�O�p�e�L�X�g�X�V����
        if (debugFlg) DebugUpdate();

        // �q���g�ʒmUI�X�V����
        HintUIUpdate();
    }

    //==============================================================
    // ���j���[�R�}���h�X�V����
    //==============================================================
    private void MenuCommandOperation()
    {
        switch (command)
        {
            //-----------------------------------
            // �X�e�[�W�Z���N�g�I��
            //-----------------------------------
            case COMMAND.STAGE_SELECT:
                if (menuCommandFirstFlg)
                {
                    changeSceneName = "StageSelectScene";
                    // ���ԉ�]
                    menuSelectGear.GetComponent<GearRotation>().SetRotFlg(true);
                    Debug.Log("select");
                    menuSelect.transform.localScale = new Vector3(2.75f, 2.75f, 1.0f);
                    menuRetryGear.GetComponent<GearRotation>().SetRotFlg(false);
                    menuRetry.transform.localScale = new Vector3(2.5f, 2.5f, 1.0f);
                    menuCommandFirstFlg = false;
                }

                if (Input.GetAxis("Vertical") < 0)
                {
                    command = COMMAND.RETRY;
                    menuCommandFirstFlg = true;
                }
                break;

            //-----------------------------------
            // ���g���C�I��
            //-----------------------------------
            case COMMAND.RETRY:
                if (menuCommandFirstFlg)
                {
                    changeSceneName = SceneManager.GetActiveScene().name;
                    // ���ԉ�]
                    menuSelectGear.GetComponent<GearRotation>().SetRotFlg(false);
                    menuSelect.transform.localScale = new Vector3(2.5f, 2.5f, 1.0f);
                    menuRetryGear.GetComponent<GearRotation>().SetRotFlg(true);
                    menuRetry.transform.localScale = new Vector3(2.75f, 2.75f, 1.0f);
                    menuCommandFirstFlg = false;
                }

                if (Input.GetAxis("Vertical") > 0)
                {
                    command = COMMAND.STAGE_SELECT;
                    menuCommandFirstFlg = true;
                }
                break;

            default:
                break;
        }
    }

    //==============================================================
    // �N���A�R�}���h�X�V����
    //==============================================================
    private void ClearCommandOperation()
    {
        switch (command)
        {
            //-----------------------------------
            // �X�e�[�W�Z���N�g�I��
            //-----------------------------------
            case COMMAND.STAGE_SELECT:
                if (clearCommandFirstFlg)
                {
                    changeSceneName = "StageSelectScene";
                    // ���ԉ�]
                    clearSelectGear.GetComponent<GearRotation>().SetRotFlg(true);
                    clearSelectGear2.GetComponent<GearRotation>().SetRotFlg(true);
                    clearNextGear.GetComponent<GearRotation>().SetRotFlg(false);
                    clearNextGear2.GetComponent<GearRotation>().SetRotFlg(false);
                    clearSelect.transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
                    clearNext.transform.localScale = new Vector3(1.1f, 1.1f, 1.0f);

                    clearCommandFirstFlg = false;
                }
                if (nextPossibleFlg)
                {
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        command = COMMAND.NEXT;
                        clearCommandFirstFlg = true;
                    }
                }
                break;

            //-----------------------------------
            // �l�N�X�g�I��
            //-----------------------------------
            case COMMAND.NEXT:
                if (clearCommandFirstFlg)
                {
                    StageManager.stageNum = stageNum + 1;
                    changeSceneName = "Stage1Scene";
                    // ���ԉ�]
                    clearSelectGear.GetComponent<GearRotation>().SetRotFlg(false);
                    clearSelectGear2.GetComponent<GearRotation>().SetRotFlg(false);
                    clearNextGear.GetComponent<GearRotation>().SetRotFlg(true);
                    clearNextGear2.GetComponent<GearRotation>().SetRotFlg(true);
                    clearNext.transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
                    clearSelect.transform.localScale = new Vector3(1.1f, 1.1f, 1.0f);

                    clearCommandFirstFlg = false;
                }

                if (Input.GetAxis("Horizontal") < 0)
                {
                    command = COMMAND.STAGE_SELECT;
                    clearCommandFirstFlg = true;
                }
                break;

            default:
                break;
        }
    }

    //==============================================================
    // �⃁�_���擾����
    //==============================================================
    private void SilverMedalConditions()
    {
        if (StageSelectManager.silverConditions[StageManager.stageNum] >=
            stageManager.GetComponent<StageManager>().rotateNum)
        {
            StageSelectManager.score[StageManager.stageNum].isSilver = true;
            this.GetComponent<ScoreAnimation>().SilverFlgOn();
            Debug.Log("�m���}" + StageSelectManager.silverConditions[1]);
            Debug.Log("�܂�����" + stageManager.GetComponent<StageManager>().rotateNum);
        }
    }

    //==============================================================
    // �f�o�b�O�p�e�L�X�g�X�V����
    //==============================================================
    private void DebugUpdate()
    {
        hintTimeText.text = "�^�C�}�[:" + hintCnt.ToString("f2");

        rotateNumText.text = "�܂�����:" + stageManager.GetComponent<StageManager>().rotateNum;

        //if (!inputFlg)
        //{
        //    if (Input.GetKeyDown(KeyCode.F1))
        //    {
        //        inputField.ActivateInputField();
        //        inputFlg = true;
        //    }
        //}
        //else
        //{
        //    if (Input.GetKeyDown(KeyCode.Return))
        //    {
        //        StageManager.stageNum = int.Parse(inputField.text);
        //        SceneManager.LoadScene("Stage1Scene");
        //    }
        //}
    }

    //==============================================================
    // �X�e�[�W�\���ؑ�
    //==============================================================
    public void StageDisplay(bool sts)
    {
        if (stageDisplayFlg == sts) return;

        if (sts)
        {
            // �v���C���[�ƃX�e�[�W��\��
            player.SetActive(true);
            //player.GetComponent<Player>().waitMoveTimer = 0.5f;
            stageManager.SetActive(true);
            if (!stageManager.GetComponent<StageManager>().initFlg)
            {
                stageManager.GetComponent<StageManager>().CreateParticle();
            }
            frontEffectCamera.SetActive(true);
        }
        else
        {
            // �v���C���[�ƃX�e�[�W���\��
            player.SetActive(false);
            stageManager.SetActive(false);
            stageManager.GetComponent<StageManager>().DeleteCopyForMenu();
            frontEffectCamera.SetActive(false);
        }
        stageDisplayFlg = sts;
    }

    //==============================================================
    // ���_���̎擾�󋵂�ۑ�
    //==============================================================
    private void MedalDataSave()
    {
        int temp = System.Convert.ToInt32(StageSelectManager.score[stageNum].isCopper);
        PlayerPrefs.SetInt("Copper" + stageNum, temp);
        temp = System.Convert.ToInt32(StageSelectManager.score[stageNum].isSilver);
        PlayerPrefs.SetInt("Silver" + stageNum, temp);
        temp = System.Convert.ToInt32(StageSelectManager.score[stageNum].isGold);
        PlayerPrefs.SetInt("Gold" + stageNum, temp);

        StageSelectManager.SaveClearDate();

        Debug.Log("���_���f�[�^ �Z�[�u����");
    }

    //==============================================================
    // �q���g�ʒmUI�X�V����
    //==============================================================
    private void HintUIUpdate()
    {
        hintCnt += Time.deltaTime;

        // �q���g�ʒmUI�\��
        if (hintCnt > hintDispTime && hintOpenCnt == 0 && stageNum != 1)
        {
            hintFirstFlg = true;
            hintOpenCnt++;
        }

        // �q���g�ʒmUI��\��
        if (hintCnt > hintDeleteTime && stageNum != 1)
        {
            hintUIFlg = false;
        }

        if (hintFlg)
        {
            if (eIconTime == 0)
            {
                if (eIconNum == 0) eIconNum = 1;
                else eIconNum = 0;
                eIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/e_" + eIconNum);
                eIconTime = eIconTimeInit;
            }
            else eIconTime--;
        }

        if (hintFirstFlg)
        {
            hintFlg = true;
            hintUIFlg = true;
            tv.transform.DOScale(new Vector3(4000, 4000, 1100), 0.5f);
            hukidasi.transform.DOScale(new Vector3(2.5f, 2.5f, 1), 0.5f);
            hintFirstFlg = false;
        }

        if (hintFlg && !hintUIFlg)
        {
            tv.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
            hukidasi.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        }
    }



    public bool GetStageDisplayFlg() { return stageDisplayFlg; }

    public void StageImageDisplay(bool sts)
    {
        stageImage.SetActive(sts);
    }

    public void SetGoldMedalFlg(bool sts)
    {
        goldMedalFlg = sts;
    }
}