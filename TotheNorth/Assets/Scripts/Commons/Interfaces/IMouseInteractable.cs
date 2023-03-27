using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commons.Interfaces
{
    internal interface IMouseInteractable
    {
        public void OnLeftMouseDown(Vector3 mousePosition);
        public void OnLeftMouseUp(Vector3 mousePosition);
        public void OnLeftMouseDrag(Vector3 mousePosition);
        public void OnLeftMouseClick(Vector3 mousePosition);
        public void OnRightMouseDown(Vector3 mousePosition);
        public void OnRightMouseUp(Vector3 mousePosition);
        public void OnRightMouseDrag(Vector3 mousePosition);
        public void OnRightMouseClick(Vector3 mousePosition);
        public void OnMiddleMouseDown(Vector3 mousePosition);
        public void OnMiddleMouseUp(Vector3 mousePosition);
        public void OnMiddleMouseDrag(Vector3 mousePosition);
        public void OnMiddleMouseClick(Vector3 mousePosition);
    }
}
