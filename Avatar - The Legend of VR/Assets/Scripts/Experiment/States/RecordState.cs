public class RecordState : IState
{
    private readonly TrialInfo _info;

    public RecordState(TrialInfo info)
    {
        _info = info;
    }
    public void OnStateEnter()
    {
        Snapshot.TakeCSVSnapshot($"{_info.participantID}_{_info.trialNumber}.csv", _info);
        ExperimentController.TransitionToNextScene();
    }

    public void Tick()
    {
    }

    public void OnStateExit()
    {
    }
}
