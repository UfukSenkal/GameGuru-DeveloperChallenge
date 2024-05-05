using GameGuru.SecondCase.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameGuru.SecondCase.Platform
{
    public class PlatformController : MonoBehaviour, IMovable
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float platformSideSpeed;
        [SerializeField] private float endXValue;


        public int ID { get; set; }
        public Vector3 MiddleCenter => meshRenderer.bounds.center;
        public Vector3 Size => meshRenderer.bounds.size;


        public Action<PlatformController> onSnapped;


        public void Initiliaze()
        {
            Move();
        }


        public void Move()
        {
            transform.DOMoveX(endXValue, 1, false).SetLoops(-1,LoopType.Yoyo);
        }

        public void Snap()
        {
            Debug.Log("Snap");
            DOTween.KillAll();
            onSnapped?.Invoke(this);
        }
        public void Cut(PlatformController lastPlatform,out bool isGameOver)
        {
            float overPiece = transform.position.x - lastPlatform.transform.position.x;
            float direction = overPiece > 0 ? 1f : -1f;

            float newXSize = lastPlatform.transform.localScale.x - Mathf.Abs(overPiece);
            float fallingPlatformSize = transform.localScale.x - newXSize;

            isGameOver = newXSize <= 0;
            if (isGameOver)
            {
                transform.DOMoveY(-5f, 1f).OnComplete(() =>
                {
                    transform.DOKill();
                    gameObject.SetActive(false);
                });
                return;
            }

            float newXPosition = lastPlatform.transform.position.x + (overPiece / 2f);
            transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

            float platformEdge = transform.position.x + (newXSize / 2f * direction);
            float fallingPlatformXPosition = platformEdge + fallingPlatformSize / 2f * direction;


            SpawnFallingPlatform(fallingPlatformXPosition, fallingPlatformSize);
        }

        public void SpawnFallingPlatform(float fallingPlatformXPosition, float fallingPlatformSize)
        {
            var fallPlatform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fallPlatform.transform.localScale = new Vector3(fallingPlatformSize, transform.localScale.y, transform.localScale.z);
            fallPlatform.transform.position = new Vector3(fallingPlatformXPosition, transform.position.y, transform.position.z);
            fallPlatform.transform.DOMoveY(-5f, 1f).OnComplete(() =>
            {
                fallPlatform.transform.DOKill();
                fallPlatform.gameObject.SetActive(false);
            });
        }
    }
}