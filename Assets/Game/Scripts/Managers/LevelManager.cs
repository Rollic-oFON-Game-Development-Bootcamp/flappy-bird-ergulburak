using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Managers
{
  public class LevelManager : MonoBehaviour
  {
    private const float pipeWidth = 2.5f;
    private const float pipeHeadHeight = 1.25f;
    private const float cameraOrthograficSize = 10f;

    private List<GameObject> obstacles = new List<GameObject>();

    [SerializeField] private float pipeSpeed = 3f;
    [SerializeField] private GameObject pipeHeadPrefab;
    [SerializeField] private GameObject pipeBodyPrefab;

    private void Start()
    {
      //CreatePipe(10f, 10f, true);
      //CreatePipe(10f, 10f, false);
      //CreatePipe(20f, 20f, true);
      //CreatePipe(20f, 20f, false);

      CreateObstacle(10f, 3f, 10f);
      CreateObstacle(8f, 3f, 5f);
      CreateObstacle(5f, 3f, 15f);
      CreateObstacle(15f, 3f, 20f);
    }

    private void Update()
    {
      PipeMovement();
    }

    private void PipeMovement()
    {
      foreach (var obstacle in obstacles)
      {
        obstacle.transform.Translate(new Vector3(-1, 0, 0) * pipeSpeed * Time.deltaTime);
      }
    }

    private void CreateObstacle(float yHeight, float rangeSize, float xPosition)
    {
      var bottomHeight = yHeight - rangeSize / 2f;
      var topHeight = cameraOrthograficSize * 2f - yHeight - rangeSize / 2f;

      CreatePipe(bottomHeight, xPosition, true);
      CreatePipe(topHeight, xPosition, false);
    }

    private void CreatePipe(float height, float xPosition, bool isLookingUp)
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
      pipeHead.transform.position = new Vector3(xPosition, pipeHeadYPos);

      var pipeBody = Instantiate(pipeBodyPrefab);
      pipeBody.transform.position = new Vector3(xPosition, pipeBodyYPos);

      var pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
      pipeBodySpriteRenderer.size = new Vector2(pipeWidth, height);
      pipeBody.transform.localScale = new Vector3(1, isLookingUp ? 1 : -1, 1);

      var pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
      pipeBodyBoxCollider.size = new Vector2(pipeWidth, height);
      pipeBodyBoxCollider.offset = new Vector2(0f, height / 2f);
    }
  }
}
