//морской бой

using System.Numerics;
using System.Xml;

void PrintText(string text)
{
    Console.Write(text);
}

void NewLine()
{
    Console.WriteLine();
}

int[] Ships(int howMuchShips)
{
    int[] linkor = new int[4] { 3, 3, 3, 3 };//1
    int[] creyser = new int[3] { 3, 3, 3 };//2
    int[] esminec = new int[2] { 3, 3 };//3
    int[] kater = new int[1] { 3 };//4
    int[] stop = new int[1] { 0 };

    if (howMuchShips == 0) return linkor;
    else if (howMuchShips < 3) return creyser;
    else if (howMuchShips < 7) return esminec;
    else if (howMuchShips < 11) return kater;
    else return stop;
}

void PrintField(int[,,] arr, bool begin, int now = new int())
{
    string line = "-+-+-+-+-+-+-+-+-+-", vert = "|";
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (begin) now = arr[i, j, 1];
            else
            {
                if (arr[i, j, 1] != 0) arr[i, j, 0] = arr[i, j, 1];
                now = arr[i, j, 0];
            }

            if (now == 0) Console.Write(" ");//ничего
            else if (now == 1) Console.Write("о");//промах
            else if (now == 2) Console.Write("Ж");//попадание
            else if (now == 3)
            {
                if (begin) Console.Write(" ");//отображение корабля во время игры
                else Console.Write("#");//во время расстановки
            }
            if (j < arr.GetLength(1) - 1)
            {
                Console.Write($"{vert}");
            }
        }
        if (i < arr.GetLength(0) - 1)
        {
            NewLine();
            Console.Write(line);
            NewLine();
        }
    }
}

int CheckIndex(int[,,] arr, bool index, int now = new int())
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            now = arr[i, j, 1];

            if (now == 0)
            {
                if (index) return i;
                else return j;
            }
        }
    }
    return 0;
}

bool CheckWinner(int[,,] arr, int booms1, int booms2)
{
    if (booms1 == 20 || booms2 == 20) return true;
    return false;
}

bool Cont(bool winner, int player, bool quit)
{
    if (winner == true)
    {
        if (player == 2) PrintText("Победил игрок 1!");
        else PrintText("Победил игрок 2!");
        return false;
    }
    else if (quit == true)
    {
        PrintText("Вы вышли из игры.");
        return false;
    }
    else
    {
        if (player == 1) PrintText("Ходит игрок 1");
        else PrintText("Ходит игрок 2");
        Console.WriteLine();
        return true;
    }
}

void AddShip2(int[,,] array1, int[] array2, int indI, int indJ, bool rotation)
{

    for (int k = 0; k < array2.Length; k++)
    {
        array1[indI / 2, indJ / 2, 0] = array2[k];
        if (rotation) indI += 2;
        else indJ += 2;
    }
}

void DeleteOldShip2(int[,,] array1, int[] array2, int indI, int indJ, bool rotation, System.ConsoleKey key)
{
    int helpJ = indJ / 2, helpI = indI / 2;
    for (int k = 0; k < array2.Length; k++)
    {
        if (key == ConsoleKey.LeftArrow
        && (indI <= array1.GetLength(1) * 2 - 2 || indJ >= 0)) array1[helpI, helpJ + 1, 0] = 0;
        else if (key == ConsoleKey.UpArrow
        && (indJ <= array1.GetLength(0) * 2 - 2 || indI >= 0)) array1[helpI + 1, helpJ, 0] = 0;


        else if (key == ConsoleKey.RightArrow
           && (indJ > 0 || indJ < array1.GetLength(0) * 2 - 4)) array1[helpI, helpJ - 1, 0] = 0;
        else if (key == ConsoleKey.DownArrow
                    && (indJ >= 0 || indI <= array1.GetLength(1) * 2 - 2)) array1[helpI - 1, helpJ, 0] = 0;
        else if (key == ConsoleKey.Z)
        {
            array1[helpI, helpJ, 0] = 0;
        }
        if (rotation) helpI++;
        else helpJ++;
    }
}



