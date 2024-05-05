using UnityEngine;
using System;

[Serializable]
public struct RotateAround
{
    public float rotationSpeed;
    public float rotationDuration; 

    private float elapsedTime;
    private bool isRotating;

    public Action onFinishedRotate;

    public void Rotate(Transform rotate,Transform rotateAround)
    {
        if (!isRotating)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= rotationDuration)
            {
                isRotating = true;
                elapsedTime = 0f;
            }
        }
        else
        {
            rotate.RotateAround(rotateAround.position, Vector3.up, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= rotationDuration)
            {
                isRotating = false;
                elapsedTime = 0f;

                onFinishedRotate?.Invoke();
            }
        }
    }

}

