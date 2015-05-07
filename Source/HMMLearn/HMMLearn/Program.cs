//
//  Program.cs
//
//  Author:
//       Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 Chen Weiqing
//
//
using System;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;

namespace HMMLearn
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var data = new Sequences ("nbt.1882-S6.txt");

			// Now we create a hidden Markov model with arbitrary probabilities
			var hmm = new HiddenMarkovModel (states: data.StateNum, symbols: data.SymbolNum);

			// Create a Baum-Welch learning algorithm to teach it
			var teacher = new BaumWelchLearning (hmm);

			// and call its Run method to start learning
			var trainSamples = data.PartOfSequences (0, 2);
			double error = teacher.Run (trainSamples);

			var testSamples = data.PartOfSequences (1, 2);

			// Let's now check the probability of some sequences:
			double prob1 = Math.Exp (hmm.Evaluate (trainSamples [0]));
			double prob2 = Math.Exp (hmm.Evaluate (trainSamples [1]));
			double prob3 = Math.Exp (hmm.Evaluate (trainSamples [2]));

			// Now those obviously violate the form of the training set:
			double prob4 = Math.Exp (hmm.Evaluate (testSamples [0]));
			double prob5 = Math.Exp (hmm.Evaluate (testSamples [1]));
		}
	}
}
