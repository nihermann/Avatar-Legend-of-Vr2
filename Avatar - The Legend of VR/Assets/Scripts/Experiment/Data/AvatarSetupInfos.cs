using System;

[Serializable]
public class AvatarSetupInfos
{
    public LevelOfMatch firstChoiceA, firstChoiceB;

    public LevelOfMatch secondChoice, thirdChoice, fourthChoice;

    public LevelOfMatch[] MatchChoices => new[]{ firstChoiceA, firstChoiceB, secondChoice, thirdChoice, fourthChoice };
}

public enum LevelOfMatch
{
    Opposite,
    LessOpposite,
    SlightMatch,
    BetterMatch,
    FullMatch
}
