﻿﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SimulationParameters p = new SimulationParameters();
            MontecarloSimimulator euro = new MontecarloSimimulator();
            int CPUnum = System.Environment.ProcessorCount;
            int SimulationMultiplier = 1;
            Console.Write("What is the Underlying Price: ");
            p.S0 = Convert.ToDouble(Console.ReadLine());
            Console.Write("What is the Strike Price: ");
            p.Strike = Convert.ToDouble(Console.ReadLine());
            Console.Write("How much years the Option expired: ");
            p.Tenor = Convert.ToDouble(Console.ReadLine());
            Console.Write("What is the risk free rate: ");
            p.r = Convert.ToDouble(Console.ReadLine());
            Console.Write("What is the Volatility: ");
            p.Volatility = Convert.ToDouble(Console.ReadLine());
            Console.Write("Is Call? (true or false) ");
            euro.IsCall = Convert.ToBoolean(Console.ReadLine());
            Console.Write("Do you want Antithetic method for montecarlo? (true or false) ");
            euro.IsAntithetic = Convert.ToBoolean(Console.ReadLine());
            Console.Write("DO you want delta delta-based control variate? (true or false) ");
            euro.IsDeltaCV = Convert.ToBoolean(Console.ReadLine());
            Console.Write("Do you want multithreading function to calculate S paths? (true or flase)");
            euro.IsThread = Convert.ToBoolean(Console.ReadLine());
            if (euro.IsThread == true)
            {
                Console.WriteLine();
                Console.WriteLine("NOTICE!!!");
                Console.WriteLine("The Simulation number will be multiplied by the number of CPU core since you choose the multithread function");
                SimulationMultiplier = SimulationMultiplier * CPUnum;
                Console.WriteLine();
            }
            Console.Write("How many simulations you want ");
            p.Simulations = Convert.ToInt32(Console.ReadLine())*SimulationMultiplier;
            Console.Write("How many steps you want ");
            p.Steps = Convert.ToInt32(Console.ReadLine());
            if (euro.IsAntithetic == true)
            {
                OptionResult AntitheticResult = new OptionResult();
                AntitheticResult.Price = euro.OptionPandStd(p, euro.IsCall, euro.IsAntithetic, euro.IsDeltaCV).VP;
                (AntitheticResult.Delta, AntitheticResult.Gamma) = euro.DeltaandGamma(p, euro.IsCall, euro.IsAntithetic);
                AntitheticResult.Theta = euro.ThetaValue(p, euro.IsCall, euro.IsAntithetic);
                AntitheticResult.Vega = euro.VegaValue(p, euro.IsCall, euro.IsAntithetic);
                AntitheticResult.Rho = euro.RhoValue(p, euro.IsCall, euro.IsAntithetic);
                AntitheticResult.StdErrorNorm = euro.StandardError(p, euro.IsCall, false);
                AntitheticResult.StdErrorReduce = euro.StandardError(p, euro.IsCall, true); 
                Console.WriteLine("Option Price: ");
                Console.WriteLine(AntitheticResult.Price);
                Console.WriteLine("Option Delta: ");
                Console.WriteLine(AntitheticResult.Delta);
                Console.WriteLine("Option Gamma: ");
                Console.WriteLine(AntitheticResult.Gamma);
                Console.WriteLine("Option Vega: ");
                Console.WriteLine(AntitheticResult.Vega);
                Console.WriteLine("Option Theta: ");
                Console.WriteLine(AntitheticResult.Theta);
                Console.WriteLine("Option Rho: ");
                Console.WriteLine(AntitheticResult.Rho);
                Console.WriteLine("Regular Standard Error");
                Console.WriteLine(AntitheticResult.StdErrorNorm);
                Console.WriteLine("Antithetic Standard Error");
                Console.WriteLine(AntitheticResult.StdErrorReduce);
            }
            else
            {
                OptionResult Result = new OptionResult();
                Result.Price = euro.OptionPandStd(p, euro.IsCall, euro.IsAntithetic, euro.IsDeltaCV).VP;
                (Result.Delta, Result.Gamma) = euro.DeltaandGamma(p, euro.IsCall, euro.IsAntithetic);
                Result.Theta = euro.ThetaValue(p, euro.IsCall, euro.IsAntithetic);
                Result.Vega = euro.VegaValue(p, euro.IsCall, euro.IsAntithetic);
                Result.Rho = euro.RhoValue(p, euro.IsCall, euro.IsAntithetic);
                Result.StdErrorNorm = euro.StandardError(p, euro.IsCall, false);
                Console.WriteLine("Option Price: ");
                Console.WriteLine(Result.Price);
                Console.WriteLine("Option Delta: ");
                Console.WriteLine(Result.Delta);
                Console.WriteLine("Option Gamma: ");
                Console.WriteLine(Result.Gamma);
                Console.WriteLine("Option Vega: ");
                Console.WriteLine(Result.Vega);
                Console.WriteLine("Option Theta: ");
                Console.WriteLine(Result.Theta);
                Console.WriteLine("Option Rho: ");
                Console.WriteLine(Result.Rho);
                Console.WriteLine("Regular Standard Error");
                Console.WriteLine(Result.StdErrorNorm);
            }

            
        }
    
        class SimulationParameters
        {
            public double S0{get; set;}
            public double r{get; set;}
            public int Steps{get; set;}
            public int Simulations{get; set;}
            public double Tenor {get; set;}
            public double Volatility{get; set;}

            public double Strike{get; set;}
            public int Start{get; set;}
            public int End{get; set;}
        }
        class ThreadingSimParam
        {
            public int Start{get; set;}
            public int End{get; set;}
        }
        class OptionResult 
        {
            public double Price {get; set;}
            public double Delta{get; set;}
            public double Theta{get; set;}
            public double Gamma{get; set;}
            public double Vega{get; set;}
            public double Rho{get; set;}
            public double StdErrorNorm{get; set;}
            public double StdErrorReduce{get; set;}
        }
        class GaussianRandoms
        {
            
            public double [,] PopulatedNrands(int rows, int cols)
            {   
                double [,] NRands = new double [rows, cols];
                Random r_1 = new Random(8);
                Random r_2 = new Random(11);
                for(int i = 0; i <rows; i++)
                {
                    for (int j = 0; j< cols; j++)
                    {
                        double x1 = r_1.NextDouble();
                        double x2 = r_2.NextDouble();
                        double z1 = Math.Sqrt(-2 * Math.Log(x1))*Math.Cos(-2*Math.PI*x2);
                        double z2 = Math.Sqrt(-2 * Math.Log(x1))*Math.Sin(-2*Math.PI*x2);
                        NRands[i,j] = z1;
                    }

                }
                return NRands;
            }

            public double[,] ReverseNrands(int rows, int cols)
            {
                double [, ]NRands = PopulatedNrands(rows, cols);
                double [, ] NegNrands = new double [rows, cols];
                for(int i = 0; i <rows; i++)
                {
                    for (int j = 0; j< cols; j++)
                    {
                        NegNrands[i,j] = NRands[i,j] *-1;
                    }

                }
                return NegNrands;
                
            }
        }

        class SimulationResult
        {
            public double[,] SimulatedPaths {get; set;}
        }

    
        class MontecarloSimimulator
        {   
            public bool IsCall {get; set;}
            public bool IsAntithetic {get; set;}
            public bool IsDeltaCV {get; set;}
            public bool IsThread {get; set;}
            public static SimulationResult GeneratePaths(SimulationParameters p ,bool IsThread)
            {
                GaussianRandoms Nrands = new GaussianRandoms();
                double [, ]R1 = Nrands.PopulatedNrands(p.Simulations,p.Steps);
                
                SimulationResult results = new SimulationResult();
                results.SimulatedPaths = new double [p.Simulations,p.Steps];
                p.Start = 0;
                p.End = p.Simulations;
                if (IsThread == true)
                {   
                    threadinit ti = new threadinit();
                    ti.paramthread = p;
                    ti.resultthread = results;
                    ti.R1thread = R1;
                    Thread [] threads = new Thread [System.Environment.ProcessorCount];
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {   
                        threads[i] = new Thread(new ThreadStart(ti.run));
                        threads[i].Start(/*new ThreadingSimParam() {Start = 0 + i*(p.Simulations/System.Environment.ProcessorCount), End = i*(p.Simulations/System.Environment.ProcessorCount) - 1}*/);
                    }
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {
                        threads[i].Join();
                    }
                    
                }
                else
                {               
                    populatevalues(p, results, R1);
                }
                
                return results;
            }

            public static SimulationResult AntetheticPaths(SimulationParameters p,bool IsThread)
            {
                GaussianRandoms Nrands2 = new GaussianRandoms();
                double [,] R2 = Nrands2.ReverseNrands(p.Simulations,p.Steps);
                SimulationResult reverseresult = new SimulationResult();
                reverseresult.SimulatedPaths = new double [p.Simulations, p.Steps];
                p.Start = 0;
                p.End = p.Simulations;
                if (IsThread == true)
                {   
                    threadinit ti = new threadinit();
            
                    ti.resultthread = reverseresult;
                    ti.R1thread = R2;
                    ti.paramthread = p;

                    Thread [] threads = new Thread [System.Environment.ProcessorCount];
                    
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {   
                        
                        threads[i] = new Thread(new ThreadStart(ti.run));
    
                        threads[i].Start(/*new SimulationParameters {Start = 0 + i*(p.Simulations/System.Environment.ProcessorCount), End = i*(p.Simulations/System.Environment.ProcessorCount) - 1}*/);
                    }
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {
                        threads[i].Join();
                    }
                    
                }
                else
                {
                    populatevalues(p, reverseresult, R2);
                }
                return reverseresult;
            }

            public static void populatevalues(SimulationParameters p, SimulationResult result, double [,] R1 )
            {   
                double dt = p.Tenor/p.Steps;

                for (int i = p.Start; i < p.End; i++)
                {   
                    result.SimulatedPaths[i,0] = p.S0;
                    for (int j = 1; j < p.Steps; j++)
                    {
                        result.SimulatedPaths[i,j] = result.SimulatedPaths[i, j-1] * Math.Exp(((p.r - 0.5 * Math.Pow(p.Volatility,2))* dt) + (p.Volatility * Math.Sqrt(dt)*R1[i,j]));
                    }
                }
            } 

            public class threadinit
            {   
                public SimulationResult resultthread;
                public double [,] R1thread;
                public SimulationParameters paramthread;
                public void run()
                {
                    populatevalues(paramthread, resultthread, R1thread);
                }
            }

// Abromowitz and Stegun approximation
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

            public double BS_delta(SimulationParameters p, bool IsCall, double j, double S)
            {
                    double d1 = 0.0;
                    double delta_bs = 0.0;
                    double dt = p.Tenor/p.Steps;
                    d1 = (Math.Log(S/ p.Strike) + ((p.r + Math.Pow(p.Volatility,2)/2)* (p.Tenor - j*dt))/(p.Volatility*Math.Sqrt(p.Tenor - j*dt)));
                    if (IsCall == true)
                    {
                        delta_bs = CND(d1);
                    }
                    else if (IsCall == false)
                    {
                        delta_bs = CND(d1) - 1;
                    }
                    return delta_bs;
            }

            public (double VP, double std) OptionPandStd(SimulationParameters p, bool IsCall, bool IsAntithetic, bool IsDeltaCV)
            {
                double VP= new double();
                double VNDC = new double();
                double std = new double();
                double var = new double();
                double total1 = new double();
                double total2 = new double();
                double discontfator = new double();
                double beta1 = -1;
                double dt = p.Tenor/p.Steps;
                double erdt = Math.Exp(p.r*dt);
                discontfator = Math.Exp(-p.r*p.Tenor);
                
                SimulationResult SpathNorm = GeneratePaths(p, IsThread);
                SimulationResult SpathAnti = AntetheticPaths(p, IsThread);




                List<double> eachOption = new List<double>();
                List<double> reverseOption = new List<double>();
                GaussianRandoms Nrands = new GaussianRandoms();
                double [, ]eps = Nrands.PopulatedNrands(p.Simulations,p.Steps);;
                

                if (IsDeltaCV == true)
                {   
                    if (IsAntithetic == false)
                    {
                        double sum_VT = 0;
                        double sum_VT2 = 0;
                        
                        for (int i=0; i<p.Simulations; i++)
                        {   
                            
                            if (IsCall == true)
                            {   
                                double CV = 0;
                                double St = p.S0;
                                double CT = 0;
                                for (int j=0; j<p.Steps; j++)
                                {                        
                                    double dlt = BS_delta(p, IsCall, j, St);
                                    CV = CV + dlt*(SpathNorm.SimulatedPaths[i,j] - St*erdt);
                                    St = SpathNorm.SimulatedPaths[i,j];    
                                }
                                CT = Math.Max(0, St - p.Strike) - beta1*CV;
                                sum_VT = sum_VT + CT;
                                sum_VT2 = sum_VT2 + CT*CT;
                            }
                            else if (IsCall == false)
                            {
                                double PV = 0;
                                double St = p.S0;
                                double PT = 0;
                                for (int j=0; j<p.Steps; j++)
                                {                        
                                    double dlt = BS_delta(p, IsCall, j, St);
                                    PV = PV + dlt*(SpathNorm.SimulatedPaths[i,j] - St*erdt);
                                    St = SpathNorm.SimulatedPaths[i,j];    
                                }
                                PT = Math.Max(0,  p.Strike - St) - beta1*PV;
                                sum_VT = sum_VT + PT;
                                sum_VT2 = sum_VT2 + PT*PT;
                            }
                        }
                        VP = discontfator*sum_VT/p.Simulations;
                        std = Math.Sqrt(( (sum_VT2)  - Math.Pow((sum_VT),2)/ (p.Simulations))* Math.Exp(-2*p.r*p.Tenor) / (p.Simulations -1));
                        return (VP,std);
                    }    
                    if (IsAntithetic == true)
                    {
                        double sum_VT = 0;
                        double sum_VT2 = 0;
                        double nudt = (p.r - 0.5*Math.Pow(p.Volatility,2))*dt;
                        double sigsdt = p.Volatility*Math.Sqrt(dt);
                        

                        for (int i=0; i<p.Simulations; i++)
                        {   
                            
                            if (IsCall == true)
                            {   
                                double CV = 0;
                                double CV_2 = 0;
                                double St = p.S0;
                                double St_2 = p.S0;
                                double CT = 0;
                                for (int j=0; j<p.Steps; j++)
                                {   
                           
                                    double dlt = BS_delta(p, IsCall, j, St);
                                    double dlt_2 = BS_delta(p, IsCall, j, St_2);
                                    
                                    double Stn = St*Math.Exp(nudt + sigsdt* eps[i,j]);
                                    double Stn2 = St*Math.Exp(nudt + sigsdt* (-1)*eps[i,j]);
                                    CV = CV + dlt*(Stn -St*erdt);
                                    CV_2 = CV_2 + dlt_2*(Stn2 - St_2*erdt);
                                    St = Stn;
                                    St_2 = Stn2;    
                                }
                                CT = 0.5* (Math.Max(0, St - p.Strike) + beta1*CV + Math.Max(0, St_2 - p.Strike) + beta1*CV_2);
                                sum_VT = sum_VT + CT;
                                sum_VT2 = sum_VT2 + CT*CT;
                            }
                            else if (IsCall == false)
                            {
                                double PV = 0;
                                double PV_2 = 0;
                                double St = p.S0;
                                double St_2 = p.S0;
                                double PT = 0;
                                for (int j=0; j<p.Steps; j++)

                                {   
                   
                                    double dlt = BS_delta(p, IsCall, j,St);
                                    double dlt_2 = BS_delta(p, IsCall, j, St_2);

                                    double Stn = St*Math.Exp(nudt + sigsdt* eps[i,j]);
                                    double Stn2 = St*Math.Exp(nudt + sigsdt*(-1)*eps[i,j]);
                                    PV = PV + dlt*(Stn -St*erdt);
                                    PV_2 = PV_2 + dlt_2*(Stn2 - St_2*erdt);
                                    St = Stn;
                                    St_2 = Stn2; 
                                }
                                PT = 0.5* (Math.Max(0, p.Strike - St) + beta1*PV + Math.Max(0,  p.Strike - St_2) + beta1*PV_2);
                                sum_VT = sum_VT + PT;
                                sum_VT2 = sum_VT2 + PT*PT;
                            }
                            
                        }
                        VP = discontfator*sum_VT/p.Simulations;
                        std = Math.Sqrt(((sum_VT2)  - Math.Pow((sum_VT),2)/ (p.Simulations))* Math.Exp(-2*p.r*p.Tenor) / (p.Simulations -1));
                        return (VP,std);
                    } 
                    
                }


                else if (IsDeltaCV == false)
                {
                    for (int i = 0; i < p.Simulations; i++)
                    {
                        if (IsCall == true)
                        {   
                            eachOption.Add(Math.Max(SpathNorm.SimulatedPaths[i, p.Steps - 1]-p.Strike, 0));
                        }
                    
                        else if (IsCall == false)
                        {
                            eachOption.Add(Math.Max(p.Strike - SpathNorm.SimulatedPaths[i, p.Steps - 1], 0));
                        }

                    } 
                    total1 = eachOption.Sum();
                    if (IsAntithetic == true)
                    {
                        for (int i = 0; i < p.Simulations; i++)
                        {
                            if (IsCall == true)
                            {
                                reverseOption.Add(Math.Max(SpathAnti.SimulatedPaths[i, p.Steps - 1]-p.Strike, 0));
                            }
                    
                            else if (IsCall == false)
                            {   
                                reverseOption.Add(Math.Max(p.Strike - SpathAnti.SimulatedPaths[i, p.Steps - 1], 0));
                            } 
                        }
                        total2 = reverseOption.Sum();
                        VNDC  = (total1 + total2)/(2*p.Simulations);
                        VP = discontfator*(total1 + total2)/(2*p.Simulations);
                        List<double> varlistOne = new List<double>();
                        for (int i = 0; i< p.Simulations; i++)
                        {
                            var = Math.Pow(eachOption[i] - VNDC,2) + Math.Pow(reverseOption[i] - VNDC, 2);
                            varlistOne.Add(var);
                        }
                        std = Math.Sqrt(varlistOne.Sum() / (p.Simulations * 2));
                        return (VP,std);
                    }
                    else
                    {
                        VP = discontfator*total1/p.Simulations;
                        VNDC = total1/p.Simulations;
                        List<double> varlistTwo = new List<double>();
                        for (int i = 0; i < p.Simulations; i++)
                        {
                            var = Math.Pow(eachOption[i]- VNDC, 2);
                            varlistTwo.Add(var);
                        }
                        std = Math.Sqrt(varlistTwo.Sum() / p.Simulations);
                        return (VP,std);
                    }    
                }  
                return (VP,std);   
            } 
            public double StandardError(SimulationParameters p, bool IsCall, bool IsAntithetic)
            {
                double SE = new double();
                double mu = OptionPandStd(p, IsCall, IsAntithetic, IsDeltaCV).VP;
                double std = OptionPandStd(p, IsCall, IsAntithetic, IsDeltaCV).std;
                if (IsAntithetic == true)
                {
                    SE = std/Math.Sqrt(p.Simulations * 2);
                }
                else 
                {
                    SE = std/Math.Sqrt(p.Simulations);
                }
                return SE;
            }

            public (double Delta, double Gamma) DeltaandGamma(SimulationParameters p, bool IsCall, bool IsAntithetic)
            {
                double deltaS = 0.1;
                double Delta = new double();
                double Gamma = new double();
                SimulationParameters p1 = new SimulationParameters();
                SimulationParameters p2 = new SimulationParameters();

                p1.S0 = p.S0 + deltaS;
                p1.Strike = p.Strike;
                p1.r = p.r;
                p1.Steps = p.Steps;
                p1.Simulations = p.Simulations;
                p1.Tenor = p.Tenor;
                p1.Volatility= p.Volatility ;

                p2.S0 = p.S0 - deltaS;
                p2.Strike  = p.Strike;
                p2.r = p.r;
                p2.Steps = p.Steps;
                p2.Simulations = p.Simulations;
                p2.Tenor = p.Tenor;
                p2.Volatility = p.Volatility;

                double VP1 = OptionPandStd(p1, IsCall, IsAntithetic, IsDeltaCV).VP;
                double VP2 = OptionPandStd(p2, IsCall, IsAntithetic, IsDeltaCV).VP;
                double VP = OptionPandStd(p, IsCall, IsAntithetic,IsDeltaCV).VP;

                Delta = (VP1 - VP2)/ ((p1.S0 - p2.S0));
                Gamma = (VP1 + VP2 - 2*VP)/ Math.Pow(deltaS,2); 
                             p2.Steps = p.Steps;

                return(Delta, Gamma);
            }
            public double VegaValue(SimulationParameters p, bool IsCall, bool IsAntithetic)
            {
                double deltaSig = 0.001;
                double Vega = new double();
                SimulationParameters p1 = new SimulationParameters();
                SimulationParameters p2 = new SimulationParameters();
                
                p1.S0 = p.S0;
                p1.Strike = p.Strike;
                p1.r = p.r;
                p1.Steps = p.Steps;
                p1.Simulations = p.Simulations;
                p1.Tenor = p.Tenor;
                p1.Volatility= p.Volatility + deltaSig;
                p2.S0 = p.S0;
                p2.Strike = p.Strike;
                p2.r = p.r;
                p2.Steps = p.Steps;
                p2.Simulations = p.Simulations;
                p2.Tenor = p.Tenor;
                p2.Volatility = p.Volatility - deltaSig;
                double VP1 = OptionPandStd(p1, IsCall, IsAntithetic, IsDeltaCV).VP;
                double VP2 = OptionPandStd(p2, IsCall, IsAntithetic, IsDeltaCV).VP;
                Vega = (VP1 - VP2) / (2 * deltaSig);
                return Vega;
            }
            public double ThetaValue(SimulationParameters p, bool IsCall, bool IsAntithetic)
            {
                double deltaT = 0.01;
                double Theta = new double();
                double VP = OptionPandStd(p, IsCall, IsAntithetic, IsDeltaCV).VP;
                SimulationParameters p1 = new SimulationParameters();
                p1.S0 = p.S0;
                p1.Strike = p.Strike;
                p1.r = p.r;
                p1.Steps = p.Steps;
                p1.Simulations = p.Simulations;
                p1.Volatility= p.Volatility;
                p1.Tenor = p.Tenor + deltaT;
                double VP1 = OptionPandStd(p1, IsCall, IsAntithetic, IsDeltaCV).VP;
                Theta = -(VP1 - VP)/ (deltaT);
                return Theta;
            }
            public double RhoValue(SimulationParameters p, bool IsCall, bool IsAntithetic)
            {
                double deltar = 0.0001;
                double Rho = new double();
                SimulationParameters p1 = new SimulationParameters();
                SimulationParameters p2 = new SimulationParameters();
                double VP = OptionPandStd(p, IsCall, IsAntithetic, IsDeltaCV).VP;

                
                p1.S0 = p.S0;
                p1.Strike = p.Strike;
                p1.r= p.r + deltar;
                p1.Steps = p.Steps;
                p1.Simulations = p.Simulations;
                p1.Tenor = p.Tenor;
                p1.Volatility= p.Volatility;

                p2.S0 = p.S0;
                p2.Strike = p.Strike;
                p2.r = p.r - deltar;
                p2.Steps = p.Steps;
                p2.Simulations = p.Simulations;
                p2.Tenor = p.Tenor;
                double VP1 = OptionPandStd(p1, IsCall, IsAntithetic, IsDeltaCV).VP;
                double VP2 = OptionPandStd(p2, IsCall, IsAntithetic, IsDeltaCV).VP;
                Rho = (VP1 - VP)/( deltar);
                return Rho;

            }


        }

    }
}