using UnityEngine;

public static class CAsepriteGUIData
{
    #region Content
    static GUIContent m_UseConfiguredTargets;
    static public GUIContent UseConfiguredTargets
    {
        get
        {
            if (m_UseConfiguredTargets == null)
                m_UseConfiguredTargets = new GUIContent("Use configured default targets",
                    "If on, imported files will be saved to the configured target directories," +
                    " otherwise they will be saved to the same directory as the aseprite file");
            return m_UseConfiguredTargets;
        }
    }

    static GUIContent m_TargetDir;
    static public GUIContent TargetDir
    {
        get
        {
            if (m_TargetDir == null)
                m_TargetDir = new GUIContent("Target sprites directory",
                    "Export output will be saved here");
            return m_TargetDir;
        }
    }

    static GUIContent m_TargetAnimDir;
    static public GUIContent TargetAnimDir
    {
        get
        {
            if (m_TargetAnimDir == null)
                m_TargetAnimDir = new GUIContent("Target animations directory",
                    "Output animations and animators will be saved here");
            return m_TargetAnimDir;
        }
    }

    static GUIContent m_TargetPrefabsDir;
    static public GUIContent TargetPrefabsDir
    {
        get
        {
            if (m_TargetPrefabsDir == null)
                m_TargetPrefabsDir = new GUIContent("Target prefabs directory",
                    "Output prefabs will be saved here");
            return m_TargetPrefabsDir;
        }
    }

    static GUIContent m_UseTags;
    static public GUIContent UseTags
    {
        get
        {
            if (m_UseTags == null)
                m_UseTags = new GUIContent("Use tags",
                    "Tags in aseprite will be imported as separate animation clips");
            return m_UseTags;
        }
    }

    static GUIContent m_TargetTexture;
    static public GUIContent TargetTexture
    {
        get
        {
            if (m_TargetTexture == null)
                m_TargetTexture = new GUIContent("Target texture",
                    "Texture containing the animation spritesheet");
            return m_TargetTexture;
        }
    }

    static GUIContent m_TargetAtlas;
    static public GUIContent TargetAtlas
    {
        get
        {
            if (m_TargetAtlas == null)
                m_TargetAtlas = new GUIContent("Target atlas",
                    "Text file with the animation information");
            return m_TargetAtlas;
        }
    }

    static GUIContent m_UseConfig;
    static public GUIContent UseConfig
    {
        get
        {
            if (m_UseConfig == null)
                m_UseConfig = new GUIContent("Use custom configuration",
                    "Custom configuration can be added to the tags (see documentation)");
            return m_UseConfig;
        }
    }

    static GUIContent m_LoopAnim;
    static public GUIContent LoopAnim
    {
        get
        {
            if (m_LoopAnim == null)
                m_LoopAnim = new GUIContent("Loop animation",
                    "Imported animations loop (overriden by \"Use custom animation\"");
            return m_LoopAnim;
        }
    }

    static GUIContent m_UseChild;
    static public GUIContent UseChild
    {
        get
        {
            if (m_UseChild == null)
                m_UseChild = new GUIContent("Use child object for animation",
                    "The animated sprite will be placed as a child of the GameObject with the Animator component");
            return m_UseChild;
        }
    }

    static GUIContent m_Border;
    static public GUIContent Border
    {
        get
        {
            if (m_Border == null)
                m_Border = new GUIContent("Border",
                    "Adds transparent pixels around the sprites (generally not necesary)");
            return m_Border;
        }
    }

    static GUIContent m_Alignment;
    static public GUIContent Alignment
    {
        get
        {
            if (m_Alignment == null)
                m_Alignment = new GUIContent("Pivot",
                    "Sprite pivot point in its localspace");
            return m_Alignment;
        }
    }

    static GUIContent m_Pivot;
    static public GUIContent Pivot
    {
        get
        {
            if (m_Pivot == null)
                m_Pivot = new GUIContent("Custom pivot",
                    "Pivot used when \"Pivot\" is set to \"Custom\"");
            return m_Pivot;
        }
    }

    static GUIContent m_AnimType;
    static public GUIContent AnimType
    {
        get
        {
            if (m_AnimType == null)
                m_AnimType = new GUIContent("Animation type",
                    "Select Sprite Renderer for 2D sprite and Image for UI");
            return m_AnimType;
        }
    }

    static GUIContent m_ImportSpritesheet;
    static public GUIContent ImportSpritesheet
    {
        get
        {
            if (m_ImportSpritesheet == null)
                m_ImportSpritesheet = new GUIContent("Import spritesheet",
                    "Create the spritesheet on the exported texture using the exported atlas");
            return m_ImportSpritesheet;
        }
    }

    static GUIContent m_ImportAnimations;
    static public GUIContent ImportAnimations
    {
        get
        {
            if (m_ImportAnimations == null)
                m_ImportAnimations = new GUIContent("Import animations",
                    "Create the animation clips");
            return m_ImportAnimations;
        }
    }

    static GUIContent m_CreateAnimator;
    static public GUIContent CreateAnimator
    {
        get
        {
            if (m_CreateAnimator == null)
                m_CreateAnimator = new GUIContent("Create animator",
                    "Create the animator with all the animation clips");
            return m_CreateAnimator;
        }
    }

    static GUIContent m_AutoImport;
    static public GUIContent AutoImport
    {
        get
        {
            if (m_AutoImport == null)
                m_AutoImport = new GUIContent("Auto import",
                    "Will import new assets automatically and everytime there are changes to the file");
            return m_AutoImport;
        }
    }
    #endregion
    #region Styles
    static GUIStyle m_Box;
    public static GUIStyle Box
    {
        get
        {
            if (m_Box == null)
                m_Box = new GUIStyle("box");
            return m_Box;
        }
    }
    #endregion
}