//============================
//name:StageManager
//�T�v:�^�C����܂�Ȃ��鏈��
//�x��������������u��
//============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] m_tileObj;  //�^�C��
    public OrimeObj[] m_OriObj;    //�܂��

    float[] angle;

    int cnt=0;


    bool m_isInputSp = false;
    bool m_isInputRight = false;
    bool m_isInputLeft = false;
    bool m_isInputAny = false;

    //��]�p
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
        
        //�܂�ڂ̖{����
        angle = new float[m_OriObj.Length];
        for(int i = 0; i < m_OriObj.Length; i++)
        {
            angle[i] = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_isInputSp = false;
        m_isInputRight = false;
        m_isInputLeft = false;


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cnt++;
            Debug.Log(cnt);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cnt--;
            Debug.Log(cnt);
        }



        //��]������Ȃ�����
        if (!rotStart)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                m_isInputSp = true;
            }
            if (Input.GetKey(KeyCode.R))
            {
                ParentReset();
            }
            if (!m_isInputAny)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    m_isInputRight = true;
                    m_isInputAny = true;

                     OnBtn(m_OriObj[cnt]);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    m_isInputLeft = true;
                    m_isInputAny = true;

                     OnBtn(m_OriObj[cnt]);
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
                //�e�q�֌W���Z�b�g����
                ParentSetLefttoRight();
                //��]
                //RotationOri(m_OriObj[1]);
                m_LeftStart = true;
            }
            else if (m_isInputRight)
            {
                //�e�q�֌W���Z�b�g����
                ParentSetRighttoLeft();
                //��]
                //RotationOri(m_OriObj[1]);
                m_RightStart = true;
            }
        }

        //��]
        if (m_LeftStart)
        {
            RotationOriLeft(m_OriObj[cnt]);
        }
        else if (m_RightStart)
        {
            RotationOriRight(m_OriObj[cnt]);
        }

        //Debug.Log(angle);
    }


    //�܂�ڂ̕������獶����E�֐e�q�֌W���Z�b�g����
    private void ParentSetLefttoRight()
    {
        for(int cnt = m_tileObj.Length-1; cnt > 0; cnt--)
        {
            m_tileObj[cnt].transform.parent = m_OriObj[cnt - 1].transform;
            m_OriObj[cnt-1].transform.parent = m_tileObj[cnt - 1].transform;
        }
    }
    //�܂�ڂ̕�������E����Ђ���֐e�q�֌W���Z�b�g����
    private void ParentSetRighttoLeft()
    {
        for (int cnt = 0; cnt < m_tileObj.Length-1; cnt++)
        {
            m_tileObj[cnt].transform.parent = m_OriObj[cnt].transform;
            m_OriObj[cnt].transform.parent = m_tileObj[cnt + 1].transform;
        }
    }
    //�e�q�֌W�����Z�b�g
    private void ParentReset()
    {
        for(int i = 0; i < m_tileObj.Length; i++)
        {
            m_tileObj[i].transform.parent = this.gameObject.transform;
        }
        for(int i = 0; i < m_OriObj.Length; i++)
        {
            m_OriObj[i].transform.parent = this.gameObject.transform;
        }
    }

    //�܂�ڂ�180�x��](�܂�ڂ̃I�u�W�F�N�g)
    private void RotationOriLeft(OrimeObj obj)
    {
        if (rotStart)
        {
            obj.transform.Rotate(0, variation * Time.deltaTime, 0);
            rot += variation * Time.deltaTime;
            angle[obj.m_Number] += variation * Time.deltaTime;
            if (rot >= rotAngle)
            {
                m_LeftStart = false;
                rotStart = false;
                m_isInputAny = false;
                obj.transform.localRotation = Quaternion.Euler(0, angle[obj.m_Number], 0);
               // ParentReset();
            }
        }
    }

    private void RotationOriRight(OrimeObj obj)
    {
        if (rotStart)
        {
            obj.transform.Rotate(0, -variation * Time.deltaTime, 0);
            rot += variation * Time.deltaTime;
            angle[obj.m_Number] += -variation * Time.deltaTime;
            if (rot >= rotAngle)
            {
                m_RightStart = false;
                rotStart = false;
                m_isInputAny = false;
                obj.transform.localRotation = Quaternion.Euler(0, angle[obj.m_Number], 0);
                //ParentReset();
            }
        }
    }

    private void OnBtn(OrimeObj obj)
    {
        //��]�p�x������������B
        rot = 0f;
        obj.transform.localRotation = Quaternion.Euler(0, angle[obj.m_Number], 0);
        rotStart = true;
    }
}
