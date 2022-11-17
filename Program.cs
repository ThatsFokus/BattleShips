// See https://aka.ms/new-console-template for more information
Console.Title = "Battleships";

Console.WriteLine("Press any Key to bombard a random map!");
Console.ReadKey(true);

char[][] map = Helper.generateField(16, 16);

char[][] mapwithships = Helper.placeShips(map, 3, Helper.Ships.Length2);

