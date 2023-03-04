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

    public static class Image
    {
        private const string root = "Images/";
        public static class Common
        {
            private const string root = Image.root + "Commons/";
            public const string MouseCursor = root + "MouseCursors/";
        }

        public static class Map
        {
            private const string root = Image.root + "Maps/";
            public const string tile = root + "Tiles/";
        }
    }
}
