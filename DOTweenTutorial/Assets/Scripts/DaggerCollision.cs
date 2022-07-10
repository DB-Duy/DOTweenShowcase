using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DaggerCollision : MonoBehaviour
{
  public bool HitsLog = true;
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Dagger"))
    {
      HitsLog = false;
    }
  }
  private void OnTriggerStay(Collider other)
  {
    if (HitsLog)
    {
      if (other.gameObject.CompareTag("Dagger"))
      {
        HitsLog = false;
      }
    }
  }
  private Vector3 RandomEjectPoint()
  {
    Vector3 point = Vector3.zero;
    if (UnityEngine.Random.value - 0.5 > 0)
    {
      point.x = 4;
    }
    else
    {
      point.x = -4;
    }
    point.z = UnityEngine.Random.Range(-4f, 0.5f);

    return point;
  }
}
