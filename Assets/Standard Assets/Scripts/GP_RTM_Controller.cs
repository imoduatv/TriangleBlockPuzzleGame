using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;

public class GP_RTM_Controller : iRTM_Matchmaker
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

	public GP_RTM_Controller()
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
		GooglePlayRTM.ActionInvitationReceived += HandleActionInvitationReceived;
		GooglePlayRTM.ActionInvitationRemoved += HandleActionInvitationRemoved;
		GooglePlayRTM.ActionInvitationAccepted += HandleActionInvitationAccepted;
		GooglePlayRTM.ActionRoomCreated += HandleActionRoomCreated;
		GooglePlayRTM.ActionDataRecieved += HandleActionMatchDataReceived;
		GooglePlayRTM.ActionRoomUpdated += HandleActionRoomUpdated;
		GooglePlayConnection.ActionPlayerConnected += HandleActionPlayerConnected;
	}

	public void OpenInvitationUI(int minPlayers, int maxPlayers)
	{
		Singleton<GooglePlayRTM>.Instance.OpenInvitationBoxUI(minPlayers, maxPlayers);
	}

	public void AcceptInvite(UM_RTM_Invite invite)
	{
		Singleton<GooglePlayRTM>.Instance.AcceptInvitation(invite.Id);
	}

	public void DeclineInvite(UM_RTM_Invite invite)
	{
		Singleton<GooglePlayRTM>.Instance.DeclineInvitation(invite.Id);
	}

	public void FindMatch(int minPlayers, int maxPlayers)
	{
		Singleton<GooglePlayRTM>.Instance.FindMatch(minPlayers, maxPlayers);
	}

	public void SendDataToAll(byte[] data, UM_RTM_PackageType type)
	{
		Singleton<GooglePlayRTM>.Instance.SendDataToAll(data, type.GetGPPackageType());
	}

	public void SendDataToPlayer(byte[] data, UM_RTM_PackageType type, params string[] receivers)
	{
		Singleton<GooglePlayRTM>.Instance.SendDataToPlayers(data, type.GetGPPackageType(), receivers);
	}

	public void LeaveMatch()
	{
		Singleton<GooglePlayRTM>.Instance.LeaveRoom();
	}

	private void HandleActionRoomUpdated(GP_RTM_Room room)
	{
		_CurrentRoom = new UM_RTM_Room(room);
		this.RoomUpdated();
	}

	private void HandleActionMatchDataReceived(GP_RTM_Network_Package package)
	{
		this.MatchDataReceived(package.participantId, package.buffer);
	}

	private void HandleActionRoomCreated(GP_GamesStatusCodes status)
	{
		UM_RTM_RoomCreatedResult obj = new UM_RTM_RoomCreatedResult(status);
		_CurrentRoom = new UM_RTM_Room(Singleton<GooglePlayRTM>.Instance.currentRoom);
		this.RoomCreated(obj);
	}

	private void HandleActionPlayerConnected()
	{
		Singleton<GooglePlayInvitationManager>.Instance.RegisterInvitationListener();
	}

	private void HandleActionInvitationReceived(GP_Invite invite)
	{
		UM_RTM_Invite uM_RTM_Invite = new UM_RTM_Invite(invite);
		_Invitations.Add(uM_RTM_Invite);
		this.InvitationReceived(uM_RTM_Invite);
	}

	private void HandleActionInvitationRemoved(string id)
	{
		RemoveInvitation(id);
		this.InvitationDeclined(id);
	}

	private void HandleActionInvitationAccepted(GP_Invite invite)
	{
		if (invite.InvitationType == GP_InvitationType.INVITATION_TYPE_REAL_TIME)
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

	private void RemoveInvitation(string id)
	{
		foreach (UM_RTM_Invite invitation in _Invitations)
		{
			if (invitation.Id.Equals(id))
			{
				_Invitations.Remove(invitation);
				break;
			}
		}
	}
}