bool begin = false;
int[,,] field1 = new int[10, 10, 2];
PrintField(field1, begin);
int[,,] field2 = new int[10, 10, 2];
bool quit = false;

bool PrintShips(int[,,] field)
{
    int howMuchShips = 0;
    bool rotation = false;
    while (true)
    {
        int[] actualShip = Ships(howMuchShips);
        if (actualShip[0] == 0) break;
        else
        {
            int i = CheckIndex(field, true);
            int j = CheckIndex(field, false);
            while (true)
            {
                Console.Clear();
                AddShip2(field, actualShip, i, j, rotation);
                PrintField(field, begin);
                Console.SetCursorPosition(j, i);
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow && j > 0) j -= 2;
                else if (key == ConsoleKey.RightArrow)
                {
                    if (rotation && j < field.GetLength(1) * 2 - 2) j += 2;
                    else if (!rotation &&
                        j / 2 < field.GetLength(1) - actualShip.Length) j += 2;
                }
                else if (key == ConsoleKey.UpArrow && i > 0) i -= 2;
                else if (key == ConsoleKey.DownArrow)
                {
                    if (rotation
                        && i / 2 < field.GetLength(0) - actualShip.Length) i += 2;
                    else if (!rotation &&
                        i < field.GetLength(0) * 2 - 2) i += 2;
                }
                DeleteOldShip2(field, actualShip, i, j, rotation, key);
                if (key == ConsoleKey.Spacebar)
                {
                    bool check = true;
                    int helpI = i / 2;
                    int helpJ = j / 2 - 1;
                    if (j == 0) helpJ = j / 2;

                    if (rotation)
                    {
                        helpJ = j / 2;
                        helpI = i / 2 - 1;
                        if (i == 0) helpI = i / 2;
                    }
                    for (int k = 0; k < actualShip.Length + 2; k++) //для проверки свободных ячеек вокруг корабля и корабля 
                    {
                        if (rotation)
                        {
                            if (helpI < field.GetLength(0) - 1
                                && field[helpI, helpJ, 1] != 0) check = false;
                            if (helpI > 0
                                 && helpI < field.GetLength(0) - 1
                                && field[helpI, helpJ - 1, 1] != 0) check = false;
                            if (helpJ < field.GetLength(1) - 1
                                && helpI < field.GetLength(0) - 1
                                && field[helpI, helpJ + 1, 1] != 0) check = false;
                            helpI++;
                        }
                        else
                        {
                            if (helpJ < field.GetLength(1) - 1
                                && field[helpI, helpJ, 1] != 0) check = false;
                            if (helpI > 0
                                 && helpJ < field.GetLength(1) - 1
                                && field[helpI - 1, helpJ, 1] != 0) check = false;
                            if (helpI < field.GetLength(0) - 1
                                && helpJ < field.GetLength(1) - 1
                                && field[helpI + 1, helpJ, 1] != 0) check = false;
                            helpJ++;
                        }
                        if (!check) break;
                    }

                    if (check)//проверить занятость всех клеток
                    {
                        //helpI = i / 2;
                        //helpJ = j / 2;
                        for (int k = 0; k < actualShip.Length; k++)
                        {
                            field[i / 2, j / 2, 1] = actualShip[k];
                            if (rotation) i++;
                            else j++;
                        }
                        howMuchShips++;
                        break;
                    }
                }
                else if (key == ConsoleKey.Z)
                {
                    if (rotation)
                    {
                        rotation = false;
                        if (j / 2 + actualShip.Length > field.GetLength(1))
                        {
                            j = (field.GetLength(1) - actualShip.Length) * 2;
                        }
                    }
                    else
                    {
                        rotation = true;
                        if (i + actualShip.Length > field.GetLength(0))
                        {
                            i = (field.GetLength(0) - actualShip.Length) * 2;
                        }
                    }
                }
                else if (key == ConsoleKey.Escape)
                {
                    return true;
                }
            }
        }
    }
    return false;
}
quit = PrintShips(field1);


//Console.Read();