public class UM_TMB_ParticipantResult
{
	private string _ParticipantId;

	private UM_TBM_Outcome _Outcome;

	public string ParticipantId => _ParticipantId;

	public UM_TBM_Outcome Outcome => _Outcome;

	public UM_TMB_ParticipantResult(string participantId, UM_TBM_Outcome outcome)
	{
		_Outcome = outcome;
		_ParticipantId = participantId;
	}
}
