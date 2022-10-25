using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    Transform visual;
    [Header("World")]
    [SerializeField] Vector3 thisWorldPosition;
    [SerializeField] Vector3 thisWorldRotation;
    Vector3 allWorldPosition;
    Vector3 allWorldRotation;
    [Header("Local")]
    [SerializeField] Vector3 thisLocalPosition;
    [SerializeField] Vector3 thisLocalRotation;
    [SerializeField] Vector3 thisLocalScale;
    Vector3 allLocalPosition;
    Vector3 allLocalRotation;
    Vector3 allLocalScale;
    public Vector3 AllWorldPosition { get { return allWorldPosition; } set { allWorldPosition = value; } }
    public Vector3 AllWorldRotation { get { return allWorldRotation; } set { allWorldRotation = value; } }
    public Vector3 AllLocalPosition { get { return allLocalPosition; } set { allLocalPosition = value; } }
    public Vector3 AllLocalRotation { get { return allLocalRotation; } set { allLocalRotation = value; } }
    public Vector3 AllLocalScale { get { return allLocalScale; } set { allLocalScale = value; } }
    void LateUpdate()
    {
        if (transform.childCount != 0)
        {
            visual = transform.GetChild(0);
            transform.position = (allWorldPosition + thisWorldPosition);
            transform.localRotation = Quaternion.Euler(allWorldRotation + thisWorldRotation);
            visual.localPosition = allLocalPosition + thisLocalPosition;
            visual.localRotation = Quaternion.Euler(allLocalRotation + thisLocalRotation);
            visual.localScale = allLocalScale + thisLocalScale;
        }
    }
}
