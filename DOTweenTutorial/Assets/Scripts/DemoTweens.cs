using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DemoTweens : TweeningManager
{
  public override Tween TweenDaggerDeflect(GameObject dagger)
  {
    Vector3 randomPoint = new Vector3(0, 2, 0);
    randomPoint.x = (UnityEngine.Random.value > 0.5f) ? 4f : -4f;
    randomPoint.z = UnityEngine.Random.Range(-4f, 0.5f);
    Vector3 rotation = new Vector3(0,360,0);

    Sequence sequence = DOTween.Sequence();
    sequence.Append(dagger.transform.DORotate(rotation, 0.1f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Incremental))
    .Join(dagger.transform.DOMove(randomPoint, 0.3f).SetEase(Ease.OutCubic));

    return sequence;
  }

  public override Tween TweenDaggerThrow(GameObject dagger)
  {
    return dagger.transform.DOMoveZ(1f, 0.1f, false).SetEase(Ease.InSine);
  }

  public override Tween TweenHeartbeat(GameObject[] hearts)
  {
    Sequence beatSequence = DOTween.Sequence();
    Vector3 originalScale = hearts[0].transform.localScale;
    for (int i = 0; i < hearts.Length; i++)
    {
      beatSequence.Append(hearts[i].transform.DOScale(originalScale * 1.1f, 0.1f))
      .Append(hearts[i].transform.DOScale(originalScale, 0.1f))
      .Append(hearts[i].transform.DOScale(originalScale * 1.3f, 0.1f))
      .AppendInterval(0.2f)
      .Append(hearts[i].transform.DOScale(originalScale, 0.1f))
      .AppendInterval(0.5f);
    }
    beatSequence.SetLoops(-1);
    return beatSequence;
  }

  public override Tween TweenHeartLost(GameObject heart)
  {
    Material currentHeartMat = heart.GetComponent<Renderer>().material;
    return currentHeartMat.DOColor(Color.gray, 1f).OnComplete(() => currentHeartMat.DOFade(0, 1f));
  }

  public override Tween TweenLog(GameObject log)
  {
    // _logTransform.DORotate(_logRotation, 3f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    // _logTransform.DOShakePosition(4f, fadeOut: false).SetLoops(-1, LoopType.Incremental);
    // DOTween.Shake(() => _logTransform.position, (shakeVector) =>
    // {
    //   shakeVector.y = 0;
    //   _logTransform.position = shakeVector;
    // }, 4f, 1, 3, fadeOut: false, ignoreZAxis: false).SetLoops(-1, LoopType.Incremental);
    return log.transform.DORotate(new Vector3(0,360,0), 5f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
  }

  public override Tween TweenRestartButton(GameObject restartButton)
  {
    restartButton.GetComponent<DOTweenAnimation>().DOPlayById("Entry");
    return null;
  }
}
