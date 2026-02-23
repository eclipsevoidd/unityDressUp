using UnityEngine;
using UnityEngine.UI;

public class BGScroller : MonoBehaviour
{
    [SerializeField] RawImage img;
    [SerializeField] float x, y;

    void Update()
    {
        img.uvRect = new Rect(
            img.uvRect.position + new Vector2(x, y) * Time.unscaledDeltaTime,
            img.uvRect.size
        );
    }
}
