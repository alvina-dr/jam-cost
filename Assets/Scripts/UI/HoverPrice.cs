using TMPro;
using UnityEngine;

public class HoverPrice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private UI_ShowSizeAnimation _showSizeAnimation;

    public void ShowPrice(int price, Vector3 worldPosition)
    {
        transform.position = worldPosition; // Camera.main.WorldToScreenPoint(worldPosition);
        _showSizeAnimation.Show();
        _price.text = price.ToString();
    }

    public void HidePrice()
    {
        _showSizeAnimation.Hide();
    }
}
