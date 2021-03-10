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
    public GameObject[] m_OriObj;    //�܂��

    bool m_isInputSp = false;
    bool m_isInputRight = false;
    bool m_isInputLeft = false;
    bool m_isInputAny = false;

    //��]�p
    bool rotStart = false;
    float speed = 3.0f;
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
        //��]������Ȃ�����
        if (!rotStart)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                m_isInputSp = true;
            }
            else if (Input.GetKey(KeyCode.R))
            {
                //�e�q�֌W�����Z�b�g
                ParentReset();
            }
            if (!m_isInputAny)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    m_isInputRight = true;
                    m_isInputAny = true;
                    OnBtn(m_OriObj[1]);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    m_isInputLeft = true;
                    m_isInputAny = true;
                    OnBtn(m_OriObj[1]);
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
            }
            else if (m_isInputRight)
            {
                //�e�q�֌W���Z�b�g����
                ParentSetRighttoLeft();
                //��]
                //RotationOri(m_OriObj[1]);
            }
        }
        //��]
        RotationOriLeft(m_OriObj[1]);
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
    private void RotationOriLeft(GameObject obj)
    {
        if (rotStart)
        {
            obj.transform.Rotate(0, variation * Time.deltaTime,0);
            rot += variation * Time.deltaTime;
            if (rot >= rotAngle)
            {
                rotStart = false;
                obj.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private void OnBtn(GameObject obj)
    {
        //��]�p�x������������B
        rot = 0f;
        obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        rotStart = true;
    }
}
