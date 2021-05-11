//============================
//name:StageManager
//�T�v:�^�C����܂�Ȃ��鏈��
//============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class StageManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject MapManager;

    private int[,] BlockNum_Map;
    private GameObject[,] Block_Map;
    private bool isCopy = false;


    private GameObject BigParent;

    private Vector3 startPos;
    private Vector3 endPos;

    WARPSTATE MoveState;

    private List<GameObject> Tile_List;
    private List<GameObject> Tile_subList;
    private List<GameObject> Bar_List;
    private List<GameObject> Bar_subList;

    private GameObject yugami;

    private Vector3 MapPos;
    private Vector3 MapPos_Add;

    public GameObject UnderBorder;

    public GameObject L_Smoke;
    public GameObject R_Smoke;

    //�p�[�e�B�N���`��p
    public GameObject particleObject;

    // �X�e�[�W�ԍ�
    public static int stageNum = 1;
    public int rotateNum;
    private bool initFlg = true;

    private bool isInputOff = false;

    private int HitBarIdx = -1;   // �v���C���[�ƏՓ˂����o�[
    private int LeftBarIdx = -1;   // ��ԍ��̃o�[
    private int RightBarIdx = -1;   // ��ԉE�̃o�[

    public bool IsGameOver { get; private set; }
    public bool IsGameClear { get; private set; }


    bool flg = false;
    bool rerotFlg = false;
    bool resetFlg = false;


    bool CanYouCopy = true;
    float timer = 0.0f;


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
        NEUTRAL,
        TO_LEFT,    // ���փ��[�v
        TO_RIGHT,   // �E�փ��[�v
    }

    private enum ROTATESTATE
    {
        NEUTRAL,
        L_ROTATE,
        R_ROTATE,
    }
    ROTATESTATE RotateState = ROTATESTATE.NEUTRAL;

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

        Tile_subList = new List<GameObject>();
        Bar_subList = new List<GameObject>();

        RotateState = ROTATESTATE.NEUTRAL;
        rotateNum = 0;


        BigParent = new GameObject();
        BigParent.AddComponent<InvisibleBlock>();



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
        if (isStopStage() && !Camera.main.GetComponent<MoveCamera>().isMoveEx)
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
            //if (isLeftBar(HitBarIdx))
            //{
            //    if (!isCopy && !rerotFlg)
            //    {
            //        CopyStage(WARPSTATE.TO_RIGHT);
            //        isCopy = true;
            //    }
            //}
            //// �v���C���[���E�[�̃o�[�ɐڐG�����ꍇ
            //else if (isRightBar(HitBarIdx))
            //{
            //    if (!isCopy && !rerotFlg)
            //    {
            //        CopyStage(WARPSTATE.TO_LEFT);
            //        isCopy = true;
            //    }
            //}
            //else if (isCopy)
            //{
            //    DeleteCopy();
            //    isCopy = false;
            //}
            if (Player.transform.position.x - Bar_List[LeftBarIdx].transform.position.x < 1)
            {
                if (!isCopy && !rerotFlg && Player.transform.position.x > Bar_List[LeftBarIdx].transform.position.x && CanYouCopy)
                {
                    CopyStage(WARPSTATE.TO_RIGHT);
                    isCopy = true;
                }
            }
            // �v���C���[���E�[�̃o�[�ɐڐG�����ꍇ
            else if (Bar_List[RightBarIdx].transform.position.x - Player.transform.position.x < 1)
            {
                if (!isCopy && !rerotFlg && Player.transform.position.x < Bar_List[RightBarIdx].transform.position.x && CanYouCopy)
                {
                    CopyStage(WARPSTATE.TO_LEFT);
                    isCopy = true;
                }
            }
            else if (isCopy)
            {
                DeleteCopy();
                isCopy = false;
            }


            L_Smoke.SetActive(true);
            R_Smoke.SetActive(true);
            if (!L_Smoke.GetComponent<ParticleSystem>().isPlaying)
            {
                L_Smoke.GetComponent<ParticleSystem>().Play();
                R_Smoke.GetComponent<ParticleSystem>().Play();
            }




            timer += Time.deltaTime;
            if (timer >= 1.0f) CanYouCopy = true;

        }
        else
        {
            timer = 0;
            CanYouCopy = false;

            // �X�e�[�W�������Ă��鎞�̓v���C���[���~
            Player.GetComponent<Player>().TurnOffMove();

            L_Smoke.SetActive(false);
            R_Smoke.SetActive(false);
            //L_Smoke.GetComponent<ParticleSystem>().Stop();
            //R_Smoke.GetComponent<ParticleSystem>().Stop();
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
                RotateState = ROTATESTATE.L_ROTATE;
                ScreenShot();
                rotateNum++;
            }
            if (R_Stick_Value == (int)CONTROLLERSTATE.L_TRIGGER || Input.GetKeyDown(KeyCode.L))
            {
                RotateBar(HitBarIdx, BarRotate.ROTSTATEOUTERDATA.ROTATE_RIGHT);
                RotateState = ROTATESTATE.R_ROTATE;
                ScreenShot();
                rotateNum++;
            }
        }

        if (rerotFlg)
        {
            if (flg)
            {
                SecondFunc();
            }
            if (Tile_List[0].activeSelf)
            {
                if (Tile_List[0].transform.Find("TileChild").GetComponent<ScreenShot>().isFinishedScreenShot())
                {
                    ThirdFunc();
                }
            }
            if (Tile_List[Tile_List.Count - 1].activeSelf)
            {
                if (Tile_List[Tile_List.Count - 1].transform.Find("TileChild").GetComponent<ScreenShot>().isFinishedScreenShot())
                {
                    FourFunc();
                }
            }
        }
        else
        {
            for (int i = 0; i < Tile_List.Count; i++)
            {
                if (Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().isFinishedScreenShot())
                {

                    SetAllBlockActive(false);
                    Player.transform.Find("walk_UV").gameObject.SetActive(true);
                    break;
                }
            }
        }

        // ��]�ς݂̃X�e�[�W��߂�����
        if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.K))
        {
            if (!Camera.main.GetComponent<MoveCamera>().isMoveEx)
            {
                // ��]�ς݂̃o�[�����o�����猳�ɖ߂���]�������J�n
                for (int i = 0; i < Bar_List.Count; i++)
                {
                    if (Bar_List[i].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED)
                    {
                        rotateNum++;
                        FirstFunc();
                        break;
                    }
                }
            }
        }

        if (Player.transform.position.x < Bar_List[LeftBarIdx].transform.position.x)
        {
            if (isCopy)
            {
                DecidedStage(WARPSTATE.TO_RIGHT);
                //isCopy = false;
            }
        }
        else if (Player.transform.position.x > Bar_List[RightBarIdx].transform.position.x)
        {
            if (isCopy)
            {
                DecidedStage(WARPSTATE.TO_LEFT);
                //isCopy = false;
            }
        }

        if (!isCopy && !IsGameClear)
        {
            Camera.main.GetComponent<MoveCamera>().SetPos(Bar_List[LeftBarIdx].transform.position.x + (Bar_List[RightBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.position.x) / 2.0f);
        }

        // �Q�[���N���A���m
        if (Player.GetComponent<Player>().IsHitGoalBlock)
        {
            IsGameClear = true;
        }
        // �Q�[���I�[�o�[���m
        if (UnderBorder.GetComponent<HitAction>().isHit)
        {
            //SceneManager.LoadScene("Stage1Scene");
            IsGameOver = true;
        }



        //foreach (var bar in Bar_List)
        //{
        //    bar.GetComponent<MeshRenderer>().enabled = false;
        //}
        //Bar_List[LeftBarIdx].GetComponent<MeshRenderer>().enabled = true;
        //Bar_List[RightBarIdx].GetComponent<MeshRenderer>().enabled = true;



        if (!isStopStage())
        {
            resetFlg = true;
        }
        else
        {
            if (resetFlg)
            {
                for (int k = 0; k < Tile_List.Count; k++)
                {
                    Tile_List[k].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                }
                resetFlg = false;
            }
            ParentReset();
        }



        L_Smoke.transform.position = Bar_List[LeftBarIdx].transform.position;
        R_Smoke.transform.position = Bar_List[RightBarIdx].transform.position;

    }

    // �o�[�̉�]����
    private void RotateBar(int bar_idx, BarRotate.ROTSTATEOUTERDATA rotstate)
    {
        Bar_List[bar_idx].GetComponent<BarRotate>().ReverseRotateFlg = GetComponent<StageRotate>().isReverse;
        ParentReset();
        // �X�e�[�W��܂钼�O�A�o�[�ɃX�e�[�W�����]���Ă��邩�ǂ�����m�点��
        if (rotstate != BarRotate.ROTSTATEOUTERDATA.REROTATE)
        {
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
        else
        {
            if (RotateState == ROTATESTATE.L_ROTATE)
            {
                SettingParent(BarRotate.ROTSTATEOUTERDATA.ROTATE_LEFT);
            }
            if (RotateState == ROTATESTATE.R_ROTATE)
            {
                SettingParent(BarRotate.ROTSTATEOUTERDATA.ROTATE_RIGHT);
            }
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
        Vector3 ray_pos = new Vector3(Bar_List[HitBarIdx].transform.position.x, Player.transform.position.y, -0.25f);
        Ray ray = new Ray(ray_pos + Vector3.left * Bar_List[HitBarIdx].transform.localScale.x / 2.0f, Vector3.right);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            string tag = hit.collider.tag;
            if (tag == "Block" || tag == "GoalBlock" || tag == "GimicMoveBlock" || tag == "GimicMoveBar" || tag == "GimicBreakBlock" || tag == "ClimbBlock") return false;
        }
        ray = new Ray(ray_pos + Vector3.right * Bar_List[HitBarIdx].transform.localScale.x / 2.0f, Vector3.left);
        if (Physics.Raycast(ray, out hit, 0.5f))
        {
            string tag = hit.collider.tag;
            if (tag == "Block" || tag == "GoalBlock" || tag == "GimicMoveBlock" || tag == "GimicMoveBar" || tag == "GimicBreakBlock" || tag == "ClimbBlock") return false;
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

        SetAllBlockActive(true);
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
    private void CopyStage(WARPSTATE state, bool flg = false)
    {
        BigParent = new GameObject();
        ParentReset();

        // �^�C������
        for (int i = 0; i < Tile_List.Count; i++)
        {
            GameObject tile = GameObject.Instantiate(Tile_List[i]);
            tile.transform.Find("TileChild").GetComponent<ScreenShot>().TurnTexture();
            tile.transform.parent = BigParent.transform;

            foreach (Transform child in tile.GetComponentsInChildren<Transform>())
            {
                if (child.tag == "Block" || child.tag == "GoalBlock" || child.tag == "GimicMoveBlock" || child.tag == "GimicMoveBar" || child.tag == "GimicBreakBlock" || child.tag == "GimicClearBlock")
                {
                    child.parent = tile.transform;
                    child.transform.position = new Vector3(child.transform.position.x, -child.transform.position.y, child.transform.position.z);
                }
            }

            Tile_subList.Add(tile);
        }

        // �o�[����
        for (int i = 0; i < Bar_List.Count; i++)
        {
            GameObject bar = GameObject.Instantiate(Bar_List[i]);
            Bar_List[i].GetComponent<BarRotate>().CopyBarState(bar);
            bar.transform.parent = BigParent.transform;
            Bar_subList.Add(bar);
        }

        BigParent.AddComponent<InvisibleBlock>();
        BigParent.GetComponent<InvisibleBlock>().SetAlpha(0);
        BigParent.GetComponent<InvisibleBlock>().AlphaState = InvisibleBlock.ALPHASTATE.INCREASE;


        // ���炷
        if (state == WARPSTATE.TO_LEFT)
        {
            if (!flg)
            {
                MapPos_Add = Vector3.right * (((Bar_List[RightBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.position.x)));
                BigParent.transform.position = MapPos_Add;
            }
            else
            {
                BigParent.transform.position = Vector3.left * (((Bar_List[RightBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.position.x)));
                if (MapPos_Add.x < 0) MapPos_Add.x = -MapPos_Add.x;
            }
        }
        else
        {
            if (!flg)
            {
                MapPos_Add = Vector3.left * (((Bar_List[RightBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.position.x)));
                BigParent.transform.position = MapPos_Add;
            }
            else
            {
                BigParent.transform.position = Vector3.right * (((Bar_List[RightBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.position.x)));
                if (MapPos_Add.x > 0) MapPos_Add.x = -MapPos_Add.x;
            }
        }


        // 凋C�O����
        if (File.Exists(Application.dataPath + "/Resources/Prefabs/" + "yugami.prefab"))
        {
            yugami = Resources.Load("Prefabs/yugami") as GameObject;
            yugami = GameObject.Instantiate(yugami);
            yugami.transform.localScale = new Vector3(Mathf.Abs(MapPos_Add.x) * 0.1f, Block_Map.GetLength(0), 1.0f);

            float addPos;
            if (state == WARPSTATE.TO_LEFT)
            {
                if (!flg)
                {
                    yugami.transform.position = new Vector3(Bar_List[RightBarIdx].transform.position.x, 0.0f, -0.5f);
                    addPos = (yugami.transform.localScale.x * 10.0f) / 2.0f;
                    yugami.transform.Translate(Vector3.right * addPos);
                }
                else
                {
                    yugami.transform.position = new Vector3(Bar_List[LeftBarIdx].transform.position.x, 0.0f, -0.5f);
                    addPos = (yugami.transform.localScale.x * 10.0f) / 2.0f;
                    yugami.transform.Translate(Vector3.left * addPos);
                }
            }
            else
            {
                if (!flg)
                {
                    yugami.transform.position = new Vector3(Bar_List[LeftBarIdx].transform.position.x, 0.0f, -0.5f);
                    addPos = (yugami.transform.localScale.x * 10.0f) / 2.0f;
                    yugami.transform.Translate(Vector3.left * addPos);
                }
                else
                {
                    yugami.transform.position = new Vector3(Bar_List[RightBarIdx].transform.position.x, 0.0f, -0.5f);
                    addPos = (yugami.transform.localScale.x * 10.0f) / 2.0f;
                    yugami.transform.Translate(Vector3.right * addPos);
                }
            }
        }
    }
    private void CreateCopy()
    {
        // �v���C���[�����[�̃o�[�ɐڐG�����ꍇ
        if (isLeftBar(HitBarIdx))
        {
            if (!isCopy)
            {
                CopyStage(WARPSTATE.TO_RIGHT);
                isCopy = true;
            }
        }
        // �v���C���[���E�[�̃o�[�ɐڐG�����ꍇ
        else if (isRightBar(HitBarIdx))
        {
            if (!isCopy)
            {
                CopyStage(WARPSTATE.TO_LEFT);
                isCopy = true;
            }
        }
        else if (isCopy)
        {
            DeleteCopy();
            isCopy = false;
        }
    }
    private void DecidedStage(WARPSTATE state)
    {


        BigParent.GetComponent<InvisibleBlock>().SetAlpha(1.0f);

        for (int i = 0; i < Tile_List.Count; i++) Tile_List[i].transform.parent = BigParent.transform;
        Tile_List.Clear();

        for (int i = 0; i < Bar_List.Count; i++) Bar_List[i].transform.parent = BigParent.transform;
        Bar_List.Clear();


        Tile_List = new List<GameObject>(Tile_subList);
        Tile_subList.Clear();
        Bar_List = new List<GameObject>(Bar_subList);
        Bar_subList.Clear();

        ParentReset();


        Destroy(BigParent);
        //BigParent = new GameObject();

        Destroy(yugami);

        CopyStage(state, true);
        //isCopy = true;


        //BigParent.GetComponent<InvisibleBlock>().SetAlpha(0.2f);
        //BigParent.GetComponent<InvisibleBlock>().AlphaState = InvisibleBlock.ALPHASTATE.DECREASE;

        //float addPos = (yugami.transform.localScale.x * 10.0f) / 2.0f;

        //yugami.transform.Translate(Vector3.left * MapPos_Add.x);


        Camera.main.GetComponent<MoveCamera>().Move(MapPos_Add.x);


    }
    private void DeleteCopy()
    {
        //for (int i = 0; i < Tile_List.Count; i++) Destroy(Tile_List[i]);
        Tile_subList.Clear();

        //for (int i = 0; i < Bar_List.Count; i++) Destroy(Bar_List[i]);
        Bar_subList.Clear();

        if (isCopy)
        {
            BigParent.GetComponent<InvisibleBlock>().AlphaState = InvisibleBlock.ALPHASTATE.DECREASE;
        }
        //Destroy(BigParent);

        Destroy(yugami);
    }

    private void SetAllBlockActive(bool activeFlg)
    {
        foreach (var obj in Tile_List)
        {
            foreach (Transform childTransform in obj.transform)
            {
                if (childTransform.tag == "Block" || childTransform.tag == "GoalBlock" || childTransform.tag == "GimicMoveBlock" || childTransform.tag == "GimicMoveBar" || childTransform.tag == "GimicBreakBlock" || childTransform.tag == "GimicClearBlock")
                {
                    childTransform.gameObject.SetActive(activeFlg);
                }
            }
        }

        foreach (var obj in Bar_List)
        {
            obj.GetComponent<MeshRenderer>().enabled = activeFlg;
        }
    }

    private void ScreenShot()
    {
        for (int i = 0; i < Tile_List.Count; i++)
        {
            Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
            Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().TurnOnScreenShot();
        }
        Player.transform.Find("walk_UV").gameObject.SetActive(false);
    }

    private void FirstFunc()
    {
        ChangeBlockClear(true);
        rerotFlg = true;
        bool tileActive = true;
        for (int k = 0; k < Tile_List.Count; k++)
        {
            Tile_List[k].SetActive(tileActive);
            if (Bar_List[k + 1].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED)
            {
                tileActive = false;
            }
        }
        Player.transform.Find("walk_UV").gameObject.SetActive(false);
        DeleteCopy();
        isCopy = false;

        flg = true;

    }
    private void SecondFunc()
    {
        bool tileActive2 = true;
        for (int k = 0; k < Tile_List.Count; k++)
        {
            if (tileActive2)
            {
                Tile_List[k].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                Tile_List[k].transform.Find("TileChild").GetComponent<ScreenShot>().TurnOnScreenShot();
            }
            if (Bar_List[k + 1].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED)
            {
                tileActive2 = false;
            }
        }
        flg = false;
    }
    private void ThirdFunc()
    {
        bool tileActive = true;
        for (int k = Tile_List.Count - 1; k >= 0; k--)
        {
            Tile_List[k].SetActive(tileActive);
            if (tileActive)
            {
                Tile_List[k].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                Tile_List[k].transform.Find("TileChild").GetComponent<ScreenShot>().TurnOnScreenShot();
            }
            if (Bar_List[k].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED)
            {
                tileActive = false;
            }
        }
    }
    private void FourFunc()
    {
        for (int i = 0; i < Tile_List.Count; i++)
        {
            Tile_List[i].SetActive(true);
            if (Tile_List[i].transform.localRotation.y != 0.0f)
            {
                Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ReverseTexture();
            }
        }

        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (Bar_List[i].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED)
            {
                RotateBar(i, BarRotate.ROTSTATEOUTERDATA.REROTATE);
                break;
            }
        }
        Player.transform.Find("walk_UV").gameObject.SetActive(true);
        rerotFlg = false;
        RotateState = ROTATESTATE.NEUTRAL;

        SetAllBlockActive(true);
        ChangeBlockClear(false);
        SetAllBlockActive(false);
    }

    private void ChangeBlockClear(bool flg)
    {
        // �^�C������
        for (int i = 0; i < Tile_List.Count; i++)
        {
            foreach (Transform child in Tile_List[i].GetComponentsInChildren<Transform>())
            {
                if (child.tag == "GimicClearBlock")
                {
                    if (flg)
                    {
                        child.GetComponent<ClearBlock>().Clear();
                    }
                    else
                    {
                        child.GetComponent<ClearBlock>().start();
                    }
                }
            }
        }
    }
}