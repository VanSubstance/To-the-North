public static class GlobalStatus
{
    public static string curScene = "메인메뉴씬";
    public static class Loading
    {
        public static class System
        {
            public static bool CommonGameManager = false;
            public static bool InfoMessageManager = false;

            public static bool isSystemLoadingDone()
            {
                return CommonGameManager && InfoMessageManager;
            }
        }
    }

    public static void resetLoading()
    {
        Loading.System.CommonGameManager = false;
        Loading.System.InfoMessageManager = false;
    }
}