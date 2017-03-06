using System.Collections.Generic;
using GroceryCo.BusinessObjects;

namespace GroceryCo.Models
{
	/// <summary>
	/// This Promo class will keep track of the items that were scanned to determine if the customer
	/// qualifies for a sale price because they bought something at a certain amount.
	/// </summary>
	class Promo
	{
		Dictionary<int, int> purchaseCount;
		Dictionary<string, bool> promoStatus;

		/// <summary>
		/// The <c>Constructor</c> of this class will create a new HashMap of the ProductUPC and the count of how many times
		/// it was "scanned". This way, it can determine if the customer qualifies for a product.
		/// </summary>
		public Promo()
		{
			purchaseCount = new Dictionary<int, int>();
			promoStatus = new Dictionary<string, bool>();

			promoStatus.Add("Buy 3 Apples for $2.00", true);
			promoStatus.Add("Buy 2 Bananas Get 1 for %50 Off", true);
			promoStatus.Add("Buy 3 Oranges Get 1 Free", false);
		}

		/// <summary>
		/// The <c>ApplyDiscount</c> will have <c>Product</c> parameter that will be used to check to see if it already exist in the
		/// HashMap. It adds it in the Hashmap if it doesn't exist yet and initializes the count to 1. This is a practice of a Lazy Loading pattern.
		/// 
		/// Afterwards, the function will determine if the product count of the parametized <c>Product</c> object qualified for a promo. The
		/// promotions are grouped into if-statements.
		/// 
		/// Requirements states that I must be reading the promotion from a text-file. However, due to time-constraints, I'm not able to implement
		/// a more scalable version of this promotional feature. If I do, I will implement it in a way that we'll have different promo objects that inherits
		/// from a parent. All of these promo objects have different conditions corresponding to a promotion condition.
		/// </summary>
		/// <param name="toCheck">The <c>Product</c> to check if it qualified for a promo depending on the number of times it was checked out.</param>
		public void ApplyDiscount(Product toCheck)
		{
			if(!purchaseCount.ContainsKey(toCheck.UPC))
			{
				purchaseCount.Add(toCheck.UPC, 0);
			}

			purchaseCount[toCheck.UPC]++;
			switch (toCheck.UPC)
			{
				case 111:
					//Buy 3 Apples for $2.00
					if (promoStatus["Buy 3 Apples for $2.00"] && purchaseCount[toCheck.UPC] % 3 == 0)
						toCheck.Discount = (toCheck.Price * 3) - 200;
					break;

					//Buy 2 Banana for Half Off
				case 222:
					if (promoStatus["Buy 2 Bananas Get 1 for %50 Off"] && purchaseCount[toCheck.UPC] % 2 == 0)
						toCheck.Discount = toCheck.Price / 2;
					break;

				case 333:
					//Buy 3 Get, Get One Free
					if (promoStatus["Buy 3 Oranges Get 1 Free"] && purchaseCount[toCheck.UPC] % 3 == 0)
						toCheck.Discount = toCheck.Price;
					break;
			}

		}

		/// <summary>
		/// The <c>RemoveDiscount</c> is normally called when a customer voids an item. This method will disqualify any promotion that was applied
		/// to the item that was voided, if applicable.
		/// </summary>
		/// <param name="toCheck">The <c>Product</c> to decrement the item count.</param>
		public void RemoveDiscount(Product toCheck)
		{
			if (purchaseCount.ContainsKey(toCheck.UPC))
			{
				purchaseCount[toCheck.UPC]--;
			}
		}

		/// <summary>
		/// The <c>Clear</c> method will clear out and reset the product list to default. Normally called when the customer finished checking out
		/// and the next customer prepares to scan items.
		/// </summary>
		public void Clear()
		{
			purchaseCount.Clear();
		}

		public Dictionary<string, bool> GetAllPromotions()
		{
			return promoStatus;
		}

		public void TogglePromotion(string toToggle)
		{
			promoStatus[toToggle] = !promoStatus[toToggle];
		}
	}
}
