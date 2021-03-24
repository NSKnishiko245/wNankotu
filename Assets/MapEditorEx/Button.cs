using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public GameObject StageCountText;
    public GameObject MapManager;
    
    // �e�L�X�g�̐������グ��
    public void StageCount_Increment()
    {
        int num = int.Parse(StageCountText.GetComponent<Text>().text);
        num++;
        StageCountText.GetComponent<Text>().text = num.ToString();
        LoadMap();
    }
    // �e�L�X�g�̐�����������
    public void StageCount_Decrement()
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
        MapManager.GetComponent<MapEdit>().SaveMap(int.Parse(StageCountText.GetComponent<Text>().text));
    }
    public void LoadMap()
    {
        MapManager.GetComponent<MapEdit>().LoadMap(int.Parse(StageCountText.GetComponent<Text>().text));
    }



    // �X�e�[�W�̃T�C�Y��ύX���郁�\�b�h
    public void StageSize_Increment()
    {
        int num = int.Parse(StageCountText.GetComponent<Text>().text);
        num += 2;
        StageCountText.GetComponent<Text>().text = num.ToString();
        MapManager.GetComponent<MapEdit>().StageScaleUp();
    }
    public void StageSize_Decrement()
    {
        int num = int.Parse(StageCountText.GetComponent<Text>().text);
        if (num > 2)
        {
            num -= 2;
            StageCountText.GetComponent<Text>().text = num.ToString();
            MapManager.GetComponent<MapEdit>().StageScaleDown();
        }
    }
}
