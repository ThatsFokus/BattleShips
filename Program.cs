class Program
{
    private readonly char Water = '~';
	private readonly char ShipHit = '+';
	private readonly char HiddenShip = '#';
	private readonly char ShipMiss = 'O';
	private readonly int[] FieldSize = {10, 10};

    private static void Main(string[] args){
        new Program().run();
    }

    public void run(){
		Console.OutputEncoding = System.Text.Encoding.Unicode;
		Console.Clear();
        Console.Title = "Battleships";
		Console.WriteLine("There will be 1 2TileShip, 2 3TileShip, 1 4TileShip, 1 5TileShip");
        Console.WriteLine("Press any Key to bombard a random map!");
        Console.ReadKey(true);
        bool playing = true;

        while (playing){
			
            char[,] map = generateField();

            char[,] mapwithships = placeShips(generateField(), 1, Ships.Length2);
			mapwithships = placeShips(mapwithships, 2, Ships.Length3);
			mapwithships = placeShips(mapwithships, 1, Ships.Length4);
			mapwithships = placeShips(mapwithships, 1, Ships.Length5);
			Console.Clear();

            drawMapToScreen(map);
			bool InRound = true;
			int shotsTaken = 0;
			Console.SetCursorPosition(1,1);
			while(InRound){
				Console.CursorVisible = true;
				var cursor = Console.GetCursorPosition();
				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.UpArrow:
						if(cursor.Top <= 1) cursor.Top = FieldSize[1];
						else cursor.Top -= 1;
						break;
					case ConsoleKey.DownArrow:
						if(cursor.Top >= FieldSize[1]) cursor.Top = 1;
						else cursor.Top += 1;
						break;
					case ConsoleKey.RightArrow:
						if(cursor.Left >= FieldSize[0]) cursor.Left = 1;
						else cursor.Left += 1;
						break;
					case ConsoleKey.LeftArrow:
						if(cursor.Left <= 1) cursor.Left = FieldSize[0];
						else cursor.Left -= 1;
						break;
					
					case ConsoleKey.Spacebar:
						Console.Write(takeShot(cursor.Left -1, cursor.Top - 1, mapwithships, map));
						Console.ResetColor();
						shotsTaken++;
						Console.SetCursorPosition(FieldSize[0] + 5, 0);
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
				if (checkWinState(map, mapwithships))InRound = false;
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

	private char takeShot(int x, int y, char[, ] mapwithships, char[,] map){
		if (mapwithships[x, y] == HiddenShip) {
			Console.ForegroundColor = ConsoleColor.Green;
			map[x,y] = ShipHit;
			return ShipHit;
		}
		Console.ForegroundColor = ConsoleColor.Red;
		map[x,y] = ShipHit;
		return ShipMiss;
	}

	private bool checkWinState(char[,] map, char[,] mapwithships){
		bool state = true;
		for (int x = 0; x < FieldSize[0]; x++){
			for (int y = 0; y < FieldSize[1]; y++){
				if (mapwithships[x, y] == HiddenShip && map[x, y] != ShipHit) state = false;
			}
		}
		return state;
	}
    private char[,] generateField(){
		char[,] map = new char[FieldSize[0], FieldSize[1]];
		for (int i = 0; i < FieldSize[0]; i++){
			for(int j = 0; j < FieldSize[1]; j++){
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
					
					int randx = random.Next(FieldSize[0]);
					int randy = random.Next(FieldSize[1] - 2);
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
					int randx = random.Next(FieldSize[0] - 2);
					int randy = random.Next(FieldSize[1]);
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
					
					int randx = random.Next(FieldSize[0]);
					int randy = random.Next(FieldSize[1] - 3);
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
					int randx = random.Next(FieldSize[0] - 3);
					int randy = random.Next(FieldSize[1]);
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
					
					int randx = random.Next(FieldSize[0]);
					int randy = random.Next(FieldSize[1] - 4);
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
					int randx = random.Next(FieldSize[0] - 4);
					int randy = random.Next(FieldSize[1]);
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
					
					int randx = random.Next(FieldSize[0]);
					int randy = random.Next(FieldSize[1] - 5);
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
					int randx = random.Next(FieldSize[0] - 5);
					int randy = random.Next(FieldSize[1]);
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

	public void drawMapToScreen(char[,] map){
		for (int x = 0; x <= FieldSize[0] + 1; x++){
			for (int y = 0; y <= FieldSize[1] + 1; y++){
                Console.SetCursorPosition(x,y);
				if(x == 0 || x == FieldSize[0] + 1){
					Console.Write('|');
                    continue;
				}
				if(y == 0 || y == FieldSize[1] + 1){
					Console.Write('-');
                    continue;
				}
				if(map[x-1, y-1] == Water){
					Console.ForegroundColor = ConsoleColor.Blue;
				}
                Console.Write(map[x-1, y-1]);
				Console.ResetColor();
			}
		}

		Console.SetCursorPosition(FieldSize[0] + 5, 0);
		Console.Write("Fired cannonballs: 0");

		Console.SetCursorPosition(FieldSize[0] + 5, 3);
		Console.Write("Movement:");

		Console.SetCursorPosition(FieldSize[0] + 9, 4);
		//Console.Write((char)708);
		Console.Write((char)8593);
		Console.SetCursorPosition(FieldSize[0] + 7, 5);
		Console.Write($"{(char)8592}   {(char)8594}");
		//Console.Write((char)706);
		//Console.SetCursorPosition(FieldSize[0] + 10, 5);
		//Console.Write((char)707);
		
		Console.SetCursorPosition(FieldSize[0] + 9, 6);
		//Console.Write((char)709);
		Console.Write((char)8595);

		Console.SetCursorPosition(FieldSize[0] + 5, 8);
		Console.Write("Space to shoot");
	}
	
}

public enum Ships
	{
		Length2 = 0,
		Length3 = 1,
		Length4 = 2,
		Length5 = 3
	}