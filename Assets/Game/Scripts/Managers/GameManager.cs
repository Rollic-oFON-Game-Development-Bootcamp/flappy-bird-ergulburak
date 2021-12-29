using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private GameStates states = GameStates.Menu;
  private PlayerMovement playerMovement => FindObjectOfType<PlayerMovement>();
  private bool isGameStarted = false;

  private void Update()
  {
    switch (states)
    {
      case GameStates.Menu:
        if (!isGameStarted)
        {
          isGameStarted = Input.GetMouseButtonDown(0);
          playerMovement.PlayerCanMove = isGameStarted;
        }

        break;
      case GameStates.InGame:
        break;
      case GameStates.End:
        break;
    }
  }
}
