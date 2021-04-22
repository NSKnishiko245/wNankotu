//�v���C���[�����F�̃M�A���ӂꂽ�����

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldGear : MonoBehaviour
{

    public ParticleSystem m_particle;//�擾���̃G�t�F�N�g

    private Animator m_anim;   //�擾���̃A�j���[�V����
    private bool m_getflg = false;
    // Start is called before the first frame update
    void Start()
    {
        m_anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void DestroyGear()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Instantiate(m_particle, this.transform.position, Quaternion.identity); //���Ŏ��ɃG�t�F�N�g���g�p����
            //Destroy(this.gameObject);
            m_getflg = true;
            m_anim.SetBool("GetFlg",m_getflg);
        }
    }
}
