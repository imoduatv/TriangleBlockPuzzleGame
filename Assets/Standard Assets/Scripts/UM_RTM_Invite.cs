public class UM_RTM_Invite
{
	private string _Id = string.Empty;

	private string _SenderId = string.Empty;

	public string Id => _Id;

	public string SenderId => _SenderId;

	public UM_RTM_Invite(GP_Invite invite)
	{
		_Id = invite.Id;
		_SenderId = invite.Participant.playerId;
	}

	public UM_RTM_Invite(GK_Invite invite)
	{
		_Id = invite.Id;
		_SenderId = invite.Sender.Id;
	}
}
