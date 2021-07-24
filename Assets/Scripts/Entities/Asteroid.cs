using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroidCrasher
{

    public delegate void AsteroidEvent(Asteroid sender, bool needSplit);

    [RequireComponent(typeof(Rigidbody2D))]
    public class Asteroid : MonoBehaviour, IDamagable
    {
        public AsteroidType AsteroidType;
        public AsteroidEvent AsteroidCrash;

        Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Launch(float speed)
        {
            _rigidbody.velocity = transform.up * speed;
        }
        public void Damage()
        {
            AsteroidCrash(this, true);
        }

        public void DestroyImmideatly()
        {
            AsteroidCrash(this, false);
        }
    }

}

