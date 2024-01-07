using System.IO;
using UnityEngine;

public class Menu_Controller : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isMenuActive = false;
    private static bool loaded = false;

    void Start()
    {
        menuPanel.SetActive(false);
        if (loaded == false)
        {
            Inventory.LoadInventory();
            loaded = true;
        }
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

        Inventory.SaveInventory();

        Application.Quit();
    }
}
