using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Commons.Interfaces;

public class MouseEventManager : MonoBehaviour
{
    private static MouseEventManager instance = null;

    private MouseButton curMouseButton = MouseButton.NONE;
    private Vector3 mousePosition = new Vector3(), mousePositionForClick = new Vector3();

    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MarkMousePosition(false);
        TrackMouseEvent();
        if (GlobalStatus.Loading.System.MouseCursorManager)
        {
            TrackMousePosition();
        }
        TrackMouse();
    }

    /// <summary>
    /// 마우스 이벤트 추적
    /// </summary>
    private void TrackMouseEvent()
    {
        if (curMouseButton == MouseButton.NONE)
        {
            // OnMouseDown
            DownMouseButton();
        }
        else
        {
            DragMouseButton();
        }
    }

    /// <summary>
    /// OnMouseDown 버튼 감지
    /// </summary>
    private void DownMouseButton()
    {
        Transform tf;
        if (Input.GetMouseButton(0))
        {
            // 왼쪽 버튼 감지
            curMouseButton = MouseButton.LEFT;
            MarkMousePosition(true);
            tf = GetTransformBelow();
            TryExecuteAction(() =>
            {
                tf.GetComponent<IMouseInteractable>().OnLeftMouseDown(mousePosition);
            });
            TryExecuteAction(() =>
            {
                GlobalStatus.Util.MouseEvent.Left.actionDown(tf, mousePosition);
            });
        }
        else if (Input.GetMouseButton(1))
        {
            // 오른쪽 버튼 감지
            curMouseButton = MouseButton.RIGHT;
            MarkMousePosition(true);
            tf = GetTransformBelow();
            TryExecuteAction(() =>
            {
                tf.GetComponent<IMouseInteractable>().OnRightMouseDown(mousePosition);
            });
            TryExecuteAction(() =>
            {
                GlobalStatus.Util.MouseEvent.Right.actionDown(tf, mousePosition);
            });
        }
        else if (Input.GetMouseButton(2))
        {
            // 가운데 버튼 감지
            curMouseButton = MouseButton.MIDDLE;
            MarkMousePosition(true);
            tf = GetTransformBelow();
            TryExecuteAction(() =>
            {
                tf.GetComponent<IMouseInteractable>().OnMiddleMouseDown(mousePosition);
            });
            TryExecuteAction(() =>
            {
                GlobalStatus.Util.MouseEvent.Middle.actionDown(tf, mousePosition);
            });
        }
    }

    /// <summary>
    /// OnMouseDrag 버튼 감지
    /// </summary>
    private void DragMouseButton()
    {
        Transform tf;
        if (Input.GetMouseButton((int)curMouseButton))
        {
            // 드래그 이벤트 발생중
            // OnMouseDrag
            switch (curMouseButton)
            {
                case MouseButton.LEFT:
                    tf = GetTransformBelow();
                    TryExecuteAction(() =>
                    {
                        tf.GetComponent<IMouseInteractable>().OnLeftMouseDrag(mousePosition);
                    });
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Left.actionDrag(tf, mousePosition);
                    });
                    break;
                case MouseButton.RIGHT:
                    tf = GetTransformBelow();
                    TryExecuteAction(() =>
                    {
                        tf.GetComponent<IMouseInteractable>().OnRightMouseDrag(mousePosition);
                    });
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Right.actionDrag(tf, mousePosition);
                    });
                    break;
                case MouseButton.MIDDLE:
                    tf = GetTransformBelow();
                    TryExecuteAction(() =>
                    {
                        tf.GetComponent<IMouseInteractable>().OnMiddleMouseDrag(mousePosition);
                    });
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Middle.actionDrag(tf, mousePosition);
                    });
                    break;
            }
        }
        else
        {
            // 드래그 이벤트 종료
            // OnMouseUp
            UpMouseButton();
        }
    }

    /// <summary>
    /// OnMouseUp 버튼 감지
    /// </summary>
    private void UpMouseButton()
    {
        Transform tf;
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = -20f;
        if (Vector3.Distance(mousePositionForClick, temp) < 0.1f)
        {
            // 마우스 클릭 이벤트
            ClickMouseButton();
        }
        else
        {
            // OnMouseUp
            switch (curMouseButton)
            {
                case MouseButton.LEFT:
                    tf = GetTransformBelow();
                    TryExecuteAction(() =>
                    {
                        tf.GetComponent<IMouseInteractable>().OnLeftMouseUp(mousePosition);
                    });
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Left.actionUp(tf, mousePosition);
                    });
                    break;
                case MouseButton.RIGHT:
                    tf = GetTransformBelow();
                    TryExecuteAction(() =>
                    {
                        tf.GetComponent<IMouseInteractable>().OnRightMouseUp(mousePosition);
                    });
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Right.actionUp(tf, mousePosition);
                    });
                    break;
                case MouseButton.MIDDLE:
                    tf = GetTransformBelow();
                    TryExecuteAction(() =>
                    {
                        tf.GetComponent<IMouseInteractable>().OnMiddleMouseUp(mousePosition);
                    });
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Middle.actionUp(tf, mousePosition);
                    });
                    break;
            }
        }
        mousePosition = new Vector3();
        mousePositionForClick = new Vector3();
        curMouseButton = MouseButton.NONE;
    }

    /// <summary>
    /// OnMouseClick 버튼 감지 (딸깍)
    /// </summary>
    private void ClickMouseButton()
    {
        Transform tf;
        switch (curMouseButton)
        {
            case MouseButton.LEFT:
                tf = GetTransformBelow();
                TryExecuteAction(() =>
                {
                    tf.GetComponent<IMouseInteractable>().OnLeftMouseClick(mousePosition);
                });
                TryExecuteAction(() =>
                {
                    // Transform temp = GetTransformBelow();
                    // if (temp.GetComponent<IClickable>() != null)
                    GlobalStatus.Util.MouseEvent.Left.actionClick(tf, mousePosition);
                });
                break;
            case MouseButton.RIGHT:
                tf = GetTransformBelow();
                TryExecuteAction(() =>
                {
                    tf.GetComponent<IMouseInteractable>().OnRightMouseClick(mousePosition);
                });
                TryExecuteAction(() =>
                {
                    GlobalStatus.Util.MouseEvent.Right.actionClick(tf, mousePosition);
                });
                break;
            case MouseButton.MIDDLE:
                tf = GetTransformBelow();
                TryExecuteAction(() =>
                {
                    tf.GetComponent<IMouseInteractable>().OnMiddleMouseClick(mousePosition);
                });
                TryExecuteAction(() =>
                {
                    GlobalStatus.Util.MouseEvent.Middle.actionClick(tf, mousePosition);
                });
                break;
        }
    }

    /// <summary>
    /// 마우스 현재 위치 저장
    /// </summary>
    /// <param name="isForClick">
    /// 클릭을 위한 저장인지 아닌지
    /// </param>
    private void MarkMousePosition(bool isForClick = false)
    {
        if (isForClick)
        {
            mousePositionForClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionForClick.z = -20f;
        }
        else
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -20f;
        }
    }

    /// <summary>
    /// 현재 마우스 위치 아래의 transform 반환 (Physics.Raycast)
    /// </summary>
    /// <returns></returns>
    private Transform GetTransformBelow()
    {
        RaycastHit hit;
        if (Physics.Raycast(mousePosition, Vector3.forward, out hit, 40f))
        {
            return hit.transform;
        }
        return null;
    }

    /// <summary>
    /// 전달받은 람다식 실행
    /// </summary>
    /// <param name="targetAction"></param>
    private void TryExecuteAction(System.Action targetAction)
    {
        try
        {
            targetAction();
        }
        catch (NullReferenceException)
        {
            // 해당 이벤트 정의 안됨
        }
        catch (NotImplementedException)
        {
            // 해당 이벤트 정의 안됨
        }
    }

    public enum MouseButton
    {
        LEFT = 0, RIGHT = 1, MIDDLE = 2, NONE = -1
    }
    private void TrackMousePosition()
    {
        Transform temp;
        if ((temp = GetTransformBelow()) == null)
        {
            GlobalComponent.Common.Event.mouseCursorManager.SetMouseCursor(MouseCursorType.NORMAL);
            return;
        }
        string tag = temp.tag;
        if (tag.Contains("QUESTION"))
        {
            GlobalComponent.Common.Event.mouseCursorManager.SetMouseCursor(MouseCursorType.QUESTION);
            return;
        }
        if (tag.Contains("BUTTON"))
        {
            GlobalComponent.Common.Event.mouseCursorManager.SetMouseCursor(MouseCursorType.BUTTON);
            return;
        }
        GlobalComponent.Common.Event.mouseCursorManager.SetMouseCursor(MouseCursorType.NORMAL);
        return;
    }

    private void TrackMouse()
    {
        if (GlobalStatus.Util.MouseEvent.actionSustain == null) return;
        try
        {
            GlobalStatus.Util.MouseEvent.actionSustain(mousePosition);
        }
        catch (MissingReferenceException)
        {

        }
    }
}
