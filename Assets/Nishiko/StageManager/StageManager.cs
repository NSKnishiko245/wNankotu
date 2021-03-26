//============================
//name:StageManager
//�T�v:�^�C����܂�Ȃ��鏈��
//�x��������������u��
//============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Player Player;

    public GameObject MapManager;

    private int[,] BlockNum_Map;
    private GameObject[,] Block_Map;

    private List<GameObject> Tile_List;
    private List<GameObject> Bar_List;

    //�p�[�e�B�N���`��p
    public GameObject particleObject;


    // �X�e�[�W�ԍ�
    public int stageNum = 1;
    bool firstFlg = false;
        
    void StartEx()
    {
        // �}�b�v�G�f�B�b�g�N���X����}�b�v�\�z���\�b�h���R�[�����A���̃}�b�v�f�[�^���擾
        MapManager.GetComponent<MapEdit>().CreateStage_Game(stageNum);
        Block_Map = MapManager.GetComponent<MapEdit>().BlockList;
        BlockNum_Map = MapManager.GetComponent<MapEdit>().BlockMap;
        Tile_List = MapManager.GetComponent<MapEdit>().TileList;
        Bar_List = MapManager.GetComponent<MapEdit>().BarList;

        // �X�e�[�W���I�u�W�F�N�g���P�̃I�u�W�F�N�g�̎q���ɂ���
        //for (int i = 0; i < Tile_List.Count; i++)
        //{
        //    Tile_List[i].transform.parent = this.transform;
        //}
        //for (int i = 0; i < Bar_List.Count; i++)
        //{
        //    Bar_List[i].transform.parent = this.transform;
        //}
    }

    void Update()
    {
        if (!firstFlg)
        {
            StartEx();
            firstFlg = true;
        }

        // �g���K�[����̓��͏����擾
        float triggerValue = Input.GetAxis("L_R_Trigger");

        int rotBarNum = 0;

        // ��]�\���ǂ������`�F�b�N
        for (int i = 0; i < Bar_List.Count; i++)
        {
            // ��]�����]��̃o�[���������ꍇ�A���͂͂����Ȃ�
            if (Bar_List[i].GetComponent<Cbar>().RotateState != Cbar.ROTATESTATE.NEUTRAL)
            {
                triggerValue = 0.0f;
                break;
            }
            // �v���C���[�ƏՓ˂��Ă���o�[�̔ԍ����擾
            if (Bar_List[i].GetComponent<Cbar>().IsHit)
            {
                rotBarNum = i;
            }
        }

        // �g���K�[�̓��͒l�����Ăǂ���ɉ�]���邩���f
        if (triggerValue >= 1.0f)
        {
            ParentReset();
            SetParentToLeft();
            Bar_List[rotBarNum].GetComponent<Cbar>().Rotation_Left();
        }
        else if (triggerValue <= -1.0f)
        {
            ParentReset();
            SetParentToRight();
            Bar_List[rotBarNum].GetComponent<Cbar>().Rotation_Right();
        }

        
        // ��]�ς݂̃X�e�[�W��߂�����
        if (Input.GetKeyDown("joystick button 0"))
        {

        }
       

        // ���[�v����
        //Vector2Int hitBlockNum = new Vector2Int(-1, -1);
        //for (int y = 0; y < Block_Map.GetLength(0); y++)
        //{
        //    for (int x = 0; x < Block_Map.GetLength(1); x++)
        //    {
        //        if (BlockNum_Map[y, x] != 0)
        //        {
        //            if (Block_Map[y, x].tag == "BorderLine")
        //            {
        //                if (Block_Map[y, x].GetComponent<HitAction>().isHit)
        //                {
        //                    Debug.Log("find");
        //                    hitBlockNum = new Vector2Int(x, y);
        //                    // �Q�d�̃��[�v�𔲂���ׂ̏���
        //                    x = Block_Map.GetLength(1);
        //                    y = Block_Map.GetLength(0);
        //                }
        //            }
        //        }
        //    }
        //}

        //if (hitBlockNum.x != -1)
        //{
        //    if (hitBlockNum.x < Block_Map.GetLength(1) / 2)
        //    {
        //        for (int y = 0; y < Block_Map.GetLength(0); y++)
        //        {
        //            for (int x = Block_Map.GetLength(1) - 1; x >= 0; x--)
        //            {
        //                if (BlockNum_Map[y, x] != 0)
        //                {
        //                    if (Block_Map[y, x].tag == "BorderLine")
        //                    {
        //                        if (y == Block_Map.GetLength(0) - hitBlockNum.y)
        //                        {
        //                            Player.transform.position = new Vector3(
        //                                Block_Map[y, x].transform.position.x - Block_Map[y, x].transform.lossyScale.x,
        //                                Block_Map[y, x].transform.position.y,
        //                                Block_Map[y, x].transform.position.z
        //                            );
        //                            // �Q�d�̃��[�v�𔲂���ׂ̏���
        //                            x = -1;
        //                            y = Block_Map.GetLength(0);

        //                            if (y < Block_Map.GetLength(0) / 2)
        //                            {
        //                                Physics.gravity = new Vector3(0, -9.8f, 0);
        //                            }
        //                            else
        //                            {
        //                                Physics.gravity = new Vector3(0, 9.8f, 0);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int y = 0; y < Block_Map.GetLength(0); y++)
        //        {
        //            for (int x = 0; x < Block_Map.GetLength(1); x++)
        //            {
        //                if (BlockNum_Map[y, x] != 0)
        //                {
        //                    if (Block_Map[y, x].tag == "BorderLine")
        //                    {
        //                        if (y == Block_Map.GetLength(0) - hitBlockNum.y)
        //                        {
        //                            Player.transform.position = new Vector3(
        //                                Block_Map[y, x].transform.position.x + Block_Map[y, x].transform.lossyScale.x,
        //                                Block_Map[y, x].transform.position.y,
        //                                Block_Map[y, x].transform.position.z
        //                            );
        //                            // �Q�d�̃��[�v�𔲂���ׂ̏���
        //                            x = Block_Map.GetLength(1);
        //                            y = Block_Map.GetLength(0);

        //                            if (y < Block_Map.GetLength(0) / 2)
        //                            {
        //                                Physics.gravity = new Vector3(0, -9.8f, 0);
        //                            }
        //                            else
        //                            {
        //                                Physics.gravity = new Vector3(0, 9.8f, 0);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }


    // ������E�֏��ɐe�ɂ��Ă����i�Q�̃o�[���r�������A�v�f�ԍ��̑傫�������e�j
    private void SetParentToRight()
    {
        for (int cnt = Tile_List.Count - 1; cnt > 0; cnt--)
        {
            Tile_List[cnt].transform.parent = Bar_List[cnt - 1].transform;
            Bar_List[cnt - 1].transform.parent = Tile_List[cnt - 1].transform;
        }
    }
    // �E���獶�֏��ɐe�ɂ��Ă����i�Q�̃o�[���r�������A�v�f�ԍ��̏����������e�j
    private void SetParentToLeft()
    {
        for (int cnt = 0; cnt < Tile_List.Count - 1; cnt++)
        {
            Tile_List[cnt].transform.parent = Bar_List[cnt].transform;
            Bar_List[cnt].transform.parent = Tile_List[cnt + 1].transform;
        }
    }
    //�e�q�֌W�����Z�b�g
    private void ParentReset()
    {
        for (int i = 0; i < Tile_List.Count; i++)
        {
            Tile_List[i].transform.parent = this.gameObject.transform;
        }
        for (int i = 0; i < Bar_List.Count; i++)
        {
            Bar_List[i].transform.parent = this.gameObject.transform;
        }
    }
    
    
    //�^�C���̃R���|�[�l���g
    private void TileReset()
    {
        //for (int i = 0; i < m_tileObj.Count; i++)
        //{
        //    m_tileObj[i].GetComponent<ScreenShot>().ResetTexture();
        //}
    }

    //�^�C���̎q����
    private void SetChildActive(GameObject obj,bool flg)
    {
        //obj.transform.Find("Cube").gameObject.SetActive(flg);
        //if (flg)
        //{
        //    Instantiate(particleObject, obj.transform.Find("Cube").transform.position, Quaternion.identity); //�p�[�e�B�N���p�Q�[���I�u�W�F�N�g����
        //}
    }
}
