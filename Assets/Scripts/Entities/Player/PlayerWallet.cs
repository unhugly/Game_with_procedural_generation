using TMPro;
using UnityEngine;
using System.IO;

public class PlayerWallet : MonoBehaviour
{
    public static float money = 0;
    private static string filePath;

    private void Awake()
    {
        string dataPath = Application.persistentDataPath;

#if UNITY_EDITOR
        dataPath = Path.Combine(dataPath, "Editor");
#else
    dataPath = Path.Combine(dataPath, "Build");
#endif

        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        filePath = Path.Combine(dataPath, "wallet.json");
        LoadMoney();
    }


    private void Update()
    {
        if (GameObject.FindWithTag("Wallet") != null)
            GameObject.FindWithTag("Wallet").GetComponent<TextMeshProUGUI>().text = money.ToString();
    }

    public static void ResetMoney()
    {
        money = 0;
        PlayerReference.player.GetComponent<PlayerWallet>().SaveMoney();
        PlayerReference.player.GetComponent<PlayerWallet>().LoadMoney();
    }

    private void ToJSON()
    {
        WalletData data = new WalletData();
        data.money = money;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    private void FromJSON()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            WalletData data = JsonUtility.FromJson<WalletData>(json);
            money = data.money;
        }
    }

    public void SaveMoney()
    {
        ToJSON();
    }

    public void LoadMoney()
    {
        FromJSON();
    }
}
