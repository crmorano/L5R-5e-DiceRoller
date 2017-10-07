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
        //public int AddedDie = 0;
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
        public int Weight = 0;
        public DieTypes DieType;
        public L5rDieResult Result = new L5rDieResult();
        public abstract void Roll(int val, L5rDiceWeight RollWeight);

        public override string ToString()
        {
            string str = "";

            if(DieType == DieTypes.DieType_Ring)
            {
                str = "r";
            }
            else if(DieType == DieTypes.DieType_Skill)
            {
                str = "s";
            }

            if(Result.Explode > 0)
            {
                str += "E";
            }

            if ( Result.Success > 0)
            {
                str += "S";
            }

            if(Result.Opportunity > 0)
            {
                str += "O";
            }

            if(Result.Strife > 0)
            {
                str += "T";
            }

             return str;
        }

        //We want highest die weight first.
        public Int32 CompareTo(Object o)
        {
            Die die = ((Die)(o));
            return  die.Weight - Weight;
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

        //Roll for Most successes (Success + Strife > No success)
        public void RollForSuccess(int Skill, int Ring)
        {
            //Clear
            RolledDice = new List<Die>();
            KeptDice = new List<Die>();

            Kept = Ring;
            Rolled = Skill + Ring;

            Results = new L5rDieResult();

            L5rDiceWeight successWeights = new L5rDiceWeight(7, -2, 3, 13, 0, 1, -5);


            int i;
            for(i = 0; i < Skill; i++)
            {
                SkillDie NewDie = new SkillDie();
                NewDie.Roll(RollSkill(), successWeights);

                RolledDice.Add(NewDie);

            }

            for (i = 0; i < Ring; i++)
            {
                RingDie NewDie = new RingDie();
                NewDie.Roll(RollRing(), successWeights);
                RolledDice.Add(NewDie);
            }

            RolledDice.Sort();

            for(i = 0; i < Kept; i++)
            {
                KeptDice.Add(RolledDice[i]);
            }


            
            //foreach(Die die in KeptDice)
            for(i = 0; i < KeptDice.Count; i++)
            {
                //Die die = ;
                if(KeptDice[i].Result.Explode > 0)
                {
                    if(KeptDice[i] is SkillDie)
                    {
                        SkillDie newDie = new SkillDie();
                        newDie.Roll(RollSkill(), successWeights);

                        if(newDie.Weight > 0)
                        {
                            KeptDice.Add(newDie);
                            Kept++;
                            
                        }
                    }
                    else if(KeptDice[i] is RingDie)
                    {
                        RingDie newDie = new RingDie();
                        newDie.Roll(RollRing(), successWeights);

                        if (newDie.Weight > 0)
                        {
                            KeptDice.Add(newDie);
                            Kept++;
                        }
                    }
                }
            }

            CalcResults();
        }

        public override string ToString()
        {

            string str = "";
            foreach(Die die in KeptDice)
            {
                str += die.ToString() + " ";
            }
            return str;
        }

        public void TestStatWeight(L5rDiceWeight weight)
        {
            RolledDice = new List<Die>();

            for (int i = 1; i < 13; i++)
            {
                SkillDie skill = new SkillDie();
                skill.Roll(i, weight);

                RolledDice.Add(skill);
            }

            for (int i = 1; i < 7; i++)
            {
                RingDie skill = new RingDie();
                skill.Roll(i, weight);

                RolledDice.Add(skill);
            }
            RolledDice.Sort();
        }

        public L5R5eDice()
        {
            Results = new L5rDieResult();
            RolledDice = new List<Die>();
            KeptDice = new List<Die>();
            rnd = new Random(DateTime.Now.Millisecond + DateTime.Now.Second); //Seed
        }
        
    }
}
