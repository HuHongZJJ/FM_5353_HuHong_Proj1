using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW6_api.Models
{
    public class RangeOption: Simulator
    {
        


        public (double VP, double std) OptionPandStd(Range O)
        {
            
            double VP= new double();
            double VNDC = new double();
            double std = new double();
            double var = new double();
            double total1 = new double();
            double total2 = new double();
            double discontfator = new double();
            double dt = O.Tenor/O.S.Steps;
            double erdt = Math.Exp(O.S.r* dt);
            discontfator = Math.Exp(-O.S.r* O.Tenor);
            
            SimulationResult SpathNorm = StockPathGenerator.GeneratePaths(O);
            SimulationResult SpathAnti = StockPathGenerator.AntetheticPaths(O);

            double[] SMaxNorm = new double [O.S.Simulations];
            double[] SMinNorm = new double [O.S.Simulations];
            double[] SMaxAnti = new double [O.S.Simulations];
            double[] SMinAnti = new double [O.S.Simulations];     
            for (int i = 0; i < O.S.Simulations; i++)
            {
                double maxNorm = O.S.S0;
                double minNorm = O.S.S0;
                double maxAnti = O.S.S0;
                double minAnti = O.S.S0;
                for (int j = 0; j < O.S.Steps; j++)
                {
                    maxNorm = Math.Max(maxNorm, SpathNorm.SimulatedPaths[i, j]);
                    minNorm = Math.Min(minNorm, SpathNorm.SimulatedPaths[i, j]);
                    maxAnti = Math.Max(maxAnti, SpathAnti.SimulatedPaths[i, j]);
                    minAnti = Math.Min(minAnti, SpathAnti.SimulatedPaths[i, j]);
                }

                SMaxNorm[i] = maxNorm;
                SMinNorm[i] = minNorm;
                SMaxAnti[i] = maxAnti;
                SMinAnti[i] = minAnti;
            }



            List<double> eachOption = new List<double>();
            List<double> reverseOption = new List<double>();
            GaussianRandoms Nrands = new GaussianRandoms();
            double [, ]eps = Nrands.PopulatedNrands(O.S.Simulations,O.S.Steps);;
            

            
 
            for (int i = 0; i < O.S.Simulations; i++)
            {
                eachOption.Add(SMaxNorm[i] - SMinNorm[i]);
            } 
            total1 = eachOption.Sum();
            if (O.S.IsAntithetic == true)
            {
                for (int i = 0; i < O.S.Simulations; i++)
                {
                    reverseOption.Add(SMaxAnti[i] - SMinAnti[i]);
                }
                total2 = reverseOption.Sum();
                VNDC  = (total1 + total2)/(2*O.S.Simulations);
                VP = discontfator*(total1 + total2)/(2*O.S.Simulations);
                List<double> varlistOne = new List<double>();
                for (int i = 0; i< O.S.Simulations; i++)
                {
                    var = Math.Pow(eachOption[i] - VNDC,2) + Math.Pow(reverseOption[i] - VNDC, 2);
                    varlistOne.Add(var);
                }
                std = Math.Sqrt(varlistOne.Sum() / (O.S.Simulations * 2));
                return (VP,std);
            }
            else
            {
                VP = discontfator*total1/O.S.Simulations;
                VNDC = total1/O.S.Simulations;
                List<double> varlistTwo = new List<double>();
                for (int i = 0; i < O.S.Simulations; i++)
                {
                    var = Math.Pow(eachOption[i]- VNDC, 2);
                    varlistTwo.Add(var);
                }
                std = Math.Sqrt(varlistTwo.Sum() / O.S.Simulations);
                return (VP,std);
            }    
        } 

        public double StandardError(Range O, bool IsAnti)
        {
            double SE = new double();
            Range O1 = new Range();
            SimulationParameters S1 = new SimulationParameters();
            S1.S0 = O.S.S0;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility ;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = IsAnti;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.S = S1;     

            double mu = OptionPandStd(O1).VP;
            double std = OptionPandStd(O1).std;
            if (O1.S.IsAntithetic == true)
            {
                SE = std/Math.Sqrt(O1.S.Simulations * 2);
            }
            else 
            {
                SE = std/Math.Sqrt(O1.S.Simulations);
            }
            return SE;
        }

        public (double Delta, double Gamma) DeltaandGamma(Range O)
        {
            double deltaS = 0.1;
            double Delta = new double();
            double Gamma = new double();
            Range O1 = new Range();
            Range O2 = new Range();
            SimulationParameters S1 = new SimulationParameters();
            SimulationParameters S2 = new SimulationParameters();

            S1.S0 = O.S.S0 + deltaS;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility ;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.S =S1;     

            S2.S0 = O.S.S0 - deltaS;
            S2.r = O.S.r;
            S2.Steps = O.S.Steps;
            S2.Simulations = O.S.Simulations;
            O2.Tenor = O.Tenor;
            S2.Volatility= O.S.Volatility ;
            S2.IsThread = O.S.IsThread;
            S2.IsAntithetic = O.S.IsAntithetic;
            S2.IsDeltaCV = O.S.IsDeltaCV;
            O2.S = S2;

            double VP1 = OptionPandStd(O1).VP;
            double VP2 = OptionPandStd(O2).VP;
            double VP = OptionPandStd(O).VP;
         


            Delta = (VP1 - VP2)/ (S1.S0 - S2.S0);
            Gamma = (VP1 + VP2 - 2*VP)/ Math.Pow(deltaS,2); 

            return(Delta, Gamma);
        }
        public double VegaValue(Range O)
        {
            double deltaSig = 0.001;
            double Vega = new double();
            Range O1 = new Range();
            Range O2 = new Range();
            SimulationParameters S1 = new SimulationParameters();
            SimulationParameters S2 = new SimulationParameters();

            S1.S0 = O.S.S0;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility + deltaSig;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.S = S1;


            S2.S0 = O.S.S0;
            S2.r = O.S.r;
            S2.Steps = O.S.Steps;
            S2.Simulations = O.S.Simulations;
            O2.Tenor = O.Tenor;
            S2.Volatility= O.S.Volatility - deltaSig;
            S2.IsThread = O.S.IsThread;
            S2.IsAntithetic = O.S.IsAntithetic;
            S2.IsDeltaCV = O.S.IsDeltaCV;
            O2.S = S2;

            double VP1 = OptionPandStd(O1).VP;
            double VP2 = OptionPandStd(O2).VP;
            Vega = (VP1 - VP2) / (2 * deltaSig);
            return Vega;
        }
        public double ThetaValue(Range O)
        {
            double deltaT = 0.01;
            double Theta = new double();
            
            Range O1 = new Range();
            SimulationParameters S1 = new SimulationParameters();


            S1.S0 = O.S.S0;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor  + deltaT;
            S1.Volatility= O.S.Volatility ;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.S = S1;

            double VP = OptionPandStd(O).VP;
            double VP1 = OptionPandStd(O1).VP;
            Theta = -(VP1 - VP)/ (deltaT);

            return Theta;
        }
        public double RhoValue(Range O)
        {
            double deltar = 0.0001;
            double Rho = new double();
            Range O1 = new Range();
            Range O2 = new Range();
            SimulationParameters S1 = new SimulationParameters();
            SimulationParameters S2 = new SimulationParameters();
            double VP = OptionPandStd(O).VP;


            S1.S0 = O.S.S0;
            S1.r = O.S.r + deltar;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.S = S1;

            S2.S0 = O.S.S0;
            S2.r = O.S.r - deltar;
            S2.Steps = O.S.Steps;
            S2.Simulations = O.S.Simulations;
            O2.Tenor = O.Tenor;
            S2.Volatility= O.S.Volatility;
            S2.IsThread = O.S.IsThread;
            S2.IsAntithetic = O.S.IsAntithetic;
            S2.IsDeltaCV = O.S.IsDeltaCV;
            O2.S = S2;

            double VP1 = OptionPandStd(O1).VP;
            double VP2 = OptionPandStd(O2).VP;
            Rho = (VP1 - VP)/( deltar);
            return Rho;

        }
    }
}