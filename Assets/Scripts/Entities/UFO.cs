using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroidCrasher
{
    public delegate void UFOEvent(UFO sender, bool needAddPoints);

    [RequireComponent(typeof(Rigidbody2D))]
    public class UFO : MonoBehaviour, IDamagable
    {
        Rigidbody2D _rigidbody;
        Weapon _weapon;

        public int Points;
        public Transform Target;
        public float Lifetime;

        public event UFOEvent DieEvent = delegate { };

        [SerializeField]
        float _minFireTime;

        [SerializeField]
        float _maxFireTime;

        public void Launch(Direction direction,float speed)
        {
            switch (direction)
            {
                case Direction.LeftToRight:
                    {
                        _rigidbody.velocity = transform.right * speed;
                        break;
                    }
                    
                case Direction.RightToLeft:
                    {
                        _rigidbody.velocity = (-1) * transform.right * speed;
                        break;
                    }
                    
                default:
                    break;
            }
            StartCoroutine(LifeCorutine());
            StartCoroutine(FireCorutine());

        }

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _weapon = GetComponentInChildren<Weapon>();
        }

        void Update()
        {
            Vector2 direction = Target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        IEnumerator FireCorutine()
        {
            yield return new WaitForSeconds(Random.Range(_minFireTime, _maxFireTime));
            _weapon.Shoot();
            StartCoroutine(FireCorutine());
        }

        IEnumerator LifeCorutine()
        {
            yield return new WaitForSeconds(Lifetime);
            DieEvent(this,false);
        }

        public void DestroyImmideatly()
        {
            DieEvent(this,false);
        }

        public void Damage()
        {
            DieEvent(this,true);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
                damagable.DestroyImmideatly();
            DestroyImmideatly();
        }
    }
}