using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BookSelect : MonoBehaviour
{
    private GameObject eventSystem;
    private Animator[] bookAnim;
    private Animator[] bookLAnim;
    private GameObject[] book;
    private GameObject[] bookBack;
    private Animator cameraAnim;

    private int bookMax = 6;    // �{�̍ő吔
    private int stageMax = 43;  // �X�e�[�W�̍ő吔

    public static int bookNum = 0;    // �{�̔ԍ�
    private int bookSelectCntInit = 30;
    private int bookSelectCnt = 0;
    private bool bookSelectFlg = false;
    private int bookRemoveCnt = 60;

    // �T�E���h
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private AudioSource DecSource;


    [SerializeField] private int sceneChangeCntInit; // �V�[���J�ڂ܂ł̎���
    private int sceneChangeCnt = 0;                 // �V�[���J�ڂ̃J�E���^
    private bool sceneChangeFlg = false;

    [SerializeField] private int pageIntervalInit;  // �y�[�W���߂����܂ł̑ҋ@���Ԃ̏����l
    private int pageInterval = 0;

    [SerializeField] private int operationCntInit;  // �V�[���J�ڂ��đ���\�ɂȂ�܂ł̎���
    private int operationCnt = 0;

    // �V�[���̏��
    private enum STATUS
    {
        BOOK_SELECT,
        STAGE_SELECT,
    }
    private STATUS status;



    //==============================================================
    // ��������
    //==============================================================
    private void Awake()
    {
        sceneChangeCnt = sceneChangeCntInit;
        pageInterval = pageIntervalInit;
        operationCnt = operationCntInit;

        eventSystem = GameObject.Find("EventSystem");
        cameraAnim = GameObject.Find("Main Camera").GetComponent<Animator>();

        bookAnim = new Animator[bookMax];
        bookLAnim = new Animator[bookMax];
        book = new GameObject[bookMax];
        bookBack = new GameObject[bookMax];

        for (int i = 0; i < bookMax; i++)
        {
            bookAnim[i] = GameObject.Find("BookModel" + (i + 1)).GetComponent<Animator>();
            bookLAnim[i] = GameObject.Find("book_L" + (i + 1)).GetComponent<Animator>();
            book[i] = GameObject.Find("BookModel" + (i + 1));
            bookBack[i] = GameObject.Find("book_back" + (i + 1));
        }
        bookAnim[bookNum].SetBool("isUp", true);

        this.GetComponent<PostEffectController>().SetVigFlg(false);
    }

    //==============================================================
    // �X�V����
    //==============================================================
    private void Update()
    {
        switch (status)
        {
            //-----------------------------------
            // �{�I��
            //-----------------------------------
            case STATUS.BOOK_SELECT:

                if (!bookSelectFlg)
                {

                    if (bookSelectCnt == 0)
                    {
                        // ���̖{��I��
                        if (Input.GetAxis("Horizontal") < 0)
                        {
                            if (bookNum > 0)
                            {
                                bookNum--;
                                bookSelectCnt = bookSelectCntInit;
                                // �I�𒆂̖{����ɏオ��
                                bookAnim[bookNum].SetBool("isUp", true);
                                bookAnim[bookNum + 1].SetBool("isUp", false);
                            }
                        }
                        // �E�̖{��I��
                        if (Input.GetAxis("Horizontal") > 0)
                        {
                            if (bookNum < bookMax - 1)
                            {
                                bookNum++;
                                bookSelectCnt = bookSelectCntInit;
                                // �I�𒆂̖{����ɏオ��
                                bookAnim[bookNum].SetBool("isUp", true);
                                bookAnim[bookNum - 1].SetBool("isUp", false);
                            }
                        }
                    }
                    else bookSelectCnt--;

                    // �{������
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                    {
                        // �I�𒆂̖{�����Ɉړ�
                        bookAnim[bookNum].SetBool("isRemove", true);
                        cameraAnim.SetBool("isAnim", true);
                        bookSelectFlg = true;
                    }
                }

                else
                {
                    if (bookRemoveCnt == 0)
                    {
                        bookBack[bookNum].SetActive(false);
                        book[bookNum].transform.localPosition = new Vector3(-0.77f, 50.08f, -50.61f);
                        book[bookNum].transform.localScale = new Vector3(2.215f, 2.115f, 2.8f);
                        book[bookNum].transform.localEulerAngles = new Vector3(88.8f, 180.0f, 0.0f);
                        bookRemoveCnt = -1;
                        book[bookNum].transform.DOLocalMove(new Vector3(-0.77f, -0.08f, 0.61f), 1.0f).OnComplete(() =>
                        {
                            sceneChangeFlg = true;
                        });
                    }
                    else bookRemoveCnt--;
                }

                if (sceneChangeFlg)
                {
                    if (sceneChangeCnt == 0)
                    {
                        SceneManager.LoadScene("StageSelect" + (bookNum + 1));
                    }
                    else sceneChangeCnt--;
                }

                break;

            //-----------------------------------
            // �X�e�[�W�I��
            //-----------------------------------
            case STATUS.STAGE_SELECT:
                if (operationCnt == 1) eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();

                if (operationCnt == 0)
                {
                    if (!eventSystem.GetComponent<IgnoreMouseInputModule>().GetAllBackFlg())
                    {
                        PageOperation();
                    }
                }
                else operationCnt--;

                SceneChange();      // �V�[���J��

                break;

            default:
                break;
        }



    }

    // �y�[�W���߂���
    private void PageOperation()
    {
        if (pageInterval == 0)
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
        else pageInterval--;
    }

    // �V�[���J��
    private void SceneChange()
    {
        if (!sceneChangeFlg && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0")))
        {
            DecSource.Play();

            // ���݂̃y�[�W�擾(�X�e�[�W�ԍ�)
            StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();

            // �V�[���J�ڊJ�n
            if (StageManager.stageNum > 0)
            {
                // �{�����
                eventSystem.GetComponent<IgnoreMouseInputModule>().AllBackPage();
                // �������Â�����
                this.GetComponent<PostEffectController>().SetVigFlg(true);

                sceneChangeFlg = true;
            }
        }

        if (eventSystem.GetComponent<IgnoreMouseInputModule>().GetBookCloseFlg())
        {
            // �J��
            SceneManager.LoadScene("Stage1Scene");
        }
    }
}
