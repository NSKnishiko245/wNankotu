using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�o���o���̃p�[�c���ЂƂɖ߂�

public class FigureManager : MonoBehaviour
{
    public GameObject[] m_objPoint;

    public GameObject[] m_objModelparts;    //���f���̃p�[�c�̑���

    //�����ʒu
    private Vector3 m_firstPos;

    //�X�^�[�g�ʒu
    private Vector3[] m_startPos=new Vector3[5];


   

    private bool[] m_isStageClear = new bool[6]; 

    private bool[] m_isSet = new bool[5];   //�p�[�c�����̈ʒu�ɂ��邩�H 
    private int[] m_WaitTime = new int[5]; //�҂�����

    private bool[] m_isMove = new bool[5];  //�ړ����H
    private bool[] m_isFinish = new bool[5];

    private float[]distance_two=new float[5];
    private float[] present_Location = new float[5];

    int[] cnt=new int[5];


    //
    public int Speed = 1;

    [Header("�ړ����̃p�[�e�B�N��")]
    public ParticleSystem m_objKirakiraMove;

    [Header("�����������ɏo���p�[�e�B�N��")]
    public ParticleSystem m_PerfectParticle;
    private int m_cnt = 0;


    //�����������p�[�c�܂ŉ^��(�ő�5�܂ŉ^��)
    //�S�X�e�[�W����
    public GameObject m_objCopper;
    private bool[] m_isCopper = new bool[37];
    private bool[] m_isCopperFinish = new bool[37];
    private bool[] m_isCopperMove = new bool[37];

    private float[] distance_Copper = new float[37];�@//�����ʒu����p�[�c�܂�
    private Vector3[] m_CoppernowPos = new Vector3[37];//���݈ʒu
    private Vector3[] m_CopperTarGetPos=new Vector3[37];//�ڕW�ʒu
    private float[] Copper_Location = new float[37];
    private int[] CopperCnt = new int[37];
    private int[] m_CopperWait = new int[37];
    //private bool[] m_isCopperMove = new bool[5];
    //public GameObject m_objCopper; 
    //private bool m_isCopperMove;
    //private Vector3[] m_isTargetPos = new Vector3[5];

    //private bool[] m_isCopper = new bool[36];
    //private bool[] m_isCopperFinish = new bool[36];

    //private float[] distance_Copper = new float[5];


    // Start is called before the first frame update
    void Start()
    {
        //foreach (var obj in m_objModelparts)
        //{
        //    obj.transform.position = m_objPoint[0].transform.position;
        //}
       

        //�V�[����ς��Ă��c�葱����l��
        DontDestroyOnLoad(gameObject);

        //�����ʒu���L��������
        m_firstPos = gameObject.transform.position;

        for (int i = 0; i < distance_two.Length; i++)
        {
            m_WaitTime[i] = 0;
            cnt[i] = 0;
            m_startPos[i] = m_objModelparts[i].transform.position;  //�X�^�[�g�ʒu�ۑ�
            distance_two[i] = Vector3.Distance(m_startPos[i], m_objPoint[i].transform.position);
        }

        //�Q�_�̋��������߂�B
        for (int i = 1; i < 36; i++)
        {
            CopperCnt[i] = 0;
            m_CopperWait[i] = 0;
            m_CoppernowPos[i] = m_objCopper.transform.position;
            distance_Copper[i] = Vector3.Distance(m_objCopper.transform.position, m_objModelparts[0].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�����X�V
        CopperMove();

        //��Փx�U���N���A������L���L���p�[�e�B�N��
        if (m_isStageClear[5])
        {
            if (m_cnt%10==0)
            {
                Instantiate(m_PerfectParticle, this.transform.position, Quaternion.identity);//���炫��G�t�F�N�g
            }
            m_cnt++;
        }

        if (SceneManager.GetActiveScene().name == "Stage1Scene")
        {
            for(int i = 0; i < m_objModelparts.Length; i++)
            {
                m_objModelparts[i].SetActive(false);
            }
           // this.gameObject.transform.parent = GameObject.Find("Main Camera").transform;
            //if(GameObject.Find("Player").GetComponent<Player>().IsHitGoalBlock)
            //{
            //    this.gameObject.transform.parent = null;
            //    //�ړ������������ɖ߂�
            //    this.gameObject.transform.position = m_firstPos;
            //}
        }
        else
        {
            for (int i = 0; i < m_objModelparts.Length; i++)
            {
                m_objModelparts[i].SetActive(true);
            }
        }
       

        for (int i = 0; i < 6; i++)
        {
            if (StageClearManager.StageClear[i])
            {
                m_isStageClear[i] = true;
            }
        }


        //if ((Input.GetAxis("LTrigger") > 0 && Input.GetAxis("RTrigger") > 0))
        //{
        //    m_isStageClear[0] = true;
        //    m_isStageClear[1] = true;
        //    m_isStageClear[2] = true;
        //    m_isStageClear[3] = true;
        //    m_isStageClear[4] = true;
        //    m_isStageClear[5] = true;
        //}

        //if (Input.GetKeyDown(KeyCode.F1)) m_isStageClear[0] = true;
        //if (Input.GetKeyDown(KeyCode.F2)) m_isStageClear[1] = true;
        //if (Input.GetKeyDown(KeyCode.F3)) m_isStageClear[2] = true;
        //if (Input.GetKeyDown(KeyCode.F4)) m_isStageClear[3] = true;
        //if (Input.GetKeyDown(KeyCode.F5)) m_isStageClear[4] = true;
        //if (Input.GetKeyDown(KeyCode.F6)) m_isStageClear[5] = true;

        //�{���ƂɑS�Ă̓����_�����W�߂����H
        for (int i = 0; i < m_isStageClear.Length-1; i++)
        {
            if (m_isStageClear[i])//����S�ďW�߂���
            {
                if (!m_isFinish[i])//���̈ʒu�ɖ߂��Ă��Ȃ����
                {
                    m_isSet[i] = true;
                    //m_isMove[i] = true;
                }
                if (m_isSet[i])
                {
                    //Instantiate(m_objKirakiraMove, m_objModelparts[i].transform.position, Quaternion.identity);//���炫��G�t�F�N�g
                    if (m_WaitTime[i] >= 240)
                    {
                        m_isMove[i] = true;
                    }
                    m_WaitTime[i]++;
                }
            }

        }




        //�ړ�
        for(int i = 0; i < m_isMove.Length; i++)
        {
            if (m_isMove[i])
            {
                if (!m_isFinish[i])
                {
                    //���݂̈ʒu
                    present_Location[i] = (cnt[i]/60.0f) / distance_two[i];
                    cnt[i]+=Speed;
                    m_objModelparts[i].transform.position = Vector3.Lerp(m_startPos[i], m_objPoint[i].transform.position, present_Location[i]);
                    Instantiate(m_objKirakiraMove, m_objModelparts[i].transform.position, Quaternion.identity);//���炫��G�t�F�N�g
                    m_objModelparts[i].SetActive(false);
                    if (Vector3.Distance(m_objModelparts[i].transform.position, m_objPoint[i].transform.position) <= 0.1f)
                    {
                        //�p�x�����ɖ߂�
                        //Quaternion rot = new Quaternion(66, 0, 0, 1);
                        m_objModelparts[i].transform.rotation = transform.rotation;
                        m_objModelparts[i].SetActive(true);
                        m_isSet[i] = false;
                        m_isFinish[i] = true;

                    }
                }
            }
        }

    }

    //�X�e�[�W���Ƃɓ�����������`�F�b�N
    void CopperMove()
    {
       // Debug.Log(m_isCopper[1]);
        //�{�œ�����������X�V����
        for (int cnt = 1; cnt < 36; cnt++)
        {
            if (cnt == 6) continue;
            if (cnt == 12) continue;
            if (cnt == 18) continue;
            if (cnt == 24) continue;
            if (cnt == 30) continue;
            if (cnt == 36) continue;

            if (StageClearManager.m_isGetCopper[cnt])
            {
                m_isCopper[cnt] = StageClearManager.m_isGetCopper[cnt];
            }
        }

        //�����p�[�c�܂ňړ����������H
        for(int i = 1; i <= 36; i++)
        {
            if (m_isCopper[i])
            {
                if (!m_isCopperFinish[i])//�ړ����I����Ă��Ȃ����
                {
                    //�ڕW�ʒu�����߂�
                    if (i >= 1 && i < 6)//�X�e�[�W�P�`�T
                    {
                        m_CopperTarGetPos[i] = m_objModelparts[0].transform.position;
                    }
                    else if (i >= 7 && i < 12)//�X�e�[�W�P�`�T
                    {
                        m_CopperTarGetPos[i] = m_objModelparts[1].transform.position;
                    }
                    else if (i >= 13 && i < 18)//�X�e�[�W�P�`�T
                    {
                        m_CopperTarGetPos[i] = m_objModelparts[2].transform.position;
                    }
                    else if (i >= 19 && i < 24)//�X�e�[�W�P�`�T
                    {
                        m_CopperTarGetPos[i] = m_objModelparts[3].transform.position;
                    }
                    else if (i >= 25 && i < 30)//�X�e�[�W�P�`�T
                    {
                        m_CopperTarGetPos[i] = m_objModelparts[4].transform.position;
                    }
                    else if (i >= 31 && i < 36)//�X�e�[�W�P�`�T
                    {
                        m_CopperTarGetPos[i] = this.transform.position;
                    }
                    //��Œǉ�

                    if (m_CopperWait[i] >= (60 + ((i % 6) * 20)))
                    {
                        m_isCopperMove[i] = true;//�ړ����ɂ���
                    }
                    m_CopperWait[i]++;
                }
            }
        }

        //�����ړ�������
        for (int i = 1; i <= 36; i++)
        {
            if (m_isCopperMove[i])//�ړ����Ȃ�
            {
                Copper_Location[i]= (CopperCnt[i] / 60.0f) / distance_Copper[i];
                CopperCnt[i] += Speed;
                m_CoppernowPos[i] = Vector3.Lerp(m_objCopper.transform.position, m_CopperTarGetPos[i], Copper_Location[i]);
                Instantiate(m_objKirakiraMove, m_CoppernowPos[i], Quaternion.identity);
                if (Vector3.Distance(m_CoppernowPos[i], m_CopperTarGetPos[i]) <= 0.1f)
                {
                    m_isCopperMove[i] = false;
                    m_isCopperFinish[i] = true;
                }
            }
        }















        //for(int cnt = 0; cnt < 36; cnt++)
        //{
        //    if (StageSelectManager.score[cnt].isCopper)
        //    {
        //        m_isCopper[cnt] = StageSelectManager.score[cnt].isCopper;
        //    }
        //}

            //for(int i = 0; i < 36; i++)
            //{
            //    if (m_isCopper[i])//���̃X�e�[�W�œ�����������H
            //    {
            //        if (!m_isCopperFinish[i])//�ړ����I����Ă��邩�H
            //        {
            //            if (!m_isCopperMove)//���[�u��Ԃł͂Ȃ����
            //            {
            //                //���[�u��Ԃ�
            //                m_isCopperMove = true;
            //                //�ړI�n��ݒ�
            //                if (i >= 1 && i < 6)//�X�e�[�W�P�`�T
            //                {
            //                    m_isTargetPos[0] = m_objModelparts[0].transform.position;
            //                }
            //            }

            //                //�܂��Ȃ�ړ������X�^�[�g
            //                //for(int cnt = 0; cnt < 5; cnt++)
            //                //{
            //                //    if (!m_isCopperMove[cnt])
            //                //    {
            //                //        m_isCopperMove[cnt] = true;
            //                //        break;
            //                //    }
            //                //}

            //            if (m_isCopperMove)
            //            {
            //                ////�ړI�n��ݒ�
            //                //if (i >= 1 && i < 6)//�X�e�[�W�P�`�T
            //                //{
            //                //    m_isTargetPos[0] = m_objModelparts[0].transform.position;
            //                //}



            //            }
            //        }
            //    }
            //}
    }

    public void FigurePositionInit(int stageClearNum)
    {
        if (stageClearNum < 5)
        {
            m_objModelparts[stageClearNum].transform.position = m_objPoint[stageClearNum].transform.position;
            m_objModelparts[stageClearNum].transform.rotation = m_objPoint[stageClearNum].transform.rotation;
            m_isFinish[stageClearNum] = m_isCopperFinish[stageClearNum] = true;
        }
        m_isStageClear[stageClearNum] = true;
    }
}
