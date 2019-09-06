# SharedSource.RuleValidators

## Summary
This module adds functionality to the existing `Sitecore.Data.Validators.ItemValidators.RuleValidator` to support forcing certain fields to be loaded, even if the field values from the database are standard values.

## Example Use Case

### Requirements

* Item has two phone number fields: "Home Phone", "Mobile Phone"
* One or both of the phone number fields must have a value; both cannot be simultaneously empty

### Configuration

* Create new `Rules Validation Rule with Forced Fields` validator somewhere below `/sitecore/system/Settings/Validation Rules/Item Rules`
* Pick the two fields ("Home Phone", "Mobile Phone") in the `Load Fields` field of the new validator
* Configure the `Rule` field to check if the two fields are empty and if so, set an error result with a relevant message
* Update the standard values item of your template with the two fields to have this new validator selected in all four validation fields:
   * `Quick action bar validation rules`
   * `Validation button validation rules`
   * `Validation bar validation rules`
   * `Workflow validation rules`

# Notes and Disclaimers

1. The majority of this code is decompiled from the `Sitecore.Data.Validators.ItemValidators.RuleValidator` class in `Sitecore.Kernel` for version 9.0.2 and left as-is. If Sitecore is updated, you will want to verify that this code has not changed in the Kernel and update it if it has.
2. This solution has only been tested with Sitecore 9.0.2 and cannot be guaranteed to work with other versions of Sitecore.