using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public bool PlayerCanMove;

  [SerializeField] private float timeMultiplier = 5f;
  [SerializeField] private float downForce = -10f;
  [SerializeField] private float jumpForce = 3f;
  [SerializeField] private float maxRotationAngle = 50f;

  private Vector3 direction = Vector3.zero;

  private void Update()
  {
    if (PlayerCanMove)
    {
      MoveVertical();
      CalculateAngle();
    }
  }

  private void MoveVertical()
  {
    direction.y = Input.GetMouseButtonDown(0)
      ? jumpForce
      : direction.y + (downForce * Time.deltaTime);

    transform.Translate(direction * Time.deltaTime * timeMultiplier, Space.World);
  }

  private void CalculateAngle()
  {
    var rot = Mathf.Clamp(direction.y * 45, -maxRotationAngle, maxRotationAngle);
    transform.eulerAngles = new Vector3(0, 0, rot);
  }
}
