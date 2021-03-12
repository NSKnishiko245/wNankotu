using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleController : MonoBehaviour
{
    private GameObject startButton;

    void Awake()
    {
        startButton = GameObject.Find("StartButton");
        // 選択状態にする
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("SelectScene");
    }

}
