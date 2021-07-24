using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AstroidCrasher
{
    public delegate void GameEvent();

    public class HealthController : MonoBehaviour, IDamagable
    {
        public event GameEvent HealthChanged = delegate { };
        public event GameEvent PlayerDamaged = delegate { };
        public event GameEvent Die = delegate { };

        [SerializeField]
        int _maxLivesCount;

        [SerializeField]
        private int _currentLivesCount;


        public int CurrenåLivesCount
        {
            get
            {
                return _currentLivesCount;
            }
            private set
            {
                _currentLivesCount = value;
                HealthChanged();
                if (value == 0)
                    Die();
            }

        }


        void Start()
        {

            CurrenåLivesCount = _maxLivesCount;
        }

        public void ResetContoller()
        {
            CurrenåLivesCount = _maxLivesCount;
        }

        public void DestroyImmideatly()
        {
            Damage();
        }

        public void Damage()
        {
            PlayerDamaged();
            CurrenåLivesCount--;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.DestroyImmideatly();
                Damage();
            }
        }
    }

}

