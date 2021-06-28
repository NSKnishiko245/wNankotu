using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static bool bonusFlg;
    private int bookSelectCntInit = 15;
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

        if (bonusFlg) bookMax = 7;
        else GameObject.Find("BookModel7").SetActive(false);

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
                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 0"))
                    {
                        DecSource.Play();
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
                        book[bookNum].transform.DOLocalMove(new Vector3(-0.77f, -0.08f, 0.61f), 0.75f).OnComplete(() =>
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
                        if (bookNum == 6) SceneManager.LoadScene("BonusScene");
                        else SceneManager.LoadScene("StageSelectScene");
                    }
                    else sceneChangeCnt--;
                }
                break;
            default:
                break;
        }
    }
}
