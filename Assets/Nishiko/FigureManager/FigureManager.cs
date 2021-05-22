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


   

    private bool[] m_isStageClear = new bool[5]; 

    private bool[] m_isSet = new bool[5];   //�p�[�c�����̈ʒu�ɂ��邩�H 
    private bool[] m_isMove = new bool[5];  //�ړ����H
    private bool[] m_isFinish = new bool[5];

    private float[]distance_two=new float[5];
    private float[] present_Location = new float[5];

    int[] cnt=new int[5];


    //
    public int Speed = 1;

    public ParticleSystem m_objKirakiraMove;

    // Start is called before the first frame update
    void Start()
    {
        //�V�[����ς��Ă��c�葱����l��
        DontDestroyOnLoad(gameObject);

        //�����ʒu���L��������
        m_firstPos = gameObject.transform.position;

        for (int i = 0; i < distance_two.Length; i++)
        {
            cnt[i] = 0;
            m_startPos[i] = m_objModelparts[i].transform.position;  //�X�^�[�g�ʒu�ۑ�
            distance_two[i] = Vector3.Distance(m_startPos[i], m_objPoint[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
       

        if (Input.GetKeyDown(KeyCode.F1)) m_isStageClear[0] = true;
        for (int i = 0; i < 5; i++)
        {
            if (StageClearManager.StageClear[i])
            {
                m_isStageClear[i] = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2)) m_isStageClear[1] = true;
        if (Input.GetKeyDown(KeyCode.F3)) m_isStageClear[2] = true;
        if (Input.GetKeyDown(KeyCode.F4)) m_isStageClear[3] = true;

        //�{���ƂɑS�Ă̓����_�����W�߂����H
        for (int i = 0; i < m_isStageClear.Length; i++)
        {
            if (m_isStageClear[i])//����S�ďW�߂���
            {
                if (!m_isFinish[i])//���̈ʒu�ɖ߂��Ă��Ȃ����
                {
                    m_isSet[i] = true;
                    m_isMove[i] = true;
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
                        m_isFinish[i] = true;
                    }
                }
            }
        }

    }

    
}
