//������������������������
//�b�Ԋu�ŉ��𔭐�������
//
//������������������������

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMist : MonoBehaviour
{
    public ParticleSystem m_objMist;
    [Header("�����Ԋu")]
    public float timeOut=30;
    private float timeElapsed=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed++;

        Instantiate(m_objMist, this.transform.position, Quaternion.identity);
        if (timeElapsed >= timeOut)
        {
            //������

            timeElapsed = 0.0f;
        }
    }
}
