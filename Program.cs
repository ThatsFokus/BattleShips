class Program
{
    public const char Water = '~';
	public const char ShipHit = '+';
	private const char HiddenShip = '#';
	public const char ShipMiss = 'O';
	public const int FieldSize = 10;
	private char[,] map;
	private char[,] mapwithships;
	private int shotsTaken;
	public int ShotsTaken{
		get{return shotsTaken;}
	}
	public char[,] Map{
		get{ return map;}
	}

	private bool hasWon;
	public bool HasWon{
		get{return hasWon;}
	}


    private static void Main(string[] args){
        new MyGame(800, 600, "Battleships").Run();
    }

	public Program(){
		GenNewField();
	}

	public void GenNewField(){
		map = generateField();
		mapwithships = placeShips(generateField(), 1, Ships.Length2);
		mapwithships = placeShips(mapwithships, 2, Ships.Length3);
		mapwithships = placeShips(mapwithships, 1, Ships.Length4);
		mapwithships = placeShips(mapwithships, 1, Ships.Length5);
		shotsTaken = 0;
		hasWon = false;
	}
    public void run(){
        bool playing = true;

        while (playing){
			
            GenNewField();
			Console.Clear();

			bool InRound = true;
			shotsTaken = 0;
			Console.SetCursorPosition(1,1);
			while(InRound){
				Console.CursorVisible = true;
				var cursor = Console.GetCursorPosition();
				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.UpArrow:
						if(cursor.Top <= 1) cursor.Top = FieldSize;
						else cursor.Top -= 1;
						break;
					case ConsoleKey.DownArrow:
						if(cursor.Top >= FieldSize) cursor.Top = 1;
						else cursor.Top += 1;
						break;
					case ConsoleKey.RightArrow:
						if(cursor.Left >= FieldSize) cursor.Left = 1;
						else cursor.Left += 1;
						break;
					case ConsoleKey.LeftArrow:
						if(cursor.Left <= 1) cursor.Left = FieldSize;
						else cursor.Left -= 1;
						break;
					
					case ConsoleKey.Spacebar:
						shotsTaken++;
						Console.SetCursorPosition(FieldSize + 5, 0);
						Console.Write($"Fired cannonballs: {shotsTaken}");
						break;
					case ConsoleKey.Escape:
						playing = false;
						InRound = false;
						break;
					default:
						break;
				}
				Console.SetCursorPosition(cursor.Left, cursor.Top);
				if (checkWinState())InRound = false;
			}
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"You took {shotsTaken} shots!");
			Console.WriteLine("Winner");
			Console.ResetColor();
			Console.CursorVisible = false;
			Console.WriteLine("Play another round? (y/n)");
			if (Console.ReadKey().Key == ConsoleKey.N){
				playing = false;
				Console.Clear();
				Console.ResetColor();
			}
        }
    }

	public void TakeShot(int x, int y){

		if(map[x, y] != Water) return;
		shotsTaken++;
		if (mapwithships[x, y] == HiddenShip) {
			map[x,y] = ShipHit;
			hasWon = checkWinState();
			return;
		}
		map[x,y] = ShipMiss;
	}

	private bool checkWinState(){
		bool state = true;
		for (int x = 0; x < FieldSize; x++){
			for (int y = 0; y < FieldSize; y++){
				if (mapwithships[x, y] == HiddenShip && map[x, y] != ShipHit) state = false;
			}
		}
		return state;
	}
    private char[,] generateField(){
		char[,] map = new char[FieldSize, FieldSize];
		for (int i = 0; i < FieldSize; i++){
			for(int j = 0; j < FieldSize; j++){
			    map[i,j] = Water;
			}
		}

		return map;
	}

	private char[,] placeShips(char[,] maps, int amount, Ships type){
		char[, ] map = maps;
		var random = new Random(((int)DateTime.Now.Ticks));
		int placedShips = 0;
		int tries = 0;
		while (placedShips < amount && tries < 20){
			switch (type)
			{
			case Ships.Length2:
				if(random.Next(2) == 0){ //vertical
					
					int randx = random.Next(FieldSize);
					int randy = random.Next(FieldSize - 2);
					bool valid = true;
					for (int y = randy; y < randy + 2; y++){
						if(map[randx, y] != Water) valid = false;
					}
					if (valid){
						for (int y = randy; y < randy + 2; y++){
							map[randx, y] = HiddenShip;
						}
						placedShips++;
					}
				}else{ //horizontal
					int randx = random.Next(FieldSize - 2);
					int randy = random.Next(FieldSize);
					bool valid = true;
					for (int x = randx; x < randx + 2;x++){
						if (map[x, randy] != Water) valid = false;
					}
					if(valid){
						for (int x = randx; x < randx + 2; x++){
							map[x, randy] = HiddenShip;
						}
						placedShips++;
					}
				}
				break;

				case Ships.Length3:
					if(random.Next(2) == 0){ //vertical
					
					int randx = random.Next(FieldSize);
					int randy = random.Next(FieldSize - 3);
					bool valid = true;
					for (int y = randy; y < randy + 3; y++){
						if(map[randx, y] != Water) valid = false;
					}
					if (valid){
						for (int y = randy; y < randy + 3; y++){
							map[randx, y] = HiddenShip;
						}
						placedShips++;
					}
				}else{ //horizontal
					int randx = random.Next(FieldSize - 3);
					int randy = random.Next(FieldSize);
					bool valid = true;
					for (int x = randx; x < randx + 3;x++){
						if (map[x, randy] != Water) valid = false;
					}
					if(valid){
						for (int x = randx; x < randx + 3; x++){
							map[x, randy] = HiddenShip;
						}
						placedShips++;
					}
				}
				break;

				case Ships.Length4:
					if(random.Next(2) == 0){ //vertical
					
					int randx = random.Next(FieldSize);
					int randy = random.Next(FieldSize - 4);
					bool valid = true;
					for (int y = randy; y < randy + 4; y++){
						if(map[randx, y] != Water) valid = false;
					}
					if (valid){
						for (int y = randy; y < randy + 4; y++){
							map[randx, y] = HiddenShip;
						}
						placedShips++;
					}
				}else{ //horizontal
					int randx = random.Next(FieldSize - 4);
					int randy = random.Next(FieldSize);
					bool valid = true;
					for (int x = randx; x < randx + 4;x++){
						if (map[x, randy] != Water) valid = false;
					}
					if(valid){
						for (int x = randx; x < randx + 4; x++){
							map[x, randy] = HiddenShip;
						}
						placedShips++;
					}
				}
				break;

				case Ships.Length5:
					if(random.Next(2) == 0){ //vertical
					
					int randx = random.Next(FieldSize);
					int randy = random.Next(FieldSize - 5);
					bool valid = true;
					for (int y = randy; y < randy + 5; y++){
						if(map[randx, y] != Water) valid = false;
					}
					if (valid){
						for (int y = randy; y < randy + 5; y++){
							map[randx, y] = HiddenShip;
						}
						placedShips++;
					}
				}else{ //horizontal
					int randx = random.Next(FieldSize - 5);
					int randy = random.Next(FieldSize);
					bool valid = true;
					for (int x = randx; x < randx + 5;x++){
						if (map[x, randy] != Water) valid = false;
					}
					if(valid){
						for (int x = randx; x < randx + 5; x++){
							map[x, randy] = HiddenShip;
						}
						placedShips++;
					}
				}
				break;

				default:
					Console.WriteLine("[ERROR] Wrong ship type");
					break;
			}
		}
		return map;
	}
}

public enum Ships
	{
		Length2 = 0,
		Length3 = 1,
		Length4 = 2,
		Length5 = 3
	}