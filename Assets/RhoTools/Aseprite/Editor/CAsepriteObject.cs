using System;
using UnityEditor.Animations;
using UnityEngine;

namespace RhoTools.Aseprite
{
    /// <summary>
    /// Type of animation to be used
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// <see cref="UnityEngine.SpriteRenderer"/>
        /// </summary>
        SpriteRenderer,
        /// <summary>
        /// <see cref="UnityEngine.UI.Image"/>
        /// </summary>
        Image,
    }

    /// <summary>
    /// This object contains all information about the animation
    /// </summary>
    public class CAsepriteObject : ScriptableObject
    {
        /// <summary>
        /// Aseprite file
        /// </summary>
        public UnityEngine.Object asepriteAsset;
        /// <summary>
        /// Target texture with spritemap
        /// </summary>
        public Texture2D targetTexture;
        /// <summary>
        /// Target TextAsset with atlas
        /// </summary>
        public TextAsset targetAtlas;
        /// <summary>
        /// Animator controller
        /// </summary>
        public AnimatorController targetAnimator;
        /// <summary>
        /// Saved prefab (will be replaced when creating a new one)
        /// </summary>
        public GameObject targetPrefab;
        /// <summary>
        /// Use tags to separate animations
        /// </summary>
        public bool useTags;
        /// <summary>
        /// Loop animations (ignored if "useConfig" is true)
        /// </summary>
        public bool loopAnim;
        /// <summary>
        /// Use custom configuration in tags
        /// </summary>
        public bool useConfig;
        /// <summary>
        /// Add empty border to sprites
        /// </summary>
        public int border;
        /// <summary>
        /// Create a child GameObject to hold the SpriteRenderer
        /// or Image that's being animated (recommended)
        /// </summary>
        public bool useChild;
        /// <summary>
        /// Alignment of the sprite
        /// </summary>
        public SpriteAlignment alignment;
        /// <summary>
        /// Position of the pivot (only used when alignment is set to CustomPivot)
        /// </summary>
        public Vector2 pivot;
        /// <summary>
        /// Type of animation (SpriteRenderer or Image (GUI))
        /// </summary>
        public AnimationType animType;
        /// <summary>
        /// List of imported animation clips
        /// </summary>
        public AnimationClip[] clips;
        /// <summary>
        /// Last time the asset was exported
        /// </summary>
        public DateTime LastExport
        {
            get
            {
                DateTime tDate;
                if (DateTime.TryParse(lastExportString, out tDate))
                    return tDate;
                return DateTime.MinValue;
            }
            set { lastExportString = value.ToString(); }
        }
        [SerializeField]
        string lastExportString;
    }
}
