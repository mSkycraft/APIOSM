using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace APITestOSM.Classes
{

    public class MapWindow : GameWindow
    {
        private float frameTime = 0f;
        private int fps = 0;
        public MapWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));
        }
        //Начальные параметры
        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);
            base.OnLoad();
        }
        //Изменение размера
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }
        //Логика
        protected override void OnUpdateFrame(FrameEventArgs args)
        {

            frameTime += (float)args.Time;
            fps++;
            if(frameTime >= 1)
            {
                Title = $"Навигатор - fps:{fps}";
                frameTime = 0f;
                fps = 0;
            }
            var key = KeyboardState;
            if(KeyboardState.IsKeyPressed(Keys.Escape))
            {
                base.Close();
            }
            base.OnUpdateFrame(args);
        }
        //Рисование
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }
    }
}
