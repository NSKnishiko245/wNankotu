using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    // �N���A��UI
    private GameObject clearSelectGear;
    private GameObject clearSelectGear2;
    private GameObject clearNextGear;
    private GameObject clearNextGear2;

    [SerializeField] private Material[] material = new Material[6];

    // �T�E���h
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource resultSource;
    [SerializeField] private AudioSource selectDecSource;

    [SerializeField] private bool editFlg = false;  // true:�G�f�B�b�g�\��
    [SerializeField] private bool debugFlg = false;  // true:�f�o�b�O�e�L�X�g�\��

    [SerializeField] private int stageNum;   // �X�e�[�W�ԍ�

    // �f�o�b�N�p�e�L�X�g
    GameObject debug;
    Text stageNumText;
    Text rotateNumText;
    Text silverMedalNumText;
    InputField inputField;

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
        // �X�e�[�W�̉摜���擾
        stageImage = GameObject.Find("StageImage");
        Sprite sprite = Resources.Load<Sprite>("StageImage/st" + StageManager.stageNum);
        stageImage.GetComponent<Image>().sprite = sprite;

        eventSystem = GameObject.Find("EventSystem");
        stageManager = GameObject.Find("stageManager");
        tutorialUI = GameObject.Find("TutorialUI");
        editCanvas = GameObject.Find("EditCanvas");
        player = GameObject.Find("Player");
        bookL = GameObject.Find("book_L");
        menuSelectGear = GameObject.Find("SelectGearImage");
        menuRetryGear = GameObject.Find("RetryGearImage");
        clearSelectGear = GameObject.Find("C_SelectGearImage");
        clearSelectGear2 = GameObject.Find("SelectUnderGearImage");
        clearNextGear = GameObject.Find("NextGearImage");
        clearNextGear2 = GameObject.Find("NextUnderGearImage");
        menuSelect = GameObject.Find("StageSelect");
        menuRetry = GameObject.Find("Retry");

        GameObject.Find("book_L2").GetComponent<Renderer>().material = material[BookSelect.bookNum];
        GameObject.Find("book_R2").GetComponent<Renderer>().material = material[BookSelect.bookNum];

        // �X�e�[�W�ԍ��擾
        stageNum = StageManager.stageNum;

        if (stageNum != 1) tutorialUI.SetActive(false);

        // �G�f�B�b�g���\��
        if (!editFlg) editCanvas.SetActive(false);

        StageDisplay(false);

        // �ϐ�������
        status = STATUS.START;
        stageDisplayCnt = stageDisplayCntInit;
        clearCommandOperationCnt = clearCommandOperationCntInit;
        gameOverCnt = gameOverCntInit;

        bookLAnim = bookL.GetComponent<Animator>();
        bookLAnim.SetBool("isAnim", true);

        if (debugFlg)
        {
            stageNumText = GameObject.Find("StageNumText").GetComponent<Text>();
            stageNumText.text = "�X�e�[�W�ԍ�:" + StageManager.stageNum;

            rotateNumText = GameObject.Find("RotateNumText").GetComponent<Text>();

            silverMedalNumText = GameObject.Find("SilverMedalNumText").GetComponent<Text>();
            silverMedalNumText.text = "�⃁�_���̉�:" + StageSelectManager.silverConditions[StageManager.stageNum];

            inputField = GameObject.Find("InputField").GetComponent<InputField>();
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
                        sceneChangeCnt = 120;
                        endBookCnt = 0;
                    }
                    else gameOverCnt--;
                }

                // ���j���[���J��
                if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 7"))
                {
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
                }

                // �X�e�[�W�N���A
                if (stageManager.GetComponent<StageManager>().IsGameClear || Input.GetKeyDown(KeyCode.C))
                {
                    status = STATUS.CLEAR;
                }

                if (goldMedalFlg)
                {
                    GameObject goal1 = GameObject.Find("2(Clone)/GoalObj").gameObject;
                    GameObject goal2 = GameObject.Find("2(Clone)/GoalObj (1)").gameObject;
                    goal1.GetComponent<GoalScript>().ChangeColor(GoalScript.E_ParticleColor.GOLD);
                    goal2.GetComponent<GoalScript>().ChangeColor(GoalScript.E_ParticleColor.GOLD);
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
                }

                // ���j���[�R�}���h�X�V����
                MenuCommandOperation();

                // �R�}���h����
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                {
                    status = STATUS.COMMAND_DECISION;
                    selectDecSource.Play();
                    sceneChangeCnt = 120;
                    endBookCnt = 0;
                }

                // ���j���[�����
                if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 7"))
                {
                    status = STATUS.PLAY;

                    // �y�[�W��߂�
                    eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();

                    // �X�e�[�W��\������܂ł̎��Ԃ��Z�b�g
                    stageDisplayCnt = stageDisplayCntInit;

                    // �`���[�g���A���\��
                    if (stageNum == 1)
                    {
                        Point.SetActive(true);
                        tutorialUI.SetActive(true);
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

                    bgmSource.Stop();
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

                    // �l�N�X�g�X�e�[�W���I���\������
                    if (StageManager.stageNum % 6 == 0) nextPossibleFlg = false;
                    if (StageManager.stageNum % 6 == 5)
                    {
                        if (StageSelectManager.enterExtraFlg[BookSelect.bookNum] == true) nextPossibleFlg = true;
                        else nextPossibleFlg = false;
                    }
                    else nextPossibleFlg = true;

                    statusFirstFlg = false;
                }

                if (clearCommandOperationCnt == 0)
                {
                    if (this.GetComponent<ScoreAnimation>().GetOperationFlg())
                    {
                        // �N���A�R�}���h�X�V����
                        ClearCommandOperation();

                        // �R�}���h����
                        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                        {
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
                if (sceneChangeCnt == 0) SceneManager.LoadScene(changeSceneName);
                else sceneChangeCnt--;
                break;

            default:
                break;
        }

        if (tempStatus != status) statusFirstFlg = true;

        // �f�o�b�O�p�e�L�X�g�X�V����
        if (debugFlg) DebugUpdate();
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
                    changeSceneName = "StageSelect" + (BookSelect.bookNum + 1);
                    // ���ԉ�]
                    menuSelectGear.GetComponent<GearRotation>().SetRotFlg(true);
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
                    changeSceneName = "StageSelect" + (BookSelect.bookNum + 1);
                    // ���ԉ�]
                    clearSelectGear.GetComponent<GearRotation>().SetRotFlg(true);
                    clearSelectGear2.GetComponent<GearRotation>().SetRotFlg(true);
                    clearNextGear.GetComponent<GearRotation>().SetRotFlg(false);
                    clearNextGear2.GetComponent<GearRotation>().SetRotFlg(false);
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

            //GameObject goal1 = GameObject.Find("2(Clone)/GoalObj").gameObject;
            //GameObject goal2 = GameObject.Find("2(Clone)/GoalObj (1)").gameObject;
            //goal1.GetComponent<GoalScript>().ChangeColor(GoalScript.E_ParticleColor.SILVER);
            //goal2.GetComponent<GoalScript>().ChangeColor(GoalScript.E_ParticleColor.SILVER);
        }
    }

    //==============================================================
    // �f�o�b�O�p�e�L�X�g�X�V����
    //==============================================================
    private void DebugUpdate()
    {
        rotateNumText.text = "�܂�����:" + stageManager.GetComponent<StageManager>().rotateNum;

        if (!inputFlg)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                inputField.ActivateInputField();
                inputFlg = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StageManager.stageNum = int.Parse(inputField.text);
                SceneManager.LoadScene("Stage1Scene");
            }
        }
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
            stageManager.SetActive(true);
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