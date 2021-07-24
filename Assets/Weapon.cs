using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    Transform _firePoint;

    [SerializeField]
    Bullet _bulletPrefab;

    [SerializeField]
    float _fireRate;

    float _nextShootTime;
    float _reloadtime;

    [SerializeField]
    AudioClip _shootSound;

    AudioSource _audioSource;

    Pool<Bullet> _bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        _bulletPool = new Pool<Bullet>(transform, _bulletPrefab, 5);
        _bulletPool.InitPool();
        _nextShootTime = Time.time;
        _reloadtime = 1f / _fireRate;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        float time = Time.time;
        if(time >= _nextShootTime)
        {
            _nextShootTime  = time + _reloadtime;
            var instance =  _bulletPool.GetObject();
            instance.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
            instance.ConnectedPool = _bulletPool;
            instance.Launch();
            _audioSource.PlayOneShot(_shootSound);
        }
    }
}
