using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        private AudioSource _audioSource;

        public AudioClip ClickStartClip;
        public AudioClip ClickBtnClip;
        public AudioClip AchievementClip;
        public AudioClip GetItemClip;
        public AudioClip LoseClip;
        public AudioClip WinClip;

        public enum ClipName
        {
            ClickStart = 0,
            ClickBtn = 1,
            Achievement = 2,
            GetItem = 3,
            Lose = 4,
            Win = 5
        };
        // Start is called before the first frame update
        void Start()
        {
            _audioSource = this.gameObject.AddComponent<AudioSource>();

            //设置循环播放

            _audioSource.loop = false;

            //设置音量为最大，区间在0-1之间

            _audioSource.volume = 1.0f;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayAudioClip(ClipName clipName)
        {
            if (clipName == ClipName.ClickStart)
            {
                this._audioSource.clip = this.ClickStartClip;
            }
            else if (clipName == ClipName.ClickBtn)
            {
                this._audioSource.clip = this.ClickBtnClip;
            }
            else if (clipName == ClipName.Achievement)
            {
                this._audioSource.clip = this.AchievementClip;
            }
            else if (clipName == ClipName.GetItem)
            {
                this._audioSource.clip = this.GetItemClip;
            }
            else if (clipName == ClipName.Lose)
            {
                this._audioSource.clip = this.LoseClip;
            }
            else if (clipName == ClipName.Win)
            {
                this._audioSource.clip = this.WinClip;
            }
            this._audioSource.Play();
        }
    }

}
