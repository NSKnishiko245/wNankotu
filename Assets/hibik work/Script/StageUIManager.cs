using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageUIManager : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject stageManager;
    [SerializeField] private GameObject stageUIManager;
    [SerializeField] private GameObject editCanvas;

    [SerializeField] private GameObject selectGear;
    [SerializeField] private GameObject retryGear;

    [SerializeField] private int stageDisplayCntInit;   // �X�e�[�W��\������܂ł̎���
    private int stageDisplayCnt;
    [SerializeField] private bool editFlg = false;  // true:�G�f�B�b�g�\��

    private bool menuFlg = false;                �@ // true:���j���[�\����
    private bool menuFirstFlg = false;              // true:���j���[���J�����u��

    [SerializeField] private AudioSource StageBgmSource;

    // �I�𒆂̍���
    private enum STATUS
    {
        STAGE_SELECT,
        RETRY,
    }
    private STATUS status;

    private void Awake()
    {
        if (!editFlg) editCanvas.SetActive(false);

        // �X�e�[�W��\������܂ł̎��Ԃ��Z�b�g
        stageDisplayCnt = stageDisplayCntInit;
        // �X�e�[�W���\��
        player.SetActive(false);
        stageManager.SetActive(false);
    }

    private void Update()
    {
        if (stageDisplayCnt == 0)
        {
            // �X�e�[�W��\��
            player.SetActive(true);
            stageManager.SetActive(true);
        }
        else stageDisplayCnt--;

        if (stageManager.GetComponent<StageManager>().IsGameClear || Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("clear");
            SilverMedalConditions();
            stageUIManager.GetComponent<ClearUI>().ClearFlgOn();
        }

        // ���j���[�\����
        if (menuFlg)
        {
            MenuOperation(); // ���j���[��ʂ̑���

            // �X�e�[�W���\��
            player.SetActive(false);
            stageManager.SetActive(false);

            // ���j���[���\���ɂ���
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 3"))
            {
                menuFlg = false;
                // �y�[�W��߂�
                eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();
                // �X�e�[�W��\������܂ł̎��Ԃ��Z�b�g
                stageDisplayCnt = stageDisplayCntInit;
            }
        }
        // ���j���[��\����
        else
        {
            // ���j���[��\������
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 3"))
            {
                // �N���A��͕\�����Ȃ�
                if (!stageUIManager.GetComponent<ClearUI>().GetCLearFlg())
                {
                    menuFlg = true;
                    menuFirstFlg = true;
                    // �y�[�W��i�߂�
                    eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();
                }
            }
        }
    }

    // ���j���[��ʂ̑���
    private void MenuOperation()
    {
        // ���j���[���J��������̏���
        if (menuFirstFlg)
        {
            status = STATUS.STAGE_SELECT;
            menuFirstFlg = false;
        }

        // �I��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            status = STATUS.STAGE_SELECT;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            status = STATUS.RETRY;
        }

        // �X�e�[�W�Z���N�g�I��
        if (status == STATUS.STAGE_SELECT)
        {
            // ���ԉ�]
            selectGear.GetComponent<GearRotation>().SetRotFlg(true);
            retryGear.GetComponent<GearRotation>().SetRotFlg(false);

            // ����ŃV�[���J��
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
            {
                SceneManager.LoadScene("SelectScene");
            }
        }
        // ���g���C�I��
        if (status == STATUS.RETRY)
        {
            // ���ԉ�]
            selectGear.GetComponent<GearRotation>().SetRotFlg(false);
            retryGear.GetComponent<GearRotation>().SetRotFlg(true);

            // ����ŃV�[���J��
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
            {
                // ���݂�Scene�����擾����
                Scene loadScene = SceneManager.GetActiveScene();
                // Scene�̓ǂݒ���
                SceneManager.LoadScene(loadScene.name);
            }
        }
    }

    // �⃁�_���擾
    private void SilverMedalConditions()
    {
        if (StageSelectManager.silverConditions[StageManager.stageNum] >=
            stageManager.GetComponent<StageManager>().rotateNum)
        {
            StageSelectManager.score[StageManager.stageNum].isSilver = true;
            Debug.Log("�⃁�_���擾�̃m���}" + StageSelectManager.silverConditions[1]);
            Debug.Log("�X�e�[�W��܂�����" + stageManager.GetComponent<StageManager>().rotateNum);
        }
    }
}