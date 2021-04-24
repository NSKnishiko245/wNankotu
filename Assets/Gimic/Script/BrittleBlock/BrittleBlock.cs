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
    private AudioSource m_audioSource;


    // Start is called before the first frame update
    void Start()
    {
        //Component���擾
        m_audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Block")
        {
            m_audioSource.PlayOneShot(m_sound); //SE�Đ�
            Instantiate(m_endeffect, this.transform.position, Quaternion.identity); //���Ŏ��ɃG�t�F�N�g���g�p����
            Destroy(this.gameObject);   //�u���b�N���m���Ԃ���Ɖ���
        }
    }
}