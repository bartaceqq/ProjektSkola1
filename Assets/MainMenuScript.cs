using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject helpmenu;
    public GameObject mainmenu;


    void Start()
    {
        helpmenu.SetActive(false);
        mainmenu.SetActive(true);
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HelpToggle()
    {
        helpmenu.SetActive(!helpmenu.activeSelf);
        mainmenu.SetActive(!mainmenu.activeSelf);

    }

}
