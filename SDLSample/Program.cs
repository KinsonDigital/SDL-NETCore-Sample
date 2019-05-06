using SDL2;
using System;

namespace SDLTesting
{
    class Program
    {
        /*INFO
            SDL2-CS.dll is a dll created by compiling the C# NET wrapper project on github.
            GitHub Repo: https://github.com/flibitijibibo/SDL2-CS

            SDL2.dll is the original SDL2 library written in C and C++.  SDL2-CS is the wrapper/binding that
            calls into SDL2.dll.
         */

        static void Main(string[] args)
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Unable to initialize SDL. Error: {0}", SDL.SDL_GetError());
            }
            else
            {
                var window = SDL.SDL_CreateWindow(".NET Core SDL2-CS Tutorial",
                    SDL.SDL_WINDOWPOS_CENTERED,
                    SDL.SDL_WINDOWPOS_CENTERED,
                    1020,
                    800,
                    SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
                );

                if (window == IntPtr.Zero)
                {
                    Console.WriteLine("Unable to create a window. SDL. Error: {0}", SDL.SDL_GetError());
                }
                else
                {
                    //SDL.SDL_Delay(5000);

                    bool quit = false;

                    while (!quit)
                    {
                        SDL.SDL_Event e;

                        while (SDL.SDL_PollEvent(out e) != 0)
                        {
                            switch (e.type)
                            {
                                case SDL.SDL_EventType.SDL_QUIT:
                                    quit = true;
                                    break;

                                case SDL.SDL_EventType.SDL_KEYDOWN:

                                    switch (e.key.keysym.sym)
                                    {
                                        case SDL.SDL_Keycode.SDLK_q:
                                            quit = true;
                                            break;
                                    }

                                    break;
                            }
                        }
                    }
                }

                SDL.SDL_DestroyWindow(window);
                SDL.SDL_Quit();
            }
        }
    }
}
