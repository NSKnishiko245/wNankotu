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
    public GameObject FrontEffectCamera;
    public GameObject Grid;

    public GameObject MapManager;

    private GameObject tutorialManager;

    public GameObject Effect;

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

    public float waitTime = 1.0f;
    private float waitTimer = 0.0f;

    //�p�[�e�B�N���`��p
    public GameObject particleObject;

    // �X�e�[�W�ԍ�
    public static int stageNum = 1;
    public int rotateNum;
    public bool initFlg = true;


    private bool isInputOff = false;

    private int HitBarIdx = -1;   // �v���C���[�ƏՓ˂����o�[
    private int LeftBarIdx = -1;   // ��ԍ��̃o�[
    private int RightBarIdx = -1;   // ��ԉE�̃o�[

    public bool IsGameOver { get; private set; }
    public bool IsGameClear { get; private set; }

    public bool IsRotate { get; private set; }

    public bool IsSmog { get; private set; }

    bool flg = false;
    bool rerotFlg = false;
    bool resetFlg = false;


    bool CanYouCopy = true;
    float timer = 0.0f;


    bool oneFrame = false;

    bool is3D = true;

    public bool isMove { get; private set; }

    public float waitInterval = 0.5f;

    //float firstWaitTime = 1.0f;
    public float firstWaitTimer;


    public float oriInterval = 1.0f;
    private float oriIntervalTimer = 0.0f;


    private int oriBarNum = 0;



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

        //�`���[�g���A���}�l�[�W���[�擾
        tutorialManager = GameObject.Find("TutorialManager");
        // �t���O������
        IsGameClear = IsGameOver = IsRotate = IsSmog = false;

        Tile_subList = new List<GameObject>();
        Bar_subList = new List<GameObject>();

        RotateState = ROTATESTATE.NEUTRAL;
        rotateNum = 0;


        BigParent = new GameObject();
        BigParent.AddComponent<InvisibleBlock>();


        Grid.GetComponent<CreateGrid>().SetAlpha(0);

        foreach (var bar in Tile_List)
        {
            bar.transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
        }
    }

    void Update()
    {
        if (firstWaitTimer > 0)
        {
            FrontEffectCamera.SetActive(false);
            Player.SetActive(false);
            firstWaitTimer -= Time.deltaTime;
            return;
        }

        Player.SetActive(true);

        if (initFlg)
        {
            FrontEffectCamera.SetActive(true);
            Init();
            initFlg = false;
            CreateParticle();
        }

        Debug.Log(Player.transform.GetChild((int)global::Player.PLAYERHITBOX.BOTTOM).gameObject.GetComponent<HitAction>().isHit);

        oriIntervalTimer -= Time.deltaTime;

        // ���X�e�B�b�N�̓��͒l���擾
        float L_Stick_Value = Input.GetAxis("Horizontal");

        HitBarIdx = GetHitBarIndex();       // �v���C���[�ƏՓ˒��̃o�[���擾
        LeftBarIdx = GetLeftBarIndex();     // �X�e�[�W�̈�ԍ��̃o�[���擾
        RightBarIdx = GetRightBarIndex();   // �X�e�[�W�̈�ԉE�̃o�[���擾
        
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    ChangeTileTexture();
        //}

        if (HitBarIdx != -1)
        {
            Grid.GetComponent<CreateGrid>().AlphaIncrease = false;
            if ((!isLeftBar(HitBarIdx) && !isRightBar(HitBarIdx)) && Grid.GetComponent<MeshRenderer>().enabled)
            {
                for (int i = 0; i < Bar_List.Count; i++)
                {
                    if (Bar_List[i].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED) break;
                    if (i == Bar_List.Count - 1)
                    {
                        Grid.GetComponent<CreateGrid>().AlphaIncrease = true;
                    }
                }
            }
        }
        else
        {
            Grid.GetComponent<CreateGrid>().AlphaIncrease = false;
        }

        if (isStopStage() && !Camera.main.GetComponent<MoveCamera>().isMoveEx)
        {
            if (Player.transform.position.x < Bar_List[RightBarIdx].transform.position.x && Player.transform.position.x > Bar_List[LeftBarIdx].transform.position.x)
                isMove = false;
        }
        else isMove = true;


        // �X�e�[�W����~���Ă��鎞�A�v���C���[�𓮂�����
        if (isStopStage() && !Camera.main.GetComponent<MoveCamera>().isMoveEx)
        {
            ChangeBarAlpha();

            // �v���C���[�̍X�V�A�v���C���[�ɂ�����ړ��\�̈�̐ݒ�Ȃ�
            Player.GetComponent<Player>().TurnOnGravity();
            if (CanYouCopy && !rerotFlg)
            {
                Player.GetComponent<Player>().TurnOnMove();
            }
            if (Bar_List.Count > 0)
            {
                Player.GetComponent<Player>().BorderLine_l = Bar_List[LeftBarIdx].transform.position.x;
                Player.GetComponent<Player>().BorderLine_r = Bar_List[RightBarIdx].transform.position.x;
            }
            Player.transform.parent = null;

            if (Player.transform.position.x - Bar_List[LeftBarIdx].transform.position.x <= 1)
            {
                if (!isCopy && !rerotFlg && Player.transform.position.x >= Bar_List[LeftBarIdx].transform.position.x && CanYouCopy)
                {
                    CopyStage(WARPSTATE.TO_RIGHT);
                    isCopy = true;
                }
            }
            // �v���C���[���E�[�̃o�[�ɐڐG�����ꍇ
            else if (Bar_List[RightBarIdx].transform.position.x - Player.transform.position.x <= 1)
            {
                if (!isCopy && !rerotFlg && Player.transform.position.x <= Bar_List[RightBarIdx].transform.position.x && CanYouCopy)
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

            if (!IsGameClear)
            {
                L_Smoke.SetActive(true);
                R_Smoke.SetActive(true);
                if (!L_Smoke.GetComponent<ParticleSystem>().isPlaying)
                {
                    L_Smoke.GetComponent<ParticleSystem>().Play();
                    R_Smoke.GetComponent<ParticleSystem>().Play();
                }
            }

            timer += Time.deltaTime;
            if (timer >= waitInterval) CanYouCopy = true;


            if (!rerotFlg)
            {
                FrontEffectCamera.SetActive(true);
            }

            //SetModeGoalEffect(3);
        }
        else
        {
            timer = 0;
            CanYouCopy = false;

            // �X�e�[�W�������Ă��鎞�̓v���C���[���~
            Player.GetComponent<Player>().TurnOffMove();

            L_Smoke.SetActive(false);
            R_Smoke.SetActive(false);

            //Grid.GetComponent<MeshRenderer>().enabled = false;
        }

        if (IsGameClear)
        {
            L_Smoke.SetActive(false);
            R_Smoke.SetActive(false);

            for (int i = 0; i < Tile_List.Count; i++)
            {
                Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
            }
        }

        // �E�X�e�B�b�N����̓��͏����擾
        float R_Stick_Value = Input.GetAxis("Horizontal2");


        if (oneFrame)
        {
            oneFrame = false;

            for (int i = 0; i < Tile_List.Count; i++)
            {
                Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().TurnOnScreenShot();
            }
        }
        // �X�e�[�W�̏󋵂����ĉ�]�ł��邩�ǂ����𔻒f
        if (CanYouRotate() && !IsGameClear && oriIntervalTimer <= 0.0f)
        {
            if (tutorialManager.GetComponent<tutorialManagaer>().IsRMove)
            {
                // �v���C���[�ɏՓ˂��Ă���o�[���������ꍇ�A�g���K�[�̓��͒l���Q�Ƃ���]������
                if (!FinishManager.menuFlg)
                {
                    if (R_Stick_Value == (int)CONTROLLERSTATE.R_TRIGGER || Input.GetKeyDown(KeyCode.D))
                    {
                        SetModeGoalEffect(0);
                        SetModeGoalEffect(2);
                        RotateBar(HitBarIdx, BarRotate.ROTSTATEOUTERDATA.ROTATE_LEFT);
                        RotateState = ROTATESTATE.L_ROTATE;
                        ScreenShot();
                        rotateNum++;
                        Player.GetComponent<Player>().moveDir = global::Player.MOVEDIR.RIGHT;
                        IsRotate = true;
                        oriBarNum = HitBarIdx;
                    }
                }
            }
            if (tutorialManager.GetComponent<tutorialManagaer>().IsLMove)
            {
                if (!FinishManager.menuFlg)
                {
                    if (R_Stick_Value == (int)CONTROLLERSTATE.L_TRIGGER || Input.GetKeyDown(KeyCode.A))
                    {
                        SetModeGoalEffect(0);
                        SetModeGoalEffect(2);
                        RotateBar(HitBarIdx, BarRotate.ROTSTATEOUTERDATA.ROTATE_RIGHT);
                        RotateState = ROTATESTATE.R_ROTATE;
                        ScreenShot();
                        rotateNum++;
                        Player.GetComponent<Player>().moveDir = global::Player.MOVEDIR.LEFT;
                        IsRotate = true;
                        oriBarNum = HitBarIdx;
                    }
                }
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
                    FrontEffectCamera.SetActive(true);
                    Grid.GetComponent<MeshRenderer>().enabled = false;

                    break;
                }
            }
        }

        if (tutorialManager.GetComponent<tutorialManagaer>().IsRotateMove && oriIntervalTimer <= 0.0f)
        {
            // ��]�ς݂̃X�e�[�W��߂�����
            if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.S))
            {
                IsRotate = false;
                if (!Camera.main.GetComponent<MoveCamera>().isMoveEx && !IsGameClear)
                {
                    // ��]�ς݂̃o�[�����o�����猳�ɖ߂���]�������J�n
                    for (int i = 0; i < Bar_List.Count; i++)
                    {
                        if (Bar_List[i].GetComponent<BarRotate>().RotateState == BarRotate.ROTSTATEINNERDATA.ROTATED)
                        {
                            //rotateNum++;
                            SetModeGoalEffect(0);
                            SetModeGoalEffect(2);
                            FirstFunc();
                            Player.GetComponent<Player>().TurnOffMove();
                            Player.GetComponent<Player>().FixPos();

                            Player.GetComponent<Player>().PlayerFixEx();
                            break;
                        }
                    }
                }
            }
        }

        if (Player.transform.position.x < Bar_List[LeftBarIdx].transform.position.x)
        {
            if (isCopy)
            {
                oriIntervalTimer = oriInterval * 2.0f;
                DecidedStage(WARPSTATE.TO_RIGHT);
            }
        }
        else if (Player.transform.position.x > Bar_List[RightBarIdx].transform.position.x)
        {
            if (isCopy)
            {
                oriIntervalTimer = oriInterval * 3.0f;
                DecidedStage(WARPSTATE.TO_LEFT);
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
            //GameObject goal1 = GameObject.Find("2(Clone)/GoalObj1").gameObject;
            //GameObject goal2 = GameObject.Find("2(Clone)/GoalObj2").gameObject;
            //goal1.GetComponent<GoalScript>().SetStartFlg(true);
            //goal2.GetComponent<GoalScript>().SetStartFlg(true);
            //SetModeGoalEffect(3);
            SetModeGoalEffect(1);
        }
        // �Q�[���I�[�o�[���m
        else if (UnderBorder.GetComponent<HitAction>().isHit || waitTime <= waitTimer || Player.GetComponent<Player>().deadFlg)
        {
            DeleteCopyForMenu();
            IsGameOver = true;
            SetModeGoalEffect(0);
            SetModeGoalEffect(2);
        }
        else if (isStopStage() && !Camera.main.GetComponent<MoveCamera>().isMoveEx && !rerotFlg)
        {
            SetModeGoalEffect(3);
        }

        if (UnderBorder.GetComponent<HitCreateEffect>().isFinished)
        {
            Player.GetComponent<Player>().TurnOffMove();
            waitTimer += Time.deltaTime;
            Player.transform.Find("walk_UV").gameObject.SetActive(false);
        }

        if (!isStopStage())
        {
            resetFlg = true;
            //Player.transform.GetChild((int)global::Player.PLAYERHITBOX.BOTTOM).gameObject.GetComponent<HitAction>().isHit = false;
        }
        else
        {
            if (resetFlg)
            {
                //for (int k = 0; k < Tile_List.Count; k++)
                //{
                //    Tile_List[k].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                //}
                ChangeTileTexture();
                resetFlg = false;
            }
            ParentReset();
        }

        L_Smoke.transform.position = Bar_List[LeftBarIdx].transform.position;
        R_Smoke.transform.position = Bar_List[RightBarIdx].transform.position;

        if (Player.transform.position.x < Bar_List[LeftBarIdx].transform.position.x || Player.transform.position.x > Bar_List[RightBarIdx].transform.position.x)
        {
            Camera.main.GetComponent<MoveCamera>().isMove = false;
        }
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
            //Player.transform.position = playerPos;
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
        //oriIntervalTimer = oriInterval;
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
        if (UnderBorder.GetComponent<HitCreateEffect>().isFinished) return;

        Destroy(BigParent);
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

                    if (child.tag == "GimicMoveBlock")
                    {
                        child.GetComponent<MoveBlock>().TurnOffGravity();
                    }
                    // �^�C������
                    if (child.tag == "GoalBlock")
                    {
                        child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetPlayEffectFlg(false);
                        child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetPlayEffectFlg(false);
                    }

                }
                for (int j = 0; j < Tile_List.Count; j++)
                {
                    foreach (Transform child2 in Tile_List[j].GetComponentsInChildren<Transform>())
                    {
                        if (child.tag == "GoalBlock")
                        {
                            if (child2.tag=="GoalBlock")
                            {
                                //child.transform.Find("GoalObj1").GetComponent<GoalScript>().CreateSandPerticle();
                                //child.transform.Find("GoalObj2").GetComponent<GoalScript>().CreateSandPerticle();
                                //child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetGearVibrateTime(30);
                                //child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetGearVibrateTime(30);
                                //child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetSilverState(StageSelectManager.silverConditions[stageNum], rotateNum);
                                //child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetSilverState(StageSelectManager.silverConditions[stageNum], rotateNum);
                                //Debug.Log(StageSelectManager.silverConditions[StageManager.stageNum]);
                                //
                                if (child2.transform.Find("GoalObj1").GetComponent<GoalScript>().GetIsNoSilverGear())
                                {
                                    child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetIsNoSilverGear(true);
                                    child.transform.Find("GoalObj1").GetComponent<GoalScript>().GetGear().GetComponent<MeshRenderer>().enabled = false;
                                }
                                if (child2.transform.Find("GoalObj2").GetComponent<GoalScript>().GetIsNoSilverGear())
                                {
                                    child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetIsNoSilverGear(true);
                                    //child.transform.Find("GoalObj2").GetComponent<GoalScript>().GetGear().SetActive(false);
                                    child.transform.Find("GoalObj2").GetComponent<GoalScript>().GetGear().GetComponent<MeshRenderer>().enabled = false;
                                }
                            }
                        }
                    }
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

            //if (i == LeftBarIdx || i == RightBarIdx)
            //{
            //    Bar_List[i].GetComponent<MeshRenderer>().enabled = false;
            //}
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
                IsSmog = true;
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
    private void DecidedStage(WARPSTATE state)
    {
        SetModeGoalEffect(0);
        SetModeGoalEffect(2);

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

        foreach (var tile in Tile_List)
        {
            foreach (Transform child in tile.transform)
            {
                if (child.tag == "GimicMoveBlock")
                {
                    child.GetComponent<MoveBlock>().TurnOnGravity();
                }
                if (child.tag == "GoalBlock")
                {
                    child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetPlayEffectFlg(true);
                    child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetPlayEffectFlg(true);
                }
            }
        }

        //Destroy(BigParent);
        //BigParent = new GameObject();

        Destroy(yugami);

        CopyStage(state, true);
        //isCopy = true;


        //BigParent.GetComponent<InvisibleBlock>().SetAlpha(0.2f);
        //BigParent.GetComponent<InvisibleBlock>().AlphaState = InvisibleBlock.ALPHASTATE.DECREASE;

        //float addPos = (yugami.transform.localScale.x * 10.0f) / 2.0f;

        //yugami.transform.Translate(Vector3.left * MapPos_Add.x);


        Camera.main.GetComponent<MoveCamera>().Move(MapPos_Add.x);



        Grid.transform.position += MapPos_Add;

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

    public void DeleteCopyForMenu()
    {
        if (!initFlg)
        {
            Tile_subList.Clear();
            Bar_subList.Clear();
        }
        Destroy(BigParent);
        Destroy(yugami);

        isCopy = false;
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
                    //if (is3D != activeFlg)
                    //{
                    //    GameObject ef = Instantiate(Effect);
                    //    ef.transform.position = childTransform.position;
                    //    ef.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                    //    ef.GetComponent<ParticleSystem>().Play();
                    //}
                }
            }
        }

        if (!activeFlg)
        {
            foreach (var obj in Bar_List)
            {
                obj.GetComponent<MeshRenderer>().enabled = activeFlg;
            }
        }
        else
        {
            ChangeBarAlpha();
        }

        if (is3D == false && activeFlg == true)
        {
            Grid.GetComponent<MeshRenderer>().enabled = true;
            if (RotateState != ROTATESTATE.NEUTRAL)
            {
                // �������������I�I�I
                for (int i = 0; i < Tile_List.Count; i++)
                {
                    foreach (Transform child in Tile_List[i].GetComponentsInChildren<Transform>())
                    {
                        if (child.tag == "GoalBlock")
                        {
                            if (!child.transform.Find("GoalObj1").GetComponent<GoalScript>().GetIsNoSilverGear())
                            {
                                child.transform.Find("GoalObj1").GetComponent<GoalScript>().CreateSandPerticle();
                                child.transform.Find("GoalObj2").GetComponent<GoalScript>().CreateSandPerticle();
                                child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetGearVibrateTime(30);
                                child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetGearVibrateTime(30);
                                child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetSilverState(StageSelectManager.silverConditions[stageNum], rotateNum);
                                child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetSilverState(StageSelectManager.silverConditions[stageNum], rotateNum);
                                //Debug.Log(StageSelectManager.silverConditions[StageManager.stageNum]);
                            }
                        }
                    }
                }
            }
        }
        if (is3D != activeFlg)
        {
            oriIntervalTimer = oriInterval;
        }
        is3D = activeFlg;

        float x = Bar_List[RightBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.position.x;
        int result;
        float excess = x % 1.0f;
        if (Mathf.Abs(excess) >= 0.5)
        {
            result = Mathf.CeilToInt(x);
        }
        else
        {
            result = Mathf.FloorToInt(x);
        }

        Grid.GetComponent<CreateGrid>().ReGrid(result, 0);
        Grid.transform.position = new Vector3(
            Bar_List[LeftBarIdx].transform.position.x + (Bar_List[RightBarIdx].transform.position.x - Bar_List[LeftBarIdx].transform.position.x) / 2.0f, 0.0f, 0.0f
        );
    }

    private void ScreenShot()
    {
        oneFrame = true;

        Player.transform.Find("walk_UV").gameObject.SetActive(false);
        FrontEffectCamera.SetActive(false);
        Grid.GetComponent<MeshRenderer>().enabled = false;
        Grid.GetComponent<CreateGrid>().SetAlpha(0);
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
        FrontEffectCamera.SetActive(false);
        DeleteCopy();
        isCopy = false;
        Grid.GetComponent<MeshRenderer>().enabled = false;
        Grid.GetComponent<CreateGrid>().SetAlpha(0);

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
        FrontEffectCamera.SetActive(true);
        rerotFlg = false;
        RotateState = ROTATESTATE.NEUTRAL;

        //Player.GetComponent<Player>().TurnOnGravity();

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

    // 0 --> ���� , 1 --> �N���A�� , 2 --> ��~ , 3 --> �Đ�
    public void SetModeGoalEffect(int modeCnt)
    {
        // �^�C������
        for (int i = 0; i < Tile_List.Count; i++)
        {
            foreach (Transform child in Tile_List[i].GetComponentsInChildren<Transform>())
            {
                if (child.tag == "GoalBlock")
                {
                    switch (modeCnt) {
                        case 0:
                            child.transform.Find("GoalObj1").GetComponent<GoalScript>().ClearParticle();
                            child.transform.Find("GoalObj2").GetComponent<GoalScript>().ClearParticle();
                            break;
                        case 1:
                            child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetStartFlg(true);
                            child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetStartFlg(true);
                            break;
                        case 2:
                            child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetPlayEffectFlg(false);
                            child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetPlayEffectFlg(false);
                            break;
                        case 3:
                            child.transform.Find("GoalObj1").GetComponent<GoalScript>().SetPlayEffectFlg(true);
                            child.transform.Find("GoalObj2").GetComponent<GoalScript>().SetPlayEffectFlg(true);
                            break;
                        case 4:
                            child.transform.Find("GoalObj1").GetComponent<GoalScript>().ChangeColor(GoalScript.E_ParticleColor.GOLD); ;
                            child.transform.Find("GoalObj2").GetComponent<GoalScript>().ChangeColor(GoalScript.E_ParticleColor.GOLD);;
                            break;
                    }
                    return;
                }
            }
        }
    }

    public void FixPlayerPos()
    {
        if (Player.transform.position.x > Bar_List[RightBarIdx].transform.position.x)
        {
            Player.transform.position = new Vector3(Bar_List[RightBarIdx].transform.position.x - 0.1f, Player.transform.position.y, Player.transform.position.z);
        }
        else if (Player.transform.position.x < Bar_List[LeftBarIdx].transform.position.x)
        {
            Player.transform.position = new Vector3(Bar_List[LeftBarIdx].transform.position.x + 0.1f, Player.transform.position.y, Player.transform.position.z);
        }
    }

    private void ChangeBarAlpha()
    {
        for (int i = 0; i < Bar_List.Count; i++)
        {
            if (isLeftBar(i) || isRightBar(i))
            {
                if (i == oriBarNum && RotateState != ROTATESTATE.NEUTRAL)
                {
                    Bar_List[i].GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    Bar_List[i].GetComponent<MeshRenderer>().enabled = false;
                }

            }
            else
            {
                if (RotateState != ROTATESTATE.NEUTRAL)
                {
                    Bar_List[i].GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    Bar_List[i].GetComponent<MeshRenderer>().enabled = true;
                }
            }

            if (i == 0 || i == Bar_List.Count - 1)
            {
                if (!(isLeftBar(i) || isRightBar(i)))
                {
                    if (RotateState != ROTATESTATE.NEUTRAL)
                    {
                        Bar_List[i].GetComponent<MeshRenderer>().enabled = true;
                    }
                    else
                    {
                        Bar_List[i].GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
        }
        if ((isLeftBar(0) || isRightBar(0)) && (isLeftBar(Bar_List.Count - 1) || isRightBar(Bar_List.Count - 1)))
        {
            if (RotateState != ROTATESTATE.NEUTRAL)
            {
                Bar_List[0].GetComponent<MeshRenderer>().enabled = true;
            }
        }

    }

    public void CreateParticle()
    {
        foreach (var obj in Tile_List)
        {
            foreach (Transform childTransform in obj.transform)
            {
                if (childTransform.tag == "Block" || childTransform.tag == "GoalBlock" || childTransform.tag == "GimicMoveBlock" || childTransform.tag == "GimicMoveBar" || childTransform.tag == "GimicBreakBlock" || childTransform.tag == "GimicClearBlock")
                {
                    GameObject ef = Instantiate(Effect);
                    ef.transform.position = childTransform.position;
                    ef.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    ef.GetComponent<ParticleSystem>().Play();
                }
            }
        }
    }

    private void ChangeTileTexture()
    {
        for (int i = 0; i < Tile_List.Count; i++)
        {
            switch (RotateState)
            {
                case ROTATESTATE.L_ROTATE:
                    if (oriBarNum > i) Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture(true);
                    else Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                    break;
                case ROTATESTATE.R_ROTATE:
                    if (oriBarNum <= i) Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture(true);
                    else Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                    break;
                case ROTATESTATE.NEUTRAL:
                    Tile_List[i].transform.Find("TileChild").GetComponent<ScreenShot>().ResetTexture();
                    break;
            }
        }
    }
}