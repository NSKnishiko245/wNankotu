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
    public Player m_player; //�v���C���[
    public GameObject[] m_tileObj;  //�^�C��
    public OrimeObj[] m_OriObj;    //�܂��
    //�p�[�e�B�N���`��p
    public GameObject particleObject;


    private int nowParentNum = 0;

    float[] angle;

    int cnt = 0;
    int m_frame = 1;

    bool m_isInputSp = false;
    bool m_isInputRight = false;
    bool m_isInputLeft = false;
    bool m_isInputAny = false;

    //��]�p
    bool m_LeftStart = false;
    bool m_RightStart = false;
    bool rotStart = false;
    float rotSpeed = 1.5f; // second
    float rotAngle = 180f;
    float variation;
    float rot;

    // Start is called before the first frame update
    void Start()
    {
        variation = rotAngle / rotSpeed;
        nowParentNum = 0;
        //�܂�ڂ̖{����
        angle = new float[m_OriObj.Length];
        for (int i = 0; i < m_OriObj.Length; i++)
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

        //�v���C���[�̐G��Ă���I�u�W�F�N�gNo���܂�ڂ̐��ȉ��Ȃ�
        if (m_player.GetHitOriobjNum() >= 0 && m_player.GetHitOriobjNum() < m_OriObj.Length)
        {
           // float tri = Input.GetAxis("L_R_Trigger");
            //��]������Ȃ�����
            if (!rotStart)
            {
                if (Input.GetKey(KeyCode.Space)|| Input.GetKey("joystick button 5"))
                {
                    m_isInputSp = true;
                }
                if (Input.GetKey(KeyCode.R))
                {
                    ParentReset();
                }

                float tri = Input.GetAxis("L_R_Trigger");
                if (!m_isInputAny)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow)||tri >= 1)
                    {
                        m_isInputRight = true;
                        m_isInputAny = true;
                        m_frame = 1;

                        //OnBtn(m_OriObj[cnt]);
                        OnBtn(m_OriObj[m_player.GetHitOriobjNum()]);

                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow)|| tri <=-1)
                    {
                        m_isInputLeft = true;
                        m_isInputAny = true;
                        m_frame = 1;

                        //OnBtn(m_OriObj[cnt]);
                        OnBtn(m_OriObj[m_player.GetHitOriobjNum()]);
                    }
                    else
                    {
                        m_isInputAny = false;
                    }
                }
            }
        }
        //�X�y�[�X�����ꂽ���
       // if (m_isInputSp)
        {
            if (m_isInputLeft)
            { 
                //�e�q�֌W���Z�b�g����
                ParentSetLefttoRight();
                //��]
                m_LeftStart = true;
            }
            else if (m_isInputRight)
            {
                //�e�q�֌W���Z�b�g����
                ParentSetRighttoLeft();
                //��]
                m_RightStart = true;
            }
        }

        if (m_frame < 0)
        {
            //��]
            if (m_LeftStart)
            {
                RotationOriLeft(m_OriObj[m_player.GetHitOriobjNum()]);
            }
            else if (m_RightStart)
            {
                RotationOriRight(m_OriObj[m_player.GetHitOriobjNum()]);
            }
        }
        m_frame--;
        Debug.Log(Input.GetAxisRaw("Horizontal2"));
    }


    //�܂�ڂ̕������獶����E�֐e�q�֌W���Z�b�g����
    private void ParentSetLefttoRight()
    {
        if (nowParentNum > m_player.GetHitOriobjNum() && Mathf.Abs(angle[m_player.GetHitOriobjNum()]) < 1.0f)
        {
            ParentReset();
            Debug.Log("LRreset");
            for (int cnt = m_tileObj.Length - 1; cnt > 0; cnt--)
            {
                m_tileObj[cnt].transform.parent = m_OriObj[cnt - 1].transform;
                m_OriObj[cnt - 1].transform.parent = m_tileObj[cnt - 1].transform;
            }
            nowParentNum = 0;
        }
    }
    //�܂�ڂ̕�������E����Ђ���֐e�q�֌W���Z�b�g����
    private void ParentSetRighttoLeft()
    {
        if (nowParentNum <= m_player.GetHitOriobjNum() && Mathf.Abs(angle[m_player.GetHitOriobjNum()]) < 1.0f)
        {
            ParentReset();
            Debug.Log("RLreset");
            for (int cnt = 0; cnt < m_tileObj.Length - 1; cnt++)
            {
                m_tileObj[cnt].transform.parent = m_OriObj[cnt].transform;
                m_OriObj[cnt].transform.parent = m_tileObj[cnt + 1].transform;
            }
            nowParentNum = m_tileObj.Length - 1;
        }
    }
    //�e�q�֌W�����Z�b�g
    private void ParentReset()
    {
        for (int i = 0; i < m_tileObj.Length; i++)
        {
            m_tileObj[i].transform.parent = this.gameObject.transform;
        }
        for (int i = 0; i < m_OriObj.Length; i++)
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
            //angle[obj.m_Number] += variation * Time.deltaTime;
           // SetChildActive(m_tileObj[0], false);
          //  SetChildActive(m_tileObj[1], false);
            //SetChildActive(m_tileObj[2], false);
            //SetChildActive(m_tileObj[3], false);
            //SetChildActive(m_tileObj[4], false);
            //SetChildActive(m_tileObj[5], false);
            if (rot >= rotAngle)
            {
               

                //SetChildActive(m_tileObj[0], true);
                //SetChildActive(m_tileObj[1], true);
                //SetChildActive(m_tileObj[2], true);
                //SetChildActive(m_tileObj[3], true);
                //SetChildActive(m_tileObj[4], true);
                //SetChildActive(m_tileObj[5], true);
                TileReset();
                m_LeftStart = false;
                rotStart = false;
                m_isInputAny = false;
                angle[obj.m_Number] += 180;
                Debug.Log(angle[obj.m_Number]);
                obj.transform.localRotation = Quaternion.Euler(0, angle[obj.m_Number], 0);
            }
        }
    }

    private void RotationOriRight(OrimeObj obj)
    {
        if (rotStart)
        {
            obj.transform.Rotate(0, -variation * Time.deltaTime, 0);
            rot += variation * Time.deltaTime;
            //angle[obj.m_Number] += -variation * Time.deltaTime;
           // SetChildActive(m_tileObj[0], false);
           // SetChildActive(m_tileObj[1], false);
            //SetChildActive(m_tileObj[2], false);
            //SetChildActive(m_tileObj[3], false);
            //SetChildActive(m_tileObj[4], false);
            //SetChildActive(m_tileObj[5], false);
            if (rot >= rotAngle)
            {
                //SetChildActive(m_tileObj[0], true);
                //SetChildActive(m_tileObj[1], true);
                //SetChildActive(m_tileObj[2], true);
                //SetChildActive(m_tileObj[3], true);
                //SetChildActive(m_tileObj[4], true);
                //SetChildActive(m_tileObj[5], true);
                TileReset();
                m_RightStart = false;
                rotStart = false;
                m_isInputAny = false;
                angle[obj.m_Number] -= 180;
                obj.transform.localRotation = Quaternion.Euler(0, angle[obj.m_Number], 0);
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

    //�^�C���̃R���|�[�l���g
    private void TileReset()
    {
        for (int i = 0; i < m_tileObj.Length; i++)
        {
            m_tileObj[i].GetComponent<ScreenShot>().ResetTexture();
        }
    }

    //�^�C���̎q����
    private void SetChildActive(GameObject obj,bool flg)
    {
        obj.transform.Find("Cube").gameObject.SetActive(flg);
        if (flg)
        {
            Instantiate(particleObject, obj.transform.Find("Cube").transform.position, Quaternion.identity); //�p�[�e�B�N���p�Q�[���I�u�W�F�N�g����
        }
    }
}
