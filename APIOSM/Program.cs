using ApiConnector.Classes;
using APITestOSM.Classes;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RestSharp;
using System.Reflection;
using System.Xml.Serialization;


namespace ApiConnector
{

    class ApiConnector
    {
        public static string apiKey = "Basic bVNreWNyYWZ0Oiwwa3VAaFJmOA==";
        public static string osmPath = "https://api.openstreetmap.org/";



        public static osm GetMap(string path, double x1, double y1, double x2, double y2)
        {
            osm osmResponse = new osm();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            RestClient client = new RestClient(
                $"{path}api/0.6/map?bbox={x1},{y1},{x2},{y2}");
            RestRequest request = new();
            request.AddHeader("Content-Type", "application/xml; charset=utf-8");
            request.AddHeader("Authorization", apiKey);
            RestResponse response = client.Execute(request);
            string? xmlResponse = response.Content;
            if (xmlResponse != "You requested too many nodes (limit is 50000). Either request a smaller area, or use planet.osm")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(osm));

                using (var sr = new StringReader(xmlResponse))
                    osmResponse = (osm)serializer.Deserialize(sr);

                // Пример извлечения данных из класса
                foreach (osmNode node in osmResponse.node)
                {
                    Console.WriteLine($"{node.id},{node.lat}:{node.lon}");
                }
                return osmResponse;
            }
            else
            {
                Console.WriteLine("Слишком большая область");
                return osmResponse;
            }
        }
/*
        [STAThread]
        public static void Draw()
        {
            Shader shader = null;
            //osm map = GetMap(osmPath, 30.1523, 59.8657, 30.5815, 59.9919);
            int VertexBufferObject = 0;
            int VertexArrayObject = 0;
            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();
            nativeWindowSettings.Vsync = VSyncMode.Adaptive;
            float[] vertices = {
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
            };

            using (var game = new GameWindow(gameWindowSettings, nativeWindowSettings))
            {
                game.Load += () =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;
                    GL.Viewport(0, 0, 800, 600);
                    VertexBufferObject = GL.GenBuffer();
                    VertexArrayObject = GL.GenVertexArray();
                    shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
                };

                game.Unload += () =>
                {
                    shader.Dispose();
                };

                game.Resize += (e) =>
                {
                    GL.Viewport(0, 0, e.Width, e.Height);
                };

                game.UpdateFrame += (e) =>
                {
                    // add game logic, input handling
                    if (game.IsKeyPressed(Keys.Escape))
                    {
                        game.Close();
                    }
                };

                game.RenderFrame += (e) =>
                {
                    
                    

                    
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.ClearColor(Color4.CornflowerBlue);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
                    GL.BindVertexArray(VertexArrayObject);
                    GL.EnableVertexAttribArray(0);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
                    GL.VertexAttribPointer(0, 36, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(0);
                    GL.UseProgram(VertexArrayObject);

                    shader.Use();
                    //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

                    Matrix4 model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(DateTime.Now.Second));
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
                    GL.End();

                    game.SwapBuffers();

                };

                // Run the game at 60 updates per second
                game.Run();
            }
        }

        */
        



        public static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Title = "Навигатор",
                Size = new Vector2i(800, 600),
                Location = new Vector2i(300, 300),
                WindowBorder = WindowBorder.Resizable,
                WindowState = WindowState.Normal,
                Flags = ContextFlags.ForwardCompatible,
                APIVersion = new Version(3,3),
                Profile = ContextProfile.Core,
                API = ContextAPI.OpenGL,
                Vsync = VSyncMode.On,
                NumberOfSamples = 0,
                
            };
            
            using(MapWindow map = new MapWindow(GameWindowSettings.Default, nativeWindowSettings))
            {
                map.Run();
            }
        }
    }

}
