using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleMode : MonoBehaviour
{
    public GameObject m_objBrittleBlock;
    bool m_IsMove=false;//���Ă����Ԃ��H
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_objBrittleBlock.GetComponent<BrittleBlock>().GetIsBroke())
        {
            if (!m_IsMove)//�����Ă��Ȃ����
            {
                m_IsMove = true;
                Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbody���擾
                Vector3 force = new Vector3(Random.Range(-1, 1),    // X��
                                            Random.Range(1.0f, 2.0f), // Y��
                                            Random.Range(-1, -2));   // Z��
                rb.AddForce(force, ForceMode.Impulse);          // �͂�������
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }

    }
}
