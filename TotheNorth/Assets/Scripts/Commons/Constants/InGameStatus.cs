using Assets.Scripts.Components.Progress;
using Assets.Scripts.Users.Objects;

namespace Assets.Scripts.Commons.Constants
{
    public static class InGameStatus
    {
        public static class User
        {
            public static bool isPause = false;

            public static class status
            {
                public static BarBaseController hpBar;
                public static BarBaseController staminaBar;
            }
            public static class Movement
            {
                /** 현재 바라보고 있는 시야 방향 각도 */
                public static int curdegree = 0;
                public static float spdWalk = 4f;
                public static float weightRun = 7f;
                public static float weightCrouch = 2.5f;
                public static MovementType curMovement = MovementType.WALK;
            }
            public static class Detection
            {
                public static float distanceInteraction = 2f;
                public static class Instinct
                {
                    public static float range = 2.0f;
                }
                public static class Sight
                {
                    public static float rangeMin = 45.0f;
                    public static float rangeMax = 60.0f;
                    public static float range = 45.0f;
                    public static int degree = 90;
                    public static bool isControllInRealTime = false;
                }
            }
        }
    }
}
