using System;
using System.Collections.Generic;

public class WP8_InAppsInitResult : WP8_Result
{
	[Obsolete("This property is deprecated. Use 'Products' property instead")]
	public List<WP8ProductTemplate> products => Products;

	public List<WP8ProductTemplate> Products => WP8InAppPurchasesManager.Instance.Products;

	public WP8_InAppsInitResult(int statusCode)
		: base(statusCode == 0)
	{
	}
}
