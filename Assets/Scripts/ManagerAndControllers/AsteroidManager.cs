using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroidCrasher;


namespace AstroidCrasher
{
    public class AsteroidManager : MonoBehaviour
    {
    
        [SerializeField]
        List<AsteroidInfo> _asteroidsData;

        [SerializeField]
        System.Array _typesArray;

        AudioSource _audioSource;

        List<Asteroid> _activeAsteroids = new List<Asteroid>();
        // в целом можно было бы и Dictionary<AsteroidType,AsteroidInfo> для ускорения поиска

        public GameEvent AllAsteroidsDestroyed = delegate { };

        Pool<Asteroid> GetPool(AsteroidType asteroidType)
        {
            return _asteroidsData.Find(x => x.asteroidType == asteroidType).Pool;
        }

        float GetRandomSpeed(AsteroidType asteroidType)
        {
            var inst = _asteroidsData.Find(x => x.asteroidType == asteroidType);
            return Random.Range(inst.minSpeed, inst.maxSpeed);
        }

        void Start()
        {
            _typesArray = System.Enum.GetValues(typeof(AsteroidType));
            _audioSource = GetComponent<AudioSource>();

            for (int i = 0; i < _asteroidsData.Count; i++)
            {
                _asteroidsData[i].Pool = new Pool<Asteroid>(this.transform, _asteroidsData[i].Prefab, Mathf.RoundToInt(Mathf.Pow(i + 2, 2)));
                _asteroidsData[i].Pool.InitPool();
            }
        }

        public void SpawnAsteroidsInRandomPos(int count, AsteroidType asteroidType)
        {
            var pool = GetPool(asteroidType);
            for (int i = 0; i < count; i++)
            {
                var inst = pool.GetObject();
                _activeAsteroids.Add(inst);
                inst.AsteroidCrash += OnAsteroidCrash;
                inst.AsteroidType = asteroidType;
                inst.transform.position = CameraStats.GetRandomPosOnCamBorder();
                inst.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                inst.Launch(GetRandomSpeed(asteroidType));
            }
        }

        void SpawnSplittedAsteroids(Transform asterTransform, AsteroidType asteroidType)
        {
            var pool = GetPool(asteroidType);
            Asteroid inst = pool.GetObject();
            inst.transform.position = asterTransform.position;
            inst.transform.rotation = asterTransform.rotation * Quaternion.Euler(0, 0, 45);
            inst.AsteroidType = asteroidType;
            inst.AsteroidCrash += OnAsteroidCrash;
            _activeAsteroids.Add(inst);
            inst.Launch(GetRandomSpeed(asteroidType));

            inst = pool.GetObject();
            inst.transform.position = asterTransform.position;
            inst.transform.rotation = asterTransform.rotation * Quaternion.Euler(0, 0, -45);
            inst.AsteroidType = asteroidType;
            inst.AsteroidCrash += OnAsteroidCrash;
            _activeAsteroids.Add(inst);
            inst.Launch(GetRandomSpeed(asteroidType));

        }

        bool CanSplit(AsteroidType asteroidType)
        {
            return _asteroidsData.Find(x => x.asteroidType == asteroidType).Splitable;
        }

        void CheckActiveAsteroids()
        {
            if (_activeAsteroids.Count == 0)
                AllAsteroidsDestroyed();
        }

        AsteroidType GetSmallerAsterodType(AsteroidType asteroidType)
        {
            //можно было бы и свичом, но так в теории универсальней

            for (int i = 1; i < _typesArray.Length; i++)
            {
                if ((AsteroidType)_typesArray.GetValue(i - 1) == asteroidType)
                    return (AsteroidType)_typesArray.GetValue(i);
            }
            throw new System.ArgumentException(asteroidType.ToString());
        }

        void PlaySound(AsteroidType asteroidType)
        {
            _audioSource.PlayOneShot(_asteroidsData.Find(x => x.asteroidType == asteroidType).ExplosionSound);
        }

        int GetPoints(AsteroidType asteroidType)
        {
            return _asteroidsData.Find(x => x.asteroidType == asteroidType).points;
        }
        void OnAsteroidCrash(Asteroid sender, bool needSplit)
        {
            var aType = sender.AsteroidType;
            sender.AsteroidCrash -= OnAsteroidCrash;

            if(needSplit)
            {
                PointCounter.AddPoints(GetPoints(aType));
                if(CanSplit(aType))
                    SpawnSplittedAsteroids(sender.transform, GetSmallerAsterodType(aType));
            }
                

            PlaySound(aType);
            GetPool(aType).PoolObject(sender);
            _activeAsteroids.Remove(sender);
            CheckActiveAsteroids();
        }

        public void ResetManager()
        {
            foreach (var asteroid in _activeAsteroids)
            {
                var aType = asteroid.AsteroidType;
                asteroid.AsteroidCrash -= OnAsteroidCrash;
                GetPool(aType).PoolObject(asteroid);
                

            }
            _activeAsteroids.Clear();
        }
    }
}