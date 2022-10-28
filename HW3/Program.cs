﻿﻿﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {   
            Console.Write("What Type of option you want to pricing? ");
            string optionType = Console.ReadLine();
            if (optionType.ToLower() == "european")
            {
                Simulator.European O = new Simulator.European();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                EuropeanOption euro = new EuropeanOption();

                int CPUnum = System.Environment.ProcessorCount;
                int SimulationMultiplier = 1;
                Console.Write("What is the Underlying Price: ");
                S.S0 = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Strike Price: ");
                O.Strike = Convert.ToDouble(Console.ReadLine());
                Console.Write("How much years the Option expired: ");
                O.Tenor = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the risk free rate: ");
                S.r = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Volatility: ");
                S.Volatility = Convert.ToDouble(Console.ReadLine());
                Console.Write("Is Call? (true or false) ");
                O.IsCall = Convert.ToBoolean(Console.ReadLine());                
                Console.Write("Do you want Antithetic method for montecarlo? (true or false) ");
                S.IsAntithetic = Convert.ToBoolean(Console.ReadLine());
                Console.Write("DO you want delta-based control variate? (true or false) ");
                S.IsDeltaCV = Convert.ToBoolean(Console.ReadLine());
                Console.Write("Do you want multithreading function to calculate S paths? (true or flase) ");
                S.IsThread = Convert.ToBoolean(Console.ReadLine());
                if (S.IsThread == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("NOTICE!!!");
                    Console.WriteLine("The Simulation number will be multiplied by the number of CPU core since you choose the multithread function");
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                    Console.WriteLine();
                }
                Console.Write("How many simulations you want ");
                S.Simulations = Convert.ToInt32(Console.ReadLine())*SimulationMultiplier;
                Console.Write("How many steps you want ");
                S.Steps = Convert.ToInt32(Console.ReadLine());
                O.S = S;
                if (O.S.IsAntithetic == true)
                {
                    OptionResult AntitheticResult = new OptionResult();
                    AntitheticResult.Price = euro.OptionPandStd(O).VP;
                    (AntitheticResult.Delta, AntitheticResult.Gamma) = euro.DeltaandGamma(O);
                    AntitheticResult.Theta = euro.ThetaValue(O);
                    AntitheticResult.Vega = euro.VegaValue(O);
                    AntitheticResult.Rho = euro.RhoValue(O);
                    AntitheticResult.StdErrorNorm = euro.StandardError(O, false);
                    AntitheticResult.StdErrorReduce = euro.StandardError(O, true); 
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
                    Result.Price = euro.OptionPandStd(O).VP;
                    (Result.Delta, Result.Gamma) = euro.DeltaandGamma(O);
                    Result.Theta = euro.ThetaValue(O);
                    Result.Vega = euro.VegaValue(O);
                    Result.Rho = euro.RhoValue(O);
                    Result.StdErrorNorm = euro.StandardError(O, false);
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
            else if (optionType.ToLower() == "asian")
            {
                Simulator.Aisan O = new Simulator.Aisan();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                AsianOption asia = new AsianOption(); 

                int CPUnum = System.Environment.ProcessorCount;
                int SimulationMultiplier = 1;
                Console.Write("What is the Underlying Price: ");
                S.S0 = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Strike Price: ");
                O.Strike = Convert.ToDouble(Console.ReadLine());
                Console.Write("How much years the Option expired: ");
                O.Tenor = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the risk free rate: ");
                S.r = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Volatility: ");
                S.Volatility = Convert.ToDouble(Console.ReadLine());
                Console.Write("Is Call? (true or false) ");
                O.IsCall = Convert.ToBoolean(Console.ReadLine());                
                Console.Write("Do you want Antithetic method for montecarlo? (true or false) ");
                S.IsAntithetic = Convert.ToBoolean(Console.ReadLine());
                Console.Write("DO you want delta-based control variate? (true or false) ");
                S.IsDeltaCV = Convert.ToBoolean(Console.ReadLine());
                Console.Write("Do you want multithreading function to calculate S paths? (true or flase) ");
                S.IsThread = Convert.ToBoolean(Console.ReadLine());
                if (S.IsThread == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("NOTICE!!!");
                    Console.WriteLine("The Simulation number will be multiplied by the number of CPU core since you choose the multithread function");
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                    Console.WriteLine();
                }
                Console.Write("How many simulations you want ");
                S.Simulations = Convert.ToInt32(Console.ReadLine())*SimulationMultiplier;
                Console.Write("How many steps you want ");
                S.Steps = Convert.ToInt32(Console.ReadLine());
                O.S = S;
                if (O.S.IsAntithetic == true)
                {
                    OptionResult AntitheticResult = new OptionResult();
                    AntitheticResult.Price = asia.OptionPandStd(O).VP;
                    (AntitheticResult.Delta, AntitheticResult.Gamma) = asia.DeltaandGamma(O);
                    AntitheticResult.Theta = asia.ThetaValue(O);
                    AntitheticResult.Vega = asia.VegaValue(O);
                    AntitheticResult.Rho = asia.RhoValue(O);
                    AntitheticResult.StdErrorNorm = asia.StandardError(O, false);
                    AntitheticResult.StdErrorReduce = asia.StandardError(O, true); 
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
                    Result.Price = asia.OptionPandStd(O).VP;
                    (Result.Delta, Result.Gamma) = asia.DeltaandGamma(O);
                    Result.Theta = asia.ThetaValue(O);
                    Result.Vega = asia.VegaValue(O);
                    Result.Rho = asia.RhoValue(O);
                    Result.StdErrorNorm = asia.StandardError(O, false);
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

            else if (optionType.ToLower() == "digital")
            {
                Simulator.Digital O = new Simulator.Digital();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                DigitalOption digi = new DigitalOption(); 

                int CPUnum = System.Environment.ProcessorCount;
                int SimulationMultiplier = 1;
                Console.Write("What is the Underlying Price: ");
                S.S0 = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Strike Price: ");
                O.Strike = Convert.ToDouble(Console.ReadLine());
                Console.Write("How much years the Option expired: ");
                O.Tenor = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the risk free rate: ");
                S.r = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Volatility: ");
                S.Volatility = Convert.ToDouble(Console.ReadLine());
                Console.Write("Is Call? (true or false) ");
                O.IsCall = Convert.ToBoolean(Console.ReadLine());
                Console.Write("What is your payout amount? ");
                O.PayOut = Convert.ToDouble(Console.ReadLine());                
                Console.Write("Do you want Antithetic method for montecarlo? (true or false) ");
                S.IsAntithetic = Convert.ToBoolean(Console.ReadLine());
                Console.Write("DO you want delta-based control variate? (true or false) ");
                S.IsDeltaCV = Convert.ToBoolean(Console.ReadLine());
                Console.Write("Do you want multithreading function to calculate S paths? (true or flase) ");
                S.IsThread = Convert.ToBoolean(Console.ReadLine());
                if (S.IsThread == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("NOTICE!!!");
                    Console.WriteLine("The Simulation number will be multiplied by the number of CPU core since you choose the multithread function");
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                    Console.WriteLine();
                }
                Console.Write("How many simulations you want ");
                S.Simulations = Convert.ToInt32(Console.ReadLine())*SimulationMultiplier;
                Console.Write("How many steps you want ");
                S.Steps = Convert.ToInt32(Console.ReadLine());
                O.S = S;
                if (O.S.IsAntithetic == true)
                {
                    OptionResult AntitheticResult = new OptionResult();
                    AntitheticResult.Price = digi.OptionPandStd(O).VP;
                    (AntitheticResult.Delta, AntitheticResult.Gamma) = digi.DeltaandGamma(O);
                    AntitheticResult.Theta = digi.ThetaValue(O);
                    AntitheticResult.Vega = digi.VegaValue(O);
                    AntitheticResult.Rho = digi.RhoValue(O);
                    AntitheticResult.StdErrorNorm = digi.StandardError(O, false);
                    AntitheticResult.StdErrorReduce = digi.StandardError(O, true); 
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
                    Result.Price = digi.OptionPandStd(O).VP;
                    (Result.Delta, Result.Gamma) = digi.DeltaandGamma(O);
                    Result.Theta = digi.ThetaValue(O);
                    Result.Vega = digi.VegaValue(O);
                    Result.Rho = digi.RhoValue(O);
                    Result.StdErrorNorm = digi.StandardError(O, false);
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
            else if (optionType.ToLower() == "barrier")
            {
                Simulator.Barrier O = new Simulator.Barrier();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                BarrierOption barrier = new BarrierOption(); 

                int CPUnum = System.Environment.ProcessorCount;
                int SimulationMultiplier = 1;
                Console.Write("What is the Underlying Price: ");
                S.S0 = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Strike Price: ");
                O.Strike = Convert.ToDouble(Console.ReadLine());
                Console.Write("How much years the Option expired: ");
                O.Tenor = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the risk free rate: ");
                S.r = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Volatility: ");
                S.Volatility = Convert.ToDouble(Console.ReadLine());
                Console.Write("Is Call? (true or false) ");
                O.IsCall = Convert.ToBoolean(Console.ReadLine());
                Console.Write("What is the barrier amount? ");
                O.BarrierLevel = Convert.ToDouble(Console.ReadLine());
                Console.Write("What knock type you would like to use? ");
                string kt = Console.ReadLine();
                if (kt.ToLower() == "up and in")
                {
                    O.KnockType = Simulator.KnockType.UpAndIn;
                }
                else if (kt.ToLower() == "up and out")
                {
                    O.KnockType = Simulator.KnockType.UpAndOut;
                }
                if (kt.ToLower() == "down and in")
                {
                    O.KnockType = Simulator.KnockType.DownAndIn;
                }
                if (kt.ToLower() == "down and out")
                {
                    O.KnockType = Simulator.KnockType.DownAndOut;
                }
        


                Console.Write("Do you want Antithetic method for montecarlo? (true or false) ");
                S.IsAntithetic = Convert.ToBoolean(Console.ReadLine());
                Console.Write("DO you want delta-based control variate? (true or false) ");
                S.IsDeltaCV = Convert.ToBoolean(Console.ReadLine());
                Console.Write("Do you want multithreading function to calculate S paths? (true or flase) ");
                S.IsThread = Convert.ToBoolean(Console.ReadLine());
                if (S.IsThread == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("NOTICE!!!");
                    Console.WriteLine("The Simulation number will be multiplied by the number of CPU core since you choose the multithread function");
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                    Console.WriteLine();
                }
                Console.Write("How many simulations you want ");
                S.Simulations = Convert.ToInt32(Console.ReadLine())*SimulationMultiplier;
                Console.Write("How many steps you want ");
                S.Steps = Convert.ToInt32(Console.ReadLine());
                O.S = S;
                if (O.S.IsAntithetic == true)
                {
                    OptionResult AntitheticResult = new OptionResult();
                    AntitheticResult.Price = barrier.OptionPandStd(O).VP;
                    (AntitheticResult.Delta, AntitheticResult.Gamma) = barrier.DeltaandGamma(O);
                    AntitheticResult.Theta = barrier.ThetaValue(O);
                    AntitheticResult.Vega = barrier.VegaValue(O);
                    AntitheticResult.Rho = barrier.RhoValue(O);
                    AntitheticResult.StdErrorNorm = barrier.StandardError(O, false);
                    AntitheticResult.StdErrorReduce = barrier.StandardError(O, true); 
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
                    Result.Price = barrier.OptionPandStd(O).VP;
                    (Result.Delta, Result.Gamma) = barrier.DeltaandGamma(O);
                    Result.Theta = barrier.ThetaValue(O);
                    Result.Vega = barrier.VegaValue(O);
                    Result.Rho = barrier.RhoValue(O);
                    Result.StdErrorNorm = barrier.StandardError(O, false);
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
            else if (optionType.ToLower() == "lookback")
            {
                Simulator.Lookback O = new Simulator.Lookback();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                LookbackOption lb = new LookbackOption(); 

                int CPUnum = System.Environment.ProcessorCount;
                int SimulationMultiplier = 1;
                Console.Write("What is the Underlying Price: ");
                S.S0 = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Strike Price: ");
                O.Strike = Convert.ToDouble(Console.ReadLine());
                Console.Write("How much years the Option expired: ");
                O.Tenor = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the risk free rate: ");
                S.r = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Volatility: ");
                S.Volatility = Convert.ToDouble(Console.ReadLine());
                Console.Write("Is Call? (true or false) ");
                O.IsCall = Convert.ToBoolean(Console.ReadLine());                
                Console.Write("Do you want Antithetic method for montecarlo? (true or false) ");
                S.IsAntithetic = Convert.ToBoolean(Console.ReadLine());
                Console.Write("DO you want delta-based control variate? (true or false) ");
                S.IsDeltaCV = Convert.ToBoolean(Console.ReadLine());
                Console.Write("Do you want multithreading function to calculate S paths? (true or flase) ");
                S.IsThread = Convert.ToBoolean(Console.ReadLine());
                if (S.IsThread == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("NOTICE!!!");
                    Console.WriteLine("The Simulation number will be multiplied by the number of CPU core since you choose the multithread function");
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                    Console.WriteLine();
                }
                Console.Write("How many simulations you want ");
                S.Simulations = Convert.ToInt32(Console.ReadLine())*SimulationMultiplier;
                Console.Write("How many steps you want ");
                S.Steps = Convert.ToInt32(Console.ReadLine());
                O.S = S;
                if (O.S.IsAntithetic == true)
                {
                    OptionResult AntitheticResult = new OptionResult();
                    AntitheticResult.Price = lb.OptionPandStd(O).VP;
                    (AntitheticResult.Delta, AntitheticResult.Gamma) = lb.DeltaandGamma(O);
                    AntitheticResult.Theta = lb.ThetaValue(O);
                    AntitheticResult.Vega = lb.VegaValue(O);
                    AntitheticResult.Rho = lb.RhoValue(O);
                    AntitheticResult.StdErrorNorm = lb.StandardError(O, false);
                    AntitheticResult.StdErrorReduce = lb.StandardError(O, true); 
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
                    Result.Price = lb.OptionPandStd(O).VP;
                    (Result.Delta, Result.Gamma) = lb.DeltaandGamma(O);
                    Result.Theta = lb.ThetaValue(O);
                    Result.Vega = lb.VegaValue(O);
                    Result.Rho = lb.RhoValue(O);
                    Result.StdErrorNorm = lb.StandardError(O, false);
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
            else if (optionType.ToLower() == "range")
            {
                Simulator.Range O = new Simulator.Range();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                RangeOption rg = new RangeOption(); 

                int CPUnum = System.Environment.ProcessorCount;
                int SimulationMultiplier = 1;
                Console.Write("What is the Underlying Price: ");
                S.S0 = Convert.ToDouble(Console.ReadLine());
                Console.Write("How much years the Option expired: ");
                O.Tenor = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the risk free rate: ");
                S.r = Convert.ToDouble(Console.ReadLine());
                Console.Write("What is the Volatility: ");
                S.Volatility = Convert.ToDouble(Console.ReadLine());                
                Console.Write("Do you want Antithetic method for montecarlo? (true or false) ");
                S.IsAntithetic = Convert.ToBoolean(Console.ReadLine());
                Console.Write("Do you want multithreading function to calculate S paths? (true or flase) ");
                S.IsThread = Convert.ToBoolean(Console.ReadLine());
                if (S.IsThread == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("NOTICE!!!");
                    Console.WriteLine("The Simulation number will be multiplied by the number of CPU core since you choose the multithread function");
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                    Console.WriteLine();
                }
                Console.Write("How many simulations you want ");
                S.Simulations = Convert.ToInt32(Console.ReadLine())*SimulationMultiplier;
                Console.Write("How many steps you want ");
                S.Steps = Convert.ToInt32(Console.ReadLine());
                O.S = S;
                if (O.S.IsAntithetic == true)
                {
                    OptionResult AntitheticResult = new OptionResult();
                    AntitheticResult.Price = rg.OptionPandStd(O).VP;
                    (AntitheticResult.Delta, AntitheticResult.Gamma) = rg.DeltaandGamma(O);
                    AntitheticResult.Theta = rg.ThetaValue(O);
                    AntitheticResult.Vega = rg.VegaValue(O);
                    AntitheticResult.Rho = rg.RhoValue(O);
                    AntitheticResult.StdErrorNorm = rg.StandardError(O, false);
                    AntitheticResult.StdErrorReduce = rg.StandardError(O, true); 
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
                    Result.Price = rg.OptionPandStd(O).VP;
                    (Result.Delta, Result.Gamma) = rg.DeltaandGamma(O);
                    Result.Theta = rg.ThetaValue(O);
                    Result.Vega = rg.VegaValue(O);
                    Result.Rho = rg.RhoValue(O);
                    Result.StdErrorNorm = rg.StandardError(O, false);
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
        
    }
}