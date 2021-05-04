using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnimation : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject stageUIManager;

    [SerializeField] private GameObject stageClear;
    [SerializeField] private GameObject stageSelect;
    [SerializeField] private GameObject nextStage;

    private Animator stageClearAnim;
    private Animator stageSelectAnim;
    private Animator nextStageAnim;

    [SerializeField] private GameObject[] medal = new GameObject[3];
    private Animator[] medalAnim = new Animator[3];

    // �N���A��̃J�����̈ʒu�����p(�S�[������̍����Z�b�g����)
    [SerializeField] private Vector3 cameraAdjustmentPos;
    private Vector3 cameraNowPos;       // �J�����̌��ݍ��W
    private Vector3 goalPos;            // �S�[���̍��W
    private Vector3 cameraTargetPos;    // �J�����̈ړ���̍��W
    private Vector3 cameraMovePos;      // �J�������P�t���[���Ɉړ�������W
    [SerializeField] private int cameraMoveSpeed; // �J�������ړ����鑬�x
    private int cameraMoveCnt;          // �J�������ړ������

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



    //==============================================================
    // ��������
    //==============================================================
    private void Start()
    {
        stageClearAnim = stageClear.GetComponent<Animator>();
        stageSelectAnim = stageSelect.GetComponent<Animator>();
        nextStageAnim = nextStage.GetComponent<Animator>();

        for (int i = 0; i < 3; i++) medalAnim[i] = medal[i].GetComponent<Animator>();

        cameraNowPos = mainCamera.transform.position;
        cameraMoveCnt = cameraMoveSpeed;

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
        if (stageUIManager.GetComponent<ClearUI>().GetCLearFlg())
        {
            // �J�����̈ʒu�����X�ɕύX
            if (cameraMoveCnt != 0)
            {
                cameraNowPos -= cameraMovePos;
                mainCamera.transform.position = cameraNowPos;
                cameraMoveCnt--;
            }

            // �N���A���1�x�����s������
            FirstClearFunc();

            // �N���A���UI�̃A�j���[�V����
            ClearUiAnimation();

            // ���_�����o�Ă���A�j���[�V����
            MedalAnimation();

            // �X�e�[�W���̑S�Ẵ��_�����擾����ƃ��_������]����
            MedalRotation();
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
            goalPos = GameObject.Find("2(Clone)").transform.position;

            // �J�����̈ړ���̍��W���Z�b�g
            cameraTargetPos = goalPos + cameraAdjustmentPos;

            // �J�������P�t���[���Ɉړ�������W���Z�b�g
            cameraMovePos = (cameraNowPos - cameraTargetPos) / cameraMoveSpeed;

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
            nextStageAnim.SetBool("isRot", true);
            stageSelectAnim.SetBool("isRot", true);
        }
        else clearUiRotCnt--;
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
                    if (StageSelectManager.score[StageManager.stageNum].isSilver && medalNum == 1)
                    {
                        medalAnim[medalNum].SetBool("isMove", true);
                    }
                    if (StageSelectManager.score[StageManager.stageNum].isGold && medalNum == 2)
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
}