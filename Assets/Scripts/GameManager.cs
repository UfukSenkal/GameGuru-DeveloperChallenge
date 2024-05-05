using GameGuru.Controls;
using GameGuru.SecondCase.Character;
using GameGuru.SecondCase.Platform;
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

        private bool _isGameOver;


        private void Start()
        {
            platformManager.Initiliaze();
        }

        private void Update()
        {
            RotateCamForWin();

            if (!inputControl.IsDown) return;

            platformManager.SnapPlatform();
        }
        public void FinishGame(bool isWin)
        {
            if (isWin)
            {
                RotateCamForWin();
                return;
            }

            platformManager.ResetAllPlatforms();
            ResetCharacter();
            RemoveFollowTarget();

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

            player.Revive();
            ResetFollowTarget();
        }
    }
}
