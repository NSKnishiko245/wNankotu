using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject yes;
    [SerializeField] private GameObject no;
    [SerializeField] private GameObject galiver;

    public static bool menuFlg = false;  // true:�E�C���h�E��\����

    // �E�C���h�E�̏��
    enum STATUS
    {
        ESCAPE,
        DATA_DELETE,
    }
    private STATUS status;

    // �I�����
    enum SELECT
    {
        YES,
        NO,
    }
    private SELECT select;

    private float selectSize = 4.5f;    // �I������UI�̃T�C�Y
    private float unselectSize = 4.0f;  // ��I������UI�̃T�C�Y


    //==============================================================
    // ��������
    //==============================================================
    private void Awake()
    {
        panel.SetActive(false);
    }

    //==============================================================
    // �X�V����
    //==============================================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            status = STATUS.ESCAPE;
            text.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Finish/FinishText");

            // �E�C���h�E�\���ؑ�
            ChangeMenuDisplay();
        }

        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            if (!menuFlg && Input.GetKeyDown(KeyCode.K))
            {
                status = STATUS.DATA_DELETE;
                text.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Finish/DeleteText");

                // �E�C���h�E�\���ؑ�
                ChangeMenuDisplay();
            }
        }

        if (menuFlg)
        {
            // �E�C���h�E���̑���
            MenuOperetion();
        }
    }

    //==============================================================
    // �E�C���h�E�\���ؑ�
    //==============================================================
    private void ChangeMenuDisplay()
    {
        if (!menuFlg)
        {
            // �E�C���h�E�\��
            panel.SetActive(true);
            menuFlg = true;
            select = SELECT.NO;
        }
        else
        {
            // �E�C���h�E��\��
            panel.SetActive(false);
            menuFlg = false;
        }
    }

    //==============================================================
    // �E�C���h�E���̑���
    //==============================================================
    private void MenuOperetion()
    {
        // ���L�[������YES��I��
        if (select != SELECT.YES && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            select = SELECT.YES;
        }

        // �E�L�[������NO��I��
        if (select != SELECT.NO && Input.GetKeyDown(KeyCode.RightArrow))
        {
            select = SELECT.NO;
        }

        // YES�I����
        if (select == SELECT.YES)
        {
            // UI�̃T�C�Y�ύX
            yes.transform.localScale = new Vector3(selectSize, selectSize, 1);
            no.transform.localScale = new Vector3(unselectSize, unselectSize, 1);

            // UI�̉摜�ύX
            yes.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/finish/yes_on");
            no.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/finish/no_off");

            // Z�L�[����Ō���
            if (Input.GetKeyUp(KeyCode.Z))
            {
                // �I����ʂ̏���
                if (status == STATUS.ESCAPE)
                {
                    // �Q�[���I��
                    Application.Quit();
                }

                // �f�[�^�폜��ʂ̏���
                else if (status == STATUS.DATA_DELETE)
                {
                    menuFlg = false;
                    StageSelectManager.DeleteSaveDate();
                    Destroy(galiver.gameObject);
                    SceneManager.LoadScene("Op");
                }
            }
        }

        // NO�I����
        else if (select == SELECT.NO)
        {
            // UI�̃T�C�Y�ύX
            yes.transform.localScale = new Vector3(unselectSize, unselectSize, 1);
            no.transform.localScale = new Vector3(selectSize, selectSize, 1);

            // UI�̉摜�ύX
            yes.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/finish/yes_off");
            no.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/finish/no_on");

            // Z�L�[����Ō���
            if (Input.GetKeyUp(KeyCode.Z))
            {
                // �E�C���h�E�\���ؑ�
                ChangeMenuDisplay();
            }
        }
    }
}