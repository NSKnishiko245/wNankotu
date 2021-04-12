using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageUIManager : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject stageManager;
    [SerializeField] private GameObject stageUIManager;
    [SerializeField] private GameObject editCanvas;

    [SerializeField] private GameObject selectGear;
    [SerializeField] private GameObject retryGear;

    [SerializeField] private int stageNum;          // �X�e�[�W�ԍ�
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
    }

    private void Update()
    {
        if (stageManager.GetComponent<StageManager>().IsGameClear)
        {
            stageUIManager.GetComponent<ClearUI>().ClearFlgOn();
        }

        // ����ŃV�[���J��
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown("joystick button 6"))
        {
            SceneManager.LoadScene("SelectScene");
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 7"))
        {
            // ���݂�Scene�����擾����
            Scene loadScene = SceneManager.GetActiveScene();
            // Scene�̓ǂݒ���
            SceneManager.LoadScene(loadScene.name);
        }

        // ���j���[�\����
        if (menuFlg)
        {
            //stageManager.SetActive(false);
            MenuOperation(); // ���j���[��ʂ̑���

            // ���j���[���\���ɂ���
            if (Input.GetKeyDown(KeyCode.M))
            {
                menuFlg = false;
                // �y�[�W��߂�
                eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();
                stageManager.SetActive(true);

            }
        }
        // ���j���[��\����
        else
        {
            // ���j���[��\������
            if (Input.GetKeyDown(KeyCode.M))
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

    // �X�e�[�W�ԍ��擾
    public int GetStageNum()
    {
        return stageNum;
    }
}
