using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
  [SerializeField]
  private GameObject _objectPrefab;
  [SerializeField]
  private int _objectCount;

  private List<GameObject> _objects = new List<GameObject>();
  private void Awake()
  {
    for (int i = 0; i < _objectCount; i++)
    {
      _objects.Add(Instantiate(_objectPrefab, transform.position, Quaternion.identity));
      _objects[i].SetActive(false);
    }
  }

  public GameObject ActivateObject(Vector3 position, Quaternion rotation)
  {
    for (int i = 0; i < _objectCount; i++)
    {
      if (!_objects[i].activeInHierarchy)
      {
        _objects[i].transform.position = position;
        _objects[i].transform.rotation = rotation;
        _objects[i].SetActive(true);
        return _objects[i];
      }
    }

    return null;
  }
}
