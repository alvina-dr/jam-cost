using UnityEngine;

public class UI_Run : MonoBehaviour
{
    #region Singleton
    public static UI_Run Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public UI_TextValue PPTextValue;
    public UI_TextValue MealTicketTextValue;
}
