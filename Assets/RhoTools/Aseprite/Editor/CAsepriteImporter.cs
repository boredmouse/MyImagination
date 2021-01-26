﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace RhoTools.Aseprite
{
    /// <summary>
    /// Aseprite importer tool
    /// </summary>
    public class CAsepriteImporter
    {
        /// <summary>
        /// The name of the gameobject created if the animation 
        /// is imported with the option "use child sprite for animation"
        /// </summary>
        public const string BINDING_PATH = "sprite";
        const char TAG_SEPARATOR = '_';
        const char CONFIG_SEPARATOR = '-';
        const string NO_LOOP = "noloop";
        const char SEPARATOR = ',';
        const string EVENT = "event";
        const string EVENT_PLUS = "event+";
        const char EVENT_SEPARATOR = ':';
        readonly static string[] FRAME_OPTIONS = { EVENT, EVENT_PLUS };
        // -event:name,value

        static EditorCurveBinding _binding;

        static Dictionary<string, string[]> _options;
        static Dictionary<string, KeyValuePair<int, string[]>> _frameOptions;
        static Dictionary<string, int> _animationLengths;

        class Frame
        {
            public ObjectReferenceKeyframe frame;
            public float length;
        }

        static Sprite GetSprite(Sprite[] aList, string aName)
        {
            for (int i = 0; i < aList.Length; i++)
            {
                Sprite tSprite = aList[i];
                if (tSprite.name == aName)
                    return tSprite;
            }
            return null;
        }

        /// <summary>
        /// Creates spritesheet on aTexture using the json aAtlas generated by aseprite
        /// </summary>
        /// <param name="aTexture">Spritesheet texture</param>
        /// <param name="aObj">Aseprite object</param>
        public static void ImportSheet(TextureImporter aTexture, CAsepriteObject aObj)
        {
            // Change metadata
            CAtlasData tData = JsonConvert.DeserializeObject<CAtlasData>(aObj.targetAtlas.text);
            List<SpriteMetaData> tMetaData = new List<SpriteMetaData>();
            aTexture.textureType = TextureImporterType.Sprite;
            aTexture.spriteImportMode = SpriteImportMode.Multiple;
            aTexture.filterMode = FilterMode.Point;
#if UNITY_5_6_OR_NEWER
            aTexture.textureCompression = TextureImporterCompression.Uncompressed;
#else
            aTexture.textureFormat = TextureImporterFormat.AutomaticTruecolor;
#endif
            List<string> tKeys = new List<string>();
            tKeys.AddRange(tData.frames.Keys);
            tKeys.Sort();

            Dictionary<string, int> tAnimations = new Dictionary<string, int>();

            Vector2 tPivot = aObj.pivot;
            SpriteAlignment tAlignment = aObj.alignment;
            for (int i = 0; i < tKeys.Count; i++)
            {
                string tKey = tKeys[i];
                CAtlasData.Frame tFrame = tData.frames[tKey];
                SpriteMetaData tThisData = new SpriteMetaData();
                string tTag = GetTag(tKey);
                if (tTag != "")
                {
                    if (tAnimations.ContainsKey(tTag))
                        tAnimations[tTag] += 1;
                    else
                        tAnimations.Add(tTag, 0);

                    string tName = tTag + "_" + tAnimations[tTag];
                    tThisData.name = tName;
                    tThisData.rect = new Rect(
                        tFrame.frame.x,
                        tFrame.frame.y,
                        tFrame.frame.w,
                        tFrame.frame.h
                    );
                    tThisData.rect.y = tData.meta.size.h - tThisData.rect.y - tThisData.rect.height;
                    tThisData.alignment = (int)tAlignment;
                    tThisData.pivot = tPivot;
                    tMetaData.Add(tThisData);
                }
            }

            aTexture.spritesheet = tMetaData.ToArray();
            AssetDatabase.ImportAsset(aTexture.assetPath, ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// Creates AnimationClips using the information in the json atlas created
        /// by aseprite.
        /// If CAsepriteWindow.UseTags is true, it separates the animations,
        /// otherwise it creates only one animation.
        /// </summary>
        /// <param name="aTexture">Spritesheet texture</param>
        /// <param name="aObj">Aseprite object</param>
        /// <returns>AnimationClips created</returns>
        public static AnimationClip[] ImportAnimation(TextureImporter aTexture, CAsepriteObject aObj)
        {
            CAtlasData tData = JsonConvert.DeserializeObject<CAtlasData>(aObj.targetAtlas.text);
            aTexture.textureType = TextureImporterType.Sprite;
            aTexture.spriteImportMode = SpriteImportMode.Multiple;
            aTexture.filterMode = FilterMode.Point;
            List<string> tKeys = new List<string>();
            tKeys.AddRange(tData.frames.Keys);

            Dictionary<string, int> tAnimationsIndices = new Dictionary<string, int>();
            Dictionary<string, List<Frame>> tAnimations = new Dictionary<string, List<Frame>>();
            Sprite[] tSprites = AssetDatabase.LoadAllAssetsAtPath(aTexture.assetPath)
                .OfType<Sprite>().ToArray();
            ResetOptions();

            foreach (string tKey in tKeys)
            {
                string tTag = GetTag(tKey);
                if (tTag != "")
                {
                    List<Frame> tFrameList;
                    if (tAnimations.ContainsKey(tTag))
                    {
                        tFrameList = tAnimations[tTag];
                    }
                    else
                    {
                        tAnimationsIndices.Add(tTag, 0);
                        tFrameList = new List<Frame>();
                        tAnimations.Add(tTag, tFrameList);
                    }
                    ObjectReferenceKeyframe kf = new ObjectReferenceKeyframe();
                    if (tFrameList.Count > 0)
                    {
                        float tTime = 0;
                        for (int i = 0; i < tFrameList.Count; i++)
                        {
                            tTime += tFrameList[i].length;
                        }
                        kf.time = tTime;
                    }
                    else
                        kf.time = 0;

                    kf.value = GetSprite(tSprites, tTag + "_" + tAnimationsIndices[tTag].ToString());
                    Frame tFrame = new Frame();
                    tFrame.frame = kf;
                    tFrame.length = tData.frames[tKey].duration / 1000f;
                    tAnimations[tTag].Add(tFrame);
                    tAnimationsIndices[tTag] += 1;
                }
            }

            foreach (KeyValuePair<string, KeyValuePair<int, string[]>> tPair in _frameOptions)
            {
                KeyValuePair<int, string[]> tEvent = tPair.Value;
                for (int i = 0; i < tEvent.Value.Length; i++)
                    Debug.Log(tEvent.Key + ", " + tEvent.Value[i]);
            }

            EditorCurveBinding tBinding = GetBinding(aObj);
            AnimationClip[] tClips = new AnimationClip[tAnimations.Keys.Count];
            int j = 0;
            foreach (string tKey in tAnimations.Keys)
            {
                int tInd = ClipInList(aObj.clips, tKey);
                AnimationClip tClip;
                if (tInd >= 0)
                    tClip = aObj.clips[tInd];
                else
                    tClip = new AnimationClip();

                List<Frame> tAnimation = tAnimations[tKey];
                int tFramecount = tAnimation.Count;

                tClip.frameRate = 60;
                tClip.wrapMode = WrapMode.Loop;

                ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[tFramecount + 1];

                for (int i = 0; i < tFramecount; i++)
                {
                    keyFrames[i] = tAnimation[i].frame;
                }
                ObjectReferenceKeyframe kf = new ObjectReferenceKeyframe();
                kf.time = keyFrames[tFramecount - 1].time + tAnimation[0].length;
                if ((!aObj.useConfig && aObj.loopAnim)
                    || (aObj.useConfig && !HasOption(tKey, NO_LOOP)))
                {
                    // Make last frame the same as first
                    kf.value = tAnimation[0].frame.value;
                    // Make animation loop
                    AnimationClipSettings tSettings = AnimationUtility.GetAnimationClipSettings(tClip);
                    tSettings.loopTime = true;
                    AnimationUtility.SetAnimationClipSettings(tClip, tSettings);
                }
                else
                    kf.value = tAnimation[tAnimation.Count - 1].frame.value;
                keyFrames[tFramecount] = kf;

                AnimationUtility.SetObjectReferenceCurve(tClip, tBinding, keyFrames);

                tClip.name = tKey;
                tClips[j] = tClip;
                j++;
                /*
                string tAnimPath;
                if (aEnv.targetAnimator != null)
                {
                    string tPath = AssetDatabase.GetAssetPath(aEnv.targetAnimator);
                    tAnimPath = Path.GetDirectoryName(tPath) + "/" + aEnv.name + "_" + tKey + ".anim";
                }
                else
                {
                    if (CAsepriteWindow.UseConfiguredTargets)
                        tAnimPath = CAsepriteWindow.TargetAnimDir;
                    else
                        tAnimPath = Path.GetDirectoryName(
                            AssetDatabase.GetAssetPath(aEnv.targetTexture));
                    if (string.IsNullOrEmpty(tAnimPath))
                        tAnimPath = Path.GetDirectoryName(
                            AssetDatabase.GetAssetPath(aEnv.targetTexture));
                    tAnimPath += "/" + aEnv.name + "_" + tKey + ".anim";
                }
                */

                if (tInd < 0)
                {
                    //if (tAnimPath.StartsWith(Application.dataPath))
                      //  tAnimPath = "Assets/" + tAnimPath.Substring(Application.dataPath.Length);
                    //AssetDatabase.CreateAsset(tClip, tAnimPath);
                    AssetDatabase.AddObjectToAsset(tClip, aObj);
                }
            }
            AssetDatabase.SaveAssets();

            // Frame options
            foreach (KeyValuePair<string, KeyValuePair<int, string[]>> tOption in _frameOptions)
            {
                AnimationClip tClip = GetClip(tClips, tOption.Key);
                List<AnimationEvent> tEvents = new List<AnimationEvent>();
                if (tClip != null)
                {
                    ObjectReferenceKeyframe[] tFrameList = AnimationUtility.GetObjectReferenceCurve(tClip, tBinding);
                    int tFrameNum = tOption.Value.Key;
                    if (tFrameNum >= 0 && tFrameNum < tFrameList.Length)
                    {
                        ObjectReferenceKeyframe tFrame = tFrameList[tFrameNum];

                        for (int i = 0; i < tOption.Value.Value.Length; i++)
                        {
                            string tOptionName = tOption.Value.Value[i];
                            if (tOptionName.StartsWith(EVENT_PLUS) && tFrameNum < tFrameList.Length - 1)
                            {
                                tFrame = tFrameList[tFrameNum + 1];
                                AnimationEvent tEvent = new AnimationEvent();
                                tEvent.time = tFrame.time;
                                string[] tValues = tOptionName.Split(EVENT_SEPARATOR);
                                if (tValues.Length > 1)
                                {
                                    tValues = tValues[1].Split(SEPARATOR);
                                    if (tValues.Length > 1)
                                    {
                                        tEvent.functionName = tValues[0];
                                        tEvent.stringParameter = tValues[1];
                                        tEvents.Add(tEvent);
                                    }
                                }
                            }
                            else if (tOptionName.StartsWith(EVENT))
                            {
                                AnimationEvent tEvent = new AnimationEvent();
                                tEvent.time = tFrame.time;
                                string[] tValues = tOptionName.Split(EVENT_SEPARATOR);
                                if (tValues.Length > 1)
                                {
                                    tValues = tValues[1].Split(SEPARATOR);
                                    if (tValues.Length > 1)
                                    {
                                        tEvent.functionName = tValues[0];
                                        tEvent.stringParameter = tValues[1];
                                        tEvents.Add(tEvent);
                                    }
                                }
                            }
                        }

                    }
                    AnimationUtility.SetAnimationEvents(tClip, tEvents.ToArray());
                }
            }

            aObj.clips = tClips;
            return tClips;
        }

        static int ClipInList(AnimationClip[] aList, string aClip)
        {
            if (aList == null)
                return -1;
            for (int i = 0; i < aList.Length; i++)
            {
                if (aList[i] != null && aList[i].name == aClip)
                    return i;
            }
            return -1;
        }

        static EditorCurveBinding GetBinding(CAsepriteObject aEnv)
        {
            EditorCurveBinding curveBinding = new EditorCurveBinding();

            // Set type
            if (aEnv.animType == AnimationType.SpriteRenderer)
                curveBinding.type = typeof(SpriteRenderer);
            else if (aEnv.animType == AnimationType.Image)
                curveBinding.type = typeof(UnityEngine.UI.Image);

            curveBinding.propertyName = "m_Sprite";
            if (aEnv.useChild)
                curveBinding.path = BINDING_PATH;
            else
                curveBinding.path = "";

            return curveBinding;
        }

        static AnimationClip GetClip(AnimationClip[] aClips, string aTag)
        {
            for (int i = 0; i < aClips.Length; i++)
            {
                AnimationClip tClip = aClips[i];
                if (tClip.name == aTag)
                    return tClip;
            }
            return null;
        }

        static string GetTag(string aName)
        {
            if (_animationLengths == null)
                _animationLengths = new Dictionary<string, int>();
            string[] tSplit = aName.Split(TAG_SEPARATOR);
            string tTag = "";
            if (tSplit.Length > 1)
                tTag += tSplit[1];
            for (int i = 2; i < tSplit.Length; i++)
                tTag += "_" + tSplit[i];
            if (CAsepriteWindow.UseConfig)
            {
                tSplit = tTag.Split(CONFIG_SEPARATOR);
                tTag = tSplit[0];
                if (_options == null)
                    _options = new Dictionary<string, string[]>();
                if (!_options.ContainsKey(tTag))
                {
                    string[] tOptions = new string[tSplit.Length - 1];
                    for (int i = 1; i < tSplit.Length; i++)
                    {
                        tOptions[i - 1] = tSplit[i].ToLower();
                    }
                    _options.Add(tTag, tOptions);
                }

                if (_frameOptions == null)
                    _frameOptions = new Dictionary<string, KeyValuePair<int, string[]>>();
                int tFrameNum;
                if (!_animationLengths.ContainsKey(tTag))
                    tFrameNum = 0;
                else
                    tFrameNum = _animationLengths[tTag];
                if (!_frameOptions.ContainsKey(tTag))
                {
                    List<string> tFrameOptions = new List<string>();
                    for (int i = 1; i < tSplit.Length; i++)
                    {
                        for (int j = 0; j < FRAME_OPTIONS.Length; j++)
                        {
                            if (tSplit[i].StartsWith(FRAME_OPTIONS[j]))
                            {
                                tFrameOptions.Add(tSplit[i]);
                                break;
                            }
                        }
                    }
                    if (tFrameOptions.Count > 0)
                        _frameOptions.Add(tTag, new KeyValuePair<int, string[]>(tFrameNum, tFrameOptions.ToArray()));
                }
            }
            if (!_animationLengths.ContainsKey(tTag))
                _animationLengths.Add(tTag, 1);
            else
                _animationLengths[tTag]++;
            return tTag;
        }

        static bool HasOption(string aTag, string aOption)
        {
            if (_options.ContainsKey(aTag))
            {
                string[] tOptions = _options[aTag];
                for (int i = 0; i < tOptions.Length; i++)
                {
                    if (tOptions[i] == aOption)
                        return true;
                }
            }
            return false;
        }

        static void ResetOptions()
        {
            _options = new Dictionary<string, string[]>();
            _frameOptions = new Dictionary<string, KeyValuePair<int, string[]>>();
            _animationLengths = new Dictionary<string, int>();
        }
    }
}
