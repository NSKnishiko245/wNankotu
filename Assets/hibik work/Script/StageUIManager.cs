using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageUIManager : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject stageManager;
    [SerializeField] private GameObject editCanvas;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject bookL;
    private Animator bookLAnim;

    // ���j���[��UI
    [SerializeField] private GameObject menuSelectGear;
    [SerializeField] private GameObject menuRetryGear;

    // �N���A��UI
    [SerializeField] private GameObject clearSelectGear;
    [SerializeField] private GameObject clearSelectGear2;
    [SerializeField] private GameObject clearNextGear;
    [SerializeField] private GameObject clearNextGear2;

    // �T�E���h
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource resultSource;
    [SerializeField] private AudioSource selectDecSource;

    [SerializeField] private bool editFlg = false;  // true:�G�f�B�b�g�\��
    [SerializeField] private int stageNum;   // �X�e�[�W�ԍ�

    // �V�[���J�ڂ܂ł̎���
    private int sceneChangeCnt;
    private string changeSceneName;

    // �v���C���[�ƃX�e�[�W��\������܂ł̎���
    [SerializeField] private int stageDisplayCntInit;
    private int stageDisplayCnt = 90;

    // �N���A�R�}���h������\�ɂȂ�܂ł̎���
    [SerializeField] private int clearCommandOperationCntInit;
    private int clearCommandOperationCnt = 0;

    private int startPageCnt = 45;  // �J�n���̃y�[�W���߂����܂ł̎���
    private int endBookCnt;    // �I�����̖{������܂ł̎���

    private bool statusFirstFlg = true;
    private bool menuCommandFirstFlg = true;
    private bool clearCommandFirstFlg = true;
    private bool goldMedalFlg = false;

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
        // �X�e�[�W�ԍ��擾
        stageNum = StageManager.stageNum;

        // �G�f�B�b�g���\��
        if (!editFlg) editCanvas.SetActive(false);

        StageDisplay(false);

        // �ϐ�������
        status = STATUS.START;
        stageDisplayCnt = stageDisplayCntInit;
        clearCommandOperationCnt = clearCommandOperationCntInit;

        bookLAnim = bookL.GetComponent<Animator>();
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
                // �J�E���g���O�ɂȂ�ƃv���C���[�ƃX�e�[�W��\������
                if (stageDisplayCnt == 0)
                {
                    StageDisplay(true);
                }
                else stageDisplayCnt--;

                // ���j���[���J��
                if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 3"))
                {
                    status = STATUS.MENU;

                    // �y�[�W��i�߂�
                    eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();
                }

                // �X�e�[�W�N���A
                if (stageManager.GetComponent<StageManager>().IsGameClear || Input.GetKeyDown(KeyCode.C))
                {
                    status = STATUS.CLEAR;
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
                if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 3"))
                {
                    status = STATUS.PLAY;

                    // �y�[�W��߂�
                    eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();

                    // �X�e�[�W��\������܂ł̎��Ԃ��Z�b�g
                    stageDisplayCnt = stageDisplayCntInit;
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

                    bgmSource.Stop();
                    resultSource.Play();

                    // �����_���擾
                    StageSelectManager.score[StageManager.stageNum].isCopper = true;

                    // �⃁�_���擾
                    SilverMedalConditions();

                    // �����_���擾
                    if(goldMedalFlg) StageSelectManager.score[StageManager.stageNum].isGold = true;

                    statusFirstFlg = false;
                }

                if (clearCommandOperationCnt == 0)
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
                    changeSceneName = "SelectScene";
                    // ���ԉ�]
                    menuSelectGear.GetComponent<GearRotation>().SetRotFlg(true);
                    menuRetryGear.GetComponent<GearRotation>().SetRotFlg(false);
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
                    changeSceneName = "Stage1Scene";
                    // ���ԉ�]
                    menuSelectGear.GetComponent<GearRotation>().SetRotFlg(false);
                    menuRetryGear.GetComponent<GearRotation>().SetRotFlg(true);
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
                    changeSceneName = "SelectScene";
                    // ���ԉ�]
                    clearSelectGear.GetComponent<GearRotation>().SetRotFlg(true);
                    clearSelectGear2.GetComponent<GearRotation>().SetRotFlg(true);
                    clearNextGear.GetComponent<GearRotation>().SetRotFlg(false);
                    clearNextGear2.GetComponent<GearRotation>().SetRotFlg(false);
                    clearCommandFirstFlg = false;
                }

                if (Input.GetAxis("Horizontal") > 0)
                {
                    command = COMMAND.NEXT;
                    clearCommandFirstFlg = true;
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
            Debug.Log("�m���}" + StageSelectManager.silverConditions[1]);
            Debug.Log("�܂�����" + stageManager.GetComponent<StageManager>().rotateNum);
        }
    }

    //==============================================================
    // �X�e�[�W�\���ؑ�
    //==============================================================
    public void StageDisplay(bool sts)
    {
        if(sts)
        {
            // �v���C���[�ƃX�e�[�W��\��
            player.SetActive(true);
            stageManager.SetActive(true);
        }
        else
        {
            // �v���C���[�ƃX�e�[�W���\��
            player.SetActive(false);
            stageManager.SetActive(false);
        }
    }

    public void SetGoldMedalFlg(bool sts)
    {
        goldMedalFlg = sts;
    }
}