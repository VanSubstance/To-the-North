using System;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public abstract class AbsCreatureActionController : MonoBehaviour, ICreatureAction
    {
        [SerializeField]
        private Transform detectionTf, sightTf, handTf;

        protected bool isCrouching = false;

        public void Crouch()
        {
            if (isCrouching) return;
            isCrouching = true;
            detectionTf.localPosition = Vector3.up * (-.6f + detectionTf.localPosition.y);
            sightTf.localPosition = Vector3.up * (-.4f + sightTf.localPosition.y);
            handTf.localPosition = Vector3.up * (-.6f + handTf.localPosition.y);
        }

        public void Stand()
        {
            if (!isCrouching) return;
            isCrouching = false;
            detectionTf.localPosition = Vector3.up * (.6f + detectionTf.localPosition.y);
            sightTf.localPosition = Vector3.up * (.4f + sightTf.localPosition.y);
            handTf.localPosition = Vector3.up * (.6f + handTf.localPosition.y);
        }

        public abstract void Dodge(Vector3 dir);
    }
}
