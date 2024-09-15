using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    private Texture DefaultTexture;
    private Vector2 DefaultScale;

    private bool IsFinishedScreenShot;
    public bool isFinishedScreenShot()
    {
        if (IsFinishedScreenShot)
        {
            IsFinishedScreenShot = false;
            return true;
        }
        return false;
    }

    void Start()
    {
        // ������Texture��ێ����Ă����i�摜���Z�b�g�p�j
        DefaultTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false);

        DefaultTexture = GetComponent<Renderer>().material.GetTexture("_BaseMap");

        // �e�q�֌W���`�������ƃX�P�[�����ς��̂ŁA���߂Ɏ擾���Ă���
        DefaultScale = new Vector2(transform.parent.localScale.x, transform.parent.localScale.y);

        IsFinishedScreenShot = false;
    }

    void Update()
    {
    }


    // �w�肵���͈͂̃X�N�V�������A���̃X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g�ɓ\��
    // �����ŃX�N�V���������͈͂̍����̓_�Ɣ͈͂̕���n���i���͈͂̎w��̓X�N���[�����W�Łj
    private IEnumerator SelectAreaScreenShot(Vector2Int leftBottom, int width, int height)
    {
        // �X�N�V���͕`���ɂ����s���Ȃ��̂ŁA�`���Ɏ��s
        yield return new WaitForEndOfFrame();

        // �X�N���[���S�̂��X�N�V�����A���̃f�[�^��ێ�
        Vector2Int size = new Vector2Int(Screen.width, Screen.height);
        Texture2D screeTex = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);
        screeTex.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
        screeTex.Apply();

        

        // ��ŎB�����X�N�V���f�[�^����w�肵���͈͂̃s�N�Z�����擾
        Color[] colors = screeTex.GetPixels(leftBottom.x, leftBottom.y, width, height);

        // �擾�����s�N�Z���f�[�^�Ńe�N�X�`�����쐬
        Texture2D tex2 = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex2.SetPixels(colors);
        tex2.Apply();

        // �I�u�W�F�N�g�̃}�e���A���Ƀe�N�X�`�����Z�b�g
        gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", tex2);

        IsFinishedScreenShot = true;
    }

    // �����e�N�X�`���Ƀ��Z�b�g
    public void ResetTexture(bool isReverse = false)
    {
        string texname = "";
        if (isReverse)
        {
            if (StageManager.stageNum <= 6)
                texname = "Prefabs/Material/bG_L1ex";
            else if (StageManager.stageNum <= 12)
                texname = "Prefabs/Material/bG_L2ex";
            else if (StageManager.stageNum <= 18)
                texname = "Prefabs/Material/bG_L3ex";
            else if (StageManager.stageNum <= 24)
                texname = "Prefabs/Material/bG_L4ex";
            else if (StageManager.stageNum <= 30)
                texname = "Prefabs/Material/bG_L5ex";
            else if (StageManager.stageNum <= 36)
                texname = "Prefabs/Material/bG_L6ex";

            GetComponent<Renderer>().rendererPriority = 2;
            GetComponent<Renderer>().material.renderQueue = 2;
        }
        else
        {
            if (StageManager.stageNum <= 6)
                texname = "Prefabs/Material/bG_L1";
            else if (StageManager.stageNum <= 12)
                texname = "Prefabs/Material/bG_L2";
            else if (StageManager.stageNum <= 18)
                texname = "Prefabs/Material/bG_L3";
            else if (StageManager.stageNum <= 24)
                texname = "Prefabs/Material/bG_L4";
            else if (StageManager.stageNum <= 30)
                texname = "Prefabs/Material/bG_L5";
            else if (StageManager.stageNum <= 36)
                texname = "Prefabs/Material/bG_L6";

            GetComponent<Renderer>().rendererPriority = 1;
            GetComponent<Renderer>().material.renderQueue = 1;
        }

        DefaultTexture = Resources.Load(texname) as Texture2D;
        gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", DefaultTexture);
    }

    public void TurnOnScreenShot()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);

        // ���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�̃T�C�Y�ŃX�N�V�����B��
        RectTransform trans = gameObject.GetComponent<RectTransform>();
        Vector2 pos = new Vector2(trans.parent.position.x, trans.parent.position.y);

        // quad�̍����̓_�ƉE��̓_���擾
        Vector2 leftBottom = pos - DefaultScale / 2.0f;
        Vector2 rightTop = pos + DefaultScale / 2.0f;
        // ���������Ă��鎞�͕␳
        //if (gameObject.transform.parent.localRotation.y != 0.0f)
        //{
        //    leftBottom.x -= DefaultScale.x;
        //    rightTop.x += DefaultScale.x;
        //}
        leftBottom = RectTransformUtility.WorldToScreenPoint(Camera.main, leftBottom);
        rightTop = RectTransformUtility.WorldToScreenPoint(Camera.main, rightTop);

        // �X�N�V�����Ă݂��I
        StartCoroutine(SelectAreaScreenShot(new Vector2Int((int)leftBottom.x, (int)leftBottom.y), (int)(rightTop.x - leftBottom.x), (int)(rightTop.y - leftBottom.y)));

        IsFinishedScreenShot = false;
    }

    public void TurnTexture()
    {
        transform.Rotate(180, 0, 0);
    }
    public void ReverseTexture()
    {
        transform.Rotate(0, 180, 0);
    }
}