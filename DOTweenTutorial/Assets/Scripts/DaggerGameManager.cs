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
  private GameObject _daggerPrefab;
  [SerializeField]
  private GameObject[] _hearts;
  [SerializeField]
  private GameObject[] _daggerInHeart;
  [SerializeField]
  private Transform _logTransform;
  [SerializeField]
  private float _shotCooldown;
  private GameObject _currentDagger;
  private int _currentHeartIdx;
  private float _lastShotTime;
  private UIManager _uiManager;

  private Vector3 _logRotation = new Vector3(0, 360, 0);
  public bool IsPlaying;

  private void OnValidate()
  {
    _effectsManager = GetComponent<EffectsManager>();
  }

  private void Start()
  {
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
    // _logTransform.DORotate(_logRotation, 3f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    // _logTransform.DOShakePosition(4f, fadeOut: false).SetLoops(-1, LoopType.Incremental);
    // DOTween.Shake(() => _logTransform.position, (shakeVector) =>
    // {
    //   shakeVector.y = 0;
    //   _logTransform.position = shakeVector;
    // }, 4f, 1, 3, fadeOut: false, ignoreZAxis: false).SetLoops(-1, LoopType.Incremental);
    _logTransform.DORotate(_logRotation, 5f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
  }
  private void BeatHeart()
  {
    Sequence beatSequence = DOTween.Sequence();
    Vector3 originalScale = _hearts[0].transform.localScale;
    for (int i = 0; i < _hearts.Length; i++)
    {
      beatSequence.Append(_hearts[i].transform.DOScale(originalScale * 1.1f, 0.1f))
      .Append(_hearts[i].transform.DOScale(originalScale, 0.1f))
      .Append(_hearts[i].transform.DOScale(originalScale * 1.3f, 0.1f))
      .AppendInterval(0.2f)
      .Append(_hearts[i].transform.DOScale(originalScale, 0.1f))
      .AppendInterval(0.5f);
    }
    beatSequence.SetLoops(-1);
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
    _currentDagger.transform.DOMoveZ(1f, 0.1f, false).SetEase(Ease.InSine).OnComplete(OnLaunchComplete);
  }

  private void OnLaunchComplete()
  {
    if (_currentDagger.GetComponent<DaggerCollision>().HitsLog)
    {
      _currentDagger.GetComponent<TrailRenderer>().enabled = false;
      _effectsManager.PlayHitWoodAudio();
      _currentDagger.transform.parent = _logTransform;
      GetNewDagger();
    }
    else
    {
      _effectsManager.PlayHitDaggerAudio();
      _effectsManager.PlayEffectOnce(_currentDagger.transform.position + new Vector3(0, 1, -1));
      EjectKnife();
    }
  }


  private void EjectKnife()
  {
    Vector3 randomPoint = new Vector3(0, 2, 0);
    randomPoint.x = (UnityEngine.Random.value > 0.5f) ? 4f : -4f;
    randomPoint.z = UnityEngine.Random.Range(-4f, 0.5f);

    Sequence sequence = DOTween.Sequence();
    sequence.Append(_currentDagger.transform.DORotate(_logRotation, 0.1f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Incremental))
    .Join(_currentDagger.transform.DOMove(randomPoint, 0.3f).SetEase(Ease.OutCubic))
    .AppendCallback(LoseHeart).AppendCallback(GetNewDagger).SetId("EjectTween");
  }
  private void LoseHeart()
  {
    Material currentHeartMat = _hearts[_currentHeartIdx].GetComponent<Renderer>().material;
    currentHeartMat.DOColor(Color.gray, 1f).OnComplete(() => currentHeartMat.DOFade(0, 1f));
    _currentHeartIdx++;
    if (_currentHeartIdx >= _hearts.Length)
    {
      IsPlaying = false;
      _uiManager.LoseGame();
    }
  }

  public void RestartGame()
  {
    DOTween.Clear();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
  }
}
