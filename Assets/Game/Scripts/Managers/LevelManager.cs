using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Managers
{
  public class LevelManager : MonoBehaviour
  {
    private const float pipeHeadHeight = 1.25f;
    private const float cameraOrthograficSize = 10f;
    private const float pipeHeightLimit = 20f;
    private const float pipeDestroyPosition = -30f;
    private const float pipeSpawnPosition = 30f;

    private List<Pipe> obstacles = new List<Pipe>();
    private float pipeRangeLimit = 6f;
    private int pipesSpawned;

    [SerializeField] private float pipeSpeed = 3f;
    [SerializeField] [Range(.2f, 5f)] private float pipeSpawnTimer;
    [SerializeField] private GameObject pipeParentPrefab;
    [SerializeField] private GameObject pipeHeadPrefab;
    [SerializeField] private GameObject pipeBodyPrefab;

    private void Awake()
    {
      StartSpawning();
      SetDifficulty(Difficulty.Easy);
    }

    private void Update()
    {
      PipeMovement();
    }

    private void PipeMovement()
    {
      foreach (var obstacle in obstacles.ToList())
      {
        obstacle.Move();
        if (obstacle.PipeXPosition() < pipeDestroyPosition)
        {
          Destroy(obstacle.gameObject);
          obstacles.Remove(obstacle);
        }
      }
    }

    public void StartSpawning()
    {
      InvokeRepeating(nameof(PipeSpawning), 0f, pipeSpawnTimer);
    }

    public void StopSpawning()
    {
      CancelInvoke(nameof(PipeSpawning));
    }


    private void SetDifficulty(Difficulty difficulty)
    {
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

    private Difficulty GetDifficulty()
    {
      if (pipesSpawned >= 30) return Difficulty.Impossible;
      if (pipesSpawned >= 20) return Difficulty.Hard;
      if (pipesSpawned >= 10) return Difficulty.Medium;
      return Difficulty.Easy;
    }

    private void PipeSpawning()
    {
      var heightEdgeLimit = 2f;
      var minHeight = pipeRangeLimit * .5f + heightEdgeLimit;
      var maxHeight = pipeHeightLimit - pipeRangeLimit * .5f - heightEdgeLimit;
      var randomHeight = Random.Range(minHeight, maxHeight);
      CreateObstacle(randomHeight, pipeRangeLimit, pipeSpawnPosition);
      pipesSpawned++;
      SetDifficulty(GetDifficulty());
    }

    private void CreateObstacle(float yHeight, float rangeSize, float xPosition)
    {
      var bottomHeight = yHeight - rangeSize / 2f;
      var topHeight = cameraOrthograficSize * 2f - yHeight - rangeSize / 2f;

      var bottomList = CreatePipe(bottomHeight, true);
      var topList = CreatePipe(topHeight, false);

      var pipeParent = Instantiate(pipeParentPrefab);

      foreach (var obj in bottomList)
      {
        obj.transform.SetParent(pipeParent.transform);
      }

      foreach (var obj in topList)
      {
        obj.transform.SetParent(pipeParent.transform);
      }

      pipeParent.transform.position = new Vector3(xPosition, 0);

      var pipeRigidBody = pipeParent.AddComponent<Rigidbody2D>();
      pipeRigidBody.isKinematic = true;

      var pipeScript = pipeParent.AddComponent<Pipe>();
      pipeScript.SetTopPipe(topList);
      pipeScript.SetBottomPipe(bottomList);
      pipeScript.SetSpeed(pipeSpeed);

      obstacles.Add(pipeScript);
    }

    private List<GameObject> CreatePipe(float height, bool isLookingUp)
    {
      float pipeHeadYPos;
      float pipeBodyYPos;
      if (isLookingUp)
      {
        pipeHeadYPos = -cameraOrthograficSize + height - pipeHeadHeight * .45f;
        pipeBodyYPos = -cameraOrthograficSize;
      }
      else
      {
        pipeHeadYPos = +cameraOrthograficSize - height + pipeHeadHeight * .45f;
        pipeBodyYPos = +cameraOrthograficSize;
      }

      var pipeHead = Instantiate(pipeHeadPrefab);
      pipeHead.transform.position = new Vector3(0, pipeHeadYPos);

      var pipeBody = Instantiate(pipeBodyPrefab);
      pipeBody.transform.position = new Vector3(0, pipeBodyYPos);
      pipeBody.transform.localScale = new Vector3(1, isLookingUp ? height : -height, 1);

      var pipeBodyList = new List<GameObject>() { pipeBody, pipeHead };
      return pipeBodyList;
    }
  }
}
