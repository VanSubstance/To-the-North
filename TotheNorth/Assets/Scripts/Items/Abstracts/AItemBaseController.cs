using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Commons.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Items.Abstracts
{
    /// <summary>
    /// 아이템 베이스 컨트롤러
    /// 아이템은 기본적으로 아래의 마우스 이벤트만 가진다:
    /// - 왼쪽 드래그/클릭/업/다운, 오른쪽 클릭
    /// </summary>
    public abstract class AItemBaseController : MonoBehaviour, IMouseInteractable
    {
        public abstract void OnLeftMouseClick(Vector3 mousePosition);

        public abstract void OnRightMouseClick(Vector3 mousePosition);

        public abstract void OnLeftMouseDrag(Vector3 mousePosition);

        public abstract void OnLeftMouseDown(Vector3 mousePosition);

        public abstract void OnLeftMouseUp(Vector3 mousePosition);

        public void OnMiddleMouseClick(Vector3 mousePosition)
        {
            throw new NotImplementedException();
        }

        public void OnMiddleMouseDown(Vector3 mousePosition)
        {
            throw new NotImplementedException();
        }

        public void OnMiddleMouseDrag(Vector3 mousePosition)
        {
            throw new NotImplementedException();
        }

        public void OnMiddleMouseUp(Vector3 mousePosition)
        {
            throw new NotImplementedException();
        }

        public void OnRightMouseDown(Vector3 mousePosition)
        {
            throw new NotImplementedException();
        }

        public void OnRightMouseDrag(Vector3 mousePosition)
        {
            throw new NotImplementedException();
        }

        public void OnRightMouseUp(Vector3 mousePosition)
        {
            throw new NotImplementedException();
        }
    }
}
