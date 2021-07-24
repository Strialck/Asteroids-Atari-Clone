using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AstroidCrasher
{
    public class PointsPresenter : MonoBehaviour
    {
        [SerializeField]
        TMP_Text _pointsText;

        void Start()
        {
            PointCounter.PointsChanged += UpdatePoints;
        }
        void UpdatePoints()
        {
            _pointsText.text = PointCounter.Points.ToString();
        }
    }

}