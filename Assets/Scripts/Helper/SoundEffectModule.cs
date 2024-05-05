using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameGuru.SecondCase.Helper
{
    [System.Serializable]
    public struct SoundEffectModule
    {
        [SerializeField] private AudioSource audioSourceRes;
        [SerializeField] private float pitchIncrease;

        private AudioSource _audioSourceInstance;
        private float _defaultPitch;

        public void Initiliaze()
        {
            if (_audioSourceInstance == null)
                _audioSourceInstance = GameObject.Instantiate(audioSourceRes);

            _defaultPitch = _audioSourceInstance.pitch;
        }

        public void PlaySound()
        {
            _audioSourceInstance?.Stop();
            _audioSourceInstance?.Play();
        }

        public void IncreasePitch(int comboCount)
        {
            _audioSourceInstance.pitch = _defaultPitch + pitchIncrease * comboCount;
        }

        public void Reset()
        {
            _audioSourceInstance.pitch = _defaultPitch;
        }
    }
}
