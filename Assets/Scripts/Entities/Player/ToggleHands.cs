using UnityEngine;

public class ToggleHands : MonoBehaviour
{
    private GameObject rightHand;
    private GameObject leftHand;

    private float maxSwingAmount = 0.1f; // ћаксимальное смещение руки вперед-назад
    private float swingSpeed = 0.3f; 
    private float originalSwingSpeed;

    private void Start()
    {
        originalSwingSpeed = swingSpeed;

        rightHand = transform.Find("RightHand").gameObject;
        leftHand = transform.Find("LeftHand").gameObject;
        SetBothHands(false);
    }

    private void Update()
    {
        switch (PlayerController.status)
        {
            case Walk_Mode.Walking:
                SetBothHands(true);
                SwingHands();
                break;

            case Walk_Mode.Holding_Position:
                SetBothHands(false);
                break;

            case Walk_Mode.Attack:
                ResetHandsPosition();
                SetBothHands(false);
                rightHand.SetActive(true);
                break;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            swingSpeed = originalSwingSpeed * 2;
        }
        else
        {
            swingSpeed = originalSwingSpeed;
        }
    }

    private void ResetHandsPosition()
    {
        rightHand.transform.localPosition = new Vector3(rightHand.transform.localPosition.x, rightHand.transform.localPosition.y, 0);
        leftHand.transform.localPosition = new Vector3(leftHand.transform.localPosition.x, leftHand.transform.localPosition.y, 0);
    }

    private void SwingHands()
    {
        float swingValue = Mathf.PingPong(Time.time * swingSpeed, maxSwingAmount) - maxSwingAmount / 2;
        rightHand.transform.localPosition = new Vector3(rightHand.transform.localPosition.x, rightHand.transform.localPosition.y, swingValue);
        leftHand.transform.localPosition = new Vector3(leftHand.transform.localPosition.x, leftHand.transform.localPosition.y, -swingValue);
    }

    private void SetBothHands(bool value) 
    {
        rightHand.SetActive(value);
        leftHand.SetActive(value);
    }
}