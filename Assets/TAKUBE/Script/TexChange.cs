using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexChange : MonoBehaviour
{
    public Text text;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.A))
        {
            text.text = "���]�������E�ɓ��邱�Ƃ��ł����I\n�ڂ̑O�ɂ���S�[���Ɍ������Đi�����I";
        }
    }
}
