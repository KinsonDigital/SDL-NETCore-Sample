using SDL2;
using System;
using System.IO;
using System.Reflection;

namespace SDLTesting
{
    public class Program
    {
        private static string _contentPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Content";

        /*INFO
            SDL2-CS.dll is a dll created by compiling the C# NET wrapper project on github.
            GitHub Repo: https://github.com/flibitijibibo/SDL2-CS

            SDL2.dll is the original SDL2 library written in C and C++.  SDL2-CS is the wrapper/binding that
            calls into SDL2.dll.

            Useful Links:
                1. https://samulinatri.com/blog/net-core-sdl2-window-creation/
                2. https://samulinatri.com/blog/net-core-sdl2-image-rendering/
         */

        static void Main(string[] args)
        {
            //Check to make sure that the video card can be initialized
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Unable to initialize SDL. Error: {0}", SDL.SDL_GetError());
            }
            else
            {
                //Create a window
                var windowPtr = SDL.SDL_CreateWindow(".NET Core SDL2-CS Tutorial",
                    SDL.SDL_WINDOWPOS_CENTERED,
                    SDL.SDL_WINDOWPOS_CENTERED,
                    1020,
                    800,
                    SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
                );

                //Create a renderer for rendering graphics to the screen
                var rendererPtr = SDL.SDL_CreateRenderer(windowPtr, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
                var texturePtr = SDL_image.IMG_LoadTexture(rendererPtr, $@"{_contentPath}\Graphics\OrangeBox.png");

                //The section of the image to render
                SDL.SDL_Rect srcRect;
                srcRect.x = 0;
                srcRect.y = 0;
                srcRect.w = 25;
                srcRect.h = 25;

                //The location on the surface of where to render the image
                SDL.SDL_Rect targetRect;
                targetRect.x = 100;
                targetRect.y = 100;
                targetRect.w = 25;
                targetRect.h = 25;

                if (windowPtr == IntPtr.Zero)
                {
                    Console.WriteLine("Unable to create a window. SDL. Error: {0}", SDL.SDL_GetError());
                }
                else
                {
                    //SDL.SDL_Delay(5000); //This can be used to delay the update of SDL

                    bool quit = false;

                    while (!quit)
                    {
                        while (SDL.SDL_PollEvent(out var e) != 0)
                        {
                            //Check for which type event the window has thrown
                            switch (e.type)
                            {
                                case SDL.SDL_EventType.SDL_QUIT://Quit app event
                                    quit = true;
                                    break;
                                case SDL.SDL_EventType.SDL_KEYDOWN://Key event
                                    //Check for various key input
                                    switch (e.key.keysym.sym)
                                    {
                                        case SDL.SDL_Keycode.SDLK_q:
                                            quit = true;
                                            break;
                                    }
                                    break;
                            }
                        }

                        //Clear the rendering surface, which is the window
                        SDL.SDL_RenderClear(rendererPtr);

                        //Copies the texture onto the render surface.  It is not visible at this point
                        SDL.SDL_RenderCopy(rendererPtr, texturePtr, ref srcRect, ref targetRect);

                        //Actually display the results onto the rendering texture
                        SDL.SDL_RenderPresent(rendererPtr);
                    }
                }

                //Destroy the texture, renderer and window
                SDL.SDL_DestroyTexture(rendererPtr);
                SDL.SDL_DestroyRenderer(rendererPtr);
                SDL.SDL_DestroyWindow(windowPtr);

                //And quit the application
                SDL.SDL_Quit();
            }
        }
    }
}
