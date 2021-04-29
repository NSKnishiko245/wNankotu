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
    //�o������SE
    public AudioClip m_sound01;

    // Start is called before the first frame update
    void Start()
    {
        m_objblock.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "GimicClearBlock")
        {
            AudioSource.PlayClipAtPoint(m_sound01, this.transform.position);    //SE�Đ�
            m_objblock.SetActive(true);
            m_objNotblock.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "GimicClearBlock")
        {
            m_objblock.SetActive(false);
            m_objNotblock.SetActive(true);
        }
    }
}
