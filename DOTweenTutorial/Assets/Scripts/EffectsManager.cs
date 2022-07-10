using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
  [SerializeField]
  private GameObject[] _effectsPrefabs;
  [SerializeField]
  private AudioClip _daggerFlySound;
  [SerializeField]
  private AudioClip _hitWoodSound;
  [SerializeField]
  private AudioClip _hitDaggerSound;

  [SerializeField]
  [HideInInspector]
  private AudioSource _audioSource;

  private void OnValidate()
  {
    _audioSource = GetComponent<AudioSource>();
  }


  public void PlayEffectOnce(Vector3 position)
  {
    GameObject effect = Instantiate(_effectsPrefabs[UnityEngine.Random.Range(0, _effectsPrefabs.Length)], position, Quaternion.identity);
    ParticleSystem particle = effect.GetComponent<ParticleSystem>();
    particle.Play();
    Destroy(effect.gameObject, particle.main.duration);
  }
  public void PlayDaggerFlyAudio()
  {
    _audioSource.clip = _daggerFlySound;
    _audioSource.Play();
  }
  public void PlayHitWoodAudio()
  {
    _audioSource.clip = _hitWoodSound;
    _audioSource.Play();
  }
  public void PlayHitDaggerAudio()
  {
    _audioSource.clip = _hitDaggerSound;
    _audioSource.Play();
  }
}
