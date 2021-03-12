using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

// �C���X�y�N�^�[���烂�[�h�I��������
public enum Mode
{
    Game = 0,
    Edit = 1
}

public class MapEdit : MonoBehaviour
{
    private List<GameObject> Block;
    private int BlockIndex = 1;

    private int[,] Map;
    private GameObject[,] ObjMap;

    private Vector2Int MapSize = new Vector2Int(20, 20);

    public Mode mode = Mode.Game;

    private GameObject MainCamera;
    private float NoInputBottom;

    void Start()
    {
        MainCamera = GameObject.FindWithTag("MainCamera");
        NoInputBottom = 0.15f;

        Map = new int[MapSize.x , MapSize.y];
        ObjMap = new GameObject[MapSize.x, MapSize.y];

        Block = new List<GameObject>();
        for (int i = 1; true; i++)
        {
            if (!File.Exists(Application.dataPath + "/Resources/Prefabs/" + i.ToString() + ".prefab"))
            {
                return;
            }
            Block.Add(Resources.Load("Prefabs/" + i.ToString()) as GameObject);
        }
    }

    void Update()
    {
        if (mode == Mode.Edit)
        {
            InputForEdit();
        }
    }

    // �}�E�X�ʒu���X�e�[�W�̉��}�X�ڂ���Ԃ�
    private Vector2Int GetCellPosFromMousePos()
    {
        // �}�E�X�J�[�\���̈ʒu���X�N���[�����W���烏�[���h���W��
        Vector3 mousePoint_screen = Input.mousePosition;
        mousePoint_screen.z = 1.0f;
        Vector3 mousePoint_world = Camera.main.ScreenToWorldPoint(mousePoint_screen) * (-MainCamera.transform.position.z);
        mousePoint_world.x -= MainCamera.transform.position.x * (-MainCamera.transform.position.z - 1.0f);
        mousePoint_world.y -= MainCamera.transform.position.y * (-MainCamera.transform.position.z - 1.0f);
        // �␳
        if (mousePoint_world.x < 0) mousePoint_world.x--;
        if (mousePoint_world.y < 0) mousePoint_world.y--;
        // �����_�؂�̂�
        return new Vector2Int((int)mousePoint_world.x, (int)mousePoint_world.y);
    }

    // �����œn���ꂽ�}�X�ڈʒu�����Ă��ꂽ�}�b�v�T�C�Y�����ǂ���
    private bool IsOutArea(Vector2Int cellpos, Vector2Int mapSize)
    {
        if (cellpos.x >= mapSize.x / 2 || cellpos.x < -mapSize.x / 2) return false;
        if (cellpos.y >= mapSize.y / 2 || cellpos.y < -mapSize.y / 2) return false;

        // �͈͓�
        return true;
    }

    // �w�肳�ꂽ�}�X�ڈʒu���z��̉��Ԗڂ̗v�f���ɂȂ邩��Ԃ�
    private Vector2Int GetElementNumFormCellPos(Vector2Int cellpos, Vector2Int mapSize)
    {
        return new Vector2Int(cellpos.x + mapSize.x / 2, (mapSize.y / 2 - 1) - cellpos.y);
    }

    public void SaveMap(string fileName)
    {
        gameObject.GetComponent<CsvWrite>().WriteMapFromCsv(Map, fileName);
    }
    public void LoadMap(string fileName)
    {
        // ���[�h���s���� Map �� 0 ����
        if (!gameObject.GetComponent<CsvWrite>().ReadMapFromCsv(Map, fileName))
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                for (int x = 0; x < MapSize.x; x++)
                {
                    Map[y, x] = 0;
                }
            }
        }
        ReloadMap();
    }

    private void CreateBlock(Vector2Int cellpos, int blockIdx)
    {
        // �w�肵���u���b�N�̃v���n�u�����݂��Ȃ��ꍇ�͒��f
        if (blockIdx > Block.Count) return;

        // �}�E�X�̈ʒu����A���̈ʒu�ɑΉ������}�b�v�̗v�f���ɕϊ�
        Vector2Int elementNum = GetElementNumFormCellPos(cellpos, MapSize);

        // �}�b�v�Ƀu���b�N��o�^
        Map[elementNum.y, elementNum.x] = blockIdx;

        // ��x�w�肳�ꂽ�ꏊ�̃I�u�W�F�N�g���폜�A�̂�����
        Destroy(ObjMap[elementNum.y, elementNum.x]);
        GameObject obj = Instantiate(Block[blockIdx - 1], new Vector3(cellpos.x + 0.5f, cellpos.y + 0.5f, 0.0f), new Quaternion(0, 0, 0, 1));
        ObjMap[elementNum.y, elementNum.x] = obj;
    }
    private void DeleteBlock(Vector2Int cellpos)
    {
        Vector2Int elementNum = GetElementNumFormCellPos(cellpos, MapSize);

        // �}�b�v����u���b�N���폜
        Map[elementNum.y, elementNum.x] = 0;
        Destroy(ObjMap[elementNum.y, elementNum.x]);
    }

    private void ReloadMap()
    {
        // ���������͍���̃Z���ʒu���Z�b�g
        Vector2Int cellPos_leftTop = new Vector2Int(-MapSize.x / 2, (MapSize.y / 2) - 1);
        Vector2Int cellPos = cellPos_leftTop;
        for (int y = 0; y < MapSize.y; y++)
        {
            for (int x = 0; x < MapSize.x; x++)
            {
                if (Map[y, x] != 0)
                {
                    CreateBlock(cellPos, Map[y, x]);
                }
                else
                {
                    DeleteBlock(cellPos);
                }
                cellPos.x++;
            }
            cellPos.x = cellPos_leftTop.x;
            cellPos.y--;
        }
    }

    private void InputForEdit()
    {
        if (Input.mousePosition.y > Screen.height * NoInputBottom)
        {
            // �u���b�N�̐ݒu
            if (Input.GetMouseButtonDown(0))
            {
                Vector2Int cellpos = GetCellPosFromMousePos();
                if (IsOutArea(cellpos, MapSize))
                {
                    CreateBlock(cellpos, BlockIndex);
                }
            }
            // �u���b�N�̍폜
            if (Input.GetMouseButtonDown(1))
            {
                Vector2Int cellpos = GetCellPosFromMousePos();
                if (IsOutArea(cellpos, MapSize))
                {
                    Vector2Int elementNum = GetElementNumFormCellPos(cellpos, MapSize);

                    // �}�b�v����u���b�N���폜
                    Map[elementNum.y, elementNum.x] = 0;
                    Destroy(ObjMap[elementNum.y, elementNum.x]);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) BlockIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) BlockIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) BlockIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) BlockIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) BlockIndex = 5;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MapEdit))]
public class MapEditEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapEdit myTarget = (MapEdit)target;
        myTarget.mode = (Mode)EditorGUILayout.EnumPopup(myTarget.mode);
    }
}
#endif