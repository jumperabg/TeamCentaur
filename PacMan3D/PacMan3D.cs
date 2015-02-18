using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

struct Element
{
    //adding comment
    // define new object type with structure
    // screen position x,y
    public int x;
    public int y;
    // type of game object is defined by ASCII char
    public char skin;
    public ConsoleColor colour;
    public string direction;
}

class PacMan3D
{
    //Global variables
    public static Element enemy = new Element();
    public static Element pacMan = new Element();
    public static string[] labyrinth;

    public static int playfieldHeight;
    public static int playfieldWidth;

    //Is game over ?
    public static bool isGameOver = false;


    static void Main()
    {
        #region Memory Initialization


        //Method for creating playing field
        SetPlayfieldSize(out playfieldHeight, out playfieldWidth);

        // define hero pacMan as a variable of type element



        // initial pacman position in center of playfield
        pacMan.x = (playfieldWidth + 1) / 2 + 1;
        pacMan.y = playfieldHeight / 2;
        pacMan.skin = (char)9787; // utf8 decimal code 9787 (smile face) is our hero character
        pacMan.colour = ConsoleColor.Yellow;



        // initial pacman position in center of playfield
        enemy.x = 5;
        enemy.y = 1;
        enemy.skin = (char)9785; // utf8 decimal code 9787 (smile face) is our hero character
        enemy.colour = ConsoleColor.Red;
        enemy.direction = "right";

        // define labyrinth variable and build example
        //Test map
        labyrinth = new string[20]{
            "####################",
            "#                  #",
            "#          ####### #",
            "#  #########     # #",
            "# ##         # # # #",
            "#  # ######### # # #",
            "#  # #           # #",
            "#  # #    #### ### #",
            "# ## #       # # # #",
            "#  # # ####  # # # #",
            "#  # # #  #  # # # #",
            "#  # # ## #        #",
            "# ## #    #  # # # #",
            "#### ######  # # # #",
            "#            #     #",
            "#   #              #",
            "#  #               #",
            "#  ########### ### #",
            "#                  #",
            "####################"
        };

        #endregion
        // Main game logic
        while (true)
        {
            MoveEnemy();
            MovePacMan();

            if (isGameOver)
            {
                break;
            }
            CheckForImpact();

            PrintFrame();

            Thread.Sleep(50);  // control game speed
        }
    }

    private static void CheckForImpact()
    {
        //Check for impact
        if (enemy.x == pacMan.x && enemy.y == pacMan.y)
        {
            Console.WriteLine("Game Over !");
            isGameOver = true;
        }
    }



    private static void PrintFrame()
    {
        Console.Clear();    // fast screen clear
        PrintLabyrinth(labyrinth);
        PrintElement(pacMan);
        PrintElement(enemy);
    }

    private static void MovePacMan()
    {
        while (Console.KeyAvailable)
        {
            // we assign the pressed key value to a variable pressedKey
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            // next we start checking the value of the pressed key and take action if neccessary
            if (pressedKey.Key == ConsoleKey.LeftArrow && pacMan.x > 1) // if left arrow is pressed then
            {
                if (labyrinth[pacMan.y][pacMan.x - 1] != '#')
                {
                    pacMan.x = pacMan.x - 1;
                }
            }
            else if (pressedKey.Key == ConsoleKey.RightArrow && pacMan.x < playfieldWidth - 1)
            {
                if (labyrinth[pacMan.y][pacMan.x + 1] != '#')
                {

                    pacMan.x = pacMan.x + 1;
                }
            }
            else if (pressedKey.Key == ConsoleKey.UpArrow && pacMan.y > 1)
            {
                if (labyrinth[pacMan.y - 1][pacMan.x] != '#')
                {
                    pacMan.y = pacMan.y - 1;
                }
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow && pacMan.y < playfieldHeight - 1)
            {
                if (labyrinth[pacMan.y + 1][pacMan.x] != '#')
                {
                    pacMan.y = pacMan.y + 1;
                }
            }
        }
    }

    private static void MoveEnemy()
    {
        /*Move enemy
            1-right
            2-left
            3-up
            4-down

            */

        if (enemy.direction == "right" && (labyrinth[enemy.y][enemy.x + 1] == '#'))
        {
            enemy.direction = "down";
        }

        if (enemy.direction == "down" && (labyrinth[enemy.y + 1][enemy.x] == '#'))
        {
            enemy.direction = "left";
        }

        if (enemy.direction == "left" && (labyrinth[enemy.y][enemy.x - 1] == '#'))
        {
            enemy.direction = "up";
        }

        if (enemy.direction == "up" && labyrinth[enemy.y - 1][enemy.x] == '#')
        {
            enemy.direction = "right";
        }



        if (enemy.direction == "right")
        {
            enemy.x += 1;
        }
        if (enemy.direction == "down")
        {
            enemy.y += 1;
        }
        if (enemy.direction == "left")
        {
            enemy.x -= 1;
        }
        if (enemy.direction == "up")
        {
            enemy.y -= 1;
        }
    }

    private static void SetPlayfieldSize(out int playfieldHeight, out int playfieldWidth)
    {
        // set console size (screen resolution)
        Console.BufferHeight = Console.WindowHeight = 21;
        Console.BufferWidth = Console.WindowWidth = 40;

        // set playfield size
        playfieldHeight = Console.WindowHeight - 1;
        playfieldWidth = Console.WindowWidth - 20;
    }

    static void PrintElement(Element thisObject)
    {
        // print object of type Element
        Console.SetCursorPosition(thisObject.x, thisObject.y);
        Console.ForegroundColor = thisObject.colour;
        Console.Write(thisObject.skin);
    }

    static void PrintLabyrinth(string[] thisArray)
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Gray;
        for (int i = 0; i < thisArray.Length; i++)
        {
            Console.WriteLine(thisArray[i]);
        }
    }
}

