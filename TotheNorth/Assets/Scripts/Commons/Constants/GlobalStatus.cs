using UnityEngine;
public static class GlobalStatus
{
    public static string curScene = "";
    public static string nextScene = "";
    public static float[] userInitPosition = new float[2] { 0, 0 };

    public static class Constant
    {
        public static LayerMask obstacleMask = 1 << 7, eventMask = 1 << 9, creatureMask = 1 << 8;
    }

    public static class Loading
    {
        public static class System
        {
            public static bool CommonGameManager = false;
            public static bool MouseCursorManager = false;
            public static bool PopupModalController = false;
            public static bool InfoMessageManager = false;
            public static bool ConversationManager = false;

            public static bool isSystemLoadingDone()
            {
                return
                    CommonGameManager &&
                    MouseCursorManager &&
                    PopupModalController &&
                    InfoMessageManager &&
                    ConversationManager &&
                    true
                    ;
            }
        }
    }

    public static class Util
    {
        public static class MouseEvent
        {
            public static System.Action<Vector3> actionSustain = null;
            public static MouseActionBundle Left = new MouseActionBundle(), Right = new MouseActionBundle(), Middle = new MouseActionBundle();
            public class MouseActionBundle
            {
                public System.Action<Transform, Vector3> actionDown, actionUp, actionDrag, actionClick;
                public MouseActionBundle()
                {
                    setActions();
                }
                /// <summary>
                /// 마우스 이벤트용 람다식 4개 선언
                /// </summary>
                /// <param name="actionDown"></param>
                /// <param name="actionUp"></param>
                /// <param name="actionDrag"></param>
                /// <param name="actionClick"></param>
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
        Loading.System.MouseCursorManager = false;
        Loading.System.PopupModalController = false;
        Loading.System.InfoMessageManager = false;
        Loading.System.ConversationManager = false;
    }
}