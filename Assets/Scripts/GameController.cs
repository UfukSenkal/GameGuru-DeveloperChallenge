using GameGuru.Controls;
using GameGuru.SecondCase.Character;
using GameGuru.SecondCase.Helper;
using GameGuru.SecondCase.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameGuru.SecondCase
{
    public class GameController : MonoBehaviour
    {
        #region Singleton
        public static GameController Instance { get; private set; }

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
        [SerializeField] private Cinemachine.CinemachineVirtualCamera rotationCinemachineCam;
        [SerializeField] private InputControl inputControl;
        [SerializeField] private TapToPlayModule tapToPlay;
        [SerializeField] private float rotationCameraSpeed;

        private bool _isRotatingCamera;

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

            if (_isRotatingCamera)
                RotateCamForWin();

            if (!inputControl.IsDown) return;

            platformManager.SnapPlatform();
        }

        private void StartLevel()
        {
            platformManager.SpawnMovingPlatform();
            player.StartMovement();
            tapToPlay.SetActiveTapToPlay(false);
        }

        public void FinishGame(bool isWin)
        {
            if (isWin)
            {
                _isRotatingCamera = true;
                rotationCinemachineCam.enabled = true;
                player.Dance();
                StartCoroutine(LoadNextLevel());
                return;
            }

            StartCoroutine(ResetCharacter());
        }

        public void RotateCamForWin()
        {
            var angles = rotationCinemachineCam.transform.eulerAngles;
            angles.y += Time.deltaTime * rotationCameraSpeed;
            angles.y = angles.y > 360 ? 0 : angles.y;

            rotationCinemachineCam.transform.eulerAngles = angles;
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
            yield return new WaitForSeconds(5f);

            tapToPlay.Initiliaze();
            platformManager.LoadNextLevel();
            ResetFollowTarget();
            _isRotatingCamera = false;
            rotationCinemachineCam.enabled = false;
            rotationCinemachineCam.transform.rotation = cinemachineCam.transform.rotation;
        }

        
    }

}
