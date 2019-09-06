using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Validators;
using Sitecore.Data.Validators.ItemValidators;
using Sitecore.Rules;
using Sitecore.Rules.Validators;
using System;
using System.Runtime.Serialization;
using System.Web;

namespace SharedSource.RuleValidators
{
	[Serializable]
	public class RuleValidatorWithForcedFields : RuleValidator
	{
		public RuleValidatorWithForcedFields() { }

		public RuleValidatorWithForcedFields(SerializationInfo info, StreamingContext context) : base(info, context) { }

		protected override ValidatorResult Evaluate()
		{
			Item item = this.GetItem();
			Item validatorItem = item?.Database.GetItem(ValidatorID);
			if (validatorItem == null)
			{
				return ValidatorResult.Valid;
			}

			RuleList<ValidatorsRuleContext> ruleList = (!validatorItem.HasChildren ? RuleFactory.GetRules<ValidatorsRuleContext>(validatorItem.Fields["Rule"]) : RuleFactory.GetRules<ValidatorsRuleContext>(validatorItem, "Rule"));
			if (ruleList == null)
			{
				return ValidatorResult.Valid;
			}

			// --------------------------------------------------------
			// ** Begin Customization
			// Force update certain fields from the values in the Content Editor even if they were standard values when loaded from the database
			string[] fieldIds = validatorItem.Fields["Load Fields"]?.Value.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
			if (fieldIds != null)
			{
				foreach (string fieldId in fieldIds)
				{
					Field field = item.Fields[fieldId];
					if (field != null)
					{
						this.UpdateField(field);
					}
				}
			}
			// ** End Customization
			// --------------------------------------------------------

			var validatorsRuleContext = new ValidatorsRuleContext
			                            {
				                            Item = item,
				                            Validator = this,
				                            Result = ValidatorResult.Valid,
				                            Text = string.Empty
			                            };
			ruleList.Run(validatorsRuleContext);
			if (validatorsRuleContext.Result == ValidatorResult.Valid)
			{
				return ValidatorResult.Valid;
			}

			this.Text = this.GetText(validatorsRuleContext.Text, item.DisplayName);
			return GetFailedResult(validatorsRuleContext.Result);
		}

		/// <summary>
		/// Updates ControlToValidate value.
		/// </summary>
		/// <param name="field">Current field id is used for searching by controlsToValidate dictionary.</param>
		protected virtual void UpdateField(Field field)
		{
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				string item = this.ControlsToValidate[field.ID.ToString()];
				if (item != null)
				{
					string str = current.Request.Form[item];
					if (str != null)
					{
						field.Value = str;
					}
				}
			}
		}
	}
}