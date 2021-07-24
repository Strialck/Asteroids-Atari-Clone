using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AstroidCrasher
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField]
        HealthController _healthController;

        [SerializeField]
         TMP_Text _livesText;


        void Start()
        {
            _healthController.HealthChanged += UpdateHealth;
        }

        void UpdateHealth()
        {
            _livesText.text = "x " + _healthController.CurrenåLivesCount.ToString();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}
