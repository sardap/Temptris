using ArtificialNeuralNetwork;
using Microsoft.Xna.Framework.Input;
using NeuralNetwork.GeneticAlgorithm.Evaluatable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemptureBlockGameGame.src;

namespace TempNerualNet
{
	class TempGameEvulation : IEvaluatable
	{
		private static double _BestScore;

		private INeuralNetwork _neuralNet;
		private GameRunner _gameRunner;
		private NerualNetKeyboardInputManger _nerualNetKeyboardInputManger;

		public TempGameEvulation(GameRunner gameRunner, NerualNetKeyboardInputManger nerualNetKeyboardInputManger, INeuralNetwork neuralNetwork)
		{
			_neuralNet = neuralNetwork;
			_gameRunner = gameRunner;
			_nerualNetKeyboardInputManger = nerualNetKeyboardInputManger;
		}

		public double GetEvaluation()
		{
			if (!_gameRunner.IsGameOver())
			{
				throw new NotSupportedException("GetSessionEvaluation is not supported when game is not finished");
			}

			if(_gameRunner.GetScore() > _BestScore)
			{
				_BestScore = _gameRunner.GetScore();
				Console.WriteLine("NEW BEST SCORE {0}", _BestScore);
			}

			return _gameRunner.GetScore();
		}

		public void RunEvaluation()
		{
			Keys[] values = (new List<Keys>() { Keys.Up, Keys.Left, Keys.Right, Keys.Down }.Shuffle()).ToArray();
			var stopwatch = new Stopwatch();

			double sincePrint = 0;

			Stack<Keys> inputs = new Stack<Keys>();

			while (!_gameRunner.IsGameOver())
			{
				var deltaTime = (double)((double)stopwatch.Elapsed.TotalMilliseconds / 1000d);
				stopwatch.Reset();
				stopwatch.Start();

				sincePrint += deltaTime;

				Keys keyToPress = Utils.RandomElement(values);

				double highestProb = double.MinValue;

				foreach (var key in values)
				{
					var input = new List<double>(_gameRunner.GetDataRepresentation())
					{
						(double)key
					};

					_neuralNet.SetInputs(input.ToArray());
					_neuralNet.Process();

					double probability = _neuralNet.GetOutputs()[0];

					if (probability > highestProb)
					{
						keyToPress = key;
						highestProb = probability;
					}
				}

				_nerualNetKeyboardInputManger.KeyPressed[keyToPress] = true;

				_gameRunner.Step(deltaTime);

				if(sincePrint > 0.5)
				{
					sincePrint = 0;
				}

				stopwatch.Stop();
			}

			//_gameRunner.ConsolePrint();

			Console.WriteLine("GAME OVER SCORE:{0}", _gameRunner.GetScore());
		}
	}
}
