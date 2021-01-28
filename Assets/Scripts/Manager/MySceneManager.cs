using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WHGame
{
    public class MySceneManager : MonoBehaviour
    {
        //public static MySceneManager Instance;
        public static string currentScene = "Start";
        public delegate void OnSceneLoadedDel(string sceneName);
        public static OnSceneLoadedDel OnSceneLoadedEvent;


        public static void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
            MySceneManager.currentScene = "Scenes/" + scene.name;
            OnSceneLoadedEvent(currentScene);
        }
        public static void EnterScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public class SceneName
    {
        public const string Start = "Scenes/Start";
        public const string Battle = "Scenes/Battle";
        public const string Home = "Scenes/Home";
    }

}
