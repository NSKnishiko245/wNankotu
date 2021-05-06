using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    // サウンド
    [SerializeField] private AudioSource selectBgmSource;
    [SerializeField] private AudioSource selectDecSource;

    [SerializeField] private GameObject eventSystem;

    [SerializeField] private float sceneChangeTime; // シーン遷移までの時間
    private int sceneChangeCnt = 0;                 // シーン遷移のカウンタ
    private bool sceneChangeFlg = false;

    [SerializeField] private int pageIntervalInit;  // ページをめくれるまでの待機時間の初期値
    private int pageInterval = 0;

    [SerializeField] private int operationCntInit;  // シーン遷移して操作可能になるまでの時間
    private int operationCnt = 0;

    private GameObject[] goldImage;
    private GameObject[] silverImage;
    private GameObject[] copperImage;
    public struct Score
    {
        public bool isGold;
        public bool isSilver;
        public bool isCopper;
    }
    public static Score[] score = new Score[51];
    public static int[] silverConditions = new int[51];

    private void Awake()
    {
        pageInterval = pageIntervalInit;
        operationCnt = operationCntInit;

        goldImage = GameObject.FindGameObjectsWithTag("GoldImage");
        silverImage = GameObject.FindGameObjectsWithTag("SilverImage");
        copperImage = GameObject.FindGameObjectsWithTag("CopperImage");

        //ScoreReset();            
        SilverConditionsSet();

        this.GetComponent<PostEffectController>().SetVigFlg(false);
    }

    private void Update()
    {
        if (operationCnt == 1) eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();

        if (operationCnt == 0)
        {
            if (!eventSystem.GetComponent<IgnoreMouseInputModule>().GetAllBackFlg())
            {
                PageOperation();
            }
        }
        else operationCnt--;

        SceneChange();      // シーン遷移

        if (!eventSystem.GetComponent<IgnoreMouseInputModule>().GetAllBackFlg())
        {
            ScoreDisplay();
        }
    }

    // ページをめくる
    private void PageOperation()
    {
        if (pageInterval == 0)
        {
            // 次のページへ進む
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick button 5"))
            {
                eventSystem.GetComponent<IgnoreMouseInputModule>().NextPage();
                pageInterval = pageIntervalInit;
            }
            // 前のページに戻る
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick button 4"))
            {
                eventSystem.GetComponent<IgnoreMouseInputModule>().BackPage();
                pageInterval = pageIntervalInit;
            }
        }
        else pageInterval--;
    }

    // シーン遷移
    private void SceneChange()
    {
        if (!sceneChangeFlg && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0")))
        {
            selectDecSource.Play();

            // 現在のページ取得(ステージ番号)
            StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();

            // シーン遷移開始
            if (StageManager.stageNum > 0)
            {
                // 本を閉じる
                eventSystem.GetComponent<IgnoreMouseInputModule>().AllBackPage();
                // 部屋を暗くする
                this.GetComponent<PostEffectController>().SetVigFlg(true);

                sceneChangeFlg = true;
            }
        }

        if (eventSystem.GetComponent<IgnoreMouseInputModule>().GetBookCloseFlg())
        {
            // 遷移
            SceneManager.LoadScene("Stage1Scene");
        }
    }

    private void ScoreDisplay()
    {
        // 現在のページ取得(ステージ番号)
        StageManager.stageNum = eventSystem.GetComponent<IgnoreMouseInputModule>().GetPageNum();

        for (int i = 0; i < goldImage.Length; i++)
        {
            if (score[StageManager.stageNum].isGold) goldImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else goldImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }

        for (int i = 0; i < silverImage.Length; i++)
        {
            if (score[StageManager.stageNum].isSilver) silverImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else silverImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }

        for (int i = 0; i < copperImage.Length; i++)
        {
            if (score[StageManager.stageNum].isCopper) copperImage[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else copperImage[i].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    // 銀メダルの獲得条件をセット(ステージを折った回数)
    private void SilverConditionsSet()
    {
        silverConditions[0] = 3;
        silverConditions[1] = 5;
        silverConditions[2] = 20;
        silverConditions[3] = 20;
        silverConditions[4] = 20;
        silverConditions[5] = 20;
        silverConditions[6] = 20;
        silverConditions[7] = 20;
        silverConditions[8] = 20;
        silverConditions[9] = 20;
        silverConditions[10] = 20;
    }
}