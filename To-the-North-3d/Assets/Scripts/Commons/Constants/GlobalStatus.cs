using UnityEngine;
public static class GlobalStatus
{
    public static string curScene = "";
    public static string nextScene = "";
    public static float[] userInitPosition = new float[2] { 0, 0 };

    public static class Constant
    {
        public static LayerMask
            obstacleMask = 1 << 7,
            blockingSightMask = 1 << 18,
            eventMask = 1 << 9,
            creatureMask = 1 << 8,
            itemMask = 1 << 11,
            slotMask = 1 << 12,
            userMask = 1 << 13,
            hitMask = 1 << 17
            ;
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
            public static bool InventoryLoading = false;

            public static bool isSystemLoadingDone()
            {
                return
                    CommonGameManager &&
                    MouseCursorManager &&
                    PopupModalController &&
                    InfoMessageManager &&
                    ConversationManager &&
                    InventoryLoading &&
                    true
                    ;
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
        Loading.System.InventoryLoading = false;
    }
}