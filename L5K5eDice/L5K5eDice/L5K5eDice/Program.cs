using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5R5eDice
{
    class Program
    {

        static void RollForSuccess()
        {
            StreamWriter write = new StreamWriter("Success.txt");
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
                    write.WriteLine("{0}k{1}: Success: {2:F2}, Opportunity: {3:F2}, Strife: {4:F2}", (skill + ring), ring, success, opp, strife);
                }
            }
            write.Close();
        }

        static void RollForOpp()
        {
            StreamWriter write = new StreamWriter("Opportunity.txt");
            L5R5eDice roller = new L5R5eDice();
            double success = 0.0;
            double extraSuccess = 0.0;
            double failure = 0.0;
            double strife = 0.0;
            double opportunity = 0.0;
            int repeats = 100000;


            for (int ring = 1; ring < 6; ring++)
            {
                for (int skill = 0; skill < 6; skill++)
                {
                    for (int tn = 1; tn < 5; tn++)
                    {
                        success = 0.0;
                        extraSuccess = 0.0;
                        failure = 0.0;
                        opportunity = 0.0;
                        strife = 0.0;
                        for (int i = 0; i < repeats; i++)
                        {
                            if(roller.RollForOpp(skill, ring,tn))
                            {
                                success += 1;
                                extraSuccess += roller.Results.Success + roller.Results.Explode - tn;
                            }
                            else
                            {
                                failure += 1;
                            }
                            
                            opportunity += roller.Results.Opportunity;
                            strife += roller.Results.Strife;
                        }
                        success /= repeats;
                        failure /= repeats;
                        extraSuccess /= (repeats- failure);
                        opportunity /= (repeats - failure);
                        strife /= repeats;
                        Console.WriteLine("{0}k{1} TN{2}: Success: {3:F3}, Failure: {4:F3}, Opportunity: {5:F2}, Extra Success: {6:F2}, Strife: {7:F2}", (skill + ring), ring, tn, success, failure, opportunity, extraSuccess, strife);
                        write.WriteLine("{0}k{1} TN{2}: Success: {3:F3}, Failure: {4:F3}, Opportunity: {5:F2}, Extra Success: {6:F2}, Strife: {7:F2}", (skill + ring), ring, tn, success, failure, opportunity, extraSuccess, strife);
                    }
                }
            }

            write.Close();
        }

        static void Main(string[] args)
        {
            L5R5eDice roller = new L5R5eDice();

            //Weighting for max successes, ignoring strife
            L5rDiceWeight successWeights = new L5rDiceWeight(7, -2, 3, 13, 0, 1, -5);

            //Success, Strife, Opportunity, Exploding, Ring, Skill
            //L5rDiceWeight successOppWeights = new L5rDiceWeight(7, -2, 5, 13, 0, 1, -5);
            //L5rDiceWeight oppWeights = new L5rDiceWeight(3, -2, 13, 7, 0, 1, -5);
            //roller.TestStatWeight(successOppWeights);
            //for (int i = 0; i < 10; i++)
            //{
            //    roller.RollForOpp(3, 3, 2);

            //    Console.WriteLine(roller);
            //}

            RollForOpp();
            RollForSuccess();

            Console.ReadLine();

        }
    }
}
