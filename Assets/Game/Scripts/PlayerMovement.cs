using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float timeMultiplier = 5f;
  [SerializeField] private float downForce = -10f;
  [SerializeField] private float jumpForce = 3f;
  [SerializeField] private float maxRotationAngle = 50f;

  private Vector3 direction = Vector3.zero;

  private void Update()
  {
    MoveVertical();
    CalculateAngle();
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
    var rot = Mathf.Clamp(direction.y * 90, -maxRotationAngle, maxRotationAngle);
    transform.eulerAngles = new Vector3(0, 0, rot);
  }
}
