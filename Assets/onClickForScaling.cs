using UnityEngine;
using System.Collections;

public class onClickForScaling : MonoBehaviour
{
    void OnMouseDown()
    {
        Scaling.ScaleTransform = this.transform;
    }
}