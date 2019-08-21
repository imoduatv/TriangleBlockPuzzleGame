using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SA.IOSNative.StoreKit
{
	public class StoreProductView
	{
		private int _id;

		private List<string> _ids;

		public int Id => _id;

		public event Action Loaded;

		public event Action LoadFailed;

		public event Action Appeared;

		public event Action Dismissed;

		public StoreProductView()
		{
			this.Loaded = delegate
			{
			};
			this.LoadFailed = delegate
			{
			};
			this.Appeared = delegate
			{
			};
			this.Dismissed = delegate
			{
			};
			_ids = new List<string>();
			//base._002Ector();
			foreach (string item in IOSNativeSettings.Instance.DefaultStoreProductsView)
			{
				addProductId(item);
			}
			Singleton<PaymentManager>.Instance.RegisterProductView(this);
		}

		public StoreProductView(params string[] ids)
		{
			this.Loaded = delegate
			{
			};
			this.LoadFailed = delegate
			{
			};
			this.Appeared = delegate
			{
			};
			this.Dismissed = delegate
			{
			};
			_ids = new List<string>();
			//base._002Ector();
			foreach (string productId in ids)
			{
				addProductId(productId);
			}
			Singleton<PaymentManager>.Instance.RegisterProductView(this);
		}

		public void addProductId(string productId)
		{
			if (!_ids.Contains(productId))
			{
				_ids.Add(productId);
			}
		}

		public void Load()
		{
		}

		public void Show()
		{
		}

		public void OnProductViewAppeard()
		{
			this.Appeared();
		}

		public void OnProductViewDismissed()
		{
			this.Dismissed();
		}

		public void OnContentLoaded()
		{
			Show();
			this.Loaded();
		}

		public void OnContentLoadFailed()
		{
			this.LoadFailed();
		}

		public void SetId(int viewId)
		{
			_id = viewId;
		}
	}
}
