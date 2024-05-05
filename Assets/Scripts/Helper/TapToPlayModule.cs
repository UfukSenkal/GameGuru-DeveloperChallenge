using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGuru.SecondCase.Helper
{

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

            _taptoPlayInstance = GameObject.Instantiate(tapToPlayRes);
        }

        public void SetActiveTapToPlay(bool isActive)
        {
            _taptoPlayInstance?.gameObject.SetActive(isActive);
            CanPlay = true;
        }
    }
}
