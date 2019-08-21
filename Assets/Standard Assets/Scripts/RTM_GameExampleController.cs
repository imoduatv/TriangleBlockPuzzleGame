using SA.Common.Pattern;
using System.Text;
using UnityEngine;

public class RTM_GameExampleController : MonoBehaviour
{
	public GameObject avatar;

	public GameObject hi;

	public SA_Label playerLabel;

	public SA_Label gameState;

	public SA_Label parisipants;

	public DefaultPreviewButton connectButton;

	public DefaultPreviewButton helloButton;

	public DefaultPreviewButton leaveRoomButton;

	public DefaultPreviewButton showRoomButton;

	public DefaultPreviewButton[] ConnectionDependedntButtons;

	public SA_PartisipantUI[] patricipants;

	public SA_FriendUI[] friends;

	private Texture defaulttexture;

	private string inviteId;

	private void Start()
	{
		playerLabel.text = "Player Disconnected";
		defaulttexture = avatar.GetComponent<Renderer>().material.mainTexture;
		RTM.Matchmaker.InvitationReceived += HandleInvitationReceived;
		RTM.Matchmaker.InvitationAccepted += HandleInvitationAccepted;
		RTM.Matchmaker.RoomCreated += HandleRoomCreated;
		RTM.Matchmaker.MatchDataReceived += HandleMatchDataReceived;
		UM_GameServiceManager.OnPlayerConnected += OnPlayerConnected;
		UM_GameServiceManager.OnPlayerDisconnected += OnPlayerDisconnected;
		if (Singleton<UM_GameServiceManager>.Instance.ConnectionSate == UM_ConnectionState.CONNECTED)
		{
			OnPlayerConnected();
		}
	}

	private void HandleMatchDataReceived(string senderId, byte[] data)
	{
		string empty = string.Empty;
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		empty = uTF8Encoding.GetString(data);
		string str = senderId;
		UM_RTM_Participant participantById = RTM.Matchmaker.CurrentRoom.GetParticipantById(senderId);
		if (participantById != null)
		{
			str = participantById.Name;
		}
		AndroidMessage.Create("Data Eeceived", "player " + str + " \n data: " + empty);
	}

	private void HandleRoomCreated(UM_RTM_RoomCreatedResult result)
	{
		SA_StatusBar.text = "Room Create Result:  " + result.IsSuccess.ToString();
	}

	private void HandleInvitationAccepted(UM_RTM_Invite invite)
	{
		UnityEngine.Debug.Log("ActionInvitationAccepted called");
		UnityEngine.Debug.Log("Starting The Game");
	}

	private void HandleInvitationReceived(UM_RTM_Invite invite)
	{
		inviteId = invite.Id;
		AndroidDialog androidDialog = AndroidDialog.Create("Invite", "You have new invite from: " + invite.SenderId, "Manage Manually", "Open Google Inbox");
		androidDialog.ActionComplete += OnInvDialogComplete;
	}

	private void ConncetButtonPress()
	{
		UnityEngine.Debug.Log("UM_GameServiceManager State  -> " + Singleton<UM_GameServiceManager>.Instance.ConnectionSate.ToString());
		if (Singleton<UM_GameServiceManager>.Instance.ConnectionSate == UM_ConnectionState.CONNECTED)
		{
			SA_StatusBar.text = "Disconnecting from Play Service...";
			Singleton<UM_GameServiceManager>.Instance.Disconnect();
		}
		else
		{
			SA_StatusBar.text = "Connecting to Play Service...";
			Singleton<UM_GameServiceManager>.Instance.Connect();
		}
	}

	private void ShowWatingRoom()
	{
	}

	private void findMatch()
	{
		int minPlayers = 1;
		int maxPlayers = 2;
		RTM.Matchmaker.FindMatch(minPlayers, maxPlayers);
	}

	private void InviteFriends()
	{
	}

	private void SendHello()
	{
		string s = "hello world";
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(s);
		RTM.Matchmaker.SendDataToAll(bytes, UM_RTM_PackageType.Reliable);
	}

	private void LeaveRoom()
	{
		RTM.Matchmaker.LeaveMatch();
	}

