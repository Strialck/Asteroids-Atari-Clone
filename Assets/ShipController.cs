using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AstroidCrasher
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class ShipController : MonoBehaviour
    {
        [SerializeField]
        float _immynityTime;
        [SerializeField]
        float _timeBtwVisibility;



        [SerializeField]
        Transform _spawnPoint;

        [SerializeField]
        float _maxSpeed;

        [SerializeField]
        float _acceleration;

        [SerializeField]
        float rotateSpeed;

        SpriteRenderer _spriteRenderer;
        Rigidbody2D _rigidbody;
        Collider2D _collider;
        Weapon _weapon;

        HealthController _healthController;




        // Start is called before the first frame update
        void Awake()
        {
            _healthController = GetComponent<HealthController>();
            _healthController.PlayerDamaged += OnPlayerDamaged;
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _weapon = GetComponentInChildren<Weapon>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void MoveForward(float dTime)
        {
            _rigidbody.velocity += (Vector2)transform.up * _acceleration * dTime;
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
        }

        public void RotateAt(Vector2 position, float dTime)
        {
            Vector2 direction = position - _rigidbody.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotateSpeed * dTime);
        }

        public void Rotate(float direction, float dTime)
        {
            transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * dTime * direction);
        }

        public void Fire()
        {
            _weapon.Shoot();
        }


        void OnPlayerDamaged()
        {
            transform.position = _spawnPoint.position;
            transform.up = Vector3.up;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0;
            StartCoroutine(ImminityCorutine());
        }
        IEnumerator ImminityCorutine()
        {
            _collider.enabled = false;
            int switchCount = Mathf.CeilToInt(_immynityTime / _timeBtwVisibility);
            for (int i = 0; i < switchCount; i++)
            {
                yield return new WaitForSeconds(_timeBtwVisibility);
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
            }
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
            
        }

        public void ResetContoller()
        {
            StopAllCoroutines();
            StartCoroutine(ImminityCorutine());
        }
     
    }
}