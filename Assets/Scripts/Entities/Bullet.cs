using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;

    [SerializeField]
    float _bulletSpeed;

    [SerializeField]
    float _percentOfScreenWidth;

    float _lifetime;

    public Pool<Bullet> ConnectedPool;

    public void Launch()
    {
        _rigidbody2D.velocity = transform.up * _bulletSpeed;
        StartCoroutine(LifeCorutine());
    }

    IEnumerator LifeCorutine()
    {
        yield return new WaitForSeconds(_lifetime);
        TryPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
            damagable.Damage();
        TryPool();
    }

    void TryPool()
    {
        if (ConnectedPool != null)
            ConnectedPool.PoolObject(this);
        else
            Destroy(this.gameObject);
    }

    void Awake()
    {
        _lifetime = CameraStats.getInstance().CamWidth * _percentOfScreenWidth / _bulletSpeed;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

   
}
