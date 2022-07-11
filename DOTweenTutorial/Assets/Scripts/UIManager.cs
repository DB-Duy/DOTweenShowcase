using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
  public static UIManager instance;
  [SerializeField]
  private GameObject _restartButton;
  private void Awake()
  {
    instance = this;
  }
  public void LoseGame()
  {
    _restartButton.SetActive(true);
  }
}
