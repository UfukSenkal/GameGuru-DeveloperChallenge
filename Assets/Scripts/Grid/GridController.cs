using GameGuru.Controls;
using GameGuru.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameGuru.FirstCase.Grid
{

    public class GridController : MonoBehaviour
    {
        [SerializeField] private CellModule cellModule;
        [SerializeField] private InputControl inputControl;

        private int _matchCount = 0;

        void Start()
        {
            cellModule.Initiliaze();
            EventBus.OnRebuildButtonClicked.AddListener(SpawnGrid);
            EventBus.TriggerStartGridSizeSet(cellModule.gridSize);
            EventBus.TriggerMatchCountChanged(_matchCount);
        }

        public void SpawnGrid(int gridSize)
        {
            _matchCount = 0;
            EventBus.TriggerMatchCountChanged(_matchCount);
            cellModule.Initiliaze(gridSize);
        }

        private void Update()
        {
            if (!inputControl.IsDown) return;

            var coord = cellModule.ToCoordinate(inputControl.WorldPosition);

            if (!cellModule.ContainsCoordinate(coord)) return;

            Debug.Log(coord.x + " - " + coord.y);

            if (cellModule[coord].isMarked) return;

            cellModule[coord].isMarked = true;
            bool isMatch = cellModule.CheckMatches(coord);

            if (isMatch)
                UpdateMatchCount();

            if (!isMatch)
                cellModule[coord].cellInstance.PlayAnim(CellController.MARKED_ANIM_NAME);


        }

        private void UpdateMatchCount()
        {
            _matchCount++;
            EventBus.TriggerMatchCountChanged(_matchCount);
        }


    }


}
