//ClearBlock
//��G���Ǝ��̉�����u���b�N

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBlock : MonoBehaviour
{
    //���̉�����u���b�N
    public GameObject m_objblock;
    //�������u���b�N
    public GameObject m_objNotblock;
    public GameObject m_collision;
    //�o������SE
    public AudioClip m_sound01;

    private bool testflg = false;
    private bool HitobjColl;

    private int testcnt = 0;
    private int addcnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_objblock.SetActive(false);
        m_collision.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (addcnt < testcnt)
        {
            testflg = false;
        }
        ActiveSet();

        testcnt++;

    }

    private void ActiveSet()
    {
        if (!testflg)
        {
            if (m_objblock.activeSelf)
            {
                if (!m_objNotblock.activeSelf)
                {
                    m_objblock.SetActive(false);
                    m_objNotblock.SetActive(true);
                    m_collision.SetActive(false);
                }
            }
        }
        else
        { 
            m_objblock.SetActive(true);
            m_collision.SetActive(true);
            m_objNotblock.SetActive(false);
        }
    }


    
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "GimicClearBlock")
        {
            //AudioSource.PlayClipAtPoint(m_sound01, this.transform.position);    //SE�Đ�
            //m_objblock.SetActive(true);
            //m_objNotblock.SetActive(false)
            
            testflg = true;
            addcnt = testcnt;
            addcnt++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "GimicClearBlock")
        {
            //m_objblock.SetActive(false);
            //m_objNotblock.SetActive(true);

            
        }
    }
}
