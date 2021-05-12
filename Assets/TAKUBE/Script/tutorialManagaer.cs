using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialManagaer : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private GameObject Bar;
    private int Animnum = 1;

    //�J�E���g�_�E��
    [SerializeField] public float movecountdown = 5.0f;
    [SerializeField] public float Oricountdown = 1.0f;

    [SerializeField] private GameObject Lstick;
    [SerializeField] private GameObject Rstick;
    [SerializeField] private GameObject Controller;

    Animator LAnim;
    Animator RAnim;
    Animator ControllerAnim;

    // Start is called before the first frame update
    void Start()
    {
        LAnim = Lstick.GetComponent<Animator>();
        RAnim = Rstick.GetComponent<Animator>();
        ControllerAnim = Controller.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԃ��J�E���g�_�E������
        Controller.GetComponent<RectTransform>().SetAsLastSibling();


        // ���X�e�B�b�N����̓��͏����擾
        float input_x = Input.GetAxis("Horizontal");

        // �E�X�e�B�b�N����̓��͏����擾
        float R_Stick_Value = Input.GetAxis("Horizontal2");

        //LAnim.SetInteger("tutorialNum", Animnum);
        //RAnim.SetInteger("RtutorialNum", Animnum);


        if (input_x==0)
        {
            
            movecountdown -= Time.deltaTime;
            if (movecountdown <= 0.0f)
            {
                
                Debug.Log("�ړ�UI�\��");
                ControllerAnim.SetBool("FukidasiFlg", true);
                LAnim.SetInteger("tutorialNum", Animnum);
                RAnim.SetInteger("RtutorialNum", Animnum);
                movecountdown = 5.0f;
            }
        }
        else if(Animnum!=5)
        {
            movecountdown = 5.0f;
            ControllerAnim.SetBool("FukidasiFlg", false);
        }
        else
        {
            movecountdown = 5.0f;
            

        }

        if (Player.GetComponent<Player>().IsHitBar)
        {
            
            Oricountdown -= Time.deltaTime;
            if (Oricountdown <= 0)
            {
                Animnum = 2;
                ControllerAnim.SetBool("FukidasiFlg", true);
                Debug.Log("UI�\��");
                LAnim.SetInteger("tutorialNum", Animnum);
                RAnim.SetInteger("RtutorialNum", Animnum);
                Oricountdown = 1.0f;
            }

            if(0 < R_Stick_Value || 0 > R_Stick_Value)
            {
                Animnum = 5;
                ControllerAnim.SetBool("FukidasiFlg", true);
                LAnim.SetInteger("tutorialNum", Animnum);
                RAnim.SetInteger("RtutorialNum", Animnum);
            }
        }
        else
        {
            Oricountdown = 1.0f;
        }


        //if(Player.GetComponent<Player>().IsHitBar&& 0 < R_Stick_Value || 0 > R_Stick_Value)
        //{
        //    Animnum = 5;
        //    ControllerAnim.SetBool("FukidasiFlg", true);
        //    LAnim.SetInteger("tutorialNum",Animnum);
        //    RAnim.SetInteger("RtutorialNum", Animnum);
        //}

        if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.K))
        {
            ControllerAnim.SetBool("FukidasiFlg", false);
        }

        //Debug.Log(Animnum);
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
