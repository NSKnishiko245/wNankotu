using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionOut : MonoBehaviour
{
    // �����蔻��p�I�u�W�F�N�g
    public GameObject m_collisionobj;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "GimicMoveCol")
        {
            //�������Ă���
            // m_isHit = true;
            m_collisionobj.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "GimicMoveCol")
        {
            //����Ă���
            // m_isHit = false;
            m_collisionobj.SetActive(true);
        }
    }

    //�[�������m���������Ă��邩�Ԃ�
   
}
