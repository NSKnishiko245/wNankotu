using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proto_player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ɉړ�
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(-0.01f, 0.0f, 0.0f);
        }
        // �E�Ɉړ�
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(0.01f, 0.0f, 0.0f);
        }
       
    }
}
