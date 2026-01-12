using TMPro;
using UnityEngine;

public class HubUpgrade : MonoBehaviour
{
    [SerializeField] private int _cost;
    [SerializeField] private TextMeshPro _priceText;

    private void Start()
    {
        UpdatePrice();
    }

    public void UpdatePrice()
    {
        _priceText.text = _cost.ToString();
        if (SaveManager.Instance.CurrentSave.MealTickets >= _cost) _priceText.color = Color.green;
        else _priceText.color = Color.red;
    }

    public void BuyUpgrade()
    {
        if (SaveManager.Instance.CurrentSave.MealTickets >= _cost)
        {
            HubManager.Instance.UseMealTicket(_cost);
            // actualize UI upgrade
            Debug.Log("buy upgrade");
        }
    }
}
