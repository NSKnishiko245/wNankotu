using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public GameObject StageCountText;
    public GameObject MapManager;
    public GameObject StageManager;

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
    public void StageCount_Increment_Game()
    {
        int num = int.Parse(StageCountText.GetComponent<Text>().text);
        num++;
        StageCountText.GetComponent<Text>().text = num.ToString();
        LoadMap();
        StageManager.GetComponent<StageManager>().ResetStage(int.Parse(StageCountText.GetComponent<Text>().text));
    }
    public void StageCount_Decrement_Game()
    {
        int num = int.Parse(StageCountText.GetComponent<Text>().text);
        if (num > 1)
        {
            num--;
            StageCountText.GetComponent<Text>().text = num.ToString();
            LoadMap();
            StageManager.GetComponent<StageManager>().ResetStage(int.Parse(StageCountText.GetComponent<Text>().text));
        }
    }

    // ���ݍ쐬���Ă���X�e�[�W���O���t�@�C��(CSV)�ɏ����o��
    public void OnClick_Save()
    {
        MapManager.GetComponent<MapEdit>().SaveMap(int.Parse(StageCountText.GetComponent<Text>().text));
    }
    public void LoadMap()
    {
        if (MapManager.GetComponent<MapEdit>().mode == Mode.Edit)
        {
            MapManager.GetComponent<MapEdit>().LoadMap(int.Parse(StageCountText.GetComponent<Text>().text));
        }
        else
        {
            MapManager.GetComponent<MapEdit>().CreateStage_Game(int.Parse(StageCountText.GetComponent<Text>().text));
            StageManager.GetComponent<StageManager>().ResetStage(int.Parse(StageCountText.GetComponent<Text>().text));
        }
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
