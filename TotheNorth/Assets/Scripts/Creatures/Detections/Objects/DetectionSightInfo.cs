using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    internal struct DetectionSightInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public DetectionSightInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}
