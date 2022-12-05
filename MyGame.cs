using SkiaSharp;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Input;
using Silk.NET.OpenAL;
class MyGame
{
	public int SizeX;
	public int SizeY;
	private IWindow window;
	private SKSurface surface;
	private SKCanvas canvas;
	private GL gl;
	private AL al;
	private int cellSize = 50;

	private int distance = 50;

	private SKPaint paint;
	private List<Key> pressedKeys;
	private Program battleships;
	private int state = 0;

	private void getAsset(out SKImage texture, string path){
		SKData data = SKData.Create(path);
		texture = SKImage.FromEncodedData(data);
		if(texture == null) Console.WriteLine($"[ASSET LOADER] '{path}' asset couldn't be read");
	}

	private void playMusic(string path){
		var buffer = al.GenBuffer();
		var buffers = new uint[1];
		var source = al.GenSource();
		al.SetSourceProperty(source, SourceBoolean.Looping, true);
		al.SetSourceProperty(source, SourceVector3.Position, 0, 0, 0);
		var data = File.ReadAllBytes(path);
		al.BufferData(buffer, BufferFormat.Mono8, data, 1);
		buffers[0] = buffer;
		al.SourceQueueBuffers(source, buffers);
		al.SourcePlay(source);
		Console.WriteLine($"{data} \n PLaying Audio");
	}

	public MyGame(int width, int height, string title){
		var options = WindowOptions.Default;
		SizeX = width;
		SizeY = height;
		options.Title = title; 
		options.Size = new Silk.NET.Maths.Vector2D<int>(width, height);
		options.ShouldSwapAutomatically = false;
		window = Window.Create(options);
		window.FramesPerSecond = 60;
		window.Update += OnUpdate;
		window.Render += OnRender;
		window.Load += OnLoad;
		window.Closing += onClosing;
	}

	private void OnUpdate(double arg1){
		switch (state)
		{
			case 0:
				if(pressedKeys.Remove(Key.Space)){
					state = 1;
					battleships.GenNewField();
				}
				break;
			case 1:
			//just wait here
				break;
			case 2:
			if(pressedKeys.Remove(Key.Space))state = 0;
				break;
			default:
				state = 0;
				break;
		}
	}

	private void OnRender(double arg1){
		switch (state)
		{
			case 0:
				paint.Color = SKColors.Black;
				paint.TextSize = 60;
				canvas.DrawText("Battleships", (float)SizeX / 2 - paint.MeasureText("Battleships") / 2, 200, paint);
				paint.TextSize = 30;
				canvas.DrawText("Press Space to Play", (float)SizeX / 2 - paint.MeasureText("Press Space to Play") / 2, 500, paint);
				break;
			case 1:
				showMap();
				break;
			case 2:
				paint.Color = SKColors.ForestGreen;
				paint.TextSize = 50;
				canvas.DrawText("Winner", (float)SizeX / 2 - paint.MeasureText("Winner") / 2, 100, paint);
				paint.Color = SKColors.Black;
				paint.TextSize = 40;
				canvas.DrawText($"You had to shoot {battleships.ShotsTaken} times", (float)SizeX / 2 - paint.MeasureText($"You had to shoot {battleships.ShotsTaken} times") / 2, 300, paint);
				paint.TextSize = 30;
				canvas.DrawText("Press Space to continue", (float)SizeX / 2 - paint.MeasureText("Press Space to continue") / 2, 500, paint);
				break;
			default:
				state = 0;
				break;
		}
		swapBuffers();
	}

