using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MyApp 
{
    public class AsianOption: Simulator
    {
        public double CND(double X)
        {
            double L = 0.0;
            double K = 0.0;
            double dCND = 0.0;
            const double a1 = 0.31938153; 
            const double a2 = -0.356563782; 
            const double a3 = 1.781477937;
            const double a4 = -1.821255978;
            const double a5 = 1.330274429;
            L = Math.Abs(X);
            K = 1.0 / (1.0 + 0.2316419 * L);
            dCND = 1.0 - 1.0 / Math.Sqrt(2 * Convert.ToDouble(Math.PI.ToString())) * 
                Math.Exp(-L * L / 2.0) * (a1 * K + a2 * K  * K + a3 * Math.Pow(K, 3.0) + 
                a4 * Math.Pow(K, 4.0) + a5 * Math.Pow(K, 5.0));
            
            if (X < 0) 
            {
                return 1.0 - dCND;
            }
            else
            {
                return dCND;
            }
        }

        public double BS_delta(Aisan O, double j, double S)
        {
                double d1 = 0.0;
                double delta_bs = 0.0;
                double dt = O.Tenor/O.S.Steps;
                d1 = (Math.Log(S/ O.Strike) + ((O.S.r + Math.Pow(O.S.Volatility,2)/2)* (O.Tenor - j*dt))/(O.S.Volatility*Math.Sqrt(O.Tenor - j*dt)));
                if (O.IsCall == true)
                {
                    delta_bs = CND(d1);
                }
                else if (O.IsCall == false)
                {
                    delta_bs = CND(d1) - 1;
                }
                return delta_bs;
        }

 
        public (double VP, double std) OptionPandStd(Aisan O)
        {
            
            double VP= new double();
            double VNDC = new double();
            double std = new double();
            double var = new double();
            double total1 = new double();
            double total2 = new double();
            double discontfator = new double();
            double beta1 = -1;
            double dt = O.Tenor/O.S.Steps;
            double erdt = Math.Exp(O.S.r* dt);
            discontfator = Math.Exp(-O.S.r* O.Tenor);
            
            SimulationResult SpathNorm = StockPathGenerator.GeneratePaths(O);
            SimulationResult SpathAnti = StockPathGenerator.AntetheticPaths(O);
            double[]SMeanNorm = new double [O.S.Simulations];
            double[]SMeanAnti = new double [O.S.Simulations];


            for (int i = 0; i < O.S.Simulations; i++)
            {
                double sum_Norm = 0;
                double sum_Anti = 0;
                for (int j = 0; j < O.S.Steps ; j++)
                {
                    sum_Norm += SpathNorm.SimulatedPaths[i, j];
                    sum_Anti += SpathAnti.SimulatedPaths[i,j];
                }
                SMeanNorm[i] = sum_Norm / (double)(O.S.Steps);
                SMeanAnti[i] = sum_Anti /(double) (O.S.Steps);
            }


            List<double> eachOption = new List<double>();
            List<double> reverseOption = new List<double>();
            GaussianRandoms Nrands = new GaussianRandoms();
            double [, ]eps = Nrands.PopulatedNrands(O.S.Simulations,O.S.Steps);;
            

            if (O.S.IsDeltaCV == true)
            {   
                if (O.S.IsAntithetic == false)
                {
                    double sum_VT = 0;
                    double sum_VT2 = 0;
                    for (int i=0; i<O.S.Simulations; i++)
                    {   
                        
                        if (O.IsCall == true)
                        {   
                            double CV = 0;
                            double St = O.S.S0;
                            double CT = 0;
                            double sum_Norm_CV = 0;
                            double SMeanNorm_CV = new double();
                            for (int j=0; j<O.S.Steps; j++)
                            {                        

                                double dlt = BS_delta(O,j,St);
                                CV = CV + dlt*(SpathNorm.SimulatedPaths[i,j] - St*erdt);
                                sum_Norm_CV += SpathNorm.SimulatedPaths[i, j];
                                St = SpathNorm.SimulatedPaths[i,j];  
                            }
                            SMeanNorm_CV = sum_Norm_CV/(double)(O.S.Steps);

                            CT = Math.Max(0, SMeanNorm_CV - O.Strike) + beta1*CV;
                            sum_VT = sum_VT + CT;
                            sum_VT2 = sum_VT2 + CT*CT;
                        }
                        else if (O.IsCall == false)
                        {
                            double PV = 0;
                            double St = O.S.S0;
                            double PT = 0;
                            double sum_Norm_CV = 0;
                            double SMeanNorm_CV = new double();
                            for (int j=0; j<O.S.Steps; j++)
                            {                        
                                double dlt = BS_delta(O, j, St);
                                PV = PV + dlt*(SpathNorm.SimulatedPaths[i,j] - St*erdt);
                                sum_Norm_CV += SpathNorm.SimulatedPaths[i, j];
                                St = SpathNorm.SimulatedPaths[i,j];    
                            }
                            SMeanNorm_CV = sum_Norm_CV/(double)(O.S.Steps);
                            PT = Math.Max(0,  O.Strike - SMeanNorm_CV) + beta1*PV;
                            sum_VT = sum_VT + PT;
                            sum_VT2 = sum_VT2 + PT*PT;
                        }
                    }
                    VP = discontfator*sum_VT/O.S.Simulations;
                    std = Math.Sqrt(( (sum_VT2)  - Math.Pow((sum_VT),2)/ (O.S.Simulations))* Math.Exp(-2*O.S.r*O.Tenor) / (O.S.Simulations -1));
                    return (VP,std);
                }    
                if (O.S.IsAntithetic == true)
                {
                    double sum_VT = 0;
                    double sum_VT2 = 0;
                    double nudt = (O.S.r - 0.5*Math.Pow(O.S.Volatility,2))*dt;
                    double sigsdt = O.S.Volatility*Math.Sqrt(dt);
                    

                    for (int i=0; i<O.S.Simulations; i++)
                    {   
                        
                        if (O.IsCall == true)
                        {   
                            double CV = 0;
                            double CV_2 = 0;
                            double St = O.S.S0;
                            double St_2 = O.S.S0;
                            double CT = 0;
                            double sum_Norm_CV = 0;
                            double SMeanNorm_CV = new double();
                            double sum_Anti_CV = 0;
                            double SMeanAnti_CV = new double();

                            for (int j=0; j<O.S.Steps; j++)
                            {   
                                
                                double dlt = BS_delta(O,j, St);
                                double dlt_2 = BS_delta(O, j, St_2);
                                CV = CV + dlt*( SpathNorm.SimulatedPaths[i,j] -St*erdt);
                                CV_2 = CV_2 + dlt_2*(SpathAnti.SimulatedPaths[i,j] - St_2*erdt);
                                sum_Norm_CV += SpathNorm.SimulatedPaths[i, j];
                                sum_Anti_CV += SpathAnti.SimulatedPaths[i, j];
                                St = SpathNorm.SimulatedPaths[i,j];
                                St_2 = SpathAnti.SimulatedPaths[i,j];     
                            }
                            SMeanNorm_CV = sum_Norm_CV/(double)(O.S.Steps);
                            SMeanAnti_CV = sum_Anti_CV/(double)(O.S.Steps);


                            CT = 0.5* (Math.Max(0, SMeanNorm_CV - O.Strike) + beta1*CV + Math.Max(0, SMeanAnti_CV - O.Strike) + beta1*CV_2);
                            sum_VT = sum_VT + CT;
                            sum_VT2 = sum_VT2 + CT*CT;
                        }
                        else if (O.IsCall == false)
                        {
                            double PV = 0;
                            double PV_2 = 0;
                            double St = O.S.S0;
                            double St_2 = O.S.S0;
                            double PT = 0;
                            double sum_Norm_CV = 0;
                            double SMeanNorm_CV = new double();
                            double sum_Anti_CV = 0;
                            double SMeanAnti_CV = new double();
                            for (int j=0; j<O.S.Steps; j++)

                            {  
                
                                double dlt = BS_delta(O,j, St);
                                double dlt_2 = BS_delta(O, j, St_2);
                            
                                PV = PV + dlt*( SpathNorm.SimulatedPaths[i,j] -St*erdt);
                                PV_2 = PV_2 + dlt_2*(SpathAnti.SimulatedPaths[i,j] - St_2*erdt);
                                St = SpathNorm.SimulatedPaths[i,j];
                                St_2 = SpathAnti.SimulatedPaths[i,j];    
                                sum_Norm_CV += SpathNorm.SimulatedPaths[i, j];
                                sum_Anti_CV += SpathAnti.SimulatedPaths[i, j];
                                St = SpathNorm.SimulatedPaths[i,j];
                                St_2 = SpathAnti.SimulatedPaths[i,j];     
                            }
                            SMeanNorm_CV = sum_Norm_CV/(double)(O.S.Steps);
                            SMeanAnti_CV = sum_Anti_CV/(double)(O.S.Steps);
                            PT = 0.5* (Math.Max(0, O.Strike - SMeanNorm_CV) + beta1*PV + Math.Max(0,  O.Strike - SMeanAnti_CV) + beta1*PV_2);
                            sum_VT = sum_VT + PT;
                            sum_VT2 = sum_VT2 + PT * PT;
                        }
                        
                    }
                    VP = discontfator*sum_VT/O.S.Simulations;
                    std = Math.Sqrt(((sum_VT2)  - Math.Pow((sum_VT),2)/ (O.S.Simulations))* Math.Exp(-2*O.S.r*O.Tenor) / (O.S.Simulations -1));
                    return (VP,std);
                } 
                
            }


            else if (O.S.IsDeltaCV == false)
            {
                for (int i = 0; i < O.S.Simulations; i++)
                {
                    if (O.IsCall == true)
                    {   
                        eachOption.Add(Math.Max(SMeanNorm[i]-O.Strike, 0));
                    }
                
                    else if (O.IsCall == false)
                    {
                        eachOption.Add(Math.Max(O.Strike - SMeanNorm[i], 0));
                    }

                } 
                total1 = eachOption.Sum();
                if (O.S.IsAntithetic == true)
                {
                    for (int i = 0; i < O.S.Simulations; i++)
                    {
                        if (O.IsCall == true)
                        {
                            reverseOption.Add(Math.Max(SMeanAnti[i]-O.Strike, 0));
                        }
                
                        else if (O.IsCall == false)
                        {   
                            reverseOption.Add(Math.Max(O.Strike - SMeanAnti[i], 0));
                        } 
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
            return (VP,std);   
        } 


        public double StandardError(Aisan O, bool IsAnti)
        {
            double SE = new double();
            Aisan O1 = new Aisan();
            SimulationParameters S1 = new SimulationParameters();

            S1.S0 = O.S.S0;
            O1.Strike = O.Strike;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility ;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = IsAnti;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.IsCall = O.IsCall;
            O1.S =S1;   

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

        public (double Delta, double Gamma) DeltaandGamma(Aisan O)
        {
            double deltaS = 0.1;
            double Delta = new double();
            double Gamma = new double();
            Aisan O1 = new Aisan();
            Aisan O2 = new Aisan();
            SimulationParameters S1 = new SimulationParameters();
            SimulationParameters S2 = new SimulationParameters();

            S1.S0 = O.S.S0 + deltaS;
            O1.Strike = O.Strike;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility ;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.IsCall = O.IsCall;
            O1.S =S1;     

            S2.S0 = O.S.S0 - deltaS;
            O2.Strike = O.Strike;
            S2.r = O.S.r;
            S2.Steps = O.S.Steps;
            S2.Simulations = O.S.Simulations;
            O2.Tenor = O.Tenor;
            S2.Volatility= O.S.Volatility ;
            S2.IsThread = O.S.IsThread;
            S2.IsAntithetic = O.S.IsAntithetic;
            S2.IsDeltaCV = O.S.IsDeltaCV;
            O2.IsCall = O.IsCall;
            O2.S = S2;

            double VP1 = OptionPandStd(O1).VP;
            double VP2 = OptionPandStd(O2).VP;
            double VP = OptionPandStd(O).VP;
         


            Delta = (VP1 - VP2)/ (S1.S0 - S2.S0);
            Gamma = (VP1 + VP2 - 2*VP)/ Math.Pow(deltaS,2); 

            return(Delta, Gamma);
        }
        public double VegaValue(Aisan O)
        {
            double deltaSig = 0.001;
            double Vega = new double();
            Aisan O1 = new Aisan();
            Aisan O2 = new Aisan();
            SimulationParameters S1 = new SimulationParameters();
            SimulationParameters S2 = new SimulationParameters();

            S1.S0 = O.S.S0;
            O1.Strike = O.Strike;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility + deltaSig;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.IsCall = O.IsCall;
            O1.S = S1;


            S2.S0 = O.S.S0;
            O2.Strike = O.Strike;
            S2.r = O.S.r;
            S2.Steps = O.S.Steps;
            S2.Simulations = O.S.Simulations;
            O2.Tenor = O.Tenor;
            S2.Volatility= O.S.Volatility - deltaSig;
            S2.IsThread = O.S.IsThread;
            S2.IsAntithetic = O.S.IsAntithetic;
            S2.IsDeltaCV = O.S.IsDeltaCV;
            O2.IsCall = O.IsCall;
            O2.S = S2;

            double VP1 = OptionPandStd(O1).VP;
            double VP2 = OptionPandStd(O2).VP;
            Vega = (VP1 - VP2) / (2 * deltaSig);
            return Vega;
        }
        public double ThetaValue(Aisan O)
        {
            double deltaT = 0.01;
            double Theta = new double();
            
            Aisan O1 = new Aisan();
            SimulationParameters S1 = new SimulationParameters();


            S1.S0 = O.S.S0;
            O1.Strike = O.Strike;
            S1.r = O.S.r;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor  + deltaT;
            S1.Volatility= O.S.Volatility ;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.IsCall = O.IsCall;
            O1.S = S1;

            double VP = OptionPandStd(O).VP;
            double VP1 = OptionPandStd(O1).VP;
            Theta = -(VP1 - VP)/ (deltaT);

            return Theta;
        }
        public double RhoValue(Aisan O)
        {
            double deltar = 0.0001;
            double Rho = new double();
            Aisan O1 = new Aisan();
            Aisan O2 = new Aisan();
            SimulationParameters S1 = new SimulationParameters();
            SimulationParameters S2 = new SimulationParameters();
            double VP = OptionPandStd(O).VP;


            S1.S0 = O.S.S0;
            O1.Strike = O.Strike;
            S1.r = O.S.r + deltar;
            S1.Steps = O.S.Steps;
            S1.Simulations = O.S.Simulations;
            O1.Tenor = O.Tenor;
            S1.Volatility= O.S.Volatility;
            S1.IsThread = O.S.IsThread;
            S1.IsAntithetic = O.S.IsAntithetic;
            S1.IsDeltaCV = O.S.IsDeltaCV;
            O1.IsCall = O.IsCall;
            O1.S = S1;

            S2.S0 = O.S.S0;
            O2.Strike = O.Strike;
            S2.r = O.S.r - deltar;
            S2.Steps = O.S.Steps;
            S2.Simulations = O.S.Simulations;
            O2.Tenor = O.Tenor;
            S2.Volatility= O.S.Volatility;
            S2.IsThread = O.S.IsThread;
            S2.IsAntithetic = O.S.IsAntithetic;
            S2.IsDeltaCV = O.S.IsDeltaCV;
            O2.IsCall = O.IsCall;
            O2.S = S2;

            double VP1 = OptionPandStd(O1).VP;
            double VP2 = OptionPandStd(O2).VP;
            Rho = (VP1 - VP)/( deltar);
            return Rho;

        }



    }
}