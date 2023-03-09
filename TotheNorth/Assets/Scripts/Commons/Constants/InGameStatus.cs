using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Users.Objects;

namespace Assets.Scripts.Commons.Constants
{
    public static class InGameStatus
    {
        public static class User
        {
            public static class Movement
            {
                public static float spdWalk = 2f;
                public static float weightRun = 1.5f;
                public static float weightCrouch = 0.5f;
                public static MovementType curMovement = MovementType.WALK;
            }
            public static class Detection
            {
                public static class Sight
                {
                    public static float range = 3.0f;
                    public static int degree = 90;
                    public static bool isControllInRealTime = false;
                }
            }
        }
    }
}
