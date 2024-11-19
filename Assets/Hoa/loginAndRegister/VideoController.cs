using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button skipButton;
    public GameObject panel;
    public GameObject pnDN;
    public GameObject pnDK;
    public Button btnDN;
    public Button btnDK;
    public Button btnBack;

    private void Start()
    {
        
        if (PlayerPrefs.GetInt("VideoWatched", 0) == 1)
        {
            
            ShowPanel();
        }
        else
        {
           
            skipButton.gameObject.SetActive(false);
            panel.SetActive(false);
            videoPlayer.loopPointReached += OnVideoFinished;
            StartCoroutine(ShowSkipButtonAfter10Seconds());
        }
    }

    private IEnumerator ShowSkipButtonAfter10Seconds()
    {
        yield return new WaitForSeconds(10f);
        skipButton.gameObject.SetActive(true);
    }

    public void OnSkipButtonClicked()
    {
        ShowPanel();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        ShowPanel();
    }

    private void ShowPanel()
    {
        panel.SetActive(true);
        videoPlayer.Stop();
        gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        
        PlayerPrefs.SetInt("VideoWatched", 1);
        PlayerPrefs.Save();
    }

    public void OnSkipDN()
    {
        panel.SetActive(false);
        pnDN.SetActive(true);
    }

    public void OnSkipDK()
    {
        pnDN.SetActive(false);
        pnDK.SetActive(true);
    }
}
