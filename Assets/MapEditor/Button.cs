using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public GameObject StageCountText;
    public GameObject MapManager;
    
    // �e�L�X�g�̐������グ��
    public void OnClick_Increment()
    {
        int num = int.Parse(StageCountText.GetComponent<Text>().text);
        num++;
        StageCountText.GetComponent<Text>().text = num.ToString();
        LoadMap();
    }
    // �e�L�X�g�̐�����������
    public void OnClick_Decrement()
    {
        int num = int.Parse(StageCountText.GetComponent<Text>().text);
        if (num > 1)
        {
            num--;
            StageCountText.GetComponent<Text>().text = num.ToString();
            LoadMap();
        }
    }

    // ���ݍ쐬���Ă���X�e�[�W���O���t�@�C��(CSV)�ɏ����o��
    public void OnClick_Save()
    {
        MapManager.GetComponent<MapEdit>().SaveMap("Stage" + StageCountText.GetComponent<Text>().text);
    }
    public void LoadMap()
    {
        MapManager.GetComponent<MapEdit>().LoadMap("Stage" + StageCountText.GetComponent<Text>().text);
    }
}
