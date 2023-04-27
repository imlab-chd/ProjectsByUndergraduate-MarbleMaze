using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Transform TeamMenue;
    private void Start()
    {
        TeamMenue.gameObject.SetActive(false);
    }

    //��ʼ��Ϸ
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    //��Ա��Ϣ����
    public void TeamButton()
    {
        TeamMenue.gameObject.SetActive(true);
    }

    public void close()
    {
        TeamMenue.gameObject.SetActive(false);
    }

    //�˳���Ϸ
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

}
