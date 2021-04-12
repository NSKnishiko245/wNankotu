//============================
//name:StageManager
//�T�v:�^�C����܂�Ȃ��鏈��
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

    public GameObject UnderBorder;

    //�p�[�e�B�N���`��p
    public GameObject particleObject;

    // �X�e�[�W�ԍ�
    public int stageNum = 1;
    private bool initFlg = true;

    private bool isInputOff = false;

    private int HitBarIdx   = -1;   // �v���C���[�ƏՓ˂����o�[
    private int LeftBarIdx  = -1;   // ��ԍ��̃o�[
    private int RightBarIdx = -1;   // ��ԉE�̃o�[

    public bool IsGameOver { get; private set; }
    public bool IsGameClear { get; private set; }

    private enum CONTROLLERSTATE
    {
        L_TRIGGER = -1,
        R_TRIGGER = 1,
    }
    private enum STICKSTATE
    {
        L_DOWN = -1,
        R_DOWN = 1,
    }
    private enum WARPSTATE
    {
        TO_LEFT,    // ���փ��[�v
        TO_RIGHT,   // �E�փ��[�v
    }

    // ��ԏ��߂̍X�V�������ɍs������������
    void Init()
    {
        // �}�b�v�G�f�B�b�g�N���X����}�b�v�\�z���\�b�h���R�[�����A���̃}�b�v�f�[�^���擾
        MapManager.GetComponent<MapEdit>().CreateStage_Game(stageNum);
        Block_Map = MapManager.GetComponent<MapEdit>().BlockList;
        BlockNum_Map = MapManager.GetComponent<MapEdit>().BlockMap;
        Tile_List = MapManager.GetComponent<MapEdit>().TileList;
        Bar_List = MapManager.GetComponent<MapEdit>().BarList;
        // �}�b�v����v���C���[�̏����ʒu��{���A�v���C���[��z�u
        RespawnPlayer();
        // �X�e�[�W���I�u�W�F�N�g���P�̃I�u�W�F�N�g�̎q���ɂ���
        // �X�e�[�W�̏�����
        GetComponent<StageRotate>().Init();
        ParentReset();

        // �t���O������
        IsGameClear = IsGameOver = false;
    }

    void Update()
    {
        if (initFlg)
        {
            Init();
            initFlg = false;
        }

        // ���X�e�B�b�N�̓��͒l���擾
        float L_Stick_Value = Input.GetAxis("Horizontal");

        HitBarIdx = GetHitBarIndex();       // �v���C���[�ƏՓ˒��̃o�[���擾
        LeftBarIdx = GetLeftBarIndex();     // �X�e�[�W�̈�ԍ��̃o�[���擾
        RightBarIdx = GetRightBarIndex();   // �X�e�[�W�̈�ԉE�̃o�[���擾

        // �X�e�[�W����~���Ă��鎞�A�v���C���[�𓮂�����
        if (isStopStage())
        {
            // �v���C���[�̍X�V�A�v���C���[�ɂ�����ړ��\�̈�̐ݒ�Ȃ�
            Player.GetComponent<Player>().TurnOnMove();
            if (Bar_List.Count > 0)
            {
                Player.GetComponent<Player>().BorderLine_l = Bar_List[LeftBarIdx].transform.position.x;
                Player.GetComponent<Player>().BorderLine_r = Bar_List[RightBarIdx].transform.position.x;
            }
            Player.transform.parent = null;

            // �v���C���[�����[�̃o�[�ɐڐG�����ꍇ
            if (isLeftBar(HitBarIdx))
            {
                if (L_Stick_Value == (int)STICKSTATE.L_DOWN && isInputOff)
                {
                    // �E���փ��[�v�\���ǂ���
                    if (WarpCheck(WARPSTATE.TO_RIGHT))
                    {
                        WarpPlayer(WARPSTATE.TO_RIGHT); // �E���փ��[�v
                    }
                }
                if (L_Stick_Value == 0) isInputOff = true;
            }
            // �v���C���[���E�[�̃o�[�ɐڐG�����ꍇ
            else if (isRightBar(HitBarIdx))
            {
                if (L_Stick_Value == (int)STICKSTATE.R_DOWN && isInputOff)
                {
                    // �����փ��[�v�\���ǂ���
                    if (WarpCheck(WARPSTATE.TO_LEFT))
                    {
                        WarpPlayer(WARPSTATE.TO_LEFT);  // �����փ��[�v
                    }
                }
                if (L_Stick_Value == 0) isInputOff = true;
            }
            else
            {
                isInputOff = false;
            }
        }
        else
        {
            // �X�e�[�W�������Ă��鎞�̓v���C���[���~
            Player.GetComponent<Player>().TurnOffMove();
        }

        // �E�X�e�B�b�N����̓��͏����擾
        float R_Stick_Value = Input.GetAxis("Horizontal2");

        // �X�e�[�W�̏󋵂����ĉ�]�ł��邩�ǂ����𔻒f
        if (CanYouRotate())
        {
            // �v���C���[�ɏՓ˂��Ă���o�[���������ꍇ�A�g���K�[�̓��͒l���Q�Ƃ���]������
            if (R_Stick_Value == (int)CONTROLLERSTATE.R_TRIGGER || Input.GetKeyDown(KeyCode.J))
            {
                RotateBar(HitBarIdx, BarRotate.ROTSTATEOUTERDATA.ROTATE_LEFT);
            }
            if (R_Stick_Value == (int)CONTROLLERSTATE.L_TRIGGER || Input.GetKeyDown(KeyCode.L))
            {
                RotateBar(HitBarIdx, BarRotate.ROTSTATEOUTERDATA.ROTATE_RIGHT);
            }
        }

        // ��]�ς݂̃X�e�[�W��߂�����
        if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.K))
        {
            // ��]�ς݂̃o�[�����o�����猳�ɖ߂���]�������J�n
            for (int i = 0; i < Bar_List.Count; i++)
            {
                if (Bar_List[i].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED)
                {
                    RotateBar(i, BarRotate.ROTSTATEOUTERDATA.REROTATE);
                }
            }
        }

        //if (GetComponent<StageRotate>().isRotNow)
        //{
        //    Player.GetComponent<MeshRenderer>().enabled = false;
        //}
        //else
        //{
        //    Player.GetComponent<MeshRenderer>().enabled = true;
        //}

        // �Q�[���N���A���m
        if (Player.GetComponent<Player>().IsHitGoalBlock)
        {
            IsGameClear = true;
        }
        // �Q�[���I�[�o�[���m
        if (UnderBorder.GetComponent<HitAction>().isHit)
        {
            IsGameOver = true;
        }
    }

    // �o�[�̉�]����
    private void RotateBar(int bar_idx, BarRotate.ROTSTATEOUTERDATA rotstate)
    {
        Bar_List[bar_idx].GetComponent<BarRotate>().ReverseRotateFlg = GetComponent<StageRotate>().isReverse;
        // �X�e�[�W��܂钼�O�A�o�[�ɃX�e�[�W�����]���Ă��邩�ǂ�����m�点��
        if (rotstate != BarRotate.ROTSTATEOUTERDATA.REROTATE)
        {
            ParentReset();
            SettingParent(rotstate);
            // �܂������̈ʒu�␳
            Vector3 playerPos = Player.transform.position;
            playerPos.x = Bar_List[bar_idx].transform.position.x;
            if (rotstate == BarRotate.ROTSTATEOUTERDATA.ROTATE_LEFT)
            {
                playerPos.x += Player.transform.localScale.x * 0.6f;
            }
            if (rotstate == BarRotate.ROTSTATEOUTERDATA.ROTATE_RIGHT)
            {
                playerPos.x -= Player.transform.localScale.x * 0.6f;
            }
            Player.transform.position = playerPos;
        }
        // ��]����
        Bar_List[bar_idx].GetComponent<BarRotate>().Rotation(rotstate);
    }

    // ��]�\���ǂ������`�F�b�N
    private bool CanYouRotate()
    {
        // �o�[���v���C���[�ƏՓ˂��ĂȂ����A��]�����Ȃ�
        if (HitBarIdx < 0) return false;

        // �o�[���X�e�[�W�̗��[�Ɉʒu���鎞�A��]�����Ȃ�
        if (HitBarIdx == LeftBarIdx || HitBarIdx == RightBarIdx) return false;

        // ��]�����]��̃o�[���������ꍇ�A��]�����Ȃ�
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (Bar_List[i].GetComponent<BarRotate>().RotateState != BarRotate.ROTSTATEINNERDATA.NEUTRAL) return false;
        }

        // �v���C���[�ƏՓ˂��Ă���o�[�̗��ׂɃu���b�N���������ꍇ�́A��]�����Ȃ�
        Vector3 ray_pos = new Vector3(Bar_List[HitBarIdx].transform.position.x, Player.transform.position.y, Player.transform.localScale.z / 2.0f);
        Ray ray = new Ray(ray_pos + Vector3.left * Bar_List[HitBarIdx].transform.localScale.x / 2.0f, Vector3.right);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            if (hit.collider.CompareTag("Block")) return false;
        }
        ray = new Ray(ray_pos + Vector3.right * Bar_List[HitBarIdx].transform.localScale.x / 2.0f, Vector3.left);
        if (Physics.Raycast(ray, out hit, 0.5f))
        {
            if (hit.collider.CompareTag("Block")) return false;
        }

        return true;
    }

    // �v���C���[�ƏՓ˂��Ă���o�[��Ԃ��i�߂�l�� -1 �ȊO�Ȃ�Փ˂��Ă���j
    private int GetHitBarIndex()
    {
        int rotBarNum;
        for (rotBarNum = Bar_List.Count - 1; rotBarNum >= 0; rotBarNum--)
        {
            // �J�E���g�ϐ����v���C���[�ƏՓ˂��Ă���o�[�Ɠ����ɂȂ�΃��[�v�𔲂���
            if (Bar_List[rotBarNum].GetComponent<BarRotate>().IsHit) break;
        }
        return rotBarNum;
    }

    // ��]���O�ɐe�q�֌W���`������
    private void SettingParent(BarRotate.ROTSTATEOUTERDATA rotstate)
    {
        if (rotstate == BarRotate.ROTSTATEOUTERDATA.ROTATE_LEFT)
        {
            // �E���獶�֏��ɐe�ɂ��Ă����i�Q�̃o�[���r�������A�v�f�ԍ��̏����������e�j
            for (int cnt = Bar_List.Count - 1; cnt > 0; cnt--)
            {
                Tile_List[cnt - 1].transform.parent = Bar_List[cnt].transform;
                Bar_List[cnt - 1].transform.parent = Tile_List[cnt - 1].transform;
            }
        }
        if (rotstate == BarRotate.ROTSTATEOUTERDATA.ROTATE_RIGHT)
        {
            // ������E�֏��ɐe�ɂ��Ă����i�Q�̃o�[���r�������A�v�f�ԍ��̑傫�������e�j
            for (int cnt = 0; cnt < Bar_List.Count - 1; cnt++)
            {
                Tile_List[cnt].transform.parent = Bar_List[cnt].transform;
                Bar_List[cnt + 1].transform.parent = Tile_List[cnt].transform;
            }
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
        if (bar_idx < 0) return false;
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
        if (bar_idx < 0) return false;
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
    private int GetLeftBarIndex()
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
    private int GetRightBarIndex()
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
    
    // ���[�v�\���ǂ����𔻒f�i���[�v��Ƀu���b�N�����邩�ǂ����𒲂ׂ�j
    private bool WarpCheck(WARPSTATE state)
    {
        Vector3 pos = new Vector3(0.0f, -Player.transform.position.y, Player.transform.localScale.z / 2.0f);
        Vector3 rayDistance = Vector3.zero;
        if (state == WARPSTATE.TO_LEFT)
        {
            pos.x = Bar_List[LeftBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.localScale.x / 2.0f;
            rayDistance = Vector3.right;
        }
        if (state == WARPSTATE.TO_RIGHT)
        {
            pos.x = Bar_List[RightBarIdx].transform.position.x + Bar_List[RightBarIdx].transform.localScale.x / 2.0f;
            rayDistance = Vector3.left;
        }
        Ray ray = new Ray(pos, rayDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            if (hit.collider.CompareTag("Block")) return false;
        }
        return true;
    }
    // �v���C���[��Ίp����Ƀ��[�v������
    private void WarpPlayer(WARPSTATE state)
    {
        Vector3 pos = new Vector3(0.0f, -Player.transform.position.y, 0.0f);
        if (state == WARPSTATE.TO_LEFT)
        {
            Transform barTrans = Bar_List[LeftBarIdx].transform;
            pos.x = Player.transform.localScale.x + barTrans.position.x - barTrans.localScale.x / 2.0f;
        }
        if (state == WARPSTATE.TO_RIGHT)
        {
            Transform barTrans = Bar_List[RightBarIdx].transform;
            pos.x += -Player.transform.localScale.x + barTrans.position.x + barTrans.localScale.x / 2.0f;
        }
        Player.transform.position = pos;
        GetComponent<StageRotate>().TurnOnRotate();
        Player.transform.parent = this.transform;
    }

    // ���݃X�e�[�W����~���Ă��邩�ǂ����i�X�e�[�W��o�[����]���Ă��Ȃ����ǂ����j
    private bool isStopStage()
    {
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (!(Bar_List[i].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.NEUTRAL ||
                Bar_List[i].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED) ||
                GetComponent<StageRotate>().isRotNow)
            {
                return false;
            }
        }
        return true;
    }
    // �}�b�v����v���C���[�i���o�[��{�����̈ʒu�Ƀv���C���[��z�u
    private void RespawnPlayer()
    {
        for (int y = 0; y < Block_Map.GetLength(0); y++)
        {
            for (int x = 0; x < Block_Map.GetLength(1); x++)
            {
                if (BlockNum_Map[y, x] != 0)
                {
                    if (Block_Map[y, x].transform.tag == "Player")
                    {
                        Player.transform.position = Block_Map[y, x].transform.position;
                    }
                }
            }
        }
    }
    public void ResetStage(int stage_idx)
    {
        stageNum = stage_idx;
        initFlg = true;
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