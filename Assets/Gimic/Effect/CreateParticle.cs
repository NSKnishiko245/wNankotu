using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticle : MonoBehaviour
{
    [Header("�g�p����p�[�e�B�N��")]
    public ParticleSystem m_particle;

    [Header("�����Ԋu(�t���[��)")]
    public int m_Frame = 60;
    private int m_Cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Cnt += 1;
        if (m_Cnt > m_Frame)
        {
            Instantiate(m_particle, this.transform.position, Quaternion.identity);
            m_Cnt = 0;
        }
    }
}
