using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroidCrasher 
{
    public static class PointCounter
    {
        public static event GameEvent PointsChanged = delegate { };

        private static int _points;
        public static int Points
        {
            get
            {
                return _points;
            }
            private set
            {
                if (value >= 0)
                {
                    _points = value;
                    PointsChanged();
                }
                    
                else
                    throw new System.ArgumentException("Передаваемое значение количества очков должно быть больше нуля");
            }
        }
        public static void AddPoints(int value)
        {
            Points += value;
        }

        public static void ResetPoints()
        {
            Points = 0;
        }
    }
}
