using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW4.Models
{
    public class Simulator
    {   
        public class SimulationParameters
        {
            public double S0;
            public double r;
            public int Steps;
            public int Simulations;
            public double Volatility;
            public bool IsThread;
            public bool IsAntithetic;
            public bool IsDeltaCV;
          
        }
        public class Option 
        {
            public SimulationParameters S{get; set;}
            public double Tenor {get; set;}
            
        }

        public class European: Option
        {
            public double Strike{get; set;}
            public bool IsCall {get; set;}
        }

        public class Aisan: Option 
        {
            public double Strike{get; set;}
            public bool IsCall {get; set;}
        }

        public class Digital: Option
        {
            public double Strike{get; set;}
            public bool IsCall {get; set;}
            public double PayOut{get; set;}
        }
        public class Lookback: Option
        {
            public double Strike{get; set;}
            public bool IsCall {get; set;}
        }
        public class Range : Option
        {

        }
        public enum KnockType
        {
            DownAndOut,
            UpAndOut,
            DownAndIn,
            UpAndIn
        }
        public class Barrier: Option
        {
            public double Strike {get; set;}
            public bool IsCall {get; set;}
            public double BarrierLevel {get; set;}
            public KnockType KnockType{get; set;}
        }
        public class SimulationResult
        {
            public double[,] SimulatedPaths {get; set;}
        }
        
        public class GaussianRandoms
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
                        NegNrands[i,j] = -1 * NRands[i,j];
                    }
                }
                return NegNrands;
            }
        }
        public class StockPathGenerator
        {   
          
            
            public static SimulationResult GeneratePaths( Option O)
            {
                GaussianRandoms Nrands = new GaussianRandoms();
                double [, ]R1 = Nrands.PopulatedNrands(O.S.Simulations,O.S.Steps);
                
                SimulationResult results = new SimulationResult();
                results.SimulatedPaths = new double [O.S.Simulations,O.S.Steps];
                if (O.S.IsThread == true)
                {   
                    threadinit ti = new threadinit();
                    ti.paramthread = O;
                    ti.resultthread = results;
                    ti.R1thread = R1;
                    Thread [] threads = new Thread [System.Environment.ProcessorCount];
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {   
                        threads[i] = new Thread(new ThreadStart(ti.run));
                        threads[i].Start();
                    }
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {
                        threads[i].Join();
                    }
                    
                }
                else
                {               
                    populatevalues(O, results, R1);
                }
                
                return results;
            }

            public static SimulationResult AntetheticPaths(Option O)
            {
                GaussianRandoms Nrands2 = new GaussianRandoms();
                double [,] R2 = Nrands2.ReverseNrands(O.S.Simulations,O.S.Steps);
                SimulationResult reverseresult = new SimulationResult();
                reverseresult.SimulatedPaths = new double [O.S.Simulations, O.S.Steps];
            
                if (O.S.IsThread == true)
                {   
                    threadinit ti = new threadinit();
            
                    ti.resultthread = reverseresult;
                    ti.R1thread = R2;
                    ti.paramthread = O;

                    Thread [] threads = new Thread [System.Environment.ProcessorCount];
                    
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {   
                        
                        threads[i] = new Thread(new ThreadStart(ti.run));
    
                        threads[i].Start();
                    }
                    for (int i = 0; i < System.Environment.ProcessorCount; i++)
                    {
                        threads[i].Join();
                    }
                    
                }
                else
                {
                    populatevalues(O, reverseresult, R2);
                }
                return reverseresult;
            }
            public static void populatevalues(Option O, SimulationResult result, double [,] R1 )
            {   
                double dt = O.Tenor/O.S.Steps;
                double erdt = Math.Exp(O.S.r*dt);
                for (int i = 0; i < O.S.Simulations; i++)
                {   
                    result.SimulatedPaths[i,0] = O.S.S0;
                    for (int j = 1; j < O.S.Steps; j++)
                    {
                        result.SimulatedPaths[i,j] = result.SimulatedPaths[i, j-1] * Math.Exp(((O.S.r - 0.5 * Math.Pow(O.S.Volatility,2))* dt) + (O.S.Volatility * Math.Sqrt(dt)*R1[i,j]));
                    }
                }
            } 

            public class threadinit
            {   
                public SimulationResult resultthread;
                public double [,] R1thread;
                public Option paramthread;
                public void run()
                {
                    populatevalues(paramthread, resultthread, R1thread);
                }
            }

        }
    }
}