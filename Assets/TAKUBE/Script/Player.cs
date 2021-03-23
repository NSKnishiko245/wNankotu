using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�v���C���[�̈ړ����x")]
    public float Speed;
    [Header("�v���C���[�̃W�����v��")]
    public float Jump;

    private int m_hitOriNum = 0;

    private Rigidbody rb;
    private Vector3 pos;
    private bool Jumpflg;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //�L�[�{�[�h����
        float x = Input.GetAxis("Horizontal");
        //�ړ�����
        if (x>0)
        {
            transform.position += transform.right * Speed * Time.deltaTime;
        }
        else if (x<0)
        {
            transform.position -= transform.right * Speed * Time.deltaTime;
        }

        //�W�����v����
        if (Jumpflg)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = transform.up * Jump;
                Jumpflg = false;
            }
        }
        if (rb.velocity.magnitude == 0f)
        {
            Jumpflg = true;
        }


        //�R���g���[���[����
        float lsh = Input.GetAxis("L_Stick_H");
        float lsv = Input.GetAxis("L_Stick_V");
        if(lsh>1)
        {
            transform.position += transform.right * Speed * Time.deltaTime;
        }
        else if(lsh<-1)
        {
            transform.position -= transform.right * Speed * Time.deltaTime;
        }

        //�W�����v�����iA�{�^�������j
        if(Input.GetKeyDown("joystick button 0"))
        {
            if (Jumpflg)
            {
                rb.velocity = transform.up * Jump;
                Jumpflg = false;
            }
        }
        if (rb.velocity.magnitude == 0f)
        {
            Jumpflg = true;
        }

        //LT�ERT�g���K�[�����m�F�p
        float tri = Input.GetAxis("L_R_Trigger");
        if (tri > 0)
        {
           // Debug.Log("L trigger:" + tri);
        }
        else if (tri < 0)
        {
            //Debug.Log("R trigger:" + tri);
        }
        else
        {
            //Debug.Log(" trigger:none");
        }

        //�t�B�[���h�����ɖ߂��iX�{�^�������j�ύX����\����
        if (Input.GetKeyDown("joystick button 2"))
        {
            //Debug.Log("X Button:on");
        }
        else
        {
            //Debug.Log("X Button:none");
        }
        //Debug.Log(m_hitOriNum);
    }

    //�G��Ă���܂�ڂ̃I�u�W�F�N�g�̔ԍ����Z�b�g
    public void SetHitNum(int num)
    {
        m_hitOriNum = num;
    }
   
    public int GetHitOriobjNum()
    {
        return m_hitOriNum;
    }
}
