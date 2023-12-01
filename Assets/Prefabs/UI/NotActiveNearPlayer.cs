using UnityEngine;

public class NotActiveNearPlayer : MonoBehaviour
{
    [SerializeField] float deactivationDistance = 1f; // ����������, ��� ������� ������ ���������� ��������.

    private void Update()
    {
        if (PlayerReference.player == null)
        {
            Debug.LogError("�� ����������� ������ �� ��������� ������!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerReference.player.transform.position);

        if (distanceToPlayer <= deactivationDistance)
        {
            GetComponent<BoxCollider>().gameObject.SetActive(false);
        }
        else
        {
            GetComponent<BoxCollider>().gameObject.SetActive(true);
        }
    }
}