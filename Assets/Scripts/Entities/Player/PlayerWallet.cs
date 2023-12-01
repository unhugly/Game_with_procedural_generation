using TMPro;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public static float money = 0;

    private void Update()
    {
        if (GameObject.FindWithTag("Wallet") != null)
            GameObject.FindWithTag("Wallet").GetComponent<TextMeshProUGUI>().text = money.ToString();
    }
}