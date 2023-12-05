using UnityEngine;

public class Menu_Controller : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isMenuActive = false;

    void Start()
    {
        menuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuPanel.SetActive(isMenuActive);

        if (isMenuActive)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ExitGame()
    {
        var wallet = PlayerReference.player.GetComponent<PlayerWallet>();
        if (wallet != null)
        {
            wallet.SaveMoney();
        }
        Application.Quit();
    }
}
