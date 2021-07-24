using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroidCrasher;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip _explosionClip;
    [SerializeField]
    AudioClip _thrustClip;
    [SerializeField]
    AudioSource _thrustSource;
    [SerializeField]
    AudioSource _effectSource;
    

    InputControll _inputControll;
    HealthController _healthController;

    // Start is called before the first frame update
    void Awake()
    {
        _inputControll = GetComponentInParent<InputControll>();
        _healthController = GetComponentInParent<HealthController>();
        _inputControll.ThrustingChanged += ControlThrustSound;
        _healthController.PlayerDamaged += PlayExplosionSound;
        _thrustSource.loop = true;
        _thrustSource.clip = _thrustClip;
    }
    void PlayExplosionSound()
    {
        _effectSource.PlayOneShot(_explosionClip);
    }

    void ControlThrustSound()
    {
        if (_inputControll.isThrusting)
            _thrustSource.Play();
        else
            _thrustSource.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
