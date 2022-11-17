class Helper{
	public static readonly char Water = '#';
	public static readonly char ShipHit = '+';
	public static readonly char HiddenShip = '~';
	public static readonly char ShipMiss = 'O';

	public static char[][] generateField(int w, int h){
		char[][] map = new char[w][];
		for (int i = 0; i < map.Length; i++){
			for(int j = 0; j < h; j++){
			map[i][j] = Water;
			}
		}

		return map;
	}

	public static char[][] placeShips(char[][] map, int amount, Ships type){
		
		var random = new Random(((int)DateTime.Now.Ticks));
		
		for (int i = 0; i < amount; i++){
			bool placing = true;
			while (placing){
				switch (type)
				{
					case Ships.Length2:
						int randx = random.Next(map.Length - 2);
						int randy = random.Next(map[0].Length-2);
						if(random.Next(2) == 0){
							//vertical
						}else{
							//horizontal
							for (int x = randx; randx < randx + 2;x++){
								if (map[x][randy] != Water) break;
							}

							for (int x = randx; randx < randx + 2; x++){
								map[x][randy] = HiddenShip;
							}
						}
						break;
					case Ships.Length3:
						break;
					case Ships.Length4:
						break;
					case Ships.Length5:
						break;
					default:
						Console.WriteLine("[ERROR] Wrong ship type");
						break;
				}
			}
		}
		return map;
	}

	public enum Ships
	{
		Length2 = 0,
		Length3 = 1,
		Length4 = 2,
		Length5 = 3
	}
}