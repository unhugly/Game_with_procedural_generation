using UnityEngine;

public class HungerBar : MonoBehaviour
{
    private Vector3 originalScale;
    private float maxHunger = 100f;
    private float currentHunger = 0f;
    private float hungerIncreaseRate;

    void Start()
    {
        originalScale = transform.localScale;
        hungerIncreaseRate = maxHunger / (5 * 60); // ������ �������� ���������� ������ ��� ���������� ��������� �� 5 �����
    }

    void Update()
    {
        IncreaseHunger();
        UpdateHungerBar();
    }

    void IncreaseHunger()
    {
        currentHunger += hungerIncreaseRate * Time.deltaTime;
        currentHunger = Mathf.Min(currentHunger, maxHunger);
    }

    void UpdateHungerBar()
    {
        float hungerScale = 1 - (currentHunger / maxHunger);
        transform.localScale = new Vector3(hungerScale * originalScale.x, originalScale.y, originalScale.z);
    }
}



