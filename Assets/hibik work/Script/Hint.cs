using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class Hint : MonoBehaviour
{
    // �q���g�y�[�W��UI
    public GameObject hintCanvas;
    public GameObject hintBoard;
    public GameObject hintMovie;

    // �q���g�ʒmUI
    public GameObject tv;
    public GameObject hukidasi;
    public GameObject eKeyIcon;

    // �f�o�b�O�p�e�L�X�g
    public Text hintTimeText;

    [SerializeField]
    private float hintDispTime = 180.0f;    // �q���g�ʒmUI������鎞��
    [SerializeField]
    private float hintDeleteTime = 190.0f;  // �q���g�ʒmUI�������鎞��

    public static bool hintFlg = false;         // �q���g�\���\��

    private static int retryCnt = 0;            // ���g���C������
    private static float playTime = 0.0f;       // �X�e�[�W�̃v���C���� 
    private static bool hintNoticeFlg = false;  // �q���g�ʒmUI��\���������Ƃ����邩

    //==============================================================
    // ��������
    //==============================================================
    private void Start()
    {
        tv.transform.localScale = new Vector3(0, 0, 0);
        hukidasi.transform.localScale = new Vector3(0, 0, 0);

        // �q���g������Z�b�g
        hintMovie.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Movie/Hint/stage_" + StageManager.stageNum + "_hint");
    }

    //==============================================================
    // �X�V����
    //==============================================================
    private void Update()
    {
        if (StageManager.stageNum != 1)
        {
            // �v���C���ԉ��Z
            if (StageUIManager.status == StageUIManager.STATUS.PLAY)
            {
                playTime += Time.deltaTime;
                hintTimeText.text = "�v���C���ԁF" + playTime.ToString("f2");
            }

            // �q���g�ʒmUI�\��
            if (playTime > hintDispTime && !hintNoticeFlg)
            {
                HintNoticeDispChange(true);
            }

            // �q���g�ʒmUI��\��
            if (playTime > hintDeleteTime 
                || StageUIManager.status == StageUIManager.STATUS.HINT
                || StageUIManager.status == StageUIManager.STATUS.CLEAR)
            {
                HintNoticeDispChange(false);
            }
        }
    }

    //==============================================================
    // �q���g�y�[�W��UI�\���ؑ�
    //==============================================================
    public void HintPageDispChange(bool sts)
    {
        hintBoard.SetActive(sts);
        hintMovie.SetActive(sts);
    }

    //==============================================================
    // �q���g�ʒmUI�\���ؑ�
    //==============================================================
    public void HintNoticeDispChange(bool sts)
    {
        if (sts) // �\��
        {
            tv.transform.DOScale(new Vector3(4000, 4000, 1100), 0.5f);
            hukidasi.transform.DOScale(new Vector3(2.5f, 2.5f, 1), 0.5f);

            hintFlg = true;
            hintNoticeFlg = true;
        }
        else // ��\��
        {
            tv.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
            hukidasi.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        }
    }

    //==============================================================
    // �q���g������
    //==============================================================
    public void HintReset()
    {
        playTime = 0;
        hintFlg = false;
        hintNoticeFlg = false;
    }
}
