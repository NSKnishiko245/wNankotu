using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���[���u���b�N���ʏ�u���b�N�Ɣ�������Ƀ��[���u���b�N���j���L�j���L�Əo���̂��~�߂�B

public class Normal : MonoBehaviour
{
    public GameObject m_objMoveBlock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Block")
        {
            m_objMoveBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
    }
}
