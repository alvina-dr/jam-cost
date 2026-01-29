using BennyKok.RuntimeDebug.Actions;
using BennyKok.RuntimeDebug.Systems;
using extDebug.Menu;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    private void Awake()
    {
        //DM.Input = new DMCustomInput();
        SetupDebugMenu();

        List <BonusData> bonusDataList = Resources.LoadAll<BonusData>("Bonus").ToList();

        for (int i = 0; i < bonusDataList.Count; i++)
        {
            int index = i;
            if (bonusDataList[index].Durability == BonusData.BonusDurability.Permanent)
            {
                RuntimeDebugSystem.RegisterActions(DebugActionBuilder.Button().WithName(bonusDataList[index].Name).WithGroup("Bonus/Permanent").WithAction(() => bonusDataList[index].GetBonus()));
                //DM.Add("Bonus/Permanent/" + bonusDataList[index].Name, action => bonusDataList[index].GetBonus());
            }
            else
            {
                RuntimeDebugSystem.RegisterActions(DebugActionBuilder.Button().WithName(bonusDataList[index].Name).WithGroup("Bonus/Run").WithAction(() => bonusDataList[index].GetBonus()));
                //DM.Add("Bonus/Run/" + bonusDataList[index].Name, action => bonusDataList[index].GetBonus());
            }
        }
    }

    public void SetupDebugMenu()
    {
        //DM.Root.Clear();

        //DM.Add("Reload" , action => SceneManager.LoadScene("Game"));
        //DM.Add("Win" , action => WinCurrentNode());
        //DM.Add("Generate new map" , action => GenerateNewMap());
        //DM.Add("Gain/PP" , action => SaveManager.Instance.AddPP(100));
        //DM.Add("Gain/MealTickets", action => GainMealTickets());
        //DM.Add("Bonus/Permanent");
        //DM.Add("Bonus/Run");
        //List<BonusData> bonusDataList = Resources.LoadAll<BonusData>("Bonus").ToList();
        //for (int i = 0; i < bonusDataList.Count; i++)
        //{
        //    int index = i;
        //    if (bonusDataList[index].Durability == BonusData.BonusDurability.Permanent)
        //    {
        //        DM.Add("Bonus/Permanent/" + bonusDataList[index].Name, action => bonusDataList[index].GetBonus());
        //    }
        //    else
        //    {
        //        DM.Add("Bonus/Run/" + bonusDataList[index].Name, action => bonusDataList[index].GetBonus());
        //    }
        //}
    }

    public void WinCurrentNode()
    {
        GameManager.Instance?.SetGameState(GameManager.Instance.WinState);
    }

    public void GenerateNewMap()
    {
        SaveManager.Instance.CurrentSave.CurrentRun.CurrentDay = 0;
        SaveManager.Instance.CurrentSave.CurrentRun.FormerNodeList.Clear();

        SceneManager.LoadScene("Map");
    }

    public void GainMealTickets()
    {
        SaveManager.Instance.CurrentSave.MealTickets += 100;
        HubManager.Instance?.UpdateUpgrades();
    }
}
