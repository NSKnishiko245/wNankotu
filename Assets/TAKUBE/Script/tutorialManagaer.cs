using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class tutorialManagaer : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private GameObject Bar;
    [SerializeField] private StageManager stagemanager;


    public Text Text;

    private int Animnum = 1;
    private int TutorialNum = 1;
    private int RotateNum = 1;
    private int BordNum;
    private int Stagenum;   // �X�e�[�W�ԍ�



    private GameObject Point_prefab;
    private GameObject Point;
    private GameObject mainCam;
    private GameObject tv;

    private bool PushFlg;
    private bool textFlg;
    //�J�E���g�_�E��
    [SerializeField] public float Pointcountdown = 5.0f;
    [SerializeField] public float Oricountdown = 1.0f;
    [SerializeField] public float TextCount = 5.0f;
    [SerializeField] public float Pushcount;
    [SerializeField] public float Startcount;
    private bool CountFlg;
    private bool StopFlg;

    [SerializeField] private GameObject Lstick;
    [SerializeField] private GameObject Rstick;
    [SerializeField] private GameObject Controller;
    [SerializeField] private GameObject TutorialUI;
    [SerializeField] private GameObject Mobiusface;
    [SerializeField] private GameObject MobiusBody;
    [SerializeField] private GameObject Bord;
    [SerializeField] private GameObject Black;
    [SerializeField] private GameObject Stop;
    [SerializeField] private GameObject StartImage;
    [SerializeField] private GameObject KeyBord;

    Animator LAnim;
    Animator RAnim;
    Animator ControllerAnim;
    Animator MobiusfaceAnim;
    Animator MobiusBodyAnim;
    Animator BordAnim;
    Animator BlackAnim;
    Animator KeyBordAnim;

    public bool IsPlayerMove { get; private set; }

    public bool IsPlayerLMove { get; private set; }

    public bool IsRotateMove { get; private set; }

    public bool IsLMove { get; private set; }

    public bool IsRMove { get; private set; }

    public bool IsPoint { get; private set; }

    private void Awake()
    {
        tv = GameObject.Find("tv");
        tv.transform.localScale = new Vector3(0, 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        BordNum = 5;
        LAnim = Lstick.GetComponent<Animator>();
        RAnim = Rstick.GetComponent<Animator>();
        ControllerAnim = Controller.GetComponent<Animator>();
        MobiusfaceAnim = Mobiusface.GetComponent<Animator>();
        MobiusBodyAnim = MobiusBody.GetComponent<Animator>();
        BordAnim = Bord.GetComponent<Animator>();
        BlackAnim = Black.GetComponent<Animator>();
        KeyBordAnim = KeyBord.GetComponent<Animator>();
        

        

        //stagemanager = GameObject.Find("stageManager");
        IsPlayerMove = true;
        IsPlayerLMove = true;
        IsRotateMove = true;
        IsLMove = true;
        IsRMove = true;
        IsPoint = false;
        CountFlg = true;
        PushFlg = false;
        textFlg = false;
        StopFlg = false;
        mainCam = Camera.main.gameObject;
        Stagenum = StageManager.stageNum;


        if (Stagenum != 1)
        {
            TutorialNum = 10;
            BordNum = 2;
            BordAnim.SetInteger("text", BordNum);
        }
        else
        {
            TutorialNum = 1;
        }
        Point_prefab = Resources.Load<GameObject>("Point");
    }

    // Update is called once per frame
    void Update()
    {
        if (!FinishManager.menuFlg)
        {
            //if(ControllerCheck()==true)
            //{
            //    TutorialUI.SetActive(false);
            //}

            //Controller.SetActive(false);

            //�X�e�[�W�J�n���̃J�������[�N���́A���삵�Ȃ�
            if (mainCam.GetComponent<StartCamera>().isMoving)
            {
                Stop.SetActive(false);
                StartImage.SetActive(false);
                textFlg = true;
                
                return;
            }

            if (IsPlayerMove)
            {
                Stop.SetActive(false);
                if (StopFlg)
                {
                    //StartImage.SetActive(true);
                    Startcount -= Time.deltaTime;
                    if (Startcount <= 0)
                    {
                        //StartImage.SetActive(false);
                        Startcount = 1.0f;
                        StopFlg = false;
                    }
                }
            }
            else
            {
                Stop.SetActive(true);
                StartImage.SetActive(false);
                StopFlg = true;
            }

            //���Ԃ��J�E���g�_�E������
            TutorialUI.GetComponent<RectTransform>().SetAsLastSibling();

            switch (TutorialNum)
            {
                //���@�̈ړ����@
                case 1:
                    IsRotateMove = false;
                    IsLMove = false;
                    IsRMove = false;
                    //���@�Y�[���I�����TV�E�e�L�X�g�̃A�j���[�V�����J�n��TextCount�̎��Ԃ�����������
                    if (textFlg)
                    {
                        tv.transform.DOScale(new Vector3(4000, 4000, 1100), 0.5f);
                        BlackAnim.SetBool("Black", true);
                        BordNum = 3;
                        BordAnim.SetInteger("text", BordNum);
                        TextCount -= Time.deltaTime;
                    }
                    else { Pointcountdown -= Time.deltaTime; }          //textFlg��false�ɂȂ�����Pointcountdown�̎��Ԃ�����������


                    //TextCount���O�ɂȂ�����e�L�X�g�̐؂�ւ���textFlg��false�ɂ���
                    if (TextCount <= 0.0f)
                    {
                        Text.text = "�܂��͂��̐��E�̐i�ߕ���\n����������r�E�X��I";
                        TextCount = 1000.0f;
                        textFlg = false;
                    }
                    else { IsPlayerMove = false; }                      //���@�̈ړ����֎~�ɂ���


                    //Pointcountdown��0�ɂȂ����玩�@�̈ړ������ւ��Ė����o�������e�L�X�g��ς���
                    if (Pointcountdown <= 0.0f)
                    {
                        IsPlayerMove = true;
                        MobiusBodyAnim.SetBool("Right", true);
                        MobiusfaceAnim.SetBool("Smile", true);
                        ControllerAnim.SetBool("CFlg", true);
                        KeyBordAnim.SetBool("KeyBordFlg", true);
                        LAnim.SetBool("LStick", true);
                        RAnim.SetBool("RStickLMove", false);
                        if (CountFlg)
                        {
                            CountFlg = false;
                            Point = Instantiate(Point_prefab, new Vector3(1.5f, -2.5f, 0.0f), Quaternion.identity);
                            Text.text = "�܂��͖��Ɍ�������\n�����Ă݂�r�E�X�I";
                            //MobiusBodyAnim.SetBool("Right", true);
                            //MobiusfaceAnim.SetBool("Smile", true);
                            //ControllerAnim.SetBool("CFlg", true);
                            //KeyBordAnim.SetBool("KeyBordFlg", true);
                            //LAnim.SetBool("LStick", true);
                            //RAnim.SetBool("RStickLMove", false);
                        }
                    }


                    //���@�����ɓ�����Ζ��������e�L�X�g��ύX����case 2�֍s��
                    if (Player.GetComponent<Player>().IsHitPoint)
                    {
                        Text.text = "�����i���ɑj�܂��\n�i�ނ��Ƃ��o���Ȃ��r�E�X";
                        ControllerAnim.SetBool("CFlg", false);
                        KeyBordAnim.SetBool("KeyBordFlg", false);
                        LAnim.SetBool("LStick", false);
                        IsPoint = true;
                        TutorialNum = 2;
                        TextCount = 2.0f;                            //������TextCount��ݒ肵�Ď��̖�󂪏o������܂ł̃C���^�[�o��������Ă���
                        Destroy(Point);
                    }
                    break;

                //�X�e�[�W��܂点��
                case 2:
                    //RotateNum =>> 1�F���@����ɗU��       2�F�X�e�[�W���E�ɐ܂点��

                    IsPoint = false;
                    if (RotateNum == 1)
                    {
                        TextCount -= Time.deltaTime;
                        IsPlayerMove = true;

                        //TextCount��0�ɂȂ�܂Ńv���C���[�����R�Ɉړ�������
                        if (TextCount <= 0.0f)
                        {
                            //BordNum = 1;
                            //BordAnim.SetInteger("text", BordNum);
                            TextCount = 1000.0f;
                            CountFlg = true;
                        }

                        //�����o��������
                        if (CountFlg)
                        {
                            CountFlg = false;
                            Point = Instantiate(Point_prefab, new Vector3(0.17f, -2.5f, 0.0f), Quaternion.identity);

                        }

                        //�o�[�ɓ���������A�A
                        if (Player.GetComponent<Player>().IsHitBar)
                        {
                            BordNum = 0;
                            BordAnim.SetInteger("text", BordNum);
                            Text.text = "�t�B�[���h��܂���\n���E��ς���r�E�X�I�I";
                            IsPlayerMove = false;
                            IsPoint = true;
                            Destroy(Point);
                            RotateNum = 2;
                            TextCount = 10.0f;
                        }
                    }

                    if (RotateNum == 2)
                    {
                        IsLMove = false;
                        IsRMove = true;
                        ControllerAnim.SetBool("CFlg", true);
                        KeyBordAnim.SetBool("KeyBordDFlg", true);
                        LAnim.SetBool("LStick", false);
                        RAnim.SetBool("RStickRMove", true);
                        if (stagemanager.GetComponent<StageManager>().IsRotate)
                        {
                            Text.text = "�X�e�[�W��܂ꂽ�r�E�X�I\n���\�̖������E�ɓ˓��r�E�X�I";
                            Pointcountdown = 1.5f;
                            IsRMove = false;
                            TutorialNum = 3;
                            MobiusBodyAnim.SetBool("Both", true);
                            RAnim.SetBool("RStickRMove", false);
                            ControllerAnim.SetBool("CFlg", false);
                            KeyBordAnim.SetBool("KeyBordDFlg", false);
                        }

                    }

                    //if (RotateNum == 3)
                    //{
                    //    Debug.Log(RotateNum);
                    //    IsPlayerMove = true;
                    //    IsRotateMove = false;
                    //    Pushcount -= Time.deltaTime;
                    //    if (Pushcount <= 0.0f)
                    //    {
                    //        PushFlg = true;
                    //        ControllerAnim.SetBool("CFlg", true);
                    //        LAnim.SetBool("LStick", false);
                    //        RAnim.SetBool("RStickPush", true);
                    //        CountFlg = true;
                    //        TutorialNum = 3;
                    //        Pushcount = 9999.0f;

                    //    }

                    //LAnim.SetBool("LStick", false);
                    //RAnim.SetBool("RStickPush", true);
                    //    if (PushFlg)
                    //    {
                    //        if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.K))
                    //        {
                    //            PushFlg = false;
                    //            BordNum = 1;
                    //            BordAnim.SetInteger("text", BordNum);
                    //            Point = Instantiate(Point_prefab, new Vector3(0.17f, -2.5f, 0.0f), Quaternion.identity);
                    //            ControllerAnim.SetBool("CFlg", false);
                    //            RAnim.SetBool("RStickPush", false);
                    //        }
                    //    }

                    //    if (Player.GetComponent<Player>().IsHitPoint)
                    //    {
                    //        IsPlayerMove = false;
                    //        IsPoint = true;
                    //        Destroy(Point);
                    //        RotateNum = 4;
                    //    }
                    //}

                    //if (RotateNum == 4)
                    //{
                    //    Debug.Log(RotateNum);

                    //    IsPlayerMove = false;
                    //    IsRMove = true;
                    //    ControllerAnim.SetBool("CFlg", true);
                    //    LAnim.SetBool("LStick", false);
                    //    RAnim.SetBool("RStickPush", false);
                    //    RAnim.SetBool("RStickRMove", true);
                    //    if (stagemanager.GetComponent<StageManager>().IsRotate)
                    //    {
                    //        IsRotateMove = false;
                    //        CountFlg = true;
                    //        TutorialNum = 3;
                    //        ControllerAnim.SetBool("CFlg", false);
                    //        LAnim.SetBool("LStick", false);
                    //        RAnim.SetBool("RStickRMove", false);
                    //    }

                    // }
                    break;

                //�X�e�[�W�̖߂���
                case 3:
                    Debug.Log(IsRotateMove);
                    IsPoint = false;
                    IsPlayerMove = true;

                    Pushcount -= Time.deltaTime;
                    if (Pushcount <= 0.0f)
                    {
                        CountFlg = true;
                        Pushcount = 9999.0f;

                    }

                    if (CountFlg)
                    {

                        CountFlg = false;

                        Point = Instantiate(Point_prefab, new Vector3(4.0f, -2.5f, 0.0f), Quaternion.identity);

                    }

                    LAnim.SetBool("LStick", false);
                    RAnim.SetBool("RStickPush", true);

                    {
                        if (Player.GetComponent<Player>().IsHitPoint)
                        {
                            Text.text = "���̃X�e�[�W�͂��ł��������\�r�E�X�I";
                            IsRotateMove = true;
                            IsPoint = true;
                            PushFlg = true;
                            BordNum = 0;
                            BordAnim.SetInteger("text", BordNum);
                            //ControllerAnim.SetBool("CFlg", true);
                            //KeyBordAnim.SetBool("KeyBordSFlg", true);
                            //RAnim.SetBool("RStickPush", true);
                            Destroy(Point);
                        }
                    }

                    if (PushFlg)
                    {
                        ControllerAnim.SetBool("CFlg", true);
                        KeyBordAnim.SetBool("KeyBordSFlg", true);
                        RAnim.SetBool("RStickPush", true);
                        if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.S))
                        {
                            BordNum = 1;
                            BordAnim.SetInteger("text", BordNum);
                            PushFlg = false;
                            TutorialNum = 4;
                            CountFlg = true;
                            ControllerAnim.SetBool("CFlg", false);
                            KeyBordAnim.SetBool("KeyBordSFlg", false);
                        }
                    }
                    break;

                //�S�[���֍s��
                case 4:
                    IsRotateMove = true;
                    IsLMove = true;
                    IsRMove = true;
                    IsPoint = false;
                    IsPlayerMove = true;
                    if (CountFlg)
                    {
                        CountFlg = false;
                        Point = Instantiate(Point_prefab, new Vector3(5.5f, -2.7f, 0.0f), Quaternion.identity);
                    }

                    if (Player.GetComponent<Player>().IsHitPoint)
                    {
                        BordNum = 0;
                        BordAnim.SetInteger("text", BordNum);
                        Text.text = "���̐��E�͉��ɏ㉺���]���E��\n�����ɑ����Ă���r�E�X�I";
                        IsPlayerMove = false;
                        IsPoint = true;
                        TutorialNum = 5;
                        TextCount = 5.0f;
                        Destroy(Point);
                    }
                    break;

                case 5:
                    TextCount -= Time.deltaTime;
                    if (TextCount <= 0.0f)
                    {
                        Text.text = "凋C�O�̒��Ɏ��ۂɓ����Đi�ރr�E�X�I";
                        IsPlayerMove = true;
                        Point = Instantiate(Point_prefab, new Vector3(5.5f, -1.5f, 0.0f), Quaternion.Euler(0, 0, 90));
                        TextCount = 1000.0f;
                    }

                    if (stagemanager.GetComponent<StageManager>().IsSmog)
                    {
                        Destroy(Point);
                        TutorialNum = 6;
                        TextCount = 10.0f;
                        Pointcountdown = 5.0f;
                        IsPlayerMove = false;
                    }

                    break;
                case 6:

                    Text.text = "�܂�߂���Ƌ�̎��Ԃ�...�I\n�f�����S�[����ڎw���Ńr�E�X�I";

                    TextCount -= Time.deltaTime;
                    Pointcountdown -= Time.deltaTime;
                    if(Pointcountdown<=0.0f)
                    {
                        Text.text = "�����͂���ŏI���r�E�X\n��ꂽ���Ԃ��~�����߂Ɋ撣��Ńr�E�X�I�I";
                    }
                    if (TextCount <= 0.0f)
                    {
                        IsPlayerMove = true;
                        tv.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
                        BordNum = 1;
                        BordAnim.SetInteger("text", BordNum);
                    }


                    break;
                default:

                    break;

            }
        }
    }



    public bool ControllerCheck()
    {
        // �ڑ�����Ă���R���g���[���̖��O�𒲂ׂ�
        var controllerNames = Input.GetJoystickNames();

        Debug.Log(controllerNames);
        // �����R���g���[�����ڑ�����Ă��Ȃ���΃G���[
        if (controllerNames[0] == "")
        {
            Debug.Log("Error");
            return true;
        }

        return false;
    }



    private bool OriCheck()
    {
        // �E�X�e�B�b�N����̓��͏����擾
        float R_Stick_Value = Input.GetAxis("Horizontal2");

        if (0 < R_Stick_Value || 0 > R_Stick_Value)
        {

            Animnum = 3;
            return true;
        }
        return false;
    }

    private bool ReturnOri()
    {
        if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.K))
        {

            return true;
        }
        return false;
    }
}