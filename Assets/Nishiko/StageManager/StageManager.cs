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
    private bool initFlg = true;

    private bool turnFlg = false;

    // ��ԏ��߂̍X�V�������ɍs������������
    void Init()
    {
        // �}�b�v�G�f�B�b�g�N���X����}�b�v�\�z���\�b�h���R�[�����A���̃}�b�v�f�[�^���擾
        MapManager.GetComponent<MapEdit>().CreateStage_Game(stageNum);
        Block_Map = MapManager.GetComponent<MapEdit>().BlockList;
        BlockNum_Map = MapManager.GetComponent<MapEdit>().BlockMap;
        Tile_List = MapManager.GetComponent<MapEdit>().TileList;
        Bar_List = MapManager.GetComponent<MapEdit>().BarList;

        // �X�e�[�W���I�u�W�F�N�g���P�̃I�u�W�F�N�g�̎q���ɂ���
        ParentReset();
    }

    void Update()
    {
        if (initFlg)
        {
            Init();
            initFlg = false;
        }



        if (CanYouMovePlayer())
        {
            Player.GetComponent<Player>().TurnOnRigidbody();
            Player.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else
        {
            Player.GetComponent<Player>().TurnOffRigidbody();
        }





        // �g���K�[����̓��͏����擾
        float triggerValue = Input.GetAxis("L_R_Trigger");

        // �X�e�[�W�̏󋵂����ĉ�]�ł��邩�ǂ����𔻒f
        if (CanYouRotate())
        {
            // �v���C���[�ƏՓ˂��Ă���o�[���擾
            int hitBarNum = GetHitBar();
            if (hitBarNum != -1)
            {
                // �X�e�[�W��܂钼�O�A�o�[�ɃX�e�[�W�����]���Ă��邩�ǂ�����m�点��
                for (int i = 0; i < Bar_List.Count; i++)
                {
                    Bar_List[i].GetComponent<Cbar>().ReverseRotateFlg = GetComponent<StageRotate>().isReverse;
                }

                // �E�g���K�[������
                if (triggerValue >= 1.0f)
                {
                    ParentReset();
                    SetParentToLeft();
                    Bar_List[hitBarNum].GetComponent<Cbar>().Rotation(Cbar.ROTATESTATE.ROTATE_LEFT);
                    Player.transform.position = new Vector3(Bar_List[hitBarNum].transform.position.x + 1.0f, Player.transform.position.y, Player.transform.position.z);
                }
                // ���g���K�[������
                else if (triggerValue <= -1.0f)
                {
                    ParentReset();
                    SetParentToRight();
                    Bar_List[hitBarNum].GetComponent<Cbar>().Rotation(Cbar.ROTATESTATE.ROTATE_RIGHT);
                    Player.transform.position = new Vector3(Bar_List[hitBarNum].transform.position.x - 1.0f, Player.transform.position.y, Player.transform.position.z);
                }
            }
        }
        
        // ��]�ς݂̃X�e�[�W��߂�����
        if (Input.GetKeyDown("joystick button 0"))
        {
            // ��]�ς݂̃o�[�����o������߂��������J�n
            for (int i = 0; i < Bar_List.Count; i++)
            {
                if (Bar_List[i].GetComponent<Cbar>().RotateState == Cbar.ROTATESTATE.ROTATED)
                {
                    Bar_List[i].GetComponent<Cbar>().Rotation(Cbar.ROTATESTATE.REROTATE);
                }
            }
        }


        if (CanYouWarp())
        {
            // �X�e�[�W�̉�]�i���j
            if (!GetComponent<StageRotate>().isRotNow)
            {
                Player.transform.parent = null;
                Player.TurnOnRigidbody();


                // �v���C���[�̃��[�v
                int barNum = GetHitBar();
                if (barNum != -1)
                {
                    if (isLeftBar(barNum))
                    {
                        WarpPlayerToRight();
                    }
                    else if (isRightBar(barNum))
                    {
                        WarpPlayerToLeft();
                    }
                }
            }
        }
    }

    // ��]�\���ǂ������`�F�b�N
    private bool CanYouRotate()
    {
        // ��]�����]��̃o�[���������ꍇ�A���͂͂����Ȃ�
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (Bar_List[i].GetComponent<Cbar>().RotateState != Cbar.ROTATESTATE.NEUTRAL)
            {
                return false;
            }
        }
        return true;
    }
    // �v���C���[�ƏՓ˂��Ă���o�[��Ԃ��i�߂�l�� -1 �ȊO�Ȃ�Փ˂��Ă���j
    private int GetHitBar()
    {
        int rotBarNum;
        for (rotBarNum = Bar_List.Count - 1; rotBarNum >= 0; rotBarNum--)
        {
            // �J�E���g�ϐ����v���C���[�ƏՓ˂��Ă���o�[�Ɠ����ɂȂ�΃��[�v�𔲂���
            if (Bar_List[rotBarNum].GetComponent<Cbar>().IsHit) break;
        }
        return rotBarNum;
    }

    // �E���獶�֏��ɐe�ɂ��Ă����i�Q�̃o�[���r�������A�v�f�ԍ��̏����������e�j
    private void SetParentToLeft()
    {
        for (int cnt = Bar_List.Count - 1; cnt > 0; cnt--)
        {
            Tile_List[cnt - 1].transform.parent = Bar_List[cnt].transform;
            Bar_List[cnt - 1].transform.parent = Tile_List[cnt - 1].transform;
        }
    }
    // ������E�֏��ɐe�ɂ��Ă����i�Q�̃o�[���r�������A�v�f�ԍ��̑傫�������e�j
    private void SetParentToRight()
    {
        for (int cnt = 0; cnt < Bar_List.Count - 1; cnt++)
        {
            Tile_List[cnt].transform.parent = Bar_List[cnt].transform;
            Bar_List[cnt + 1].transform.parent = Tile_List[cnt].transform;
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
    

    // �����œn���ꂽ�v�f�ԍ��̃o�[���S�Ẵo�[�̒��ň�ԍ��Ɉʒu���Ă��邩�ǂ���
    private bool isLeftBar(int bar_idx)
    {
        float min_x = Bar_List[bar_idx].transform.position.x;
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (Bar_List[i].transform.position.x < min_x)
            {
                if (min_x - Bar_List[i].transform.position.x > 0.02) return false;
            }
        }
        return true;
    }
    // �����œn���ꂽ�v�f�ԍ��̃o�[���S�Ẵo�[�̒��ň�ԉE�Ɉʒu���Ă��邩�ǂ���
    private bool isRightBar(int bar_idx)
    {
        float max_x = Bar_List[bar_idx].transform.position.x;
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (Bar_List[i].transform.position.x > max_x)
            {
                if (Bar_List[i].transform.position.x - max_x > 0.02f) return false;
            }
        }
        return true;
    }

    // ��ԍ��̃o�[�̗v�f�ԍ����擾
    private int GetMaxLeftBar()
    {
        int bar_idx = 0;
        float min_x = 999;
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (Bar_List[i].transform.position.x < min_x)
            {
                min_x = Bar_List[i].transform.position.x;
                bar_idx = i;
            }
        }
        return bar_idx;
    }
    // ��ԉE�̃o�[�̗v�f�ԍ����擾
    private int GetMaxRightBar()
    {
        int bar_idx = 0;
        float max_x = -999;
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (Bar_List[i].transform.position.x > max_x)
            {
                max_x = Bar_List[i].transform.position.x;
                bar_idx = i;
            }
        }
        return bar_idx;
    }

    private void WarpPlayerToLeft()
    {
        Vector3 leftBar_pos = new Vector3(
            Bar_List[GetMaxLeftBar()].transform.position.x,
            - Player.transform.position.y,
            Player.transform.position.z
        );
        Ray ray = new Ray(leftBar_pos, Vector3.right);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            if (hit.collider.CompareTag("Block"))
            {
                return;
            }
        }
        Player.transform.localPosition = new Vector3(leftBar_pos.x + Player.transform.localScale.x, leftBar_pos.y, leftBar_pos.z);
        turnFlg = true;
        GetComponent<StageRotate>().TurnOnRotate();
        Player.transform.parent = this.transform;
        Player.TurnOffRigidbody();
       
    }
    private void WarpPlayerToRight()
    {
        Vector3 rightBar_pos = new Vector3(
            Bar_List[GetMaxRightBar()].transform.position.x,
            -Player.transform.position.y,
            Player.transform.position.z
        );
        Ray ray = new Ray(rightBar_pos, Vector3.left);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            if (hit.collider.CompareTag("Block"))
            {
                return;
            }
        }
        Player.transform.localPosition = new Vector3(rightBar_pos.x - Player.transform.localScale.x, rightBar_pos.y, rightBar_pos.z);
        turnFlg = true;
        GetComponent<StageRotate>().TurnOnRotate();
        Player.transform.parent = this.transform;
        Player.TurnOffRigidbody();
        
    }

    private bool CanYouMovePlayer()
    {
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (!(Bar_List[i].GetComponent<Cbar>().RotateState == Cbar.ROTATESTATE.NEUTRAL ||
                Bar_List[i].GetComponent<Cbar>().RotateState == Cbar.ROTATESTATE.ROTATED) ||
                GetComponent<StageRotate>().isRotNow)
            {
                return false;
            }
        }
        return true;
    }

    // ���[�v�\���ǂ������`�F�b�N
    private bool CanYouWarp()
    {
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (!(Bar_List[i].GetComponent<Cbar>().RotateState == Cbar.ROTATESTATE.NEUTRAL ||
                Bar_List[i].GetComponent<Cbar>().RotateState == Cbar.ROTATESTATE.ROTATED))
            {
                return false;
            }
        }
        return true;
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
