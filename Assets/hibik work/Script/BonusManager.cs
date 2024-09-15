using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BonusManager : MonoBehaviour
{
    private GameObject eventSystem;
    [SerializeField] private GameObject bookL;
    private Animator bookLAnim;
    [SerializeField] private GameObject bookBack;
    private GameObject book;
    private GameObject bookUI;
    private Animator cameraAnim;

    private int bookRemoveCnt = 90;

    // �T�E���h
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private AudioSource DecSource;


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

    //==============================================================
    // ��������
    //==============================================================
    private void Awake()
    {
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
        command = COMMAND.EMPTY;

        this.GetComponent<PostEffectController>().SetVigFlg(false);
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
            // �{������Ƃ��͑���s�\�ɂ���
            if (!eventSystem.GetComponent<IgnoreMouseInputModule>().GetAllBackFlg())
            {
                PageOperation(); // �y�[�W���߂��鑀��


                // ���݂̃y�[�W�擾(�X�e�[�W�ԍ�)
                int pageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();
                if (pageNum == 3)
                {
                    if (!FinishManager.menuFlg)
                    {
                        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 0"))
                        {
                            // �J��
                            SceneManager.LoadScene("EndRoll");
                        }
                    }
                }
            }
        }
        else operationCnt--;

        BookSelectChange(); // �{�̑I����ʂւ̑J��
    }

    //==============================================================
    // �y�[�W���߂��鑀��
    //==============================================================
    private void PageOperation()
    {
        if (pageInterval == 0)
        {
            if (!FinishManager.menuFlg)
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
    // �{�̑I����ʂւ̑J��
    //==============================================================
    private void BookSelectChange()
    {
        if (!FinishManager.menuFlg)
        {
            if (command == COMMAND.EMPTY && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown("joystick button 1")))
            {
                DecSource.Play();

                // �{�����
                eventSystem.GetComponent<IgnoreMouseInputModule>().AllBackPage();
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
                    SceneManager.LoadScene("BookSelectScene");
                }
                else sceneChangeCnt--;
            }
        }
    }
}