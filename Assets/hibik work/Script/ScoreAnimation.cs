using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreAnimation : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject stageClear;
    [SerializeField] private GameObject stageSelect;
    [SerializeField] private GameObject nextStage;

    private Animator stageClearAnim;
    private Animator stageSelectAnim;
    private Animator nextStageAnim;

    [SerializeField] private GameObject[] medal = new GameObject[3];
    private Animator[] medalAnim = new Animator[3];

    // �N���A��̃J�����̈ʒu�����p(�S�[������̍����Z�b�g����)
    private Vector3 cameraStartPos;    // �J�����̏����ʒu
    [SerializeField] private Vector3 cameraAdjustmentPos;
    private Vector3 cameraNowPos;       // �J�����̌��ݍ��W
    private Vector3 goalPos;            // �S�[���̍��W
    private Vector3 cameraTargetPos;    // �J�����̈ړ���̍��W
    private Vector3 cameraMovePos;      // �J�������P�t���[���Ɉړ�������W
    [SerializeField] private int cameraMoveSpeed; // �J�������ړ����鑬�x
    private int cameraStartMoveCnt;          // �J�������ړ������
    private int stageDeleteCnt = 120;

    // �X�e�[�W�N���A���Ă���X�e�[�W�N���A�̃e�L�X�g���o��܂ł̎���
    [SerializeField] private int stageClearTextCnt;

    // �X�e�[�W�N���A���Ă���NextStage��StageSelect���o��܂ł̎���
    [SerializeField] private int clearUiCnt;
    // �V����]����܂ł̎���
    [SerializeField] private int clearUiRotCnt;

    // �X�e�[�W�N���A���Ă���ŏ��̃��_�����o��܂ł̎���
    [SerializeField] private int medalAnimStartCnt;

    // ���̎��Ԃ��o��܂ł̎���
    [SerializeField] private int medalIntervalCnt;
    private int medalInterval = 0;
    private int medalNum = 0;

    // �X�e�[�W�N���A���Ă��烁�_������]����܂ł̎���
    [SerializeField] private int medalRotationStartCnt;

    private bool firstClearFlg = true; // �N���A���1�x�����s������
    private bool startFlg = false;      // �N���A�A�j���[�V�����J�n
    private bool endFlg = false;      // �N���A�A�j���[�V�����I��
    private bool silverFlg = false;
    private bool goldFlg = false;
    private bool operationFlg = false;

    //==============================================================
    // ��������
    //==============================================================
    private void Start()
    {
        stageClearAnim = stageClear.GetComponent<Animator>();
        stageSelectAnim = stageSelect.GetComponent<Animator>();
        nextStageAnim = nextStage.GetComponent<Animator>();

        for (int i = 0; i < 3; i++) medalAnim[i] = medal[i].GetComponent<Animator>();

        cameraStartPos = mainCamera.transform.position;
        cameraNowPos = mainCamera.transform.position;
        cameraStartMoveCnt = cameraMoveSpeed;

        // ���_���̓���m�F�p
        //StageSelectManager.score[StageManager.stageNum].isCopper = true;
        //StageSelectManager.score[StageManager.stageNum].isSilver = true;
        //StageSelectManager.score[StageManager.stageNum].isGold = true;
    }

    //==============================================================
    // �X�V����
    //==============================================================
    private void Update()
    {
        // �N���A��
        if (startFlg)
        {
            // �N���A���1�x�����s������
            FirstClearFunc();

            // �N���A���UI�̃A�j���[�V����
            ClearUiAnimation();

            // ���_�����o�Ă���A�j���[�V����
            MedalAnimation();

            // �X�e�[�W���̑S�Ẵ��_�����擾����ƃ��_������]����
            MedalRotation();
        }

        if (endFlg)
        {
            stageClearAnim.SetBool("isMove", false);
            stageSelectAnim.SetBool("isRot", false);
            stageSelectAnim.SetBool("isMove", false);
            nextStageAnim.SetBool("isRot", false);
            nextStageAnim.SetBool("isMove", false);

            if (stageDeleteCnt == 0)
            {
                for (int i = 0; i < 3; i++) medal[i].SetActive(false);

                this.GetComponent<StageUIManager>().StageDisplay(false);
                this.GetComponent<StageUIManager>().StageImageDisplay(true);
            }
            else stageDeleteCnt--;
        }
    }

    //==============================================================
    // �N���A���1�x�����s������
    //==============================================================
    private void FirstClearFunc()
    {
        if (firstClearFlg)
        {
            // �S�[���̈ʒu���擾
            goalPos = GameObject.Find("Player").transform.position;
            goalPos.z += 0.4f;

            // �J�����̈ړ���̍��W���Z�b�g
            cameraTargetPos = goalPos + cameraAdjustmentPos;

            mainCamera.transform.DOLocalMove(cameraTargetPos, 1.0f);

            // �S�[���̈ʒu�����_���ɃZ�b�g
            for (int i = 0; i < 3; i++) medal[i].transform.position = goalPos;

            firstClearFlg = false;
        }
    }

    //==============================================================
    // �N���A���UI�̃A�j���[�V����
    //==============================================================
    private void ClearUiAnimation()
    {
        // �X�e�[�W�N���A���Ă���X�e�[�W�N���A�̃e�L�X�g���o��܂ł̎��Ԃ��O
        if (stageClearTextCnt == 0)
        {
            stageClearAnim.SetBool("isMove", true);
        }
        else stageClearTextCnt--;

        // �X�e�[�W�N���A���Ă���NextStage��StageSelect���o��܂ł̎��Ԃ��O

        if (clearUiCnt == 0)
        {
            nextStageAnim.SetBool("isMove", true);
            stageSelectAnim.SetBool("isMove", true);
        }
        else clearUiCnt--;

        // �V����]����܂ł̎��Ԃ��O
        if (clearUiRotCnt == 0)
        {
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0")))
            {
                nextStageAnim.SetBool("isRot", true);
                stageSelectAnim.SetBool("isRot", true);
                operationFlg = true;
            }
        }
        else clearUiRotCnt--;

        if (operationFlg)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 pos = new Vector3(0.0f, -0.1f, 0.0f);
                medal[i].transform.position += pos;
            }
        }
    }

    //==============================================================
    // ���_�����o�Ă���A�j���[�V����
    //==============================================================
    private void MedalAnimation()
    {
        // �X�e�[�W�N���A���Ă���ŏ��̃��_�����o��܂ł̎��Ԃ��O
        if (medalAnimStartCnt == 0)
        {
            // ���̎��Ԃ��o��܂ł̎��Ԃ��O
            if (medalInterval == 0)
            {
                if (medalNum < 3)
                {
                    // �A�j���[�V�����J�n
                    if (StageSelectManager.score[StageManager.stageNum].isCopper && medalNum == 0)
                    {
                        medalAnim[medalNum].SetBool("isMove", true);
                    }
                    if (silverFlg && medalNum == 1)
                    {
                        medalAnim[medalNum].SetBool("isMove", true);
                    }
                    if (goldFlg && medalNum == 2)
                    {
                        medalAnim[medalNum].SetBool("isMove", true);
                    }
                    medalInterval = medalIntervalCnt;
                    medalNum++;
                }
            }
            else medalInterval--;
        }
        else medalAnimStartCnt--;
    }

    //==============================================================
    // �X�e�[�W���̑S�Ẵ��_�����擾����ƃ��_������]����
    //==============================================================
    private void MedalRotation()
    {
        // �X�e�[�W�N���A���Ă��烁�_������]����܂ł̎��Ԃ��O
        if (medalRotationStartCnt == 0)
        {
            // �S�Ẵ��_�����擾���Ă���Ɖ�]����
            if (StageSelectManager.score[StageManager.stageNum].isCopper &&
                StageSelectManager.score[StageManager.stageNum].isSilver &&
                StageSelectManager.score[StageManager.stageNum].isGold)
            {
                for (int i = 0; i < 3; i++) medal[i].GetComponent<GearRotation>().SetRotFlg(true);
            }
        }
        else medalRotationStartCnt--;
    }

    public bool GetOperationFlg()
    {
        return operationFlg;
    }

    public void StartFlgOn()
    {
        startFlg = true;
    }

    public void EndFlgOn()
    {
        endFlg = true;
    }

    public void SilverFlgOn()
    {
        silverFlg = true;
    }

    public void GoldFlgOn()
    {
        goldFlg = true;
    }
}