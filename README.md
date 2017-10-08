Look in the bin\Debug\ folder for example outputs.

Success.txt:
The idea behind this was to see how many success you can max out rolling for each set of dice.  In this case I prioritized Gaining successes over everything else, including strife.  The priority was to keep as many dice as possible, prioritizing as follows: Exploding > Success > Opportunity.  If the exploding dice weren't blank, I kept them.

All of the values are means over 1M rolls, with the following meanings:
 - XkY: X rolled dice, Y Kept dice For the base roll (Could roll+keep more after exploding)
 - Success: # of successes, 
 - Opportunity: # of Opportunities
 - Strife: Amount of Strife gained from the roll
 
 
 
Opportunity.txt:
The idea behind this was to see how many opportunities you would average, assuming a specific TN.  In this case, I started with the same priority as Success.txt, but once I had enough successes to match the TN, I switched to the a priority of: Opportunity > Exploding > Success.  I continued to ignore strife and kept any rerolled exploded dice that weren't blank.
 
All of the values were means over some number of rolls with the following meanings:
- XkY: X rolled dice, Y Kept dice For the base roll (Could roll+keep more after exploding).  
- TN: TN to Shoot for.
- Success: % of rolls that successeed across 1M
- Failure: % of rolls that failed across 1M (~ 1- Success)
- Opporunity: Mean amount of opportunity gained on a successful roll
- Extra Succes: mean amount of successes beyond meeting the TN on a successful roll.
- Strife: Mean amount of Strife gained on all rolls

Random.Txt:
The idea of this was to see what the results of a completely random (Unprioritized) roll was.  Dice were chosen to be kept at random from the rolled dice pool, and if one exploded, it had a 50/50 chance of being kept.

ll of the values are means over 1M rolls, with the following meanings:
 - XkY: X rolled dice, Y Kept dice For the base roll (Could roll+keep more after exploding)
 - Success: # of successes, 
 - Opportunity: # of Opportunities
 - Strife: Amount of Strife gained from the roll
