using System;
using System.Collections.Generic;
using UnityEngine;

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

public class AvatarSelectionResponse
{
    private float _startTime;
    private float _endTime;

    public LevelOfMatch levelOfMatchChosen, levelOfMatchOther;

    public float ThinkingTime => _endTime - _startTime;
    public bool keptSameAvatar;

    private float _timeLookedAtLeft, _timeLookedAtRight;

    public void LookedAtRightThisFrame() {
        _timeLookedAtRight += Time.deltaTime;
    }
    public void LookedAtLeftThisFrame() {
        _timeLookedAtLeft += Time.deltaTime;
    }

    public bool wasLeftMatch;

    public float TimeLookedAtMatch => wasLeftMatch ? _timeLookedAtLeft : _timeLookedAtRight;
    public float TimeLookedAtOther => !wasLeftMatch ? _timeLookedAtLeft : _timeLookedAtRight;

    public void StartTimer() => _startTime = Time.time;
    public void EndTimer() => _endTime = Time.time;
    public IEnumerable<string> Record()
    {
        return SnapshotTypeDefs.CreateSnapshot(
            levelOfMatchChosen,
            levelOfMatchOther,
            ThinkingTime,
            TimeLookedAtMatch,
            TimeLookedAtOther,
            keptSameAvatar);
    }

    public IEnumerable<string> Header()
    {
        return SnapshotTypeDefs.CreateHeaders(
            new(nameof(levelOfMatchChosen), levelOfMatchChosen.GetType()),
            new(nameof(levelOfMatchOther), levelOfMatchOther.GetType()),
            new(nameof(ThinkingTime), ThinkingTime.GetType()),
            new(nameof(TimeLookedAtMatch), TimeLookedAtMatch.GetType()),
            new(nameof(TimeLookedAtOther), TimeLookedAtOther.GetType()),
            new(nameof(keptSameAvatar), keptSameAvatar.GetType()));
    }
}
