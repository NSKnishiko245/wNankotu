using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockBar : MonoBehaviour
{
    //�����n�߂�܂ł̃t���[��
    public int m_frame = 30;
    private int m_cntframe = 0;//�J�E���g�p�ϐ�

    public CollisionOut m_UpCol;
    public CollisionOut m_DownCol;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
       
        //if (m_UpCol.GetIsHit())
        //{
        //    m_UpCol.gameObject.SetActive(false);
        //}
        //if (m_DownCol.GetIsHit())
        //{
        //    m_DownCol.gameObject.SetActive(false);
        //}
    }

    //MoveBlock�Ƃ̓����蔻��
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.transform.tag == "GimicMoveBlock")
    //    {
    //        Debug.Log("�o�[�ƃu���b�N�ڐG");
    //        other.gameObject.GetComponent<MoveBlock>().SetIsHit(true);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.tag == "GimicMoveBlock")
    //    {
    //        other.gameObject.GetComponent<MoveBlock>().SetIsHit(false);
    //    }
    //}
}
