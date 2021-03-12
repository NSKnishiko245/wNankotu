using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageController : MonoBehaviour
{
    private GameObject menuPanel;
    private GameObject retryButton;
    bool menuFlg = false;   // true:���j���[�\���@false:���j���[��\��

    void Awake()
    {
        menuPanel = GameObject.Find("MenuPanel");
        // �����ɂ���
        if (menuPanel != null) menuPanel.SetActive(false);
    }

    void Update()
    {
        // ���j���[�\���؂�ւ�
        if (Input.GetKeyDown(KeyCode.M))
        {
            MenuDisplay();
        }
    }

    public void OnRetryButton()
    {
        // ���݂�Scene�����擾����
        Scene loadScene = SceneManager.GetActiveScene();
        // Scene�̓ǂݒ���
        SceneManager.LoadScene(loadScene.name);
    }

    public void OnSelectButton()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void MenuDisplay()
    {
        if (!menuFlg)
        {
            menuFlg = true;
            // �L����Ԑ؂�ւ�
            menuPanel.SetActive(menuFlg);

            retryButton = GameObject.Find("RetryButton");
            // �I����Ԃɂ���
            EventSystem.current.SetSelectedGameObject(retryButton);
        }
        else
        {
            menuFlg = false;
            // �L����Ԑ؂�ւ�
            menuPanel.SetActive(menuFlg);
        }
    }
}
