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
    public string lastDirection;

}

class PacMan3D
{
    //Global variables
    public static Element enemy = new Element();
    public static Element enemy2 = new Element();

    public static Element pacMan = new Element();
    public static string[] labyrinth;

    public static int playfieldHeight;
    public static int playfieldWidth;

    //Check if the game is over variable
    public static bool isGameOver = false;

    //Enemy Even move counter
    public static long enemyEvenMoveCounter = 1;

    static void Main()
    {
        #region Memory Initialization


        //Method for creating playing field
        SetPlayfieldSize(out playfieldHeight, out playfieldWidth);


        // initial pacman position in center of playfield
        pacMan.x = (playfieldWidth + 1) / 2 + 1;
        pacMan.y = playfieldHeight / 2;
        pacMan.skin = (char)9787; // utf8 decimal code 9787 (smile face) is our hero character
        pacMan.colour = ConsoleColor.Yellow;


        //Enemy stats
        enemy.x = 5;
        enemy.y = 1;
        enemy.skin = (char)9785; // utf8 decimal code 9787 (smile face) is our hero character
        enemy.colour = ConsoleColor.Red;
        enemy.direction = "right";
        enemy.lastDirection = "right";


        enemy2.x = 15;
        enemy2.y = 15;
        enemy2.skin = (char)9785; // utf8 decimal code 9787 (smile face) is our hero character
        enemy2.colour = ConsoleColor.Green;
        enemy2.direction = "left";

        // define labyrinth variable and build example
        //Test map
        labyrinth = new string[20]{
            "####################",
            "#         #        #",
            "#  ## ##  #  ####  #",
            "#  ## ##  #  ####  #",
            "#                  #",
            "#  ####      ####  #",
            "#                  #",
            "#####   ###    #####",
            "#   # #  #  #  #   #",
            "#   # ##   ##  #   #",
            "#     #  #  #      #",
            "#  #    ###    #   #",
            "#  #           #   #",
            "#     #######      #",
            "#        #         #",
            "#  ###   #    ###  #",
            "#                  #",
            "#     #######      #",
            "#        #         #",
            "####################"
        };
        /*
            "####################",
            "##        #        #",
            "#                # #",
            "#  # #### ##     # #",
            "# #            # # #",
            "# #  ######### # # #",
            "#  # #             #",
            "#  # #    #### ### #",
            "# ## #       # # # #",
            "#  # # ####  # # # #",
            "#  # # #  #  # # # #",
            "#  # # ## #        #",
            "# ## #    #  # # # #",
            "# ## ######  # # # #",
            "#            #     #",
            "#                  #",
            "#  #            # ##",
            "#  # ######### ### #",
            "#  #               #",
            "####################"
         * 
            "####################",
            "#         #        #",
            "#  ## ##  #  ####  #",
            "#  ## ##  #  ####  #",
            "#                  #",
            "#  ####      ####  #",
            "#                  #",
            "#####   ###    #####",
            "#   # #  #  #  #   #",
            "#   # ##   ##  #   #",
            "#     #  #  #      #",
            "#  #    ###    #   #",
            "#  #           #   #",
            "#     #######      #",
            "#        #         #",
            "#  ###   #    ###  #",
            "#                  #",
            "#     #######      #",
            "#        #         #",
            "####################"
         */
        #endregion
        // Main game logic
        while (true)
        {

            // Move the enemy per 2 steps
            if (enemyEvenMoveCounter % 2 == 0)
            {
                MoveEnemy1();
                MoveEnemy2();
            }

            // pacman every step
            MovePacMan();

            //Checking for impact when you add enemy you must add the enemy in this method (CheckForImpact())
            CheckForImpact();


            //CheckForImpact gives true or false on isGameOver
            if (isGameOver)
            {

                Console.Clear();
                Console.WriteLine("Game Over");
                // remove some lives points etc.
                break;
            }

            PrintFrame();
            enemyEvenMoveCounter++;
            Thread.Sleep(100);  // control game speed
        }
    }

    private static void CheckForImpact()
    {
        //Check for impact
        if ((enemy.x == pacMan.x && enemy.y == pacMan.y) || (enemy2.x == pacMan.x && enemy2.y == pacMan.y))
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
        PrintElement(enemy2);
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

    private static void MoveEnemy1()
    {
        /*Move enemy
            1-right
            2-left
            3-up
            4-down

            */
        var lastDirection = enemy.lastDirection;


        if (enemy.direction == "right" && (labyrinth[enemy.y][enemy.x + 1] == '#'))
        {
            enemy.direction = "down";
            enemy.lastDirection = "right";
        }

        if (enemy.direction == "down" && (labyrinth[enemy.y + 1][enemy.x] == '#'))
        {
            enemy.direction = "left";
            enemy.lastDirection = "down";
        }

        if (enemy.direction == "left" && (labyrinth[enemy.y][enemy.x - 1] == '#'))
        {
            enemy.direction = "up";
            enemy.lastDirection = "left";
        }

        if (enemy.direction == "up" && labyrinth[enemy.y - 1][enemy.x] == '#')
        {
            enemy.direction = "right";
            enemy.lastDirection = "up";
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
    private static void MoveEnemy2()
    {
        /*Move enemy2
            1-right
            2-left
            3-up
            4-down

            */
        var lastDirection = enemy2.lastDirection;


        if (enemy2.direction == "right" && (labyrinth[enemy2.y][enemy2.x + 1] == '#'))
        {
            enemy2.direction = "down";
            enemy2.lastDirection = "right";
        }

        if (enemy2.direction == "down" && (labyrinth[enemy2.y + 1][enemy2.x] == '#'))
        {
            enemy2.direction = "left";
            enemy2.lastDirection = "down";
        }

        if (enemy2.direction == "left" && (labyrinth[enemy2.y][enemy2.x - 1] == '#'))
        {
            enemy2.direction = "up";
            enemy2.lastDirection = "left";
        }

        if (enemy2.direction == "up" && labyrinth[enemy2.y - 1][enemy2.x] == '#')
        {
            enemy2.direction = "right";
            enemy2.lastDirection = "up";
        }




        if (enemy2.direction == "right")
        {
            enemy2.x += 1;
        }
        if (enemy2.direction == "down")
        {
            enemy2.y += 1;
        }
        if (enemy2.direction == "left")
        {
            enemy2.x -= 1;
        }
        if (enemy2.direction == "up")
        {
            enemy2.y -= 1;
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

