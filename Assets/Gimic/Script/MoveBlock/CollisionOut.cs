using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionOut : MonoBehaviour
{
    // �����蔻��p�I�u�W�F�N�g
    public GameObject m_collisionobj;
    private bool m_isHit;
    private int m_nowCnt = 0;
    private int m_addCnt = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_addCnt < m_nowCnt)
        {
            // this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //if (m_isHit)
            {
                m_collisionobj.SetActive(true);
               // m_isHit = false;
            }
        }

        m_nowCnt++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "GimicMoveCol")
        {
            //�������Ă���
           // m_isHit = true;
            m_collisionobj.SetActive(false);
            m_addCnt = m_nowCnt;
            m_addCnt++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "GimicMoveCol")
        {
            //����Ă���
            m_isHit = false;
            m_collisionobj.SetActive(true);
        }
    }

    //�[�������m���������Ă��邩�Ԃ�
   
}
