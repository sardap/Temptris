using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	public class GameRunner
	{
		private static Random _random = new Random();

		private enum KeyStates
		{
			None, KeyDown
		}

		private enum MoveBlockStates
		{
			None, Pressed
		}

		private class MoveBlockStateEntry
		{
			public MoveBlockStates State { get; set; }
			public int XOffset { get; set; }
			public double TimeSinceLastUpdate { get; set; }
		}

		private KeyStates _fastFowardStates = KeyStates.None;
		private KeyStates _rotateStates = KeyStates.None;
		private Dictionary<Keys, MoveBlockStateEntry> _moveBlockStateMachines = new Dictionary<Keys, MoveBlockStateEntry>();
		private WeatherNotifer _weatherNotifer = new WeatherNotifer();
		private SelectedPlaceInfo _selectedPlaceInfo = new SelectedPlaceInfo();
		private InfoBar _infoBar = new InfoBar();
		private ContentManager _contentManager;
		private Grid _grid = new Grid();
		private IKeyboardInputManger _keyboardInputManger = new KeyboardInputManger();
		private double _sinceLastUpdate = 0;

		private double _timeBetweenUpdates = Const.TIME_BETWEEN_GRID_UPDATES_START;
		private double _lastTimeBetweenUpdate;

		private double _timeSinceWeatherBlock;
		private double _nextWeatherBlockTime;

		private double _windCooldown;

		private double _thunderCoolDown;
		private double _nextThunderTime;

		private double _scoreIncrmentTickTime;

		private IWeatherInfo _weatherInfo;

		private Score _score = new Score();

		public GameRunner()
		{

		}

		public int GetScore()
		{
			return _score.Value;
		}

		public CellTypes[][] GetGrid()
		{
			return _grid.Cells;
		}

		public double[] GetDataRepresentation()
		{
			return _grid.DataRepresentation;
		}


		public void ChangeKeyboarInputManger(IKeyboardInputManger keyboardInputManger)
		{
			_keyboardInputManger = keyboardInputManger;
		}

		public void GameLogicInitlise()
		{
			APIKeyManger.GetInstance().Load(PaulFileUtils.DeserializeObjectFromFile<Dictionary<string, string>>("APIKeys.json"));

			_selectedPlaceInfo.CityID = APIKeyManger.GetInstance().GetApiKey("CityID");

			var openWeatherInfo = new OpenWeatherInfo() { CityID = _selectedPlaceInfo.CityID };
			_weatherInfo = openWeatherInfo;
			_weatherInfo.Initlise();

			_selectedPlaceInfo.Location = openWeatherInfo.GetLocation();

			BlockBlueprintLibary.GetInstance().UpdateOddsTableWithWeatherData(_weatherInfo);

			_grid.Insert(BlockBlueprintLibary.GetInstance().RandomNorimeBlock(), true);
			_grid.Score = _score;

			_grid.ApplyAmbientWeatherEffects(DateTime.Now, _weatherInfo, _selectedPlaceInfo);

			_lastTimeBetweenUpdate = _timeBetweenUpdates;

			_nextWeatherBlockTime = GetTimeBetweenWeatherBlocks();
			_windCooldown = GetWindCooldown();
			_nextThunderTime = GetNextThunderTime();

			_moveBlockStateMachines.Add(Keys.Left, new MoveBlockStateEntry() { State = MoveBlockStates.None, XOffset = -1 });
			_moveBlockStateMachines.Add(Keys.Right, new MoveBlockStateEntry() { State = MoveBlockStates.None, XOffset = 1 });
		}


		public void Initlise(ContentManager contentManager)
		{
			GameLogicInitlise();

			_contentManager = contentManager;

			_weatherNotifer.Initlise(contentManager);

			PopluateInfoBar();
		}

		public bool IsGameOver()
		{
			return _grid.Failure;
		}

		public void SocreWithMove(Directions directions)
		{

		}

		public void Step(double deltaTime)
		{
			_sinceLastUpdate += deltaTime;
			_timeSinceWeatherBlock += deltaTime;
			_scoreIncrmentTickTime += deltaTime;

			if(_scoreIncrmentTickTime >= Const.SOCRE_INCREMENT)
			{
				_score.Value++;
				_scoreIncrmentTickTime = 0;
			}

			if (_sinceLastUpdate >= _timeBetweenUpdates)
			{
				_grid.Step();
				_sinceLastUpdate = 0;
			}

			if (_timeSinceWeatherBlock >= _nextWeatherBlockTime)
			{
				_grid.Insert(BlockBlueprintLibary.GetInstance().WeatherRandomBlock());
				_timeSinceWeatherBlock = 0;
				_nextWeatherBlockTime = GetTimeBetweenWeatherBlocks();
			}

			if (_grid.ActiveBlocks() == 0)
			{
				_grid.Insert(BlockBlueprintLibary.GetInstance().RandomNorimeBlock(), true);
				_timeBetweenUpdates = Math.Max(Const.TIME_BETWEEN_GRID_UPDATES_MAX, _timeBetweenUpdates - 0.005);
			}

			InputStep(deltaTime);
			ProcessWind(deltaTime);
			ProcessThunder(deltaTime);
			_weatherNotifer.Step(deltaTime);

			_keyboardInputManger.Step();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			_grid.Draw(spriteBatch, new Rectangle(0, 0, 1200, 500));
			_weatherNotifer.Draw(spriteBatch);
			_infoBar.Draw(new Point(500, 0), spriteBatch);
		}

		public void ConsolePrint()
		{
			Console.WriteLine("SCORE: {0}", _score.Value);
			Console.WriteLine(_grid.ToString());
		}

		private void InputStep(double deltaTime)
		{
			var state = Keyboard.GetState();

			if(_keyboardInputManger.KeyClicked[Keys.L])
			{
				_grid.ApplyWind(1, Directions.Left);

				//_grid.Insert(BlockBlueprintLibary.GetInstance().PureRandomBlock(), true);
			}

			switch(_fastFowardStates)
			{
				case KeyStates.None:
					if (_keyboardInputManger.KeyPressed[Keys.Down])
					{
						_lastTimeBetweenUpdate = _timeBetweenUpdates;
						_timeBetweenUpdates = Const.TIME_BETWEEN_GRID_UPDATES_MAX;
						_fastFowardStates = KeyStates.KeyDown;
					}
					break;
				case KeyStates.KeyDown:
					_score.Value++;
					if (!_keyboardInputManger.KeyPressed[Keys.Down])
					{
						_timeBetweenUpdates = _lastTimeBetweenUpdate;
						_fastFowardStates = KeyStates.None;
					}
					break;
			}

			switch (_rotateStates)
			{
				case KeyStates.None:
					if (_keyboardInputManger.KeyPressed[Keys.Up])
					{
						_grid.RotateActive();
						_rotateStates = KeyStates.KeyDown;
					}
					break;
				case KeyStates.KeyDown:
					if (!_keyboardInputManger.KeyPressed[Keys.Up])
					{
						_rotateStates = KeyStates.None;
					}
					break;
			}


			foreach (var entry in _moveBlockStateMachines)
			{
				switch(entry.Value.State)
				{
					case MoveBlockStates.None:
						if(_keyboardInputManger.KeyPressed[entry.Key])
						{
							entry.Value.State = MoveBlockStates.Pressed;
						}
						break;

					case MoveBlockStates.Pressed:
						if (!_keyboardInputManger.KeyPressed[entry.Key])
						{
							entry.Value.State = MoveBlockStates.None;
						}
						else
						{
							entry.Value.TimeSinceLastUpdate += deltaTime;

							if (entry.Value.TimeSinceLastUpdate > Const.MOVE_ACTIVE_BLOCK_COOLDOWN)
							{
								_grid.MoveActiveBlocks(entry.Value.XOffset, 0);
								entry.Value.TimeSinceLastUpdate = 0;
							}

						}
						break;
				}
			}
		}

		private double GetTimeBetweenWeatherBlocks()
		{
			// To the power of ^ of number

			//var amount = Math.Pow(12, _weatherInfo.AmountOfWeather());
			var weatherAmmount = _weatherInfo.AmountOfWeather();
			var result = _random.Next(25, 27) / weatherAmmount;

			Console.WriteLine("Amount:{0} Result{1}", weatherAmmount, result);
			return result;
		}

		private void ProcessThunder(double deltaTime)
		{
			_thunderCoolDown += deltaTime;

			if (_thunderCoolDown > _nextThunderTime)
			{
				_grid.ApplyLightingBolt();
				_thunderCoolDown = 0;
				_nextThunderTime = GetNextThunderTime();
			}
		}

		private double GetNextThunderTime()
		{
			return 300 / _weatherInfo.AmountOfThunderstrom(); // _random.Next(750, 1500)
		}

		private void ProcessWind(double deltaTime)
		{
			_windCooldown -= deltaTime;

			if(_windCooldown <= 0 && _weatherInfo.WindLevel() > Const.HIGH_WIND_BOUNDRY_METERS)
			{
				var direction = Directions.Left;// _random.NextDouble() > 0.5 ? Directions.Left : Directions.Right; 

				_weatherNotifer.PushNotifacation
				(
					new WeatherNotifcation()
					{
						MessageShownFor = 3,
						FontColor = new Color(0,0,0,255),
						Message = string.Format("HIGH WINDS\n{0}", Utils.GetDescription(direction)),
						Scale = 1
					}
				);

				_grid.ApplyWind(_weatherInfo.WindLevel(), direction);
				_windCooldown = GetWindCooldown();
			}
		}

		private double GetWindCooldown()
		{
			return _random.Next(Const.WIND_TRIGGER_MIN, Const.WIND_TRIGGER_MAX);
		}

		private void PopluateInfoBar()
		{
			var textColor = Color.Black;
			var margin = new Point(20, 5);

			var rowsToAdd = new List<Tuple<string, double>>()
			{
				new Tuple<string, double>("Snow Level {0}", _weatherInfo.AmountOfSnow()),
				new Tuple<string, double>("Drizzle Level {0}", _weatherInfo.AmountOfDrizzle()),
				new Tuple<string, double>("Rain Level {0}", _weatherInfo.AmountOfRain()),
				new Tuple<string, double>("Wind Level {0,3}", Math.Round(_weatherInfo.WindLevel())),
				new Tuple<string, double>("Lighting Level {0}", Math.Round(_weatherInfo.AmountOfThunderstrom()))
			};

			_infoBar.Add
			(
				new ScoreInfoRow()
				{
					Score = _score,
					Margin = margin,
					Color = textColor,
					Font = _contentManager.Load<SpriteFont>(@"Font/message")
				}
			);

			rowsToAdd.Where(i => i.Item2 > 0).
				ToList().
				ForEach
				( i => 
					_infoBar.Add
					(
						new TextInfoRow()
						{
							Margin = margin,
							Color = textColor,
							Message = string.Format(i.Item1, i.Item2),
							Font = _contentManager.Load<SpriteFont>(@"Font/message")
						}
					)
				);
			

		}
	}
}