using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateStageSelect : MonoBehaviour
{
    static private int stageMax = 36;      // �X�e�[�W�̍ő吔
    static private int bookStageMax = 6;   // �P��������̃X�e�[�W�̐�


    //==============================================================
    // �X�e�[�W�Z���N�g�쐬
    //==============================================================
    public void Create()
    {
        // �{�I�̖{���폜
        GameObject book = GameObject.Find("BookModel" + (BookSelect.bookNum + 1));
        book.SetActive(false);

        // �{�̃}�e���A����ݒ�
        GameObject bookL = GameObject.Find("book_L2");
        GameObject bookR = GameObject.Find("book_R2");
        GameObject bookBack = GameObject.Find("book_back2");

        Material material = Resources.Load<Material>("Material/Book/Book0" + (BookSelect.bookNum + 1));
        bookL.GetComponent<Renderer>().material = material;
        bookR.GetComponent<Renderer>().material = material;
        bookBack.GetComponent<Renderer>().material = material;

        // �e�L�X�g�t�@�C������^�C�g�������擾
        TextAsset textAsset = Resources.Load("Text/TitleText", typeof(TextAsset)) as TextAsset;
        //��s�Â��
        string[] titleString = textAsset.text.Split('_');

        // �e�L�X�g�t�@�C������X�g�[���[�����擾
        textAsset = Resources.Load("Text/StoryText", typeof(TextAsset)) as TextAsset;
        //��s�Â��
        string[] storyString = textAsset.text.Split('_');


        // �e�y�[�W�̐ݒ�
        for (int i = 0; i < bookStageMax; i++)
        {
            // �e�y�[�W��ݒ�
            GameObject bookStageL = GameObject.Find("BookStageL (" + (i + 1) + ")");
            GameObject bookStageR = GameObject.Find("BookStageR (" + (i + 1) + ")");

            // �X�e�[�W�ԍ�
            int stageNum = BookSelect.bookNum * bookStageMax + i + 1;
            Debug.Log(stageNum);

            //-----------------------------------
            // �^�C�g���e�L�X�g�̐ݒ�
            //-----------------------------------
            Text titleText = bookStageL.transform.Find("TitleText").GetComponent<Text>();
            titleText.text = titleString[stageNum - 1];

            //-----------------------------------
            // �X�g�[���[�e�L�X�g�̐ݒ�
            //-----------------------------------
            Text storyText = bookStageL.transform.Find("StoryText").GetComponent<Text>();
            storyText.text = storyString[stageNum - 1];

            //-----------------------------------
            // ���_����UI�̐ݒ�
            //-----------------------------------
            GameObject goldMedal = bookStageL.transform.Find("Medal/GoldImage").gameObject;
            GameObject silverMedal = bookStageL.transform.Find("Medal/SilverImage").gameObject;
            GameObject copperMedal = bookStageL.transform.Find("Medal/CopperImage").gameObject;

            // ���_�����擾���Ă�����F���t��
            if (StageSelectManager.score[stageNum].isGold)
            {
                goldMedal.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else goldMedal.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

            if (StageSelectManager.score[stageNum].isSilver)
            {
                silverMedal.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else silverMedal.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

            if (StageSelectManager.score[stageNum].isCopper)
            {
                copperMedal.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else copperMedal.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

            //-----------------------------------
            // �X�e�[�W�摜�̐ݒ�
            //-----------------------------------
            Image stageImage = bookStageR.transform.Find("StageImage").GetComponent<Image>();
            Sprite sprite = Resources.Load<Sprite>("Sprite/Stage/st" + stageNum);
            stageImage.sprite = sprite;

            //-----------------------------------
            // �X�e�[�W�ԍ���UI�̐ݒ�
            //-----------------------------------
            Image tensPlaceImage = bookStageR.transform.Find("StageNum/TensPlaceImage").GetComponent<Image>();
            sprite = Resources.Load<Sprite>("Sprite/Number/" + (stageNum / 10));
            tensPlaceImage.sprite = sprite;

            Image onesPlaceImage = bookStageR.transform.Find("StageNum/OnesPlaceImage").GetComponent<Image>();
            sprite = Resources.Load<Sprite>("Sprite/Number/" + (stageNum % 10));
            onesPlaceImage.sprite = sprite;
        }
    }
}