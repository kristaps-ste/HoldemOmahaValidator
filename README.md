# HoldemOmahaValidator is a hand strength calculator library for:
Holdem and Omaha card games.

It parses input string like: 2c2hQcQhAd Ac5h AhKh 5d6h and returns input hands ordered by its strength in increasing order, 5d6h Ac5h AhKh in this case.
An error message is returned if the input is invalid(not parsable).

How to run: 
Create Eval and its helper instances LookUpService, Inputvalidator) in Ioc container or manually. 

Then  call the Eval method from IEvaluator interface 
with input string and enum for hand type (Holdem Or Omaha) Eval(input,HandType.Holdem)
