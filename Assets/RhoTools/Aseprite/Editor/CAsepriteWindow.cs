using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace RhoTools.Aseprite {
    /// <summary>
    /// Editor window for the aseprite export/import tool
    /// </summary>
    public class CAsepriteWindow : EditorWindow {
        const string PREF_BORDER = "Aseprite_border";
        const string PREF_EXE_PATH = "Aseprite_exe_path";
        const string USE_TAGS = "Aseprite_use_tags";
        const string USE_CONFIGS = "Aseprite_use_configs";
        const string LOOP_ANIM = "Aseprite_loop_anim";
        const string USE_CHILD = "Aseprite_use_child";
        const string PIVOT_X = "Aseprite_pivot_x";
        const string PIVOT_Y = "Aseprite_pivot_y";
        const string ALIGNMENT = "Aseprite_alignment";
        const string ANIM_TYPE = "Aseprite_animType";
        const string AUTO_IMPORT = "Aseprite_autoImport";
        const string IMPORT_SPRITESHEET = "Aseprite_importSpritesheet";
        const string IMPORT_ANIMATIONS = "Aseprite_importAnimations";
        const string CREATE_ANIMATOR = "Aseprite_createAnimator";
        const string PREF_USE_DEFAULT_TARGET = "Aseprite_use_default_target";
        const string PREF_TARGET_DIR = "Aseprite_target_dir";
        const string PREF_TARGET_ANIM_DIR = "Aseprite_target_anim_dir";
        const string PREF_TARGET_PREFABS_DIR = "Aseprite_target_prefabs_dir";
        const string ASE_EXTENSION = "ase";
        const string DEFAULT_TARGET_DIR = "Assets/Aseprite/Sprites";
        const string DEFAULT_PREFABS_DIR = "Assets/Aseprite/Prefabs";
#if UNITY_EDITOR_LINUX
        const string DEFAULT_ASPRITE_PATH = "/usr/bin/aseprite";
#endif

        #region Window definition
        /// <summary>
        /// Open window
        /// </summary>
        [MenuItem(CConstants.ROOT_MENU + "Aseprite importer settings...")]
        public static void ShowWindow() {
            CAsepriteWindow tWindow = GetWindow<CAsepriteWindow>();
            // Create the instance of GUIContent to assign to the window. Gives the title "RBSettings" and the icon
            GUIContent titleContent = new GUIContent("AsepriteImp", tWindow.Icon);
            tWindow.titleContent = titleContent;
        }
        #endregion

        Texture m_Icon;
        Texture Icon {
            get {
                if ( m_Icon == null )
                    m_Icon = GetTexture(this, "icon.png");
                return m_Icon;
            }
        }
        /// <summary>
        /// Get texture allocated in same directory as ScriptableObject
        /// </summary>
        /// <param name="aObject">ScpritableObject to be used to get directory</param>
        /// <param name="aFile">Filename</param>
        /// <returns>Texture if found</returns>
        public static Texture GetTexture(ScriptableObject aObject, string aFile) {
            // Loads an icon from an image stored at the specified path
            MonoScript tScript = MonoScript.FromScriptableObject(aObject);
            string tPath = AssetDatabase.GetAssetPath(tScript);
            tPath = Path.GetDirectoryName(tPath) + "/" + aFile;
            return AssetDatabase.LoadAssetAtPath<Texture>(tPath);
        }

        #region Editor Prefs
        /// <summary>
        /// Path to Aseprite executable
        /// </summary>
        public static string AsepriteExePath {
            get {
                return EditorPrefs.GetString(PREF_EXE_PATH, "");
            }
            set {
                EditorPrefs.SetString(PREF_EXE_PATH, value);
            }
        }

        /// <summary>
        /// Use default directories for targets in first import
        /// </summary>
        public static bool UseConfiguredTargets {
            get {
                return EditorProjectPrefs.GetBool(PREF_USE_DEFAULT_TARGET, false);
            }
            set {
                EditorProjectPrefs.SetBool(PREF_USE_DEFAULT_TARGET, value);
            }
        }

        /// <summary>
        /// Target sprites and json directory
        /// </summary>
        public static string TargetDir {
            get {
                return EditorProjectPrefs.GetString(PREF_TARGET_DIR,
                    DEFAULT_TARGET_DIR);
            }
            set {
                string tVal = value.Replace("\\", "/").Trim();
                if ( tVal.StartsWith(Application.dataPath) )
                    tVal = "Assets" + tVal.Substring(Application.dataPath.Length);
                EditorProjectPrefs.SetString(PREF_TARGET_DIR, tVal);
            }
        }

        /// <summary>
        /// Target prefabs directory
        /// </summary>
        public static string TargetPrefabsDir {
            get {
                return EditorProjectPrefs.GetString(PREF_TARGET_PREFABS_DIR,
                    DEFAULT_PREFABS_DIR);
            }
            set {
                string tVal = value.Replace("\\", "/").Trim();
                if ( tVal.StartsWith(Application.dataPath) )
                    tVal = "Assets" + tVal.Substring(Application.dataPath.Length);
                EditorProjectPrefs.SetString(PREF_TARGET_PREFABS_DIR, tVal);
            }
        }

        // Default values
        /// <summary>
        /// Default value for useTags in Environment
        /// </summary>
        public static bool UseTags {
            get {
                return EditorProjectPrefs.GetBool(USE_TAGS, true);
            }
            set {
                EditorProjectPrefs.SetBool(USE_TAGS, value);
            }
        }

        /// <summary>
        /// Default value for loopAnim in Environment
        /// </summary>

        public static bool LoopAnim {
            get {
                return EditorProjectPrefs.GetBool(LOOP_ANIM, true);
            }
            set {
                EditorProjectPrefs.SetBool(LOOP_ANIM, value);
            }
        }

        /// <summary>
        /// Default value for useConfig in Environment
        /// </summary>
        public static bool UseConfig {
            get {
                return EditorProjectPrefs.GetBool(USE_CONFIGS, true);
            }
            set {
                EditorProjectPrefs.SetBool(USE_CONFIGS, value);
            }
        }

        /// <summary>
        /// Default value for useChild in Environment
        /// </summary>
        public static bool UseChild {
            get {
                return EditorProjectPrefs.GetBool(USE_CHILD, true);
            }
            set {
                EditorProjectPrefs.SetBool(USE_CHILD, value);
            }
        }

        /// <summary>
        /// Default value for border in Environment
        /// </summary>
        public static int Border {
            get {
                return EditorProjectPrefs.GetInt(PREF_BORDER, 0);
            }
            set {
                EditorProjectPrefs.SetInt(PREF_BORDER, value);
            }
        }

        /// <summary>
        /// Default value for alignment in Environment
        /// </summary>
        public static SpriteAlignment Alignment {
            get {
                return (SpriteAlignment)EditorProjectPrefs.GetInt(ALIGNMENT, 0);
            }
            set {
                EditorProjectPrefs.SetInt(ALIGNMENT, (int)value);
            }
        }

        static Vector2 m_Pivot;
        /// <summary>
        /// Default value for pivot in Environment
        /// </summary>
        public static Vector2 Pivot {
            get {
                m_Pivot.x = EditorProjectPrefs.GetFloat(PIVOT_X, 0.5f);
                m_Pivot.y = EditorProjectPrefs.GetFloat(PIVOT_Y, 0.5f);
                return m_Pivot;
            }
            set {
                EditorProjectPrefs.SetFloat(PIVOT_X, value.x);
                EditorProjectPrefs.SetFloat(PIVOT_Y, value.y);
            }
        }

        /// <summary>
        /// Default value for animType in Environment
        /// </summary>
        public static AnimationType AnimType {
            get {
                return (AnimationType)EditorProjectPrefs.GetInt(ANIM_TYPE, 0);
            }
            set {
                EditorProjectPrefs.SetInt(ANIM_TYPE, (int)value);
            }
        }
        #endregion

        GUIContent m_ContentExeFile;
        GUIContent ContentExeFile {
            get {
                if ( m_ContentExeFile == null ) {
                    m_ContentExeFile = new GUIContent("Drop Aseprite executable here",
                        GetTexture(this, "aseprite_exe.png"));
                }
                return m_ContentExeFile;
            }
        }

        Vector2 m_ScrollPos;

        string GetApplicationDir(string aPath) {
            if ( aPath.EndsWith(".app") ) {
                aPath += "/Contents/MacOS/";
                if ( Directory.Exists(aPath) ) {
                    string[] tFiles = Directory.GetFiles(aPath);
                    if ( tFiles.Length > 0 )
                        return tFiles[0];
                }
            }
            return aPath;
        }

#if UNITY_EDITOR_LINUX
        bool m_LinuxExecExists;
        void OnEnable()
        {
            m_LinuxExecExists = File.Exists(DEFAULT_ASPRITE_PATH);
        }
#endif

        void OnGUI() {
            m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
            GUILayout.Space(10f);
            DropAreaGUI(ContentExeFile, 80, (string aPath) => {
#if UNITY_EDITOR_OSX
                aPath = GetApplicationDir(aPath);
#endif
                if ( File.Exists(aPath) )
                    AsepriteExePath = aPath;
            });
            EditorGUI.BeginChangeCheck();
            string tExePath = CEditorGUILayout.FilePathField(
                "Aseprite executable path:",
                AsepriteExePath,
                "Aseprite executable",
#if UNITY_EDITOR_OSX
                "/Applications/",
#elif UNITY_EDITOR_LINUX
                "/usr/bin/",
#else
                "C:\\Program Files\\",
#endif
                ""
            );
            if ( AsepriteExePath == "" ) {
                CEditorGUILayout.Box(EditorGUIUtility.singleLineHeight,
                    "You must select the Aseprite executable file above.");
            }
            if ( EditorGUI.EndChangeCheck() ) {
#if UNITY_EDITOR_OSX
                tExePath = GetApplicationDir(tExePath);
#endif
                if ( File.Exists(tExePath) )
                    AsepriteExePath = tExePath;
            }
#if UNITY_EDITOR_LINUX
            if (m_LinuxExecExists && tExePath != DEFAULT_ASPRITE_PATH && GUILayout.Button("Use Aseprite found in default location"))
                AsepriteExePath = DEFAULT_ASPRITE_PATH;
#endif

            UseConfiguredTargets = EditorGUILayout.ToggleLeft(CAsepriteGUIData.UseConfiguredTargets, UseConfiguredTargets);
            if ( UseConfiguredTargets ) {
                GUILayout.BeginVertical(CAsepriteGUIData.Box);
                // Target dir
                EditorGUI.BeginChangeCheck();
                string tTargetDir = CEditorGUILayout.DirPathField(
                    CAsepriteGUIData.TargetDir, TargetDir, "Select target directory", "");
                if ( EditorGUI.EndChangeCheck() ) {
                    TargetDir = tTargetDir;
                }

                // Target prefabs dir
                EditorGUI.BeginChangeCheck();
                tTargetDir = CEditorGUILayout.DirPathField(
                    CAsepriteGUIData.TargetPrefabsDir, TargetPrefabsDir, "Select target directory", "");
                if ( EditorGUI.EndChangeCheck() ) {
                    TargetPrefabsDir = tTargetDir;
                }
                GUILayout.EndVertical();
            }

            // Defaults
            EditorGUI.BeginChangeCheck();
            GUILayout.Label("Default values", EditorStyles.boldLabel);
            GUILayout.BeginVertical(CAsepriteGUIData.Box);
            bool tUseTags = EditorGUILayout.ToggleLeft(CAsepriteGUIData.UseTags, UseTags);
            bool tUseCustomConfiguration = EditorGUILayout.ToggleLeft(CAsepriteGUIData.UseConfig, UseConfig);
            bool tLoopAnimation = EditorGUILayout.ToggleLeft(CAsepriteGUIData.LoopAnim, LoopAnim);
            bool tUseChild = EditorGUILayout.ToggleLeft(CAsepriteGUIData.UseChild, UseChild);
            int tBorder = EditorGUILayout.IntField(CAsepriteGUIData.Border, Border);
            SpriteAlignment tAlignment = (SpriteAlignment)EditorGUILayout.EnumPopup(CAsepriteGUIData.Alignment,
                Alignment);
            Vector2 tPivot = EditorGUILayout.Vector2Field(CAsepriteGUIData.Pivot, Pivot);
            AnimationType tAnimType = (AnimationType)EditorGUILayout.EnumPopup(CAsepriteGUIData.AnimType, AnimType);
            GUILayout.EndVertical();

            if ( EditorGUI.EndChangeCheck() ) {
                UseTags = tUseTags;
                UseConfig = tUseCustomConfiguration;
                LoopAnim = tLoopAnimation;
                UseChild = tUseChild;
                Border = tBorder;
                Alignment = tAlignment;
                Pivot = tPivot;
                AnimType = tAnimType;
                /*
                ImportSpritesheet = tImportSpritesheet;
                ImportAnimations = tImportAnimations;
                CreateAnimatorDefault = tCreateAnimator;
                AutoImport = tAutoImport;
                */
                EditorProjectPrefs.Save();
            }
            if ( GUILayout.Button("Restore default values") )
                RestoreDefaults();
            GUILayout.EndScrollView();
        }

        static bool DropAreaGUI(GUIContent aContent, float aHeight, Action<string> aCallback) {
            Event evt = Event.current;
            Rect drop_area = Box(aContent, aHeight);
            switch ( evt.type ) {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if ( !drop_area.Contains(evt.mousePosition) )
                        return false;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if ( evt.type == EventType.DragPerform ) {
                        DragAndDrop.AcceptDrag();
                        foreach ( string dragged_object in DragAndDrop.paths ) {
                            aCallback(dragged_object);
                        }
                        return true;
                    }
                    break;
            }
            return false;
        }

        static Rect Box(GUIContent aContent, float aHeight) {
            Rect drop_area = GUILayoutUtility.GetRect(10.0f, aHeight, GUILayout.ExpandWidth(true));
            drop_area.width -= 10;
            drop_area.x += 5;
            GUIStyle tStyle = new GUIStyle(GUI.skin.box) {
                alignment = TextAnchor.MiddleCenter,
                richText = true,
                fontSize = 10
            };

            GUI.Box(drop_area, aContent, tStyle);
            return drop_area;
        }

        /// <summary>
        /// Creates an animator asset using the aseprite object
        /// </summary>
        /// <param name="aObj">Aseprite object</param>
        public static void CreateAnimator(CAsepriteObject aObj) {
            // Check if animator already exists
            if ( aObj.targetAnimator == null || aObj.targetAnimator.layers == null
                || aObj.targetAnimator.layers.Length == 0
                || aObj.targetAnimator.layers[0].stateMachine == null
            ) {
                if ( aObj.targetAnimator != null ) {
                    DestroyImmediate(aObj.targetAnimator);
                }
                aObj.targetAnimator = new AnimatorController();
                aObj.targetAnimator.AddLayer("Base");
                aObj.targetAnimator.name = aObj.name + " Animator";
                AssetDatabase.AddObjectToAsset(aObj.targetAnimator, aObj);
                Debug.Log("Created animator");
            }

            AnimatorStateMachine tRootStateMachine = aObj.targetAnimator.layers[0].stateMachine;
            for ( int i = 0; i < aObj.clips.Length; i++ ) {
                bool tFound = false;
                AnimationClip tClip = aObj.clips[i];
                for ( int j = 0; j < tRootStateMachine.states.Length; j++ ) {
                    if ( tRootStateMachine.states[j].state.motion == tClip ) {
                        tFound = true;
                        break;
                    }
                }
                if ( !tFound ) {
                    AnimatorState tState = tRootStateMachine.AddState(tClip.name);
                    tState.motion = tClip;
                }
            }
        }

        /// <summary>
        /// Creates a prefab using the aseprite object
        /// </summary>
        /// <param name="aObj">Aseprite object</param>
        public static void CreatePrefab(CAsepriteObject aObj) {
            string tPrefabPath = AssetDatabase.GetAssetPath(aObj.targetPrefab);
            if ( string.IsNullOrEmpty(tPrefabPath) ) {
                string tPath = AssetDatabase.GetAssetPath(aObj.asepriteAsset);
                if ( UseConfiguredTargets && !string.IsNullOrEmpty(TargetPrefabsDir) ) {
                    if ( !Directory.Exists(TargetPrefabsDir) )
                        Directory.CreateDirectory(TargetPrefabsDir);
                    tPrefabPath = TargetPrefabsDir + "/" +
                        Path.GetFileNameWithoutExtension(tPath) + ".prefab";
                } else {
                    tPrefabPath = Path.GetDirectoryName(tPath) + "/" +
                        Path.GetFileNameWithoutExtension(tPath) + ".prefab";
                }
            }
            if ( tPrefabPath.StartsWith(Application.dataPath) )
                tPrefabPath = "Assets" + tPrefabPath.Substring(Application.dataPath.Length);

            GameObject tAnimGameObject = new GameObject();
            tAnimGameObject.hideFlags = HideFlags.HideInHierarchy;
            Animator tAnimator = tAnimGameObject.AddComponent<Animator>();

            // Create or edit the animator
            CreateAnimator(aObj);

            tAnimator.runtimeAnimatorController = null;
            tAnimator.runtimeAnimatorController = aObj.targetAnimator;

            bool tUseChild = aObj.useChild;
            SpriteRenderer tRenderer;
            Image tImage;
            EditorCurveBinding spriteBinding = new EditorCurveBinding();

            if ( aObj.animType == AnimationType.SpriteRenderer )
                spriteBinding.type = typeof(SpriteRenderer);
            else if ( aObj.animType == AnimationType.Image )
                spriteBinding.type = typeof(Image);

            spriteBinding.propertyName = "m_Sprite";
            if ( tUseChild )
                spriteBinding.path = CAsepriteImporter.BINDING_PATH;
            else
                spriteBinding.path = "";

            ObjectReferenceKeyframe[] tRef = AnimationUtility.GetObjectReferenceCurve(
                aObj.clips[0], spriteBinding);
            if ( aObj.animType == AnimationType.SpriteRenderer ) {
                if ( tUseChild ) {
                    tRenderer = new GameObject(CAsepriteImporter.BINDING_PATH).AddComponent<SpriteRenderer>();
                    tRenderer.transform.SetParent(tAnimGameObject.transform);
                } else
                    tRenderer = tAnimGameObject.AddComponent<SpriteRenderer>();
                if ( tRef != null && tRef.Length > 0 )
                    tRenderer.sprite = tRef[0].value as Sprite;
                else
                    Debug.LogWarning("The asset is marked as \"" + (aObj.useChild ? "don't " : "")
                        + "use child object for animation\" but the animation seems to be configured differently."
                        + "\nTry changing this configuration.");
            } else if ( aObj.animType == AnimationType.Image ) {
                tAnimGameObject.AddComponent<RectTransform>();
                if ( tUseChild ) {
                    tImage = new GameObject(CAsepriteImporter.BINDING_PATH).AddComponent<Image>();
                    tImage.transform.SetParent(tAnimGameObject.transform);
                } else
                    tImage = tAnimGameObject.AddComponent<Image>();

                if ( tRef != null && tRef.Length > 0 )
                    tImage.sprite = tRef[0].value as Sprite;
                else
                    Debug.LogWarning("The asset is marked as \"" + (aObj.useChild ? "don't " : "")
                        + "use child object for animation\" but the animation seems to be configured differently."
                        + "\nTry changing this configuration.");
            }

            if ( aObj.targetPrefab != null ) {
                aObj.targetPrefab = PrefabUtility.ReplacePrefab(tAnimGameObject,
                    aObj.targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
            } else {
#if UNITY_2019_1_OR_NEWER
                PrefabUtility.SaveAsPrefabAsset(tAnimGameObject, tPrefabPath);
                aObj.targetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(tPrefabPath);
#else
                aObj.targetPrefab = PrefabUtility.CreatePrefab(tPrefabPath, tAnimGameObject);
#endif
            }
            Selection.activeObject = aObj.targetPrefab;
            DestroyImmediate(tAnimGameObject);
            EditorUtility.SetDirty(aObj);
        }

        /// <summary>
        /// Restores all saved configurations to their default values
        /// </summary>
        public static void RestoreDefaults() {
            UseConfiguredTargets = false;
            TargetDir = DEFAULT_TARGET_DIR;
            TargetPrefabsDir = DEFAULT_PREFABS_DIR;
            UseTags = true;
            LoopAnim = true;
            UseConfig = true;
            UseChild = true;
            Border = 0;
            Alignment = SpriteAlignment.Center;
            Pivot = Vector2.one * .5f;
            AnimType = AnimationType.SpriteRenderer;
        }
    }
}
