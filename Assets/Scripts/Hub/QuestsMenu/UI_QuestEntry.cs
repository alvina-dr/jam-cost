using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuestEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Transform _newStatusParent;
    [SerializeField] private Transform _collectStatusParent;
    [SerializeField] private Transform _completedStatusParent;

    [Header("Selection parts")]
    [SerializeField] private Image _pointImage;
    [SerializeField] private GameObject _backgroundHighlight;

    private QuestData _questData;

    private bool _selected = false;

    public void Setup(QuestData data)
    {
        _selected = false;
        _questData = data;
        _nameText.text = _questData.Data.Name;

        _newStatusParent.gameObject.SetActive(false);
        _collectStatusParent.gameObject.SetActive(false);
        _completedStatusParent.gameObject.SetActive(false);

        switch (_questData.Data.State)
        {
            case QuestData.QuestState.New:
                _newStatusParent.gameObject.SetActive(true);
                break;
            case QuestData.QuestState.Completing:
                break;
            case QuestData.QuestState.WaitCollection:
                _collectStatusParent.gameObject.SetActive(true);
                break;
            case QuestData.QuestState.Collected:
                _completedStatusParent.gameObject.SetActive(true);
                break;
        }
    }

    public void TrySetupTicket()
    {
        HubManager.Instance.QuestsMenu.SetupTicket(_questData, this);

        if (_questData.Data.State == QuestData.QuestState.New) _questData.Data.State = QuestData.QuestState.Completing;
        Setup(_questData);

        Select();
        _selected = true;
    }

    public void Select()
    {
        _pointImage.color = new Color32(56, 56, 56, 255);
        _backgroundHighlight.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        _pointImage.color = new Color(0, 0, 0, 0);
        _backgroundHighlight.gameObject.SetActive(false);
        _selected = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_selected) return;

        Unselect();
    }
}
