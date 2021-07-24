using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroidCrasher;

namespace AstroidCrasher
{
    public enum AsteroidType
    {
        Big,
        Medium,
        Small
        
    }

    [CreateAssetMenu(fileName = "NewAsteroid", menuName = "ScriptableObjects/Asteroids", order = 1)]
    public class AsteroidInfo : ScriptableObject
    {
        public AsteroidType asteroidType;
        public float minSpeed;
        public float maxSpeed;
        public int points;
        public AudioClip ExplosionSound;
        public Asteroid Prefab;
        public bool Splitable;
        public Pool<Asteroid> Pool;
    }
}


