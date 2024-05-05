using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGuru.SecondCase.Helper
{
    public class FinishController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!Character.PlayerController.TryGetPlayer(other, out var player)) return;

            GameManager.Instance.FinishGame(true);
        }
    }
}
