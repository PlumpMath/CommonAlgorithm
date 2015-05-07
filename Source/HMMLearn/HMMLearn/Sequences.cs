//
//  Sequences.cs
//
//  Author:
//       Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 Chen Weiqing
//
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HMMLearn
{
	public class Sequences
	{
		static readonly Dictionary<char,int> ToLabelDict = new Dictionary<char, int> () {
			{ 'A', 0 },
			{ 'T', 1 },
			{ 'G', 2 },
			{ 'C', 3 }
		};

		static readonly Dictionary<int,char> ToStringDict = new Dictionary<int, char> () {
			{ 0, 'A' },
			{ 1, 'T' },
			{ 2, 'G' },
			{ 3, 'C' }
		};

		readonly object _sync = new object ();

		int[][] sequences;
		double[] values;

		public int SymbolNum {
			get { return ToLabelDict.Count; }
		}

		public int StateNum {
			get{ return ToLabelDict.Count; }
		}

		public int Count {
			get{ return sequences.Length; }
		}

		public int[][] PartOfSequences (int i, int n)
		{
			Debug.Assert (i >= 0 && i < n);
			var len = sequences.Length / n;
			var ret = new int[len][];
			Array.Copy (sequences, i * len, ret, 0, len);
			return ret;
		}

		public int[] PartOfValues (int i, int n)
		{
			Debug.Assert (i >= 0 && i < n);
			var len = values.Length / n;
			var ret = new int[len];
			Array.Copy (values, i * len, ret, 0, len);
			return ret;
		}

		public Sequences (string path)
		{
			var seqs = new List<int[]> ();
			var outs = new List<double> ();
			var lines = File.ReadAllLines (path);
			Parallel.ForEach (lines, x => {
				var segs = x.Split ('\t');
				if (segs.Length > 2) {
					int[] seq = ToLabel (segs [0]);
					lock (_sync) {
						seqs.Add (seq);
						outs.Add (double.Parse (segs [1]));
					}
				}
			});

			sequences = seqs.ToArray ();
			values = outs.ToArray ();
			Debug.Assert (sequences.Length == values.Length);
		}

		public static int[] ToLabel (string str)
		{
			var seq = new int[str.Length];
			for (int i = 0; i < seq.Length; i++) {
				var l = ToLabelDict [str [i]];
				Debug.Assert (l >= 0 && l <= 3);
				seq [i] = l;
			}
			return seq;
		}

		public static string ToString (int[] labels)
		{
			var ret = "";
			for (int i = 0; i < labels.Length; i++) {
				ret += ToStringDict [labels [i]];
			}
			return ret;
		}
	}
}

