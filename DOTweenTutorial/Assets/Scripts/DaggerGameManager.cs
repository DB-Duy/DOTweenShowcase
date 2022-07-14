using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DaggerGameManager : MonoBehaviour
{
  [SerializeField]
  private EffectsManager _effectsManager;
  [SerializeField]
  private TweeningManager _tweenManager;
  [SerializeField]
  private TMPro.TMP_Text _scoreText;
  [SerializeField]
  private GameObject _daggerPrefab;
  [SerializeField]
  private GameObject[] _hearts;
  [SerializeField]
  private GameObject[] _daggerInHeart;
  [SerializeField]
  private GameObject _log;
  [SerializeField]
  private float _shotCooldown;
  [SerializeField]
  private GameObject _restartButton;
  private GameObject _currentDagger;
  private int _currentHeartIdx;
  private float _lastShotTime;
  private UIManager _uiManager;
  public int Score;

  private Vector3 _logRotation = new Vector3(0, 360, 0);
  public bool IsPlaying;

  private void OnValidate()
  {
    _effectsManager = GetComponent<EffectsManager>();
  }

  private void Start()
  {
    Score = 0;
    _uiManager = UIManager.instance;
    IsPlaying = true;
    DOTween.Init(true, false);
    _lastShotTime = Time.time;
    _currentHeartIdx = 0;
    BeatHeart();
    SpinLog();
    GetNewDagger();
  }

  private void GetNewDagger()
  {
    _currentDagger = Instantiate(_daggerPrefab, transform.position, Quaternion.identity);
  }

  private void SpinLog()
  {
    _tweenManager.TweenLog(_log);
  }
  private void BeatHeart()
  {
    _tweenManager.TweenHeartbeat(_hearts);
  }

  private void Update()
  {
    if (IsPlaying && Input.GetButtonDown("Fire1") && Time.time - _lastShotTime >= _shotCooldown && !DOTween.IsTweening("EjectTween"))
    {
      LaunchDagger();
      _lastShotTime = Time.time;
    }
  }

  private void LaunchDagger()
  {
    _effectsManager.PlayDaggerFlyAudio();
    _tweenManager.TweenDaggerThrow(_currentDagger)?.OnComplete(OnLaunchComplete);
  }

  private void OnLaunchComplete()
  {
    if (_currentDagger.GetComponent<DaggerCollision>().HitsLog)
    {
      Score++;
      _currentDagger.GetComponent<TrailRenderer>().enabled = false;
      _effectsManager.PlayHitWoodAudio();
      _currentDagger.transform.parent = _log.transform;
      GetNewDagger();
    }
    else
    {
      _effectsManager.PlayHitDaggerAudio();
      _effectsManager.PlayEffectOnce(_currentDagger.transform.position + new Vector3(0, 1, -1));
      EjectKnife();
    }
    _scoreText.text = Score.ToString();
  }

  private void EjectKnife()
  {
      _tweenManager.TweenDaggerDeflect(_currentDagger)?.OnComplete(()=>{LoseHeart(); GetNewDagger();}).SetId("EjectTween");
  }
  private void LoseHeart()
  {
    _tweenManager.TweenHeartLost(_hearts[_currentHeartIdx]);
    _currentHeartIdx++;
    if (_currentHeartIdx >= _hearts.Length)
    {
      IsPlaying = false;
      _uiManager.LoseGame();
      PlayRestartButtonTween();
    }
  }

  private void PlayRestartButtonTween()
  {
    _tweenManager.TweenRestartButton(_restartButton);
  }


  public void RestartGame()
  {
    DOTween.Clear();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
  }
}
