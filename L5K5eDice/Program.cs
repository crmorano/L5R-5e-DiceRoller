using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5K5eDice
{
    class Program
    {
        static void Main(string[] args)
        {
            L5R5eDice roller = new L5R5eDice();
            double success = 0.0;
            double opp = 0.0;
            double strife = 0.0;
            int repeats = 100000;
            
            for (int ring = 1; ring < 6; ring++)
            {

                for (int skill = 0; skill < 6; skill++)
                {
                    success = 0.0;
                    opp = 0.0;
                    strife = 0.0;
                    for (int i = 0; i < repeats; i++)
                    {


                        roller.RollForSuccess(skill, ring);
                        success += roller.Results.Success + roller.Results.Explode;
                        opp += roller.Results.Opportunity;
                        strife += roller.Results.Strife;
                    }
                    success /= repeats;
                    opp /= repeats;
                    strife /= repeats;
                    Console.WriteLine("{0}k{1}: Success: {2:F2}, Opportunity: {3:F2}, Strife: {4:F2}", (skill + ring), ring, success, opp, strife);
                }
            }
            //Success, Strife, Opportunity, Exploding, Ring, Skill
            //L5rDiceWeight successWeights = new L5rDiceWeight(7, -2, 3, 13, 0, 1, -5);

            //roller.TestStatWeight(successWeights);

            //Console.WriteLine(roller);

            Console.ReadLine();

        }
    }
}
