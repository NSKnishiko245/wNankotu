using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class SamnaleMovie : MonoBehaviour
{
    /// ���䂷��Video Player�̃��X�g
    /// </summary>
    [SerializeField]private VideoPlayer playMovie;

    public int playFrame;
    // Start is called before the first frame update
    void Start()
    {
        playMovie.Play();
        //playMovie.frame = 1;
        //playMovie.time = 1;
        playMovie.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        playFrame--;
        if (playFrame < 0)
        {
            //playMovie.frame = 0;
            //playMovie.time = 0;
            //return;
            if (!playMovie.isPlaying)
            {
                playMovie.Play();
                Debug.Log("�Đ�");
            }
        }

        //if (!this.enabled)
        //{

        //}

    }
}
