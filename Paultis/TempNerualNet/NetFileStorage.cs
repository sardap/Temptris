using ArtificialNeuralNetwork;
using ArtificialNeuralNetwork.Genes;
using NeuralNetwork.GeneticAlgorithm;
using StorageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemptureBlockGameGame.src;

namespace TempNerualNet
{
	class NetFileStorage : IStorageProxy
	{
		public SortedList<double, ITrainingSession> _sessions = new SortedList<double, ITrainingSession>();
		public SortedList<double, INeuralNetwork> _networks = new SortedList<double, INeuralNetwork>();

		public ITrainingSession GetBestSession()
		{
			var bestNet = _networks.Last();

			return new FakeTrainingSession(bestNet.Value, bestNet.Key);
		}

		public Task<ITrainingSession> GetBestSessionAsync()
		{
			throw new NotImplementedException();
		}

		public void StoreNetwork(INeuralNetwork network, double eval)
		{
			if (!_networks.ContainsKey(eval))
				_networks.Add(eval, network);
			else
				_networks[eval] = network;

			var jsonVerison = new Dictionary<double, NeuralNetworkGene>();

			_networks.ToList().ForEach(i => jsonVerison.Add(i.Key, i.Value.GetGenes()));

			PaulFileUtils.WriteToJsonFile(jsonVerison, @"networks.json");

			Console.WriteLine("ADDED NEW NETWORK SCORE:{0}", eval);
		}

		public Task StoreNetworkAsync(INeuralNetwork network, double eval)
		{
			throw new NotImplementedException();
		}
	}
}
