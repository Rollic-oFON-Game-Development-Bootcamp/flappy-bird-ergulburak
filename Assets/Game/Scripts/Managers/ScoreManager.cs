using System;
using TMPro;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI scoreText;

  private void Update()
  {
    scoreText.text = LevelManager.Instance.GetPipesPassed().ToString();
  }
}
