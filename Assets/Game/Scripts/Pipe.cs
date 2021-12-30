using System;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
  private float pipeSpeed = 3f;
  private GameObject topPipeBody;
  private GameObject topPipeHead;
  private GameObject bottomPipeBody;
  private GameObject bottomPipeHead;

  public Pipe(List<GameObject> bottom, List<GameObject> up)
  {
    SetBottomPipe(bottom);
    SetTopPipe(up);
  }

  public void SetTopPipe(List<GameObject> bodyList)
  {
    if (!bodyList.Count.Equals(2))
    {
      Debug.LogError("Liste eksik!");
      return;
    }

    topPipeBody = bodyList[0];
    topPipeHead = bodyList[1];
  }

  public void SetBottomPipe(List<GameObject> bodyList)
  {
    if (!bodyList.Count.Equals(2))
    {
      Debug.LogError("Liste eksik!");
      return;
    }

    bottomPipeBody = bodyList[0];
    bottomPipeHead = bodyList[1];
  }

  public void SetSpeed(float speed)
  {
    pipeSpeed = speed;
  }

  public void Move()
  {
    transform.Translate(new Vector3(-1, 0, 0) * pipeSpeed * Time.deltaTime);
  }

  public float PipeXPosition()
  {
    return transform.position.x;
  }

  private void OnTriggerEnter2D(Collider2D col)
  {
    Debug.Log("Çarptı");
  }
}
