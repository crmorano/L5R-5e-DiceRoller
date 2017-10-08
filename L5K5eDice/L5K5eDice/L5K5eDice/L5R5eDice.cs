using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5R5eDice
{
    public class L5rDieResult
    {
        public int Success = 0;
        public int Strife = 0;
        public int Opportunity = 0;
        public int Explode = 0;
        public int AddedDie = 0;
        public override string ToString()
        {
            string str = string.Format("Successes: {0}, Opportunities: {1}, Strife: {2}", (Success+Explode), Opportunity, Strife );

            return str;
        }

    }

    public class L5rDiceWeight
    {
        public int Success = 1;
        public int Strife = -1;
        public int Opportunity = 1;
        public int Explode = 2;
        public int Skill = 1;
        public int Ring = 0;
        public int Blank = 0;

        public int CalcWeight(L5rDieResult result, DieTypes type)
        {
            int weight = 0;

            if( 0 == (result.Success + result.Explode + result.Opportunity) )
            {
                return Blank;
            }
            
            weight = (result.Success * Success) + (result.Strife * Strife) 
                + (result.Explode * Explode) + (result.Opportunity * Opportunity);
            

            if(type == DieTypes.DieType_Ring)
            {
                weight += Ring;
            }
            else if(type == DieTypes.DieType_Skill)
            {
                weight += Skill;
            }

            return weight;
        }

        public L5rDiceWeight(int suc, int str, int opp, int exp, int ring, int skill, int blank)
        {
            Success = suc;
            Strife = str;
            Opportunity = opp;
            Explode = exp;
            Ring = ring;
            Skill = skill;
            Blank = blank;
        }
    }

    public enum DieTypes
    {
        DieType_Skill,
        DieType_Ring,
    }

    public abstract class Die: IComparable
    {
        public int RollVal = 0;
        public int Weight = 0;
        public DieTypes DieType;
        public L5rDieResult Result = new L5rDieResult();
        public bool isExploded = false;
        public abstract void Roll(int val, L5rDiceWeight RollWeight);

        public void ReapplyWeight(L5rDiceWeight rollWeight)
        {
            Weight = rollWeight.CalcWeight(Result, DieType);
        }

        public override string ToString()
        {
            string str = "";

            if (DieType == DieTypes.DieType_Ring)
            {
                str = "r";
            }
            else if (DieType == DieTypes.DieType_Skill)
            {
                str = "s";
            }

            if (Result.Explode > 0)
            {
                str += "E";
            }

            if (Result.Success > 0)
            {
                str += "S";
            }

            if (Result.Opportunity > 0)
            {
                str += "O";
            }

            if (Result.Strife > 0)
            {
                str += "T";
            }

            if (isExploded)
            {
                str += "*";
            }

            return str;
        }

        //We want highest die weight first.
        public Int32 CompareTo(Object o)
        {
            Die die = ((Die)(o));
            return  die.Weight - Weight;
        }

        public bool IsSuccess()
        {
            if(Result.Success > 0 || Result.Explode > 0)
            {
                return true;
            }
            return false;
        }

        public Die(bool exploded)
        {
            isExploded = exploded;
        }

        public Die()
        {

        }
    }
 

    public class SkillDie : Die
    {
        
        public SkillDie()
        {
            DieType = DieTypes.DieType_Skill;
        }

        public override void Roll(int val, L5rDiceWeight RollWeight)
        {
            Result = new L5rDieResult();
            RollVal = val;
            switch (val)
            {
                case 1:
                case 2:
                default:
                    {
                        break;
                    }

                case 3:
                case 4:
                    {
                        Result.Success++;
                        break;
                    }

                case 5:
                    {
                        Result.Success++;
                        Result.Opportunity++;
                        break;
                    }

                case 6:
                case 7:
                    {
                        Result.Success++;
                        Result.Strife++;
                        break;
                    }

                case 8:
                    {
                        Result.Explode++;
                        break;
                    }

                case 9:
                    {
                        Result.Strife++;
                        Result.Explode++;
                        break;
                    }

                case 10:
                case 11:
                case 12:
                    {
                        Result.Opportunity++;
                        break;
                    }
            }

            Weight = RollWeight.CalcWeight(Result, DieTypes.DieType_Skill);
        }

        public SkillDie(bool explode) : base(explode)
        {

        }
    }

    public class RingDie : Die
    {
        public RingDie()
        {
            DieType = DieTypes.DieType_Ring;
        }

        public override void Roll(int val, L5rDiceWeight RollWeight)
        {
            Result = new L5rDieResult();
            RollVal = val;
            switch (val)
            {
                case 1:
                default:
                    {
                        break;
                    }

                case 2:
                    {
                        Result.Success++;
                        break;
                    }

                case 3:
                    {
                        Result.Success++;
                        Result.Strife++;
                        break;
                    }

                case 4:
                    {
                        Result.Strife++;
                        Result.Explode++;
                        break;
                    }

                case 5:
                    {
                        Result.Opportunity++;
                        break;
                    }

                case 6:
                    {
                        Result.Strife++;
                        Result.Opportunity++;
                        break;
                    }
            }

            Weight = RollWeight.CalcWeight(Result, DieTypes.DieType_Ring);
        }

        public RingDie(bool explode) : base(explode)
        {

        }
    }

    public class L5R5eDice
    {
        List<Die> RolledDice;
        List<Die> KeptDice;
        int Rolled = 0;
        int Kept = 0;
        Random rnd;
        public L5rDieResult Results;

        int RollSkill()
        {
            return rnd.Next(1, 13);
        }

        int RollRing()
        {
            return rnd.Next(1, 7);
        }

        public L5rDieResult CalcResults()
        {
            L5rDieResult result = new L5rDieResult();
            foreach (Die die in KeptDice)
            {
                result.Explode += die.Result.Explode;
                result.Strife += die.Result.Strife;
                result.Success += die.Result.Success;
                result.Opportunity += die.Result.Opportunity;
            }
            Results = result;
            return result;
        }

        //Roll the base dice with the applied weight
        void RollBaseDice(int Skill, int Ring, L5rDiceWeight weight)
        {
            Kept = Ring;
            Rolled = Skill + Ring;

            //Roll base dice.
            int i;
            for (i = 0; i < Skill; i++)
            {
                SkillDie NewDie = new SkillDie();
                NewDie.Roll(RollSkill(), weight);

                RolledDice.Add(NewDie);

            }

            for (i = 0; i < Ring; i++)
            {
                RingDie NewDie = new RingDie();
                NewDie.Roll(RollRing(), weight);
                RolledDice.Add(NewDie);
            }
        }

        /// <summary>
        /// Look throught the Kept Dice and explode them all.
        /// </summary>
        /// <param name="inPool">Input Pool of Dice to check for exploding</param>
        /// <param name="weightThreshold">What value of weight should we keep a die after the reroll</param>
        /// <param name="weight">Stat Weight to apply to the pool (For the threshold)</param>
        /// <returns></returns>
        List<Die> ExplodeDiceInPool(List<Die> inPool, int weightThreshold, L5rDiceWeight weight)
        {
            List<Die> pool = new List<Die>(inPool);

            //Check for exploding dice, and add them.  Count will go up when we add the dice so we will catch extra explodes.
            int i = 0;
            for (i = 0; i < pool.Count; i++)
            {
                //Die die = ;
                if (pool[i].Result.Explode > 0)
                {
                    if (pool[i] is SkillDie)
                    {
                        SkillDie newDie = new SkillDie(true);
                        newDie.Roll(RollSkill(), weight);

                        if (newDie.Weight > weightThreshold)
                        {
                            pool.Add(newDie);
                        }
                    }
                    else if (pool[i] is RingDie)
                    {
                        RingDie newDie = new RingDie(true);
                        newDie.Roll(RollRing(), weight);

                        if (newDie.Weight > weightThreshold)
                        {
                            pool.Add(newDie);
                        }
                    }
                }
            }
            return pool;
        }


        //Roll for Most successes (Success + Strife > No success)
        public void RollForSuccess(int Skill, int Ring)
        {
            //Clear
            RolledDice = new List<Die>();
            KeptDice = new List<Die>();

            

            Results = new L5rDieResult();

            L5rDiceWeight successWeights = new L5rDiceWeight(7, -2, 3, 14, 0, 1, -5);

            RollBaseDice(Skill, Ring, successWeights);

            //Sort by Weight, then add them to the Kept dice
            RolledDice.Sort();
            int i;
            for (i = 0; i < Kept; i++)
            {
                KeptDice.Add(RolledDice[0]);
                RolledDice.RemoveAt(0);
            }


            for (i = 0; i < KeptDice.Count; i++)
            {
                //Die die = ;
                if (KeptDice[i].Result.Explode > 0)
                {
                    if (KeptDice[i] is SkillDie)
                    {
                        SkillDie newDie = new SkillDie(true);
                        newDie.Roll(RollSkill(), successWeights);

                        if (newDie.Weight > 0)
                        {
                            KeptDice.Add(newDie);
                        }
                        else
                        {
                            RolledDice.Add(newDie);
                        }
                    }
                    else if (KeptDice[i] is RingDie)
                    {
                        RingDie newDie = new RingDie(true);
                        newDie.Roll(RollRing(), successWeights);

                        if (newDie.Weight > 0)
                        {
                            KeptDice.Add(newDie);
                        }
                        else
                        {
                            RolledDice.Add(newDie);
                        }
                    }
                }
            }


            //Math up the result totals.
            CalcResults();
        }

        //Roll for Most successes (Success + Strife > No success)
        public bool RollForOpp(int Skill, int Ring, int TN)
        {
            //Clear
            RolledDice = new List<Die>();
            KeptDice = new List<Die>();

            Kept = Ring;
            Rolled = Skill + Ring;

            Results = new L5rDieResult();

            L5rDiceWeight successWeights = new L5rDiceWeight(7, -2, 3, 14, 0, 1, -5);
            L5rDiceWeight oppWeights = new L5rDiceWeight(5, -2, 13, 9, 0, 1, -5);

            L5rDiceWeight diceWeights = successWeights;

            //Roll and sort initially by Success:
            RollBaseDice(Skill, Ring, successWeights);
            RolledDice.Sort();

            int successes = 0;
            int i;
            int j = 0;
            //Check for successes, stop once we have enough (if we have enough)
            for (i = 0; i < Kept; i++)
            {
                KeptDice.Add(RolledDice[0]);
                
                if (RolledDice[0].IsSuccess())
                {
                    successes++;
                    if (successes >= TN)
                    {
                        RolledDice.RemoveAt(0);
                        break;
                    }
                }
                RolledDice.RemoveAt(0); //We want to remove dice we've
            }

            //Have we finished collecting all of the dice?  If we don't have anymore, we barely made it and can just move on to making them EXPLODE.
            //If we do have more to collect, we don't need to worry about collecting more successes, so just add them all in.
            if (KeptDice.Count < Kept)
            {
                //Switch weighting and re-sort
                diceWeights = oppWeights;
                for (i = 0; i< RolledDice.Count; i++)
                {
                    RolledDice[i].ReapplyWeight(diceWeights);
                }
                RolledDice.Sort();
                //Start collecting new dice. Just need the difference between kept and how many we have.
                for(i = 0; i < (Kept - KeptDice.Count); i++)
                {
                    KeptDice.Add(RolledDice[0]);
                    RolledDice.RemoveAt(0);
                }

            }

            //At this point, We should have all of our kept dice.  Depending on If we DO have enough successes, we have already setup the variable diceWeights to match the correct one.

            //Now for exploding dice, after each exploding die we need to determine if we need to change the stat weights or not.

            //Check for exploding dice, and add them.  Count will go up when we add the dice so we will catch extra explodes.
            for (i = 0; i < KeptDice.Count; i++)
            {
                //Die die = ;
                if (KeptDice[i].Result.Explode > 0)
                {
                    if (KeptDice[i] is SkillDie)
                    {
                        SkillDie newDie = new SkillDie(true);
                        newDie.Roll(RollSkill(), diceWeights);

                        if (newDie.IsSuccess())
                        {
                            successes++;
                        }

                        if (newDie.Weight > 0)
                        {
                            KeptDice.Add(newDie);
                            Kept++;
                        }
                        else
                        {
                            RolledDice.Add(newDie);
                            Rolled++;
                        }
                    }
                    else if (KeptDice[i] is RingDie)
                    {
                        RingDie newDie = new RingDie(true);
                        newDie.Roll(RollRing(), diceWeights);

                        if (newDie.IsSuccess())
                        {
                            successes++;
                        }
                        if (newDie.Weight > 0)
                        {
                            KeptDice.Add(newDie);
                            Kept++;
                        }
                        else
                        {
                            RolledDice.Add(newDie);
                            Rolled++;
                        }
                    }
                    if(successes >= TN)
                    {
                        diceWeights = oppWeights;
                    }
                }
            }
            

            //Math up the result totals.
            CalcResults();

            return (successes >= TN);
        }

        //Prints out each Dice in their current order.
        public override string ToString()
        {
            string str = "";
            foreach(Die die in KeptDice)
            {
                str += die.ToString() + " ";
            }
            str += "( ";

            foreach (Die die in RolledDice)
            {
                str += die.ToString() + " ";
            }

            str += ")";
            return str;
        }

        //Pass in a set of stat weights to each die type, sort them
        public void TestStatWeight(L5rDiceWeight weight)
        {
            RolledDice = new List<Die>();
            KeptDice = new List<Die>();
            //Fill out skill dice
            for (int i = 1; i < 13; i++)
            {
                SkillDie skill = new SkillDie();
                skill.Roll(i, weight);
                
                RolledDice.Add(skill);
                KeptDice.Add(skill);
            }
            //Fill out roll dice
            for (int i = 1; i < 7; i++)
            {
                RingDie skill = new RingDie();
                skill.Roll(i, weight);

                RolledDice.Add(skill);
                KeptDice.Add(skill);
            }
            KeptDice.Sort();
            
        }

        //Initialize stuff, 
        public L5R5eDice()
        {
            Results = new L5rDieResult();
            RolledDice = new List<Die>();
            KeptDice = new List<Die>();
            rnd = new Random(DateTime.Now.Millisecond + DateTime.Now.Second); //Seed
        }
        
    }
}
