using UnityEngine;

public class MyCameraBehaviour : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Camera>().backgroundColor = new Color(26.0f / 255.0f, 89.0f / 255.0f, 31.0f / 255.0f);
    }
}
