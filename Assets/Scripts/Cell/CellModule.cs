using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGuru.FirstCase.Grid
{
    public struct Cell
    {
        public CellController cellInstance;
        public bool isMarked;
        public Vector3 pos;

        public Cell(CellController cellInstance, bool isMarked, Vector3 pos)
        {
            this.cellInstance = cellInstance;
            this.isMarked = isMarked;
            this.pos = pos;
        }

    }

    [Serializable]
    public struct CellModule
    {
        public Pool<CellController> cellPool;
        public int gridSize;
        public float cellSpacingRatio;
        public float paddingRatio;
        public Transform parent;

        private Cell[,] _cells;
        private float _cellSize;
        private float _cellSpacing;
        private Vector3 _startPos;
        private float cameraHeight;
        private float leftEdge;
        private float padding;


        public ref Cell this[Vector2Int index] => ref _cells[index.x, index.y];

        public void Initiliaze(int newGridSize = -1)
        {
            gridSize = newGridSize != -1 ? newGridSize : gridSize;

            cellPool.Initiliaze();
            FitScreen();

            _cells = new Cell[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    float xPos = leftEdge + (_cellSize + _cellSpacing) * i + _cellSize / 2f;
                    float yPos = cameraHeight / 2f - padding - (_cellSize + _cellSpacing) * j - _cellSize / 2f;

                    Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

                    if (i == 0 && j == 0)
                        _startPos = spawnPosition;

                    SpawnCell(i, j, spawnPosition);
                }
            }

        }

        private void SpawnCell(int i, int j, Vector3 spawnPosition)
        {
            CellController newCell = cellPool.Pop();
            newCell.transform.position = spawnPosition;
            newCell.transform.parent = parent;
            newCell.transform.localScale = new Vector3(_cellSize, _cellSize, 1f);

            var cell = new Cell(newCell, false, spawnPosition);
            _cells[i, j] = cell;
        }

        private void FitScreen()
        {
            var mainCamera = Camera.main;

            float cameraDistance = Mathf.Abs(mainCamera.transform.position.z);
            cameraHeight = 2f * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * cameraDistance;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            padding = cameraWidth * paddingRatio;

            leftEdge = -cameraWidth / 2f + padding;
            float rightEdge = cameraWidth / 2f - padding;

            float realGridWidth = rightEdge - leftEdge;

            _cellSize = realGridWidth / gridSize;

            _cellSpacing = _cellSize * cellSpacingRatio;
            _cellSize -= _cellSpacing;

            float gridHeight = _cellSize * gridSize + _cellSpacing * (gridSize - 1);

            if (gridHeight > cameraHeight / 2f)
            {
                float scaleRatio = (cameraHeight / 2f) / gridHeight;
                _cellSize *= scaleRatio;
                _cellSpacing *= scaleRatio;
            }
        }

        public bool CheckMatches(Vector2Int coord)
        {

            List<Vector2Int> matchingCells = new List<Vector2Int>();

            Vector2Int[] neighbors = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };


            for (int i = 0; i < neighbors.Length; i++)
            {
                var neighborCoord = coord + neighbors[i];
                if (ContainsCoordinate(neighborCoord) && this[neighborCoord].isMarked)
                {
                    for (int j = 0; j < neighbors.Length; j++)
                    {
                        var secondNeighborCoord = neighborCoord + neighbors[j];
                        if (coord != secondNeighborCoord && ContainsCoordinate(secondNeighborCoord) && this[secondNeighborCoord].isMarked)
                        {
                            matchingCells.Add(secondNeighborCoord);
                        }
                    }

                    if (!matchingCells.Contains(neighborCoord))
                        matchingCells.Add(neighborCoord);
                }
            }

            if (matchingCells.Count > 0)
                matchingCells.Add(coord);

            bool isMatched = matchingCells.Count >= 3;

            if (!isMatched) return false;

            foreach (var matchingCoord in matchingCells)
            {
                ResetCell(matchingCoord);
            }


            return isMatched;
        }




        public void ResetCell(Vector2Int coord)
        {
            this[coord].isMarked = false;
            this[coord].cellInstance.PlayAnim(CellController.UNMARKED_ANIM_NAME);
        }

        public Vector2Int ToCoordinate(Vector3 pos)
        {
            var dif = pos - _startPos;

            var x = Mathf.RoundToInt(dif.x / (_cellSize + _cellSpacing));
            var y = Mathf.RoundToInt(-dif.y / (_cellSize + _cellSpacing));

            return new Vector2Int(x, y);
        }
        public bool ContainsCoordinate(Vector2Int coord)
        {
            if (coord.x < 0 || coord.y < 0) return false;
            if (coord.x >= gridSize || coord.y >= gridSize) return false;
            return true;
        }

    }

}

