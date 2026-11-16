using PrimeTween;
using UnityEngine;

[CreateAssetMenu(fileName = "StD_SimpleBonus", menuName = "Scriptable Objects/StickerData/StD_SimpleBonus")]
public class StD_SimpleBonus : StickerData
{
    public override bool CheckSticker(BonusBehavior bonusBehavior)
    {
        if (Points > 0)
        {
            int bonusValue = Mathf.RoundToInt(Points);

            ScoreCalculationManager.Instance.CountSequence.ChainDelay(.5f);
            ScoreCalculationManager.Instance.CountSequence.ChainCallback(() =>
            {
                bonusBehavior.HighlightSticker(Name);
                ScoreCalculationManager.Instance.BaseScore += bonusValue;
                GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue, bonusBehavior.InfoSpawnPoint.position, GameDirector.Instance.AddColor, UI_TextPopper.AnimSpeed.Quick);
                AudioManager.PlaySound(AudioManager.Instance.ScoreAdd);
                CameraManager.Instance.SimpleShake();
            });
            ScoreCalculationManager.Instance.CountSequence.ChainDelay(.5f);
            ScoreCalculationManager.Instance.CountSequence.ChainCallback(() =>
            {
                GameManager.Instance.UIManager.BagMenu.AddScoreText.SetTextValue(ScoreCalculationManager.Instance.BaseScore.ToString());
                GameManager.Instance.UIManager.BagMenu.AddScoreText.ScaleAnim(2f, .3f);
            });
        }

        if (Mult > 0)
        {
            float bonusValue = Mult;

            ScoreCalculationManager.Instance.CountSequence.ChainDelay(.5f);
            ScoreCalculationManager.Instance.CountSequence.ChainCallback(() =>
            {
                bonusBehavior.HighlightSticker(Name);
                ScoreCalculationManager.Instance.MultiplierScore += bonusValue;
                GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue, bonusBehavior.InfoSpawnPoint.position, GameDirector.Instance.MultiplyColor, UI_TextPopper.AnimSpeed.Quick);
                AudioManager.PlaySound(AudioManager.Instance.ScoreMultiply);
                CameraManager.Instance.SimpleShake();
            });
            ScoreCalculationManager.Instance.CountSequence.ChainDelay(.5f);
            ScoreCalculationManager.Instance.CountSequence.ChainCallback(() =>
            {
                GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.SetTextValue(ScoreCalculationManager.Instance.MultiplierScore.ToString());
                GameManager.Instance.UIManager.BagMenu.MultiplyScoreText.ScaleAnim(2f, .3f);
            });
        }

        if (PP > 0)
        {
            int bonusValue = Mathf.RoundToInt(PP);

            ScoreCalculationManager.Instance.CountSequence.ChainDelay(.5f);
            ScoreCalculationManager.Instance.CountSequence.ChainCallback(() =>
            {
                bonusBehavior.HighlightSticker(Name);
                SaveManager.Instance.AddPP(bonusValue, bonusBehavior.InfoSpawnPoint.position);
                GameManager.Instance.UIManager.TextPopperManager_Number.PopText("+" + bonusValue + "<sprite name=PP>", bonusBehavior.InfoSpawnPoint.position, Color.white, UI_TextPopper.AnimSpeed.Quick);
                AudioManager.PlaySound(AudioManager.Instance.ScoreMultiply);
                CameraManager.Instance.SimpleShake();
            });
        }

        ScoreCalculationManager.Instance.CountSequence.ChainDelay(.5f);

        return true;
    }
}
