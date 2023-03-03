using UnityEngine;
public static class GlobalStatus
{
    public static string curScene = "메인메뉴씬";
    public static class Loading
    {
        public static class System
        {
            public static bool CommonGameManager = false;
            public static bool InfoMessageManager = false;
            public static bool PopupModalContentControllers = false;
            public static bool PopupModalController = false;
            public static bool MouseCursorManager = false;

            public static bool isSystemLoadingDone()
            {
                return CommonGameManager &&
                    InfoMessageManager &&
                    PopupModalContentControllers &&
                    PopupModalController &&
                    MouseCursorManager
                    ;
            }
        }
    }

    public static class Util
    {
        public static class MouseEvent
        {
            public static MouseActionBundle Left = new MouseActionBundle(), Right = new MouseActionBundle(), Middle = new MouseActionBundle();
            public class MouseActionBundle
            {
                public System.Action<Transform, Vector3> actionDown, actionUp, actionDrag, actionClick;
                public MouseActionBundle()
                {
                    setActions();
                }
                public void setActions(
                    System.Action<Transform, Vector3> actionDown = null,
                    System.Action<Transform, Vector3> actionUp = null,
                    System.Action<Transform, Vector3> actionDrag = null,
                    System.Action<Transform, Vector3> actionClick = null
                    )
                {
                    this.actionDown = actionDown;
                    this.actionUp = actionUp;
                    this.actionDrag = actionDrag;
                    this.actionClick = actionClick;
                }
            }
        }
    }

    public static void resetLoading()
    {
        Loading.System.CommonGameManager = false;
        Loading.System.InfoMessageManager = false;
    }
}