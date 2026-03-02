using UnityEngine;

public class HubManagerRelay : MonoBehaviour
{
    public void LaunchNewRun() => HubManager.Instance.LaunchNewRun();
    public void OpenPowerShop() => HubManager.Instance.OpenPowerShop();
    public void OpenPermanentBonusShop() => HubManager.Instance.OpenPermanentBonusShop();
    public void OpenQuestMenu() => HubManager.Instance.OpenQuestMenu();

    public void UnlockFrigo()
    {
        if (!SaveManager.CurrentSave.SeeNewFrigo)
        {
            DialogueManager.Instance.DialogueRunner.StartDialogue("NPC1_NewFrigo");
            SaveManager.CurrentSave.SeeNewFrigo = true;
        }
    }

}
