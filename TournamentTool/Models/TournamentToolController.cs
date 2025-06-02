using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TournamentTool.Models
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
	public class TournamentToolController : MonoBehaviour
    {
        private PauseMenuManager _pauseMenuManager;
        private IAudioTimeSource _audioTimeSource;
        public string _counterText;
        public Transform _backButton;
        public int _searchCounter = 0;
        public int _currentSongTime;
        public float _firstSontime = 0;
        public bool _songStarted = false;
        public bool _firstSongStarted = false;
        [Inject]
        public void Constractor(PauseMenuManager pauseMenuManager, IAudioTimeSource audioTimeSource)
        {
            this._pauseMenuManager = pauseMenuManager;
            this._audioTimeSource = audioTimeSource;
        }

        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {

        }
        /// <summary>
        /// Only ever called once on the first frame the script is Enabled. Start is called after every other script's Awake() and before Update().
        /// </summary>
        private void Start()
        {
            Plugin.Log.Info("TournamentToolController Start");
        }

        /// <summary>
        /// Called every frame if the script is enabled.
        /// </summary>
        private void Update()
        {

        }

        /// <summary>
        /// Called every frame after every other enabled script's Update().
        /// </summary>
        private void LateUpdate()
        {
            this._searchCounter++;
            if (!this._songStarted)
            {
                if (this._audioTimeSource.songTime > 0f)
                {
                    this._songStarted = true;
                    Plugin.Log.Info($"Song started at: {this._audioTimeSource.songTime} : frame {this._searchCounter} : {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
                    this._firstSontime = this._audioTimeSource.songTime;
                }
            }
            if (!this._firstSongStarted)
            {
                if (this._firstSontime != this._audioTimeSource.songTime)
                {
                    this._firstSongStarted = true;
                    Plugin.Log.Info($"Song started at: {this._audioTimeSource.songTime} : frame {this._searchCounter} : {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
                }
            }
            var songTime = Mathf.FloorToInt(this._audioTimeSource.songTime);
            if (this._currentSongTime != songTime)
            {
                this._currentSongTime = songTime;
                Plugin.Log.Info($"Song Time to: {songTime}s : frame {this._searchCounter} : {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
            }
            if (this._backButton == null)
            {
                var backButton2 = this._pauseMenuManager._backButton;
                Transform transform;
                if (backButton2 == null)
                {
                    transform = null;
                }
                else
                {
                    var gameObject4 = backButton2.gameObject;
                    if (gameObject4 == null)
                    {
                        transform = null;
                    }
                    else
                    {
                        var transform2 = gameObject4.transform;
                        transform = ((transform2 != null) ? transform2.parent : null);
                    }
                }
                var transform3 = transform;
                if (transform3 != null && transform3)
                {
                    this._backButton = transform3;
                }
            }
            if (this._backButton != null && this._backButton)
            {
                foreach (FormattableText child in this._backButton.GetComponentsInChildren<FormattableText>())
                {
                    var text = child.text;
                    if (this._counterText != text)
                    {
                        this._counterText = text;
                        Plugin.Log.Info($"Counter text changed to: {this._counterText} : frame {this._searchCounter} : {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
                        var builder = new StringBuilder(1000);
                        builder = GetFullPathName(child.transform, builder);
                        Plugin.Log.Info($"Counter text full path: {builder}");
                        builder.Clear();
                    }
                }
            }
        }
        public StringBuilder GetFullPathName(Transform transform, StringBuilder builder)
        {
            if (transform.parent == null)
                return builder.Append(transform.name);
            return GetFullPathName(transform.parent, builder).Append($"/{transform.name}");
        }

        /// <summary>
        /// Called when the script becomes enabled and active
        /// </summary>
        private void OnEnable()
        {

        }

        /// <summary>
        /// Called when the script becomes disabled or when it is being destroyed.
        /// </summary>
        private void OnDisable()
        {

        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {

        }
        #endregion
    }
}
