using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
  private GameStates state = GameStates.Menu;
  private Player Player => FindObjectOfType<Player>();
  private bool isGameStarted = false;

  private void OnEnable()
  {
    InputManager.Instance.OnClick += OnGameStarted;
  }

  private void OnDisable()
  {
    InputManager.Instance.OnClick -= OnGameStarted;
  }

  private void OnGameStarted(object sender, bool e)
  {
    if (!isGameStarted)
    {
      isGameStarted = !isGameStarted;
      HandleStates();
    }
  }

  public void ListenState(GameStates newState)
  {
    state = newState;
    HandleStates();
  }

  private void HandleStates()
  {
    switch (state)
    {
      case GameStates.Menu:
        Player.Instance.PlayerCanMove = isGameStarted;
        LevelManager.Instance.StartSpawning();
        break;
      case GameStates.InGame:
        break;
      case GameStates.End:
        Player.Instance.PlayerCanMove = false;
        LevelManager.Instance.StopSpawning();
        break;
    }
  }
}
