using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        catch (System.ArgumentException e)
        {
            Debug.LogError($"Ошибка загрузки сцены: {e.Message}");
        }
    }
}
