using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public abstract class AbsItemController : MonoBehaviour
    {
        protected KeyCode keyToPress = KeyCode.LeftControl;
        protected float timeHover = 1, timeClick = 0.5f;
        private float liveTimeHover = 0, liveTimeClick = 0;
        private bool
            isKeyPressed = false,
            isMouseEnter = false,
            isMouseDown = false,
            isMouseClickedJustNow = false;

        protected void Update()
        {
            isKeyPressed = Input.GetKey(keyToPress);
            TrackHover();
            TrackClick();
        }

        private void OnMouseEnter()
        {
            liveTimeHover = 0;
            isMouseEnter = true;
            if (isKeyPressed) OnMouseEnterWithKeyPress();
        }

        private void OnMouseExit()
        {
            OnHoverExit();
            isMouseEnter = false;
        }

        private void OnMouseDown()
        {
            liveTimeClick = 0f;
            isMouseDown = true;
            OnDown();
        }

        private void OnMouseDrag()
        {
            OnDraging();
        }

        private void OnMouseUp()
        {
            isMouseDown = false;
            OnUp();
            if (liveTimeClick < timeClick)
            {
                if (isKeyPressed) OnMouseClickWithKeyPress();
                if (isMouseClickedJustNow)
                {
                    OnDoubleClick();
                    isMouseClickedJustNow = false;
                    return;
                }
                isMouseClickedJustNow = true;
            }
        }

        private void TrackHover()
        {
            if (isMouseEnter)
            {
                if (liveTimeHover < timeHover)
                {
                    liveTimeHover += Time.deltaTime;
                }
                else
                {
                    if (!isMouseDown)
                    {
                        OnHover();
                    }
                }
            }
        }

        private void TrackClick()
        {
            if (isMouseDown)
            {
                if (liveTimeClick <= timeClick)
                {
                    liveTimeClick += Time.deltaTime;
                }
                else
                {
                    isMouseClickedJustNow = false;
                }
            }
        }

        /// <summary>
        /// 호버링중에 실행되어야 할 함수
        /// </summary>
        protected abstract void OnHover();

        /// <summary>
        /// 호버링 종료 시에 실행되어야 할 함수
        /// </summary>
        protected abstract void OnHoverExit();
        /// <summary>
        /// 드래그중에 실행되어야 할 함수
        /// </summary>
        protected abstract void OnDraging();
        /// <summary>
        /// 마우스 버튼을 눌렀을 때 실행되어야 할 함수 (onMouseDown)
        /// </summary>
        protected abstract void OnDown();
        /// <summary>
        /// 마우스 버튼을 들었을 때 실행되어야 할 함수 (onMouseUp)
        /// </summary>
        protected abstract void OnUp();
        /// <summary>
        /// 특정 키를 누른 상태에서 마우스가 진입하였을 때 실행되어야 할 함수
        /// (기본 키: 왼쪽 ctrl)
        /// </summary>
        protected abstract void OnMouseEnterWithKeyPress();
        /// <summary>
        /// 특정 키를 누른 상태에서 마우스가 클릭되었을 때 실행되어야 할 함수
        /// (기본 키: 왼쪽 ctrl)
        /// </summary>
        protected abstract void OnMouseClickWithKeyPress();

        /// <summary>
        /// 더블클릭 하였을 때 실행되어야 하는 함수
        /// </summary>
        protected abstract void OnDoubleClick();
    }
}
