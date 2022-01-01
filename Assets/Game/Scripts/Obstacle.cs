using System;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
  private float obstacleSpeed;
  private GameObject topObstacleBody;
  private GameObject topObstacleHead;
  private GameObject bottomObstacleBody;
  private GameObject bottomObstacleHead;

  public void Initialize(GameObject obstacleBodyPrefab, GameObject obstacleHeadPrefab, float yHeight, float rangeSize,
    float speed)
  {
    obstacleSpeed = speed;
    var bottomHeight = yHeight - rangeSize / 2f;
    var topHeight = LevelConsts.cameraOrthograficSize * 2f - yHeight - rangeSize / 2f;

    var bottomList = CreateObstacle(obstacleBodyPrefab, obstacleHeadPrefab, bottomHeight, true);
    var topList = CreateObstacle(obstacleBodyPrefab, obstacleHeadPrefab, topHeight, false);

    transform.position = new Vector3(LevelConsts.pipeSpawnPosition, 0);

    var pipeRigidBody = gameObject.AddComponent<Rigidbody2D>();
    pipeRigidBody.isKinematic = true;

    SetTopObstacle(topList);
    SetBottomObstacle(bottomList);
  }

  private List<GameObject> CreateObstacle(GameObject pipeBodyPrefab, GameObject pipeHeadPrefab, float height,
    bool isLookingUp)
  {
    float pipeHeadYPos;
    float pipeBodyYPos;

    if (isLookingUp)
    {
      pipeHeadYPos = -LevelConsts.cameraOrthograficSize + height - LevelConsts.pipeHeadHeight * .45f;
      pipeBodyYPos = -LevelConsts.cameraOrthograficSize;
    }
    else
    {
      pipeHeadYPos = +LevelConsts.cameraOrthograficSize - height + LevelConsts.pipeHeadHeight * .45f;
      pipeBodyYPos = +LevelConsts.cameraOrthograficSize;
    }

    var pipeHead = Instantiate(pipeHeadPrefab, transform);
    pipeHead.transform.localPosition = new Vector3(0, pipeHeadYPos);

    var pipeBody = Instantiate(pipeBodyPrefab, transform);
    pipeBody.transform.localPosition = new Vector3(0, pipeBodyYPos);
    pipeBody.transform.localScale = new Vector3(1, isLookingUp ? height : -height, 1);

    var pipeBodyList = new List<GameObject>() { pipeBody, pipeHead };
    return pipeBodyList;
  }

  public void SetTopObstacle(List<GameObject> bodyList)
  {
    if (!bodyList.Count.Equals(2))
    {
      Debug.LogError("Liste eksik!");
      return;
    }

    topObstacleBody = bodyList[0];
    topObstacleHead = bodyList[1];
  }

  public void SetBottomObstacle(List<GameObject> bodyList)
  {
    if (!bodyList.Count.Equals(2))
    {
      Debug.LogError("Liste eksik!");
      return;
    }

    bottomObstacleBody = bodyList[0];
    bottomObstacleHead = bodyList[1];
  }

  public void Move()
  {
    transform.Translate(new Vector3(-1, 0, 0) * obstacleSpeed * Time.deltaTime);
  }

  public float GetObstacleXPosition()
  {
    return transform.position.x;
  }
}
