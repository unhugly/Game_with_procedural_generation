using UnityEngine;

public class MapCamera: MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex;

    void Start()
    {
        currentCameraIndex = 0;

        // Деактивируем все камеры, кроме первой
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        // Активируем первую камеру
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Переключение камер при нажатии клавиши C
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Следующая камера
            currentCameraIndex++;
            if (currentCameraIndex < cameras.Length)
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
            else
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
        }
    }
}
