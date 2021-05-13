//==============================================
//���O�F�Ƃ��u���b�N
//�T�v�F�Ƃ��u���b�N���m���Ԃ���Ɖ���
//==============================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleBlock : MonoBehaviour
{
    [Header("���Ŏ��Ɏg��Effect")]
    public ParticleSystem m_endeffect;  //���Ŏ��Ɏg���G�t�F�N�g

    [Header("���Ŏ�SE")]
    public AudioClip m_sound;
    [Header("��ꂽ���ɎU�炷�I�u�W�F�N�g")]
    public GameObject[] m_obj;

    [Header("�j������I�u�W�F�N�g")]
    public GameObject m_blockobj;

    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ClimbBlock")
        {
            AudioSource.PlayClipAtPoint(m_sound, this.transform.position);//SE�Đ�
            Instantiate(m_endeffect, this.transform.position, Quaternion.identity); //���Ŏ��ɃG�t�F�N�g���g�p����
            for (int i = 0; i < m_obj.Length; i++)
            {
                Instantiate(m_obj[i], this.transform.position, Quaternion.identity);
            }
            Destroy(m_blockobj.gameObject);
        }
    }

}
