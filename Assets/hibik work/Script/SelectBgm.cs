using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectBgm : MonoBehaviour
{
    // �C���X�^���X�̎���
    private static SelectBgm instance = null;

    // �C���X�^���X�̃v���p�e�B�[�́A���̂����݂��Ȃ��Ƃ��i������Q�Ǝ��j���̂�T���ēo�^����
    public static SelectBgm Instance => instance
        ?? (instance = GameObject.FindWithTag("SelectBgm").GetComponent<SelectBgm>());

    private void Awake()
    {
        // �����C���X�^���X���������݂���Ȃ�A�����j������
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        // �B��̃C���X�^���X�Ȃ�A�V�[���J�ڂ��Ă��c��
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "StageSelectScene" ||
            SceneManager.GetActiveScene().name == "BookSelectScene")
        {
            this.gameObject.GetComponent<AudioSource>().UnPause();
        }
        else
        {
            this.GetComponent<AudioSource>().Pause();
        }
    }

    private void OnDestroy()
    {
        // �j�����ɁA�o�^�������̂̉������s��
        if (this == Instance) instance = null;
    }
}
