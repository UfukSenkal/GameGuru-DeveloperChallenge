using GameGuru.Controls;
using GameGuru.SecondCase.Character;
using GameGuru.SecondCase.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameGuru.SecondCase
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager Instance { get; private set; }

        private void Awake()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion

        [SerializeField] private PlayerController player;
        [SerializeField] private PlatformManager platformManager;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera cinemachineCam;
        [SerializeField] private InputControl inputControl;
        [SerializeField] private TapToPlayModule tapToPlay;

        private bool _isGameOver;


        private void Start()
        {
            tapToPlay.Initiliaze();
            platformManager.Initiliaze();
            player.onCharacterDead -= OnCharacterDead;
            player.onCharacterDead += OnCharacterDead;
        }

        private void OnCharacterDead()
        {
            FinishGame(false);
        }

        private void Update()
        {
            if (!tapToPlay.CanPlay && inputControl.IsDown)
            {
                StartLevel();
                return;
            }


            if (!inputControl.IsDown) return;

            platformManager.SnapPlatform();
        }

        private void StartLevel()
        {
            platformManager.SpawnPlatform();
            player.StartMovement();
            tapToPlay.SetActiveTapToPlay(false);
        }

        public void FinishGame(bool isWin)
        {
            if (isWin)
            {
                RemoveFollowTarget();
                RotateCamForWin();
                player.Dance();
                StartCoroutine(LoadNextLevel());
                return;
            }

            StartCoroutine(ResetCharacter());
        }

        public void RotateCamForWin()
        {
            player.rotateAround.Rotate(Camera.main.transform, player.transform);
        }
        public void RemoveFollowTarget()
        {
            cinemachineCam.Follow = null;
        }
        public void ResetFollowTarget()
        {
            cinemachineCam.Follow = player.transform;
        }
        private IEnumerator ResetCharacter()
        {
            yield return new WaitForSeconds(2f);

            tapToPlay.Initiliaze();

            platformManager.ResetAllPlatforms();

            player.Revive();
            ResetFollowTarget();
            player.onCharacterDead -= OnCharacterDead;
            player.onCharacterDead += OnCharacterDead;
        }

        private IEnumerator LoadNextLevel()
        {
            yield return new WaitForSeconds(2f);

            tapToPlay.Initiliaze();
            platformManager.LoadNextLevel();
            ResetFollowTarget();
        }

        [Serializable]
        public struct TapToPlayModule
        {
            [SerializeField] private Transform tapToPlayRes;

            private Transform _taptoPlayInstance;
            public Transform TapToPlayInstance => _taptoPlayInstance;
            public bool CanPlay { get; private set; }

            public void Initiliaze()
            {
                CanPlay = false;
                if (_taptoPlayInstance != null)
                {
                    _taptoPlayInstance.gameObject.SetActive(true);
                    return;
                }

                _taptoPlayInstance = Instantiate(tapToPlayRes);
            }

            public void SetActiveTapToPlay(bool isActive)
            {
                _taptoPlayInstance?.gameObject.SetActive(isActive);
                CanPlay = true;
            }
        }
    }
}
