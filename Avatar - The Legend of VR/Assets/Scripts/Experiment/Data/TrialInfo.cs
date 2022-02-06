using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Trial information which holds the current state of the experiment in each trail.
/// </summary>
public class TrialInfo : ISnapshotable
{
    /// <summary>
    /// Id of the current participant.
    /// </summary>
    public int participantID;

    /// <summary>
    /// The overall count of trials the participant is currently absolving.
    /// </summary>
    public int trialNumber;

    public AvatarSetupInfos avatarSetupInfos;

    public CardSetupInfos cardSetupInfos;

    // todo add header cols
    public ParticipantPreferences participantPreferences;

    // todo maybe some more information on the character needed? @Michael
    
    public readonly List<CardValues> cardsPicked = new();
    public readonly List<AvatarSelectionResponse> avatarsChosen = new();
    

    public IEnumerable<string> Header()
    {
        return SnapshotTypeDefs.CreateHeaders(
            new(nameof(participantID), participantPreferences.GetType()),
            new(nameof(trialNumber), trialNumber.GetType()))
            .Concat(participantPreferences.Header())
            .Concat(avatarsChosen[0].Header());
    }

    public IEnumerable<IEnumerable<string>> Record()
    {
        foreach(var av in avatarsChosen)
        {
            yield return SnapshotTypeDefs.CreateSnapshot(participantID, trialNumber)
                .Concat(participantPreferences.Record())
                .Concat(av.Record());
        }
    }
}
