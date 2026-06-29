using UnityEngine;

public class NormalCustomer : CustomerBase
{
    [SerializeField] private int Int_PenaltyScore = -50;
    [SerializeField] private CustomerRace CustomerRace_This = CustomerRace.Human;

    protected override int GetScoreChange()
    {
        return Int_PenaltyScore;
    }

    public override CustomerRace GetRace()
    {
        return CustomerRace_This;   
    }
}
