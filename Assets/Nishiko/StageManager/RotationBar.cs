using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBar : MonoBehaviour
{
    //�v���C���[�ƐG��Ă��邩�H
    private bool m_isPlayerHit=false;

    float angle = 0;

    bool m_isInputSp = false;
    bool m_isInputRight = false;
    bool m_isInputLeft = false;
    bool m_isInputAny = false;


    bool m_LeftStart = false;
    bool m_RightStart = false;
    bool rotStart = false;

    float speed = 1.0f;
    float rotAngle = 180f;
    float variation;
    float rot;
    // Start is called before the first frame update
    void Start()
    {
        variation = rotAngle / speed;
    }

    // Update is called once per frame
    void Update()
    {
        m_isInputSp = false;
        m_isInputRight = false;
        m_isInputLeft = false;

        if (m_isPlayerHit)
        {
            //��]������Ȃ�����
            if (!rotStart)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    m_isInputSp = true;
                }

                if (!m_isInputAny)
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        m_isInputRight = true;
                        m_isInputAny = true;

                        OnBtn();
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        m_isInputLeft = true;
                        m_isInputAny = true;

                        OnBtn();
                    }
                    else
                    {
                        m_isInputAny = false;
                    }
                }
            }
            //�X�y�[�X�����ꂽ���
            if (m_isInputSp)
            {
                if (m_isInputLeft)
                {
                    m_LeftStart = true;
                }
                else if (m_isInputRight)
                {
                    m_RightStart = true;
                }
            }
            //��]
            if (m_LeftStart)
            {
                RotationOriLeft();
            }
            else if (m_RightStart)
            {
                RotationOriRight();
            }
        }
    }

    //�܂�ڂ�180�x��](�܂�ڂ̃I�u�W�F�N�g)
    private void RotationOriLeft()
    {
        if (rotStart)
        {
            this.transform.Rotate(0, variation * Time.deltaTime, 0);
            rot += variation * Time.deltaTime;
            angle += variation * Time.deltaTime;
            if (rot >= rotAngle)
            {
                m_LeftStart = false;
                rotStart = false;
                m_isInputAny = false;
                this.transform.localRotation = Quaternion.Euler(0, angle, 0);
                // ParentReset();
            }
        }
    }

    private void RotationOriRight()
    {
        if (rotStart)
        {
            this.transform.Rotate(0, -variation * Time.deltaTime, 0);
            rot += variation * Time.deltaTime;
            angle += -variation * Time.deltaTime;
            if (rot >= rotAngle)
            {
                m_RightStart = false;
                rotStart = false;
                m_isInputAny = false;
                this.transform.localRotation = Quaternion.Euler(0, angle, 0);
                // ParentReset();
            }
        }
    }

    private void OnBtn()
    {
        //��]�p�x������������B
        rot = 0f;
        this.transform.localRotation = Quaternion.Euler(0, angle, 0);
        rotStart = true;
    }







    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            m_isPlayerHit = true;
            Debug.Log("aaaaaaa");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            m_isPlayerHit = false;
        }
    }
}
