using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Transform TeamMenue;
    private void Start()
    {
        TeamMenue.gameObject.SetActive(false);
    }

    //开始游戏
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    //组员信息界面
    public void TeamButton()
    {
        TeamMenue.gameObject.SetActive(true);
    }

    public void close()
    {
        TeamMenue.gameObject.SetActive(false);
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

}
