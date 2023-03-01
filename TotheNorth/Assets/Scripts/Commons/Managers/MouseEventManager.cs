using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventManager : MonoBehaviour
{
    private MouseButton curMouseButton = MouseButton.NONE;
    private Vector3 mousePosition = new Vector3(), mousePositionForClick = new Vector3();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TrackMouseEvent();
    }

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
    private void DownMouseButton()
    {
        if (Input.GetMouseButton(0))
        {
            // 왼쪽 버튼 감지
            curMouseButton = MouseButton.LEFT;
            MarkMousePosition(true);
            TryExecuteAction(() =>
            {
                GlobalStatus.Util.MouseEvent.Left.actionDown(GetTransformBelow(), mousePosition);
            });
        }
        else if (Input.GetMouseButton(1))
        {
            // 오른쪽 버튼 감지
            curMouseButton = MouseButton.RIGHT;
            MarkMousePosition(true);
            TryExecuteAction(() =>
            {
                GlobalStatus.Util.MouseEvent.Right.actionDown(GetTransformBelow(), mousePosition);
            });
        }
        else if (Input.GetMouseButton(2))
        {
            // 가운데 버튼 감지
            curMouseButton = MouseButton.MIDDLE;
            MarkMousePosition(true);
            TryExecuteAction(() =>
            {
                GlobalStatus.Util.MouseEvent.Middle.actionDown(GetTransformBelow(), mousePosition);
            });
        }
    }

    private void DragMouseButton()
    {
        if (Input.GetMouseButton((int)curMouseButton))
        {
            // 드래그 이벤트 발생중
            // OnMouseDrag
            MarkMousePosition();
            switch (curMouseButton)
            {
                case MouseButton.LEFT:
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Left.actionDrag(GetTransformBelow(), mousePosition);
                    });
                    break;
                case MouseButton.RIGHT:
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Right.actionDrag(GetTransformBelow(), mousePosition);
                    });
                    break;
                case MouseButton.MIDDLE:
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Middle.actionDrag(GetTransformBelow(), mousePosition);
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

    private void UpMouseButton()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = 0f;
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
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Left.actionUp(GetTransformBelow(), mousePosition);
                    });
                    break;
                case MouseButton.RIGHT:
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Right.actionUp(GetTransformBelow(), mousePosition);
                    });
                    break;
                case MouseButton.MIDDLE:
                    TryExecuteAction(() =>
                    {
                        GlobalStatus.Util.MouseEvent.Middle.actionUp(GetTransformBelow(), mousePosition);
                    });
                    break;
            }
        }
        mousePosition = new Vector3();
        mousePositionForClick = new Vector3();
        curMouseButton = MouseButton.NONE;
    }

    private void ClickMouseButton()
    {
        switch (curMouseButton)
        {
            case MouseButton.LEFT:
                TryExecuteAction(() =>
                {
                    GlobalStatus.Util.MouseEvent.Left.actionClick(GetTransformBelow(), mousePosition);
                });
                break;
            case MouseButton.RIGHT:
                TryExecuteAction(() =>
                {
                    GlobalStatus.Util.MouseEvent.Right.actionClick(GetTransformBelow(), mousePosition);
                });
                break;
            case MouseButton.MIDDLE:
                TryExecuteAction(() =>
                {
                    GlobalStatus.Util.MouseEvent.Middle.actionClick(GetTransformBelow(), mousePosition);
                });
                break;
        }
    }

    private void MarkMousePosition(bool isForClick = false)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -20f;
        if (isForClick)
        {
            mousePositionForClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionForClick.z = -20f;
        }
    }

    private Transform GetTransformBelow()
    {
        RaycastHit hit;
        if (Physics.Raycast(mousePosition, Vector3.forward, out hit, 40f))
        {
            return hit.transform;
        }
        return null;
    }

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
    }

    public enum MouseButton
    {
        LEFT = 0, RIGHT = 1, MIDDLE = 2, NONE = -1
    }
}
