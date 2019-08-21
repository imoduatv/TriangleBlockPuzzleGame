using System;
using System.Threading;
using UnityEngine;

public abstract class UM_BaseInAppClient
{
	protected bool _IsConnected;

	public bool IsConnected => _IsConnected;

	public event Action<UM_BillingConnectionResult> OnServiceConnected;

	public event Action<UM_PurchaseResult> OnPurchaseFinished;

	public event Action<UM_BaseResult> OnRestoreFinished;

	protected UM_BaseInAppClient()
	{
		this.OnServiceConnected = delegate
		{
		};
		this.OnPurchaseFinished = delegate
		{
		};
		this.OnRestoreFinished = delegate
		{
		};
		//base._002Ector();
	}

	public void Purchase(string productId)
	{
		UM_InAppProduct productById = UM_InAppPurchaseManager.GetProductById(productId);
		if (productById != null)
		{
			Purchase(productById);
		}
		else
		{
			SendNoTemplateEvent();
		}
	}

	public abstract void Purchase(UM_InAppProduct product);

	public void Subscribe(string productId)
	{
		UM_InAppProduct productById = UM_InAppPurchaseManager.GetProductById(productId);
		if (productById != null)
		{
			Subscribe(productById);
		}
		else
		{
			SendNoTemplateEvent();
		}
	}

	public abstract void Subscribe(UM_InAppProduct product);

	public void Consume(string productId)
	{
		UM_InAppProduct productById = UM_InAppPurchaseManager.GetProductById(productId);
		if (productById != null)
		{
			Consume(productById);
		}
		else
		{
			SendNoTemplateEvent();
		}
	}

	public abstract void Consume(UM_InAppProduct product);

	public void FinishTransaction(string productId)
	{
		UM_InAppProduct productById = UM_InAppPurchaseManager.GetProductById(productId);
		if (productById != null)
		{
			FinishTransaction(productById);
		}
		else
		{
			SendNoTemplateEvent();
		}
	}

	public abstract void FinishTransaction(UM_InAppProduct product);

	public bool IsProductPurchased(string productId)
	{
		UM_InAppProduct productById = UM_InAppPurchaseManager.GetProductById(productId);
		if (productById != null)
		{
			return IsProductPurchased(productById);
		}
		return false;
	}

	public virtual bool IsProductPurchased(UM_InAppProduct product)
	{
		return UM_InAppPurchaseManager.IsLocalPurchaseRecordExists(product);
	}

	protected void SendNoTemplateEvent()
	{
		UnityEngine.Debug.LogWarning("UM: Product tamplate not found");
		UM_PurchaseResult e = new UM_PurchaseResult();
		SendPurchaseFinishedEvent(e);
	}

	protected void SendServiceConnectedEvent(UM_BillingConnectionResult e)
	{
		this.OnServiceConnected(e);
	}

	protected void SendPurchaseFinishedEvent(UM_PurchaseResult e)
	{
		this.OnPurchaseFinished(e);
	}

	protected void SendRestoreFinishedEvent(UM_BaseResult e)
	{
		this.OnRestoreFinished(e);
	}
}