	private void showMap(){
		SKPaint water = new SKPaint(){
			Color = SKColors.Aquamarine
		};
		SKPaint hit = new SKPaint(){
			Color = SKColors.SeaGreen
		};
		SKPaint miss = new SKPaint(){
			Color = SKColors.DarkRed
		};
		SKPaint bg = new SKPaint(){
			Color = SKColors.Black
		};
		for (int x = 0; x < Program.FieldSize; x++){
			for (int y = 0; y < Program.FieldSize; y++){
				switch (battleships.Map[x,y])
				{
					case Program.Water:
						canvas.DrawRect(distance + x * cellSize, distance + y * cellSize, cellSize, cellSize, bg);
						canvas.DrawRect(distance + 1 + x * cellSize, distance + 1 + y * cellSize, cellSize-2, cellSize-2, water);
						break;
					case Program.ShipHit:
						canvas.DrawRect(distance + x * cellSize, distance + y * cellSize, cellSize, cellSize, bg);
						canvas.DrawRect(distance + 1 + x * cellSize, distance + 1 + y * cellSize, cellSize-2, cellSize-2, hit);
						break;
					case Program.ShipMiss:
						canvas.DrawRect(distance + x * cellSize, distance + y * cellSize, cellSize, cellSize, bg);
						canvas.DrawRect(distance + 1 + x * cellSize, distance + 1 + y * cellSize, cellSize-2, cellSize-2, miss);
						break;
					//TODO maybe add an indicator that the ship is sunk
					default:
						break;
				}
			}
		}
		bg.TextSize = 30;
		canvas.DrawText($"Shots Fired: {battleships.ShotsTaken}", distance * 2 + Program.FieldSize * cellSize, distance * 2, bg);
	}

	private void OnLoad(){

		//create and bind input context
		var input = window.CreateInput();

		foreach (var keyboard in input.Keyboards){
			keyboard.KeyDown += OnKeyDown;
			keyboard.KeyUp += OnKeyUp;
		}

		foreach (var mouse in input.Mice){
			mouse.MouseDown += OnMouseDown;
			mouse.MouseMove += OnMouseMove;
			mouse.Scroll += OnMouseScroll;
		}

		//create and configure OpenGL context
		gl = window.CreateOpenGL();
		gl.ClearColor(1f, 1f, 1f, 1f);

		//create SkiaSharp context
		var grinterface = GRGlInterface.CreateOpenGl(name => {
			if (window.GLContext.TryGetProcAddress(name, out nint fn)) return fn;
			return (nint)0;
		});

		var skiabackendcontext = GRContext.CreateGl(grinterface);
		var format = SKColorType.Rgba8888;
		var backendrendertarget = new GRBackendRenderTarget(window.Size.X, window.Size.Y, window.Samples ?? 1, window.PreferredStencilBufferBits ?? 16, new GRGlFramebufferInfo(
			0, format.ToGlSizedFormat()
		));
		//var info = new SKImageInfo(window.Size.X, window.Size.Y);
		//surface = SKSurface.Create(info);
		surface = SKSurface.Create(skiabackendcontext, backendrendertarget, format);
		canvas = surface.Canvas;
		var typeface = SKTypeface.CreateDefault();
		paint = new SKPaint(new SKFont(typeface));

		//set up audio
		al = AL.GetApi();
		var alContext = ALContext.GetApi();
		al.Enable(Capability.Invalid);
		al.SetListenerProperty(ListenerVector3.Position, 0, 0, 0);
		al.SetListenerProperty(ListenerFloat.Gain, 1);

		//add gameObjects
		//add ground
		createObjects();
	}

	public void Run(){
		window.Run();
	}

	private void OnKeyDown(IKeyboard arg1, Key arg2, int arg3){
		if (arg2 == Key.Escape) window.Close();
		if (!pressedKeys.Contains(arg2)) pressedKeys.Add(arg2);
	}

	private void OnKeyUp(IKeyboard arg1, Key arg2, int arg3){
		pressedKeys.Remove(arg2);
	}

	private void OnMouseMove(IMouse arg1, System.Numerics.Vector2 arg2){

	}

	private void OnMouseDown(IMouse arg1, MouseButton arg2){
		if(arg1.Position.X >= 50 && arg1.Position.X <= 50 + Program.FieldSize * cellSize && arg1.Position.Y >= 50 && arg1.Position.Y <= 50 + Program.FieldSize * cellSize && state == 1){
			float x = (arg1.Position.X - distance) / cellSize;
			float y = (arg1.Position.Y - distance) / cellSize;
			x -= x % 1;
			y -= y % 1;
			battleships.TakeShot((int)x, (int)y);
			if(battleships.HasWon) state = 2;
		}
	}

	private void OnMouseScroll(IMouse arg1, ScrollWheel arg2){

	}
	
	private void swapBuffers(){
		canvas.Flush();
		window.SwapBuffers();
		canvas.Clear(SKColors.DarkSlateGray); // set the background color here
	}

	private void createObjects(){
		//create all variables
		pressedKeys = new List<Key>();
		battleships = new Program();
		playMusic("./Assets/Music/Background.wav");
	}

	void onClosing(){
	}
}