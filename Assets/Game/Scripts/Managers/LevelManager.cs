using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
  private List<Obstacle> obstacles = new List<Obstacle>();
  private float pipeRangeLimit = 6f;
  private int pipesSpawned;
  private int pipesPassed;
  private bool isTheGameBeingPlayed;

  [SerializeField] private float pipeSpeed = 5f;
  [SerializeField] [Range(.2f, 5f)] private float pipeSpawnTimer;
  [SerializeField] private GameObject pipeParentPrefab;
  [SerializeField] private GameObject pipeHeadPrefab;
  [SerializeField] private GameObject pipeBodyPrefab;
  [SerializeField] [ReadOnly] private Difficulty difficulty = Difficulty.Easy;

  private void Update()
  {
    if (isTheGameBeingPlayed)
      PipeMovement();
  }

  private void PipeMovement()
  {
    foreach (var obstacle in obstacles.ToList())
    {
      bool isPlayerPassTheObstacle = obstacle.GetObstacleXPosition() > LevelConsts.playerXPosition;
      obstacle.Move();
      if (isPlayerPassTheObstacle && obstacle.GetObstacleXPosition() <= LevelConsts.playerXPosition)
        pipesPassed++;
      if (obstacle.GetObstacleXPosition() < LevelConsts.pipeDestroyPosition)
      {
        Destroy(obstacle.gameObject);
        obstacles.Remove(obstacle);
      }
    }
  }

  public void StartSpawning()
  {
    SetDifficulty();
    InvokeRepeating(nameof(PipeSpawning), 0f, pipeSpawnTimer);
    isTheGameBeingPlayed = true;
  }

  public void StopSpawning()
  {
    isTheGameBeingPlayed = false;
    CancelInvoke(nameof(PipeSpawning));
  }


  private void SetDifficulty()
  {
    CalculateDifficulty();
    switch (difficulty)
    {
      case Difficulty.Easy:
        pipeRangeLimit = 10f;
        break;
      case Difficulty.Medium:
        pipeRangeLimit = 8f;
        break;
      case Difficulty.Hard:
        pipeRangeLimit = 6f;
        break;
      case Difficulty.Impossible:
        pipeRangeLimit = 5f;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
    }
  }

  private void CalculateDifficulty()
  {
    if (pipesSpawned >= 30) difficulty = Difficulty.Impossible;
    if (pipesSpawned >= 20) difficulty = Difficulty.Hard;
    if (pipesSpawned >= 10) difficulty = Difficulty.Medium;
    difficulty = Difficulty.Easy;
  }

  private void PipeSpawning()
  {
    var heightEdgeLimit = 2f;
    var minHeight = pipeRangeLimit * .5f + heightEdgeLimit;
    var maxHeight = LevelConsts.pipeHeightLimit - pipeRangeLimit * .5f - heightEdgeLimit;
    var randomHeight = Random.Range(minHeight, maxHeight);
    CreateObstacle(randomHeight);
    pipesSpawned++;
    SetDifficulty();
  }

  private void CreateObstacle(float randomHeight)
  {
    var pipeParent = Instantiate(pipeParentPrefab);
    var pipeScript = pipeParent.GetComponent<Obstacle>();
    pipeScript.Initialize(pipeBodyPrefab, pipeHeadPrefab, randomHeight, pipeRangeLimit, pipeSpeed);
    obstacles.Add(pipeScript);
  }

  public int GetPipesPassed()
  {
    return pipesPassed;
  }
}
