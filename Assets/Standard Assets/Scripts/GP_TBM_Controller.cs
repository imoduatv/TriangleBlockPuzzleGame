using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GP_TBM_Controller : iTBM_Matchmaker
{
	public List<UM_TBM_Match> _Matches;

	public List<UM_TBM_Invite> _Invitations;

	private int DataEventCount;

	private const int PLACING_UNINITIALIZED = -1;

	public List<UM_TBM_Match> Matches => _Matches;

	public List<UM_TBM_Invite> Invitations => _Invitations;

	public event Action<UM_TBM_MatchResult> MatchFoundEvent;

	public event Action<UM_TBM_MatchResult> MatchLoadedEvent;

	public event Action<UM_TBM_MatchResult> InvitationAccepted;

	public event Action<string> InvitationDeclined;

	public event Action<UM_TBM_MatchResult> TurnEndedEvent;

	public event Action<UM_TBM_MatchResult> MatchUpdatedEvent;

	public event Action<UM_TBM_MatchesLoadResult> MatchesListLoadedEvent;

	public event Action MatchesListUpdated;

	public GP_TBM_Controller()
	{
		this.MatchFoundEvent = delegate
		{
		};
		this.MatchLoadedEvent = delegate
		{
		};
		this.InvitationAccepted = delegate
		{
		};
		this.InvitationDeclined = delegate
		{
		};
		this.TurnEndedEvent = delegate
		{
		};
		this.MatchUpdatedEvent = delegate
		{
		};
		this.MatchesListLoadedEvent = delegate
		{
		};
		this.MatchesListUpdated = delegate
		{
		};
		_Matches = new List<UM_TBM_Match>();
		_Invitations = new List<UM_TBM_Invite>();
		//base._002Ector();
		GooglePlayTBM.ActionMatchInitiated += HandleActionMatchInitiated;
		GooglePlayTBM.ActionMatchUpdated += HandleActionMatchUpdated;
		GooglePlayTBM.ActionMatchDataLoaded += HandleActionMatchDataLoaded;
		GooglePlayTBM.ActionMatchLeaved += HandleActionMatchLeaved;
		GooglePlayTBM.ActionMatchTurnFinished += HandleActionTurnFinished;
		GooglePlayInvitationManager.ActionInvitationReceived += HandleActionInvitationReceived;
		GooglePlayInvitationManager.ActionInvitationAccepted += HandleActionInvitationAccepted;
		GooglePlayInvitationManager.ActionInvitationsListLoaded += HandleActionInvitationsListLoaded;
		GooglePlayTBM.ActionMatchesResultLoaded += HandleActionMatchesResultLoaded;
		GooglePlayTBM.ActionMatchInvitationAccepted += HandleActionMatchInvitationAccepted;
		GooglePlayTBM.ActionMatchInvitationDeclined += HandleActionMatchInvitationDeclined;
		GooglePlayConnection.ActionPlayerConnected += HandleActionPlayerConnected;
	}

	public void SetGroup(int group)
	{
		Singleton<GooglePlayTBM>.Instance.SetVariant(group);
	}

	public void SetMask(int mask)
	{
		Singleton<GooglePlayTBM>.Instance.SetExclusiveBitMask(mask);
	}

	public void FindMatch(int minPlayers, int maxPlayers, string[] recipients = null)
	{
		Singleton<GooglePlayTBM>.Instance.CreateMatch(minPlayers - 1, maxPlayers - 1, recipients);
	}

	public void ShowNativeFindMatchUI(int minPlayers, int maxPlayers)
	{
		Singleton<GooglePlayTBM>.Instance.StartSelectOpponentsView(minPlayers - 1, maxPlayers - 1, allowAutomatch: true);
	}

	public void LoadMatchesInfo()
	{
		if (DataEventCount == 0)
		{
			DataEventCount = 2;
			List<GP_TBM_MatchTurnStatus> list = new List<GP_TBM_MatchTurnStatus>();
			list.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_MY_TURN);
			list.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_COMPLETE);
			list.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_INVITED);
			list.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_THEIR_TURN);
			Singleton<GooglePlayTBM>.Instance.LoadMatchesInfo(GP_TBM_MatchesSortOrder.SORT_ORDER_MOST_RECENT_FIRST, list.ToArray());
			Singleton<GooglePlayInvitationManager>.Instance.LoadInvitations();
		}
	}

	private void CheckDataCounter()
	{
		DataEventCount--;
		if (DataEventCount == 0)
		{
			UM_TBM_MatchesLoadResult uM_TBM_MatchesLoadResult = new UM_TBM_MatchesLoadResult(new GooglePlayResult(GP_GamesStatusCodes.STATUS_OK));
			uM_TBM_MatchesLoadResult.SetMatches(_Matches);
			uM_TBM_MatchesLoadResult.SetInvitations(_Invitations);
			this.MatchesListUpdated();
			this.MatchesListLoadedEvent(uM_TBM_MatchesLoadResult);
		}
	}

	public void LoadMatch(string matchId)
	{
		Singleton<GooglePlayTBM>.Instance.LoadMatchInfo(matchId);
	}

	public void TakeTurn(string matchId, byte[] matchData, UM_TBM_Participant nextParticipant)
	{
		string pendingParticipantId = string.Empty;
		if (nextParticipant != null)
		{
			pendingParticipantId = nextParticipant.Id;
		}
		Singleton<GooglePlayTBM>.Instance.TakeTrun(matchId, matchData, pendingParticipantId);
	}

	public void QuitInTurn(string matchId, UM_TBM_Participant nextParticipant)
	{
		string pendingParticipantId = string.Empty;
		if (nextParticipant != null)
		{
			pendingParticipantId = nextParticipant.Id;
		}
		Singleton<GooglePlayTBM>.Instance.LeaveMatchDuringTurn(matchId, pendingParticipantId);
	}

	public void QuitOutOfTurn(string matchId)
	{
		Singleton<GooglePlayTBM>.Instance.LeaveMatch(matchId);
	}

	public void RemoveMatch(string matchId)
	{
		Singleton<GooglePlayTBM>.Instance.DismissMatch(matchId);
		RemoveMatchFromTheList(matchId);
	}

	public void FinishMatch(string matchId, byte[] matchData, params UM_TMB_ParticipantResult[] results)
	{
		List<GP_ParticipantResult> list = new List<GP_ParticipantResult>();
		foreach (UM_TMB_ParticipantResult uM_TMB_ParticipantResult in results)
		{
			GP_TBM_ParticipantResult result = GP_TBM_ParticipantResult.MATCH_RESULT_UNINITIALIZED;
			switch (uM_TMB_ParticipantResult.Outcome)
			{
			case UM_TBM_Outcome.Won:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_WIN;
				break;
			case UM_TBM_Outcome.Lost:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_LOSS;
				break;
			case UM_TBM_Outcome.Tied:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_TIE;
				break;
			case UM_TBM_Outcome.Disconnected:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_DISCONNECT;
				break;
			case UM_TBM_Outcome.None:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_UNINITIALIZED;
				break;
			}
			GP_ParticipantResult item = new GP_ParticipantResult(uM_TMB_ParticipantResult.ParticipantId, result, -1);
			list.Add(item);
		}
		Singleton<GooglePlayTBM>.Instance.FinishMatch(matchId, matchData, list.ToArray());
	}

	public void ConfirmhMatchFinis(string matchId)
	{
		Singleton<GooglePlayTBM>.Instance.ConfirmMatchFinish(matchId);
	}

	public void Rematch(string matchId)
	{
		Singleton<GooglePlayTBM>.Instance.Rematch(matchId);
	}

	public void AcceptInvite(UM_TBM_Invite invite)
	{
		Singleton<GooglePlayTBM>.Instance.AcceptInvitation(invite.Id);
	}

	public void DeclineInvite(UM_TBM_Invite invite)
	{
		Singleton<GooglePlayTBM>.Instance.DeclineInvitation(invite.Id);
	}

	private void HandleActionMatchInitiated(GP_TBM_MatchInitiatedResult res)
	{
		UM_TBM_MatchResult uM_TBM_MatchResult = new UM_TBM_MatchResult(res);
		if (res.Match != null)
		{
			UM_TBM_Match match = new UM_TBM_Match(res.Match);
			uM_TBM_MatchResult.SetMatch(match);
			UpdateMatchData(match);
		}
		this.MatchFoundEvent(uM_TBM_MatchResult);
	}

	private void HandleActionMatchDataLoaded(GP_TBM_LoadMatchResult res)
	{
		UM_TBM_MatchResult uM_TBM_MatchResult = new UM_TBM_MatchResult(res);
		if (res.Match != null)
		{
			UM_TBM_Match match = new UM_TBM_Match(res.Match);
			uM_TBM_MatchResult.SetMatch(match);
			UpdateMatchData(match);
		}
		this.MatchLoadedEvent(uM_TBM_MatchResult);
	}

	private void HandleActionMatchesResultLoaded(GP_TBM_LoadMatchesResult res)
	{
		_Matches.Clear();
		if (res.IsSucceeded)
		{
			foreach (KeyValuePair<string, GP_TBM_Match> loadedMatch in res.LoadedMatches)
			{
				GP_TBM_Match value = loadedMatch.Value;
				UM_TBM_Match match = new UM_TBM_Match(value);
				UpdateMatchData(match);
			}
		}
		CheckDataCounter();
	}

	private void HandleActionInvitationsListLoaded(List<GP_Invite> res)
	{
		_Invitations.Clear();
		foreach (GP_Invite re in res)
		{
			UM_TBM_Invite item = new UM_TBM_Invite(re);
			_Invitations.Add(item);
		}
		CheckDataCounter();
	}

	private void HandleActionMatchUpdated(GP_TBM_UpdateMatchResult res)
	{
		UM_TBM_MatchResult uM_TBM_MatchResult = new UM_TBM_MatchResult(res);
		if (res.Match != null)
		{
			UM_TBM_Match match = new UM_TBM_Match(res.Match);
			uM_TBM_MatchResult.SetMatch(match);
			UpdateMatchData(match);
		}
		this.MatchUpdatedEvent(uM_TBM_MatchResult);
	}

	private void HandleActionTurnFinished(GP_TBM_UpdateMatchResult res)
	{
		UM_TBM_MatchResult uM_TBM_MatchResult = new UM_TBM_MatchResult(res);
		if (res.Match != null)
		{
			UM_TBM_Match match = new UM_TBM_Match(res.Match);
			uM_TBM_MatchResult.SetMatch(match);
			UpdateMatchData(match);
		}
		this.TurnEndedEvent(uM_TBM_MatchResult);
	}

	private void HandleActionPlayerConnected()
	{
		Singleton<GooglePlayTBM>.Instance.RegisterMatchUpdateListener();
		Singleton<GooglePlayInvitationManager>.Instance.RegisterInvitationListener();
	}

	private void HandleActionInvitationAccepted(GP_Invite invite)
	{
		UnityEngine.Debug.Log("GP_TBM_Controller::HandleActionInvitationAccepted");
	}

	private void HandleActionInvitationReceived(GP_Invite invite)
	{
		UnityEngine.Debug.Log("GP_TBM_Controller::HandleActionInvitationReceived");
		LoadMatchesInfo();
	}

	private void HandleActionMatchInvitationDeclined(string invitationId)
	{
		RemoveInvitationsFromTheList(invitationId);
		this.MatchesListUpdated();
		this.InvitationDeclined(invitationId);
	}

	private void HandleActionMatchInvitationAccepted(string invitationId, GP_TBM_MatchInitiatedResult res)
	{
		UnityEngine.Debug.Log("GP_TBM_Controller::HandleActionMatchInvitationAccepted");
		UM_TBM_MatchResult uM_TBM_MatchResult = new UM_TBM_MatchResult(res);
		if (res.IsSucceeded)
		{
			RemoveInvitationsFromTheList(invitationId);
			UM_TBM_Match match = new UM_TBM_Match(res.Match);
			uM_TBM_MatchResult.SetMatch(match);
			UpdateMatchData(match);
			UnityEngine.Debug.Log("GP_TBM_Controller::HandleActionMatchInvitationAccepted, list updated");
		}
		this.InvitationAccepted(uM_TBM_MatchResult);
	}

	private void HandleActionMatchLeaved(GP_TBM_LeaveMatchResult res)
	{
		if (res.IsSucceeded)
		{
			RemoveMatchFromTheList(res.MatchId);
		}
	}

	private void UpdateMatchData(UM_TBM_Match match)
	{
		bool flag = false;
		if (match.IsEnded && match.IsLocalPlayerTurn)
		{
			Singleton<GooglePlayTBM>.Instance.ConfirmMatchFinish(match.Id);
		}
		for (int i = 0; i < Matches.Count; i++)
		{
			if (Matches[i].Id.Equals(match.Id))
			{
				flag = true;
				Matches[i] = match;
			}
		}
		if (!flag)
		{
			Matches.Add(match);
		}
		this.MatchesListUpdated();
	}

	private void RemoveMatchFromTheList(string matchId)
	{
		foreach (UM_TBM_Match match in _Matches)
		{
			if (match.Id.Equals(matchId))
			{
				_Matches.Remove(match);
				this.MatchesListUpdated();
				break;
			}
		}
	}

	private void RemoveInvitationsFromTheList(string inviteId)
	{
		foreach (UM_TBM_Invite invitation in _Invitations)
		{
			if (invitation.Id.Equals(inviteId))
			{
				_Invitations.Remove(invitation);
				break;
			}
		}
	}
}
