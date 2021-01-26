using System.Collections.Generic;

namespace RhoTools.Aseprite
{
    /// <summary>
    /// Serialized animation data
    /// </summary>
    [System.Serializable]
    public class CAtlasData
    {
        /// <summary>
        /// Frame data
        /// </summary>
        [System.Serializable]
        public class Frame
        {
            public FrameRect frame;
            public bool rotated;
            public bool trimmed;
            public FrameRect spriteSourceSize;
            public Size sourceSize;
            public int duration;
        }

        /// <summary>
        /// Rectangle
        /// </summary>
        [System.Serializable]
        public class FrameRect
        {
            public int x;
            public int y;
            public int w;
            public int h;
        }

        /// <summary>
        /// Size
        /// </summary>
        [System.Serializable]
        public class Size
        {
            public int w;
            public int h;
        }

        /// <summary>
        /// Other data
        /// </summary>
        [System.Serializable]
        public class MetaData
        {
            public string app;
            public string version;
            public string image;
            public string format;
            public Size size;
            public float scale;
        }

        public Dictionary<string, Frame> frames;
        public MetaData meta;
    }
}
