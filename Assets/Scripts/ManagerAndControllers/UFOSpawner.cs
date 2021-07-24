using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroidCrasher
{
    public enum Direction
    {
        LeftToRight,
        RightToLeft
    }

    public class UFOSpawner : MonoBehaviour
    {
        [SerializeField]
        Transform _player;
        [SerializeField]
        UFO _uFOPrefab;

        [SerializeField]
        List <UFO> _curentUFOInstances;

        [SerializeField]
        float _minSpawntime;

        [SerializeField]
        float _maxSpawnTime;

        [SerializeField]
        float _borderPercent;

        float _border;

        [SerializeField]
        float _flightTime;

        float _speed;

        [SerializeField]
        AudioClip _explosionSound;
        AudioSource _audioSource;
        void PlaySound()
        {
            _audioSource.PlayOneShot(_explosionSound);
        }

        public void ResetSpawner()
        {
            StopAllCoroutines();
            if(_curentUFOInstances.Count !=0)
            {
                foreach (var ufo in _curentUFOInstances)
                    Destroy(ufo.gameObject);
            }
            _curentUFOInstances.Clear();
            StartCoroutine(SpawnCorutine());
        }

        void OnUFODestroy(UFO destroyedUFO, bool calcPoints)
        {
            if(calcPoints)
                PointCounter.AddPoints(destroyedUFO.Points);
            _curentUFOInstances.Remove(destroyedUFO);
            Destroy(destroyedUFO.gameObject);
            PlaySound();
        }

        private void Start()
        {
            _curentUFOInstances = new List<UFO>();
            _border = CameraStats.getInstance().CamHeght * _borderPercent / 100f;
            _speed = CameraStats.getInstance().CamWidth / _flightTime;
            StartCoroutine(SpawnCorutine());
            _audioSource = GetComponent<AudioSource>();

        }

        IEnumerator SpawnCorutine()
        {
            yield return new WaitForSeconds(Random.Range(_minSpawntime, _maxSpawnTime));
            Vector3 position = new Vector3();
            position.y = Random.Range(CameraStats.getInstance().MinY + _border, CameraStats.getInstance().MaxY - _border);
            Direction direction;

            if (Random.Range(0f, 1f) > 0.5f) //слева-направо
            {
                position.x = CameraStats.getInstance().MinX;
                direction = Direction.LeftToRight;
            }
            else
            {
                position.x = CameraStats.getInstance().MaxX;
                direction = Direction.RightToLeft;
            }
            var curentUFOInst = Instantiate(_uFOPrefab, position, Quaternion.identity);
            
            curentUFOInst.Lifetime = _flightTime;
            curentUFOInst.Target = _player;
            curentUFOInst.Launch(direction, _speed);
            curentUFOInst.DieEvent += OnUFODestroy;
            _curentUFOInstances.Add(curentUFOInst);
            StartCoroutine(SpawnCorutine());
        }
    }
}