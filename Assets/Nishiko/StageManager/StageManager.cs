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
    private int[,] m_blockMap;
    private GameObject[,] m_blockList;
    private List<GameObject> m_tileObj;  //�^�C��
    private List<GameObject> m_OriObj;    //�܂��
    //�p�[�e�B�N���`��p
    public GameObject particleObject;


    public GameObject MapManager;


    private int nowParentNum = 0;


    public int stageNum = 1;

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

    int orinum = 0;

    bool firstFlg = false;

    // ���݂̉�]���
    private enum ROTSTATE
    {
        NEUTRAL,
        ROTATE,
        REROTATE,
        ROTATED,
    }
    ROTSTATE rotState = ROTSTATE.NEUTRAL;

    void StartEx()
    {
        // �G�f�B�b�g�I�u�W�F�N�g���擾���A�}�b�v�\�z��A���̃N���X�����z��Ƀf�[�^���i�[
        MapManager.GetComponent<MapEdit>().CreateStage_Game(stageNum);
        m_blockList = MapManager.GetComponent<MapEdit>().BlockList;
        m_blockMap = MapManager.GetComponent<MapEdit>().BlockMap;
        m_tileObj = MapManager.GetComponent<MapEdit>().TileList;
        m_OriObj = MapManager.GetComponent<MapEdit>().BarList;

        // �^�C���ƃo�[���X�e�[�W�}�l�[�W���[�̎q���ɂ���
        for (int i = 0; i < m_tileObj.Count; i++)
        {
            m_tileObj[i].transform.parent = this.transform;
        }
        for (int i = 0; i < m_OriObj.Count; i++)
        {
            m_OriObj[i].transform.parent = this.transform;
        }
        //this.transform.Rotate(90.0f, 0.0f, 0.0f);

        variation = rotAngle / rotSpeed;
        nowParentNum = 0;
        //�܂�ڂ̖{����
        angle = new float[m_OriObj.Count];
        for (int i = 0; i < m_OriObj.Count; i++)
        {
            angle[i] = 0.0f;
        }
    }

    void Update()
    {
        if (!firstFlg)
        {
            StartEx();
            firstFlg = true;
        }

        m_isInputSp = false;
        m_isInputRight = false;
        m_isInputLeft = false;

        if (rotState == ROTSTATE.NEUTRAL)
        {
            orinum = -1;
            for (int i = 0; i < m_OriObj.Count; i++)
            {
                if (m_OriObj[i].GetComponent<Cbar>().stay)
                {
                    orinum = i;
                }
            }
        }

        //�v���C���[�̐G��Ă���I�u�W�F�N�gNo���܂�ڂ̐��ȉ��Ȃ�
        if (orinum >= 0)
        {
            float tri = Input.GetAxis("L_R_Trigger");
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
                if (!m_isInputAny && m_isInputSp)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow)||tri>=1)
                    {
                        m_isInputRight = true;
                        m_isInputAny = true;
                        m_frame = 1;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow)|| tri <= -1)
                    {
                        m_isInputLeft = true;
                        m_isInputAny = true;
                        m_frame = 1;
                    }
                    else
                    {
                        m_isInputAny = false;
                    }
                }
            }
        }
        //�X�y�[�X�����ꂽ���
        if (m_isInputSp && rotState == ROTSTATE.NEUTRAL && orinum >= 0)
        {
            if (m_isInputLeft)
            {
                ParentReset();
                ParentSetLefttoRight();
                //��]
                m_LeftStart = true;
                rotState = ROTSTATE.ROTATE;
                OnBtn();
            }
            else if (m_isInputRight)
            {
                ParentReset();
                ParentSetRighttoLeft();
                //��]
                m_RightStart = true;
                rotState = ROTSTATE.ROTATE;
                OnBtn();
            }
        }

        if (Input.GetKeyDown("joystick button 0") && rotState == ROTSTATE.ROTATED && orinum >= 0)
        {
            if (angle[orinum] >= 170)
            {
                ParentSetRighttoLeft();
                //��]
                m_RightStart = true;
                rotState = ROTSTATE.REROTATE;
                OnBtn();
            }
            else
            {
                ParentSetLefttoRight();
                //��]
                m_LeftStart = true;
                rotState = ROTSTATE.REROTATE;
                OnBtn();
            }
        }

        if (m_frame <= 0 && orinum >= 0)
        {
            //��]
            if (m_LeftStart)
            {
                RotationOriLeft(m_OriObj[orinum], orinum);
            }
            else if (m_RightStart)
            {
                RotationOriRight(m_OriObj[orinum], orinum);
            }
        }
        m_frame--;


        // ���[�v����
        Vector2Int hitBlockNum = new Vector2Int(-1, -1);
        for (int y = 0; y < m_blockList.GetLength(0); y++)
        {
            for (int x = 0; x < m_blockList.GetLength(1); x++)
            {
                if (m_blockMap[y, x] != 0)
                {
                    if (m_blockList[y, x].tag == "BorderLine")
                    {
                        if (m_blockList[y, x].GetComponent<HitAction>().isHit)
                        {
                            Debug.Log("find");
                            hitBlockNum = new Vector2Int(x, y);
                            // �Q�d�̃��[�v�𔲂���ׂ̏���
                            x = m_blockList.GetLength(1);
                            y = m_blockList.GetLength(0);
                        }
                    }
                }
            }
        }

        if (hitBlockNum.x != -1)
        {
            if (hitBlockNum.x < m_blockList.GetLength(1) / 2)
            {
                for (int y = 0; y < m_blockList.GetLength(0); y++)
                {
                    for (int x = m_blockList.GetLength(1) - 1; x >= 0; x--)
                    {
                        if (m_blockMap[y, x] != 0)
                        {
                            if (m_blockList[y, x].tag == "BorderLine")
                            {
                                if (y == m_blockList.GetLength(0) - hitBlockNum.y)
                                {
                                    m_player.transform.position = new Vector3(
                                        m_blockList[y, x].transform.position.x - m_blockList[y, x].transform.lossyScale.x,
                                        m_blockList[y, x].transform.position.y,
                                        m_blockList[y, x].transform.position.z
                                    );
                                    // �Q�d�̃��[�v�𔲂���ׂ̏���
                                    x = -1;
                                    y = m_blockList.GetLength(0);

                                    if (y < m_blockList.GetLength(0) / 2)
                                    {
                                        Physics.gravity = new Vector3(0, -9.8f, 0);
                                    }
                                    else
                                    {
                                        Physics.gravity = new Vector3(0, 9.8f, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int y = 0; y < m_blockList.GetLength(0); y++)
                {
                    for (int x = 0; x < m_blockList.GetLength(1); x++)
                    {
                        if (m_blockMap[y, x] != 0)
                        {
                            if (m_blockList[y, x].tag == "BorderLine")
                            {
                                if (y == m_blockList.GetLength(0) - hitBlockNum.y)
                                {
                                    m_player.transform.position = new Vector3(
                                        m_blockList[y, x].transform.position.x + m_blockList[y, x].transform.lossyScale.x,
                                        m_blockList[y, x].transform.position.y,
                                        m_blockList[y, x].transform.position.z
                                    );
                                    // �Q�d�̃��[�v�𔲂���ׂ̏���
                                    x = m_blockList.GetLength(1);
                                    y = m_blockList.GetLength(0);

                                    if (y < m_blockList.GetLength(0) / 2)
                                    {
                                        Physics.gravity = new Vector3(0, -9.8f, 0);
                                    }
                                    else
                                    {
                                        Physics.gravity = new Vector3(0, 9.8f, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    // �^�C�����E���獶�։�]�����鏈��
    private void ParentSetLefttoRight()
    {
        for (int cnt = m_tileObj.Count - 1; cnt > 0; cnt--)
        {
            m_tileObj[cnt].transform.parent = m_OriObj[cnt - 1].transform;
            m_OriObj[cnt - 1].transform.parent = m_tileObj[cnt - 1].transform;
        }
        nowParentNum = 0;
    }
    // �^�C����������E�։�]�����鏈��
    private void ParentSetRighttoLeft()
    {
        for (int cnt = 0; cnt < m_tileObj.Count - 1; cnt++)
        {
            m_tileObj[cnt].transform.parent = m_OriObj[cnt].transform;
            m_OriObj[cnt].transform.parent = m_tileObj[cnt + 1].transform;
        }
        nowParentNum = m_tileObj.Count - 1;
    }
    //�e�q�֌W�����Z�b�g
    private void ParentReset()
    {
        for (int i = 0; i < m_tileObj.Count; i++)
        {
            m_tileObj[i].transform.parent = this.gameObject.transform;
        }
        for (int i = 0; i < m_OriObj.Count; i++)
        {
            m_OriObj[i].transform.parent = this.gameObject.transform;
        }
    }

    // ���ʂ��ǂ���
    private bool IsBackTile()
    {
        int orinum = 999;
        for (int i = 0; i < m_player.GetHitOriNumList().Count; i++)
        {
            if (orinum > m_player.GetHitOriNumList()[i])
            {
                orinum = m_player.GetHitOriNumList()[i];
            }
        }

        for (int i = orinum; i < m_OriObj.Count; i++)
        {
            if (Mathf.Abs(angle[i]) > 170.0f)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsBackTile2()
    {
        int orinum = 0;
        for (int i = 0; i < m_player.GetHitOriNumList().Count; i++)
        {
            if (orinum < m_player.GetHitOriNumList()[i])
            {
                orinum = m_player.GetHitOriNumList()[i];
            }
        }

        for (int i = orinum; i >= 0; i--)
        {
            if (Mathf.Abs(angle[i]) > 170.0f)
            {
                return true;
            }
        }
        return false;
    }

    //�܂�ڂ�180�x��](�܂�ڂ̃I�u�W�F�N�g)
    private void RotationOriLeft(GameObject obj, int num)
    {
        if (rotStart)
        {
            obj.transform.Rotate(0, variation * Time.deltaTime, 0);
            rot += variation * Time.deltaTime;
            if (rot >= rotAngle)
            {
               
                for (int i = 0; i < m_tileObj.Count; i++)
                {
                    SetChildActive(m_tileObj[i], true);
                }
                TileReset();
                m_LeftStart = false;
                rotStart = false;
                m_isInputAny = false;
                if (rotState == ROTSTATE.ROTATE)
                {
                    angle[num] += 180;
                    rotState = ROTSTATE.ROTATED;
                }
                else
                {
                    angle[num] = 0;
                    rotState = ROTSTATE.NEUTRAL;
                }
                obj.transform.localRotation = Quaternion.Euler(0, angle[num], 0);
            }
        }
    }

    private void RotationOriRight(GameObject obj, int num)
    {
        if (rotStart)
        {
            obj.transform.Rotate(0, -variation * Time.deltaTime, 0);
            rot += variation * Time.deltaTime;
            if (rot >= rotAngle)
            {
                for (int i = 0; i < m_tileObj.Count; i++)
                {
                    SetChildActive(m_tileObj[i], true);
                }
                TileReset();
                m_RightStart = false;
                rotStart = false;
                m_isInputAny = false;
                if (rotState == ROTSTATE.ROTATE)
                {
                    angle[num] -= 180;
                    rotState = ROTSTATE.ROTATED;
                }
                else
                {
                    angle[num] = 0;
                    rotState = ROTSTATE.NEUTRAL;

                }
                obj.transform.localRotation = Quaternion.Euler(0, angle[num], 0);
            }
        }
    }

    private void OnBtn()
    {
        if (orinum < 0)
        {
            return;
        }
        //��]�p�x������������B
        rot = 0f;
        m_OriObj[orinum].transform.localRotation = Quaternion.Euler(0, angle[orinum], 0);
        rotStart = true;
    }

    //�^�C���̃R���|�[�l���g
    private void TileReset()
    {
        //for (int i = 0; i < m_tileObj.Count; i++)
        //{
        //    m_tileObj[i].GetComponent<ScreenShot>().ResetTexture();
        //}
    }

    //�^�C���̎q����
    private void SetChildActive(GameObject obj,bool flg)
    {
        //obj.transform.Find("Cube").gameObject.SetActive(flg);
        //if (flg)
        //{
        //    Instantiate(particleObject, obj.transform.Find("Cube").transform.position, Quaternion.identity); //�p�[�e�B�N���p�Q�[���I�u�W�F�N�g����
        //}
    }
}