	private void DrawParticipants()
	{
		parisipants.text = "Total Room Participants: " + RTM.Matchmaker.CurrentRoom.Participants.Count;
		SA_PartisipantUI[] array = patricipants;
		foreach (SA_PartisipantUI sA_PartisipantUI in array)
		{
			sA_PartisipantUI.gameObject.SetActive(value: false);
		}
		int num = 0;
		foreach (UM_RTM_Participant participant in RTM.Matchmaker.CurrentRoom.Participants)
		{
			patricipants[num].gameObject.SetActive(value: true);
			patricipants[num].SetParticipant(participant);
			num++;
		}
	}

	private void UpdateGameState(string msg)
	{
		gameState.text = msg;
	}

	private void FixedUpdate()
	{
		DrawParticipants();
		if (Singleton<UM_GameServiceManager>.Instance.ConnectionSate == UM_ConnectionState.CONNECTED)
		{
			if (Singleton<UM_GameServiceManager>.Instance.Player.SmallPhoto != null)
			{
				avatar.GetComponent<Renderer>().material.mainTexture = Singleton<UM_GameServiceManager>.Instance.Player.SmallPhoto;
			}
		}
		else
		{
			avatar.GetComponent<Renderer>().material.mainTexture = defaulttexture;
		}
		string text = "Connect";
		if (Singleton<UM_GameServiceManager>.Instance.ConnectionSate == UM_ConnectionState.CONNECTED)
		{
			text = "Disconnect";
			DefaultPreviewButton[] connectionDependedntButtons = ConnectionDependedntButtons;
			foreach (DefaultPreviewButton defaultPreviewButton in connectionDependedntButtons)
			{
				defaultPreviewButton.EnabledButton();
			}
		}
		else
		{
			DefaultPreviewButton[] connectionDependedntButtons2 = ConnectionDependedntButtons;
			foreach (DefaultPreviewButton defaultPreviewButton2 in connectionDependedntButtons2)
			{
				defaultPreviewButton2.DisabledButton();
			}
			text = ((Singleton<UM_GameServiceManager>.Instance.ConnectionSate != UM_ConnectionState.DISCONNECTED && Singleton<UM_GameServiceManager>.Instance.ConnectionSate != 0) ? "Connecting.." : "Connect");
		}
		connectButton.text = text;
	}

	private void OnPlayerDisconnected()
	{
		SA_StatusBar.text = "Player Disconnected";
		playerLabel.text = "Player Disconnected";
	}

	private void OnPlayerConnected()
	{
		SA_StatusBar.text = "Player Connected";
		playerLabel.text = Singleton<UM_GameServiceManager>.Instance.Player.Name;
	}

	private void OnFriendListLoaded(GooglePlayResult result)
	{
	}

	private void OnInvite(GP_Invite invitation)
	{
		if (invitation.InvitationType == GP_InvitationType.INVITATION_TYPE_REAL_TIME)
		{
			inviteId = invitation.Id;
			AndroidDialog androidDialog = AndroidDialog.Create("Invite", "You have new invite from: " + invitation.Participant.DisplayName, "Manage Manually", "Open Google Inbox");
			androidDialog.ActionComplete += OnInvDialogComplete;
		}
	}

	private void OnInvDialogComplete(AndroidDialogResult result)
	{
		switch (result)
		{
		case AndroidDialogResult.YES:
		{
			AndroidDialog androidDialog = AndroidDialog.Create("Manage Invite", "Would you like to accept this invite?", "Accept", "Decline");
			androidDialog.ActionComplete += OnInvManageDialogComplete;
			break;
		}
		case AndroidDialogResult.NO:
			Singleton<GooglePlayRTM>.Instance.OpenInvitationInBoxUI();
			break;
		}
	}

	private void OnInvManageDialogComplete(AndroidDialogResult result)
	{
		switch (result)
		{
		case AndroidDialogResult.YES:
			Singleton<GooglePlayRTM>.Instance.AcceptInvitation(inviteId);
			break;
		case AndroidDialogResult.NO:
			Singleton<GooglePlayRTM>.Instance.DeclineInvitation(inviteId);
			break;
		}
	}
}
