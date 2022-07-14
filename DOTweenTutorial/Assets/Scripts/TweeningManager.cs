using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class TweeningManager : MonoBehaviour
{
    public abstract Tween TweenRestartButton(GameObject restartButton);
    public abstract Tween TweenLog(GameObject log);
    public abstract Tween TweenHeartbeat(GameObject[] hearts);
    public abstract Tween TweenDaggerThrow(GameObject dagger);
    public abstract Tween TweenDaggerDeflect(GameObject dagger);
    public abstract Tween TweenHeartLost(GameObject heart);
}
