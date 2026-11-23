using System;
using _Project.Scripts.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private const string MasterVolumeParameter = "MasterVolume";
        private const string MusicVolumeParameter = "MusicVolume";
        private const string SfxVolumeParameter = "SfxVolume";
        
        private const float MinVolume = 0.0001f;
        private const float DefaultVolume = 1f;

        [Header("Mixer")]
        [Tooltip("Audio Mixer du jeu, avec les volumes Master, Music et Sfx exposés")]
        [SerializeField] private AudioMixer _mixer;
        
        [Header("Références")]
        [Tooltip("GameManager de la scène, dont on écoute les changements de phase et la pause")]
        [SerializeField] private GameManager _gameManager;

        [Tooltip("Source de la musique, branchée sur le groupe Music du Mixer")]
        [SerializeField] private AudioSource _musicSource;

        [Tooltip("Source des effets globaux, branchée sur le groupe SFX du Mixer")]
        [SerializeField] private AudioSource _sfxSource;

        [Header("Musiques")]
        [Tooltip("Jouée en boucle pendant le tirage des cartes et la pose des défenses")]
        [SerializeField] private AudioClip _calmMusic;

        [Tooltip("Jouée en boucle pendant la vague")]
        [SerializeField] private AudioClip _waveMusic;

        [Tooltip("Jouée une fois à la victoire")]
        [SerializeField] private AudioClip _victoryJingle;

        [Tooltip("Jouée une fois à la défaite")]
        [SerializeField] private AudioClip _defeatJingle;

        [Header("Test")]
        [Tooltip("Effet joué par le menu « Test : jouer un effet » (clic droit sur le composant)")]
        [SerializeField] private AudioClip _testSfx;

        private void Awake()
        {
            // La musique continue pendant la pause : seuls les effets se figent
            _musicSource.ignoreListenerPause = true;
        }

        private void OnEnable()
        {
            _gameManager.PhaseChanged += OnPhaseChanged;
            _gameManager.PauseChanged += OnPauseChanged;
        }

        private void OnDisable()
        {
            _gameManager.PhaseChanged -= OnPhaseChanged;
            _gameManager.PauseChanged -= OnPauseChanged;
        }

        private void Start()
        {
            foreach (AudioChannel channel in Enum.GetValues(typeof(AudioChannel)))
            {
                ApplyVolume(channel, GetVolume(channel));
            }
        }

        private void OnDestroy()
        {
            // AudioListener.pause est global : sans ça, quitter la scène pendant une pause couperait le son ensuite
            AudioListener.pause = false;
        }

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            _sfxSource.PlayOneShot(clip);
        }

        public void SetVolume(AudioChannel channel, float volume)
        {
            volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(GetParameterName(channel), volume);
            ApplyVolume(channel, volume);
        }

        public float GetVolume(AudioChannel channel)
        {
            return PlayerPrefs.GetFloat(GetParameterName(channel), DefaultVolume);
        }

        private void OnPhaseChanged(GamePhase phase)
        {
            switch (phase)
            {
                case GamePhase.DrawCard:
                case GamePhase.PlacementDefense:
                    PlayMusic(_calmMusic, true);
                    break;
                case GamePhase.WaveDefense:
                    PlayMusic(_waveMusic, true);
                    break;
                case GamePhase.Victory:
                    PlayMusic(_victoryJingle, false);
                    break;
                case GamePhase.Defeat:
                    PlayMusic(_defeatJingle, false);
                    break;
            }
        }

        private void OnPauseChanged(bool isPaused)
        {
            AudioListener.pause = isPaused;
        }

        private void PlayMusic(AudioClip clip, bool loop)
        {
            if (clip == null)
            {
                _musicSource.Stop();
                _musicSource.clip = null;
                return;
            }

            // Pas de redémarrage si la musique est déjà en cours (passage du tirage à la pose)
            if (_musicSource.clip == clip && _musicSource.isPlaying)
            {
                return;
            }

            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }

        private void ApplyVolume(AudioChannel channel, float volume)
        {
            float decibels = Mathf.Log10(Mathf.Max(volume, MinVolume)) * 20f;

            if (!_mixer.SetFloat(GetParameterName(channel), decibels))
            {
                Debug.LogError($"Paramètre « {GetParameterName(channel)} » introuvable dans le Mixer : est-il bien exposé ?", this);
            }
        }

        private static string GetParameterName(AudioChannel channel)
        {
            return channel switch
            {
                AudioChannel.Master => MasterVolumeParameter,
                AudioChannel.Music => MusicVolumeParameter,
                AudioChannel.Sfx => SfxVolumeParameter,
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }

        [ContextMenu("Test : musique à 20 %")]
        private void TestMusicLow()
        {
            SetVolume(AudioChannel.Music, 0.2f);
        }

        [ContextMenu("Test : volumes au max")]
        private void TestAllVolumesMax()
        {
            foreach (AudioChannel channel in Enum.GetValues(typeof(AudioChannel)))
            {
                SetVolume(channel, 1f);
            }
        }

        [ContextMenu("Test : jouer un effet")]
        private void TestPlaySfx()
        {
            PlaySfx(_testSfx);
        }
    }
}
