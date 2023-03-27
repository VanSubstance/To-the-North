using System;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalComponent
{
    public static class Modal
    {
        public static class Popup
        {
            public static PopupModalController controller;
            public static Dictionary<ModalType, Transform> contentPrefabs = new Dictionary<ModalType, Transform>();
        }
    }

    public static class Common
    {
        public static Transform userTf;
        public static class Info
        {
            public static InfoMessageManager controller;
        }
        public static class Event
        {
            public static MouseCursorManager mouseCursorManager;
        }
    }
}