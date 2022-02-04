using UnityEngine;

public class RecordState : IState
{
    private readonly TrialInfo _info;

    public RecordState(TrialInfo info)
    {
        _info = info;
    }
    public void OnStateEnter()
    {
        Debug.Log("Done");
        Snapshot.TakeCSVSnapshot($"{ExperimentController.Instance.ParticipantID}.csv", _info);
        ExperimentController.TransitionToNextScene();
    }

    public void Tick()
    {
    }

    public void OnStateExit()
    {
    }
}
