Assumptions
Data
It is assumed that the data in the supplied CSV files comes from an internal system so the content of the files is relatively
trustworthy.  Dates are always parsed from the dd/MM/yyyy format etc.
Also, the logic for associating entities (e.g. orders to customers etc.) does not have any handling for orphaned entities (i.e. orders without a customer or order items without a corresponding order).

Discount Rules
The existing discounts supplied in OrderItems.csv are obviously percentage figures rather than monetary values.
Only one discount can be applied for each customer and a new discount replaces any prior existing discount (as supplied in the OrderItems.csv file).
Although the discount specified in the TechnicalAssessment.pdf only evaluates the customer's home state,
it is likely that other discounts might depend upon other variables such as order value hence the DiscountRules
accept both a customer and an order entity.
Although the current discounts are percentage based, other future discounts could be for a specific monetary amount rather than a percentage.

Rounding
Discounts will be rounded to the nearest minor currency unit (cents or pence etc.)