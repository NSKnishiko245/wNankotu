using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OPMovie : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;

    public AudioClip sound1;
    AudioSource audioSource;
    void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
        //Component���擾
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 0"))
        {
            //��(sound1)��炷
            audioSource.PlayOneShot(sound1);
            SceneManager.LoadScene("TitleScene");
        }

    }

    public void LoopPointReached(VideoPlayer vp)
    {

        // ����Đ��������̏���
        SceneManager.LoadScene("TitleScene");
    }
}
