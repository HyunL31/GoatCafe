using UnityEngine;

public class NormalCustomer : CustomerBase
{
    [SerializeField] private int Int_PenaltyScore = -50;

    protected override int GetScoreChange()
    {
        return Int_PenaltyScore;
    }
}
