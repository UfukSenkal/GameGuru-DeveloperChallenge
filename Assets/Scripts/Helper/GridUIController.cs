using GameGuru.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameGuru.FirstCase.Helper
{
    public class GridUIController : MonoBehaviour
    {
        [SerializeField] private Button rebuildButton;
        [SerializeField] private TextMeshProUGUI matchCountText;
        [SerializeField] private TMP_InputField gridSizeInputField;


        public int GetGridSize => Convert.ToInt32(gridSizeInputField.text);
        public void SetMatchCount(int matchCount) => matchCountText.text = matchCount.ToString();
        public void SetStartGridSize(int gridSize) => gridSizeInputField.text = gridSize.ToString();

        private void Awake()
        {
            EventBus.OnStartGridSizeSet.AddListener(SetStartGridSize);
            EventBus.OnMatchCountChanged.AddListener(SetMatchCount);

            rebuildButton.onClick.AddListener(OnRebuildButtonClicked);
        }

        private void OnRebuildButtonClicked()
        {
            EventBus.RebuildButtonClicked(GetGridSize);
        }
    }
}
