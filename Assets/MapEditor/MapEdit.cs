using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdit : MonoBehaviour
{
    public GameObject[] prefab;

    private int[,] Map;
    private GameObject[,] ObjMap;

    private Vector2Int MapSize = new Vector2Int(10, 10);

    void Start()
    {
        Map = new int[MapSize.x , MapSize.y];
        ObjMap = new GameObject[MapSize.x, MapSize.y];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int cellpos = GetCellPosFromMousePos();
            if (IsOutArea(cellpos, MapSize))
            {
                Vector2Int elementNum = GetElementNumFormCellPos(cellpos, MapSize);
                //Map[mousePos_result.y + MapSize.y / 2, mousePos_result.x + MapSize.x / 2] = 1;
                Destroy(ObjMap[elementNum.y, elementNum.x]);
                GameObject obj = Instantiate(prefab[0], new Vector3(cellpos.x + 0.5f, cellpos.y + 0.5f, 0.0f), new Quaternion(0, 0, 0, 1));
                ObjMap[elementNum.y, elementNum.x] = obj;

                //Debug.Log(mousePos_result);
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2Int cellpos = GetCellPosFromMousePos();
            if (IsOutArea(cellpos, MapSize))
            {
                Vector2Int elementNum = GetElementNumFormCellPos(cellpos, MapSize);
                Destroy(ObjMap[elementNum.y, elementNum.x]);
            }
        }
    }

    // �}�E�X�ʒu���X�e�[�W�̉��}�X�ڂ���Ԃ�
    private Vector2Int GetCellPosFromMousePos()
    {
        // �}�E�X�J�[�\���̈ʒu���X�N���[�����W���烏�[���h���W��
        Vector3 mousePoint_screen = Input.mousePosition;
        mousePoint_screen.z = 1.0f;
        Vector3 mousePoint_world = Camera.main.ScreenToWorldPoint(mousePoint_screen) * 10.0f;
        // �␳
        if (mousePoint_world.x < 0) mousePoint_world.x--;
        if (mousePoint_world.y < 0) mousePoint_world.y--;
        // �����_�؂�̂�
        return new Vector2Int((int)mousePoint_world.x, (int)mousePoint_world.y);
    }

    // �����œn���ꂽ�}�X�ڈʒu�����Ă��ꂽ�}�b�v�T�C�Y�����ǂ���
    private bool IsOutArea(Vector2Int cellpos, Vector2Int mapSize)
    {
        if (cellpos.x >= MapSize.x / 2 || cellpos.x <= -MapSize.x / 2) return false;
        if (cellpos.y >= MapSize.y / 2 || cellpos.y <= -MapSize.y / 2) return false;

        // �͈͓�
        return true;
    }

    // �w�肳�ꂽ�}�X�ڈʒu���z��̉��Ԗڂ̗v�f���ɂȂ邩��Ԃ�
    private Vector2Int GetElementNumFormCellPos(Vector2Int cellpos, Vector2Int mapSize)
    {
        return new Vector2Int(cellpos.x + mapSize.x / 2, (mapSize.y / 2 - 1) - cellpos.y);
    }
}
