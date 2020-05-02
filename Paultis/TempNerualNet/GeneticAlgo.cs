using ArtificialNeuralNetwork;
using ArtificialNeuralNetwork.ActivationFunctions;
using ArtificialNeuralNetwork.Factories;
using ArtificialNeuralNetwork.WeightInitializer;
using NeuralNetwork.GeneticAlgorithm;
using NeuralNetwork.GeneticAlgorithm.Evaluatable;
using NeuralNetwork.GeneticAlgorithm.Evolution;
using NeuralNetwork.GeneticAlgorithm.Utils;
using StorageAPI;
using StorageAPI.Proxies.NodeJs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemptureBlockGameGame.src;

namespace TempNerualNet
{
	class MyEvaluatableFactory : IEvaluatableFactory
	{
		public MyEvaluatableFactory()
		{
		}

		public IEvaluatable Create(INeuralNetwork neuralNetwork)
		{
			var gameRunner = new GameRunner();
			var nerualNetKeyboardInputManger = new NerualNetKeyboardInputManger();
			gameRunner.ChangeKeyboarInputManger(nerualNetKeyboardInputManger);
			gameRunner.GameLogicInitlise();


			return new TempGameEvulation(gameRunner, nerualNetKeyboardInputManger, neuralNetwork);
		}

		internal static IEvaluatableFactory GetInstance()
		{
			return new MyEvaluatableFactory();
		}
	}

	public class GeneticAlgo
	{
		public GeneticAlgo()
		{
		}

		public void Run()
		{
			Const.TIME_SCLAE = 10000;

			GameRunner gameRunner = new GameRunner();
			var nerualNetKeyboardInputManger = new NerualNetKeyboardInputManger();

			gameRunner.ChangeKeyboarInputManger(nerualNetKeyboardInputManger);

			var networkFactory = NeuralNetworkFactory.GetInstance();
			var evalWorkingSetFactory = EvalWorkingSetFactory.GetInstance();
			var evaluatableFactory = MyEvaluatableFactory.GetInstance();
			var randomInit = new RandomWeightInitializer(new Random());
			var breederFactory = BreederFactory.GetInstance(networkFactory, randomInit);
			var mutatorFactory = MutatorFactory.GetInstance(networkFactory, randomInit);
			IGeneticAlgorithmFactory GAFactory = GeneticAlgorithmFactory.GetInstance(networkFactory, evalWorkingSetFactory, evaluatableFactory, breederFactory, mutatorFactory);

			NeuralNetworkConfigurationSettings networkConfig = new NeuralNetworkConfigurationSettings
			{
				NumInputNeurons = Const.GRID_WIDTH * Const.GRID_HEIGHT + 1,
				NumOutputNeurons = 1,
				NumHiddenLayers = 2,
				NumHiddenNeurons = 3,
				SummationFunction = new SimpleSummation(),
				ActivationFunction = new TanhActivationFunction()
			};

			GenerationConfigurationSettings generationSettings = new GenerationConfigurationSettings
			{
				UseMultithreading = true,
				GenerationPopulation = 20
			};

			EvolutionConfigurationSettings evolutionSettings = new EvolutionConfigurationSettings
			{
				NormalMutationRate = 0.05,
				HighMutationRate = 0.5,
				GenerationsPerEpoch = 20,
				NumEpochs = 20
			};

			MutationConfigurationSettings mutationSettings = new MutationConfigurationSettings
			{
				MutateAxonActivationFunction = true,
				MutateNumberOfHiddenLayers = true,
				MutateNumberOfHiddenNeuronsInLayer = true,
				MutateSomaBiasFunction = true,
				MutateSomaSummationFunction = true,
				MutateSynapseWeights = true
			};

			IStorageProxy proxy = new NetFileStorage();
			IEpochAction action = new BestPerformerUpdater(proxy);

			var random = new RandomWeightInitializer(new Random());
			INeuralNetworkFactory factory = NeuralNetworkFactory.GetInstance(
				SomaFactory.GetInstance(networkConfig.SummationFunction), 
				AxonFactory.GetInstance(networkConfig.ActivationFunction), 
				SynapseFactory.GetInstance(
					new RandomWeightInitializer(new Random()), 
					AxonFactory.GetInstance(networkConfig.ActivationFunction)), 
					SynapseFactory.GetInstance(
						new ConstantWeightInitializer(1.0), 
						AxonFactory.GetInstance(new IdentityActivationFunction())
					), 
					random,
					NeuronFactory.GetInstance()
				);
			IBreeder breeder = BreederFactory.GetInstance(factory, random).Create();
			IMutator mutator = MutatorFactory.GetInstance(factory, random).Create(mutationSettings);
			IEvalWorkingSet history = EvalWorkingSetFactory.GetInstance().Create(50);

			IGeneticAlgorithm evolver = GAFactory.Create(networkConfig, generationSettings, evolutionSettings, factory, breeder, mutator, history, evaluatableFactory, action);

			evolver.RunSimulation();

			INeuralNetwork best = evolver.GetBestPerformer();

			PaulFileUtils.WriteToJsonFile(best.GetGenes(), "bestnn.json");
		}
	}
}
