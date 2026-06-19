using UnityEngine;

public static class GameUtil
{
    // 현재 점수에 고정 보너스 점수 추가
    public static int AddBonusScore(int currentScore, int bonusScore)
    {
        return currentScore + bonusScore;
    }

    // 획득할 점수에 보너스 비율을 적용해 계산
    public static int GetBonusScoreByRate(int baseScore, float bonusRate)
    {
        return Mathf.RoundToInt(baseScore * bonusRate);
    }
}