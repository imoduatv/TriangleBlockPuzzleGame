using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;

public class GK_RTM_Controller : iRTM_Matchmaker
{
	private List<UM_RTM_Invite> _Invitations;

	private UM_RTM_Room _CurrentRoom;

	public List<UM_RTM_Invite> Invitations => _Invitations;

	public UM_RTM_Room CurrentRoom => _CurrentRoom;

	public event Action<UM_RTM_Invite> InvitationReceived;

	public event Action<UM_RTM_Invite> InvitationAccepted;

	public event Action<string> InvitationDeclined;

	public event Action<UM_RTM_RoomCreatedResult> RoomCreated;

	public event Action RoomUpdated;

	public event Action<string, byte[]> MatchDataReceived;

	public GK_RTM_Controller()
	{
		this.InvitationReceived = delegate
		{
		};
		this.InvitationAccepted = delegate
		{
		};
		this.InvitationDeclined = delegate
		{
		};
		this.RoomCreated = delegate
		{
		};
		this.RoomUpdated = delegate
		{
		};
		this.MatchDataReceived = delegate
		{
		};
		_Invitations = new List<UM_RTM_Invite>();
		_CurrentRoom = new UM_RTM_Room();
		//base._002Ector();
		GameCenterInvitations.ActionPlayerAcceptedInvitation += HandleActionPlayerAcceptedInvitation;
		GameCenter_RTM.ActionMatchStarted += HandleActionRoomCreated;
		GameCenter_RTM.ActionDataReceived += HandleActionMatchDataReceived;
	}

	private void HandleActionRoomCreated(GK_RTM_MatchStartedResult result)
	{
		if (result.IsSucceeded)
		{
			_CurrentRoom = new UM_RTM_Room(result.Match);
		}
		UM_RTM_RoomCreatedResult obj = new UM_RTM_RoomCreatedResult(result);
		this.RoomCreated(obj);
	}

	private void HandleActionMatchDataReceived(GK_Player sender, byte[] data)
	{
		this.MatchDataReceived(sender.Id, data);
	}

	private void HandleActionPlayerAcceptedInvitation(GK_MatchType type, GK_Invite invite)
	{
		if (type == GK_MatchType.RealTime)
		{
			UM_RTM_Invite invite2 = null;
			if (!TryGetInvitation(invite.Id, out invite2))
			{
				invite2 = new UM_RTM_Invite(invite);
				_Invitations.Add(invite2);
			}
			this.InvitationAccepted(invite2);
		}
	}

	private bool TryGetInvitation(string id, out UM_RTM_Invite invite)
	{
		invite = null;
		foreach (UM_RTM_Invite invitation in _Invitations)
		{
			if (invitation.Id.Equals(id))
			{
				invite = invitation;
				return true;
			}
		}
		return false;
	}

	public void OpenInvitationUI(int minPlayers, int maxPlayers)
	{
		Singleton<GameCenter_RTM>.Instance.FindMatchWithNativeUI(minPlayers, maxPlayers, string.Empty);
	}

	public void AcceptInvite(UM_RTM_Invite invite)
	{
	}

	public void DeclineInvite(UM_RTM_Invite invite)
	{
	}

	public void FindMatch(int minPlayers, int maxPlayers)
	{
		Singleton<GameCenter_RTM>.Instance.FindMatch(minPlayers, maxPlayers, string.Empty);
	}

	public void SendDataToAll(byte[] data, UM_RTM_PackageType type)
	{
		Singleton<GameCenter_RTM>.Instance.SendData(data, type.GetGKPackageType());
	}

	public void SendDataToPlayer(byte[] data, UM_RTM_PackageType type, params string[] receivers)
	{
	}

	public void LeaveMatch()
	{
		Singleton<GameCenter_RTM>.Instance.Disconnect();
	}
}
