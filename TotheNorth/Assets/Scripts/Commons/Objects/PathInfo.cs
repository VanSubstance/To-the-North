using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathInfo
{
    public const string root = "Assets/Resources/";
    public static class Json
    {
        public const string root = PathInfo.root + "Jsons/";
    }

    public static class Common
    {
        private const string root = "Commons/";
        public const string MouseCursor = root + "MouseCursors/";
    }
}
