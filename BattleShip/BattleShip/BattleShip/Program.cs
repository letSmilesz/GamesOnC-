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

dynamic UserEnter()
{
    string a = Console.ReadLine();
    if (a != "")
    {
        if (int.TryParse(a, out int n))
        {
            int b = Convert.ToInt32(a);
            return b;
        }
        else if (a == "y") return true;
        else if (a == "n") return false;
        else return a;
    }
    else
    {
        a = "exit";
        return a;
    }
}

int[,] Ships(int howMuchShips)
{
    int[,] linkor = new int[1, 4] { { 3, 3, 3, 3 } };//1
    int[,] kreyser = new int[1, 3] { { 3, 3, 3 } };//2  
    int[,] esminec = new int[1, 2] { { 3, 3 } };//3
    int[,] kater = new int[1, 1] { { 3 } };//4
    int[,] stop = new int[1, 1] { { 0 } };

    if (howMuchShips == 0) return linkor;
    else if (howMuchShips < 3) return kreyser;
    else if (howMuchShips < 7) return esminec;
    else if (howMuchShips < 11) return kater;
    else return stop;
}

void PrintField(int[,,] arr, bool begin, int left, int now = new int())
{
    string line = "-+-+-+-+-+-+-+-+-+-", vert = "|";
    int indI = 0;
    for (int i = 1; i < arr.GetLength(0) - 1; i++)
    {
        Console.SetCursorPosition(left, indI++);
        for (int j = 1; j < arr.GetLength(1) - 1; j++)
        {
            if (begin) now = arr[i, j, 1];
            else
            {
                if (arr[i, j, 1] != 0) arr[i, j, 0] = arr[i, j, 1];
                now = arr[i, j, 0];
            }

            if (now == 0) PrintText(" ");//ничего
            else if (now == 1) PrintText("о");//промах
            else if (now == 2) PrintText("Ж");//попадание
            else if (now == 3)
            {
                if (begin) PrintText(" ");//отображение корабля во время игры
                else PrintText("#");//во время расстановки
            }
            if (j < arr.GetLength(1) - 1)
            {
                PrintText($"{vert}");
            }
        }
        if (i < arr.GetLength(0) - 1)
        {
            Console.SetCursorPosition(left, indI++);
            PrintText(line);
        }
    }
}

int CheckIndex(int[,,] arr, bool begin, bool row, int now = new int())
{
    for (int i = 1; i < arr.GetLength(0) - 1; i++)
    {
        for (int j = 1; j < arr.GetLength(1) - 1; j++)
        {
            now = arr[i, j, 1];
            if (begin)
            {
                if (now == 0 || now == 3)
                {
                    if (row) return i;
                    else return j;
                }
            }
            else if (now == 0)
            {
                if (row) return i;
                else return j;
            }
        }
    }
    return 0;
}

void AddShip(int[,,] array1, int[,] array2, int i, int j, int layer)
{
    for (int k = 0; k < array2.GetLength(0); k++)
    {
        int helpJ = j;
        for (int l = 0; l < array2.GetLength(1); l++)
        {
            array1[i, helpJ, layer] = array2[k, l];
            helpJ++;
        }
        i++;
    }
}

void DeleteOldShip2(int[,,] array1)
{
    for (int i = 0; i < array1.GetLength(0); i++)
    {
        for (int j = 0; j < array1.GetLength(1); j++)
        {
            array1[i, j, 0] = 0;
        }
    }
}

bool CheckFreeCells(int[,,] arr, int[,] arr2, int i, int j)
{
    for (int k = 0; k < arr2.GetLength(0) + 2; k++) //для проверки свободных ячеек вокруг корабля и корабля 
    {
        int helpJ = j;
        for (int l = 0; l < arr2.GetLength(1) + 2; l++)
        {
            if (arr[i, helpJ, 1] != 0) return false;
            helpJ++;
        }
        i++;
    }
    return true;
}

string AskName(int n)
{
    PrintText($"По умолчанию имя 'Игрок {n}'. \n Если хотите, то введите имя игрока {n}: ");
    string name = UserEnter();
    return name;
}

int[,] Rotation(int[,] arr)
{
    int[,] arr2 = new int[arr.GetLength(1), arr.GetLength(0)];
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            arr2[j, i] = arr[i, j];
        }
    }
    return arr2;
}

void CheckWholeShip(int[,,] array, int i, int j)
{
    while (true)
    {
        if (array[i - 1, j, 1] == 2) i--;
        else if (array[i, j - 1, 1] == 2) j--;
        else if (array[i - 1, j, 1] == 3 || array[i, j - 1, 1] == 3) return;
        else break;
    }
    int helpI = i, helpJ = j, right = 0, down = 0;
    while (true)
    {
        if (array[helpI + 1, helpJ, 1] == 2)
        {
            if (array[helpI + 1, helpJ, 1] == 2) down++;
            helpI++;
        }
        else if (array[helpI, helpJ + 1, 1] == 2)
        {
            if (array[helpI, helpJ + 1, 1] == 2) right++;
            helpJ++;
        }
        else if (array[helpI, helpJ + 1, 1] == 3 || array[helpI + 1, helpJ, 1] == 3) return;
        else break;
    }
    int length = 3, height = down + 3;
    if (right > down)
    {
        length = right + 3;
        height = 3;
    }
    for (int k = 0; k < height; k++)
    {
        helpJ = j - 1;
        for (int l = 0; l < length; l++)
        {
            if (array[i - 1, helpJ, 1] != 2) array[i - 1, helpJ, 1] = 1;
            helpJ++;
        }
        i++;
    }
}

bool begin = false;
int[,,] field1 = new int[12, 12, 2];
int[,,] field2 = new int[12, 12, 2];
int[] booms = new int[2];
PrintText("Введите имена игроков. Если не хотите, то оставьте поля пустыми. \n");
string player1 = AskName(1);
if (player1 == "exit") player1 = "Игрок 1";
NewLine();
string player2 = AskName(2);
if (player2 == "exit") player2 = "Игрок 2";
string winner = String.Empty;

bool PrintShips(int[,,] field, string player)
{
    int howMuchShips = 0;
    while (true)
    {
        int[,] actualShip = Ships(howMuchShips);
        if (actualShip[0, 0] == 0) return false;
        else
        {
            int i = CheckIndex(field, begin, true);
            int j = CheckIndex(field, begin, false);
            while (true)
            {
                Console.Clear();
                AddShip(field, actualShip, i, j, 0);
                PrintField(field, begin, 0);
                NewLine();
                PrintText($"Поле {player}. \nДля перемещения курсора используйте стрелочки. \nДля поворота корабля - 'Z/Я'. " +
                    $"\nДля сохранения позиции корабля - пробел. \nКорабли не могут располагаться впритык друг к другу.");
                Console.SetCursorPosition((j - 1) * 2, (i - 1) * 2);
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow && j > 1) j--;
                else if (key == ConsoleKey.RightArrow
                    && (j < field.GetLength(1) - actualShip.GetLength(1) - 1)) j++;
                else if (key == ConsoleKey.UpArrow && i > 1) i--;
                else if (key == ConsoleKey.DownArrow
                    && (i < field.GetLength(0) - actualShip.GetLength(0) - 1)) i++;
                DeleteOldShip2(field);
                if (key == ConsoleKey.Spacebar)
                {
                    bool check = CheckFreeCells(field, actualShip, i - 1, j - 1);
                    if (check)//проверить занятость всех клеток
                    {
                        AddShip(field, actualShip, i, j, 1);
                        howMuchShips++;
                        break;
                    }
                }
                else if (key == ConsoleKey.Z)
                {
                    actualShip = Rotation(actualShip);
                    if (j + actualShip.GetLength(1) > field.GetLength(1) - 1)
                        j = field.GetLength(1) - actualShip.Length - 1;
                    else if (i + actualShip.GetLength(0) > field.GetLength(0) - 1)
                        i = field.GetLength(0) - actualShip.Length - 1;
                }
                else if (key == ConsoleKey.Escape)
                {
                    return true;
                }
            }
        }
    }
}

bool StartGame()
{
    begin = true;
    int playerNow = 1, i = new int(), j = new int();
    string playerNowText = player1;
    bool changePlayer = false;
    bool first = true;
    while (true)
    {
        if (playerNow == 1)
        {
            if (first)
            {
                i = CheckIndex(field2, begin, true);
                j = CheckIndex(field2, begin, false);
                first = false;
            }
            Console.Clear();
            PrintField(field2, begin, 0);
            PrintField(field1, begin, 25);
            NewLine();
            PrintText($"Ходит {playerNowText}");
            Console.SetCursorPosition((j - 1) * 2, (i - 1) * 2);
            if (changePlayer)
            {
                playerNow = 2;
                playerNowText = player2;
                changePlayer = false;
                first = true;
                continue;
            }
        }
        else
        {
            if (first)
            {
                i = CheckIndex(field1, begin, true);
                j = CheckIndex(field1, begin, false);
                first = false;
            }
            Console.Clear();
            PrintField(field1, begin, 0);
            PrintField(field2, begin, 25);
            NewLine();
            PrintText($"Ходит {playerNowText}");
            Console.SetCursorPosition((j - 1) * 2, (i - 1) * 2);
            if (changePlayer)
            {
                playerNow = 1;
                playerNowText = player1;
                changePlayer = false;
                first = true;
                continue;
            }
        }
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.LeftArrow && j > 1) j--;
        else if (key == ConsoleKey.RightArrow && j < field2.GetLength(1) - 2) j++;
        else if (key == ConsoleKey.UpArrow && i > 1) i--;
        else if (key == ConsoleKey.DownArrow && i < field2.GetLength(0) - 2) i++;
        else if (key == ConsoleKey.Spacebar)
        {
            if (playerNow == 1)
            {
                if (field2[i, j, 1] == 3)
                {
                    field2[i, j, 1] = 2;
                    booms[0]++;
                    CheckWholeShip(field2, i, j);
                }
                else
                {
                    field2[i, j, 1] = 1;
                    changePlayer = true;
                }
            }
            else
            {
                if (field1[i, j, 1] == 3)
                {
                    field1[i, j, 1] = 2;
                    booms[1]++;
                    CheckWholeShip(field1, i, j);
                }
                else
                {
                    field1[i, j, 1] = 1;
                    changePlayer = true;
                }
            }
        }
        else if (key == ConsoleKey.Escape) return true;
        if (booms[0] == 20 || booms[1] == 20)
        {
            winner = playerNowText;
            Console.Clear();
            return false;
        }
    }
}

bool quit = PrintShips(field1, player1);
if (!quit) quit = PrintShips(field2, player2);
if (!quit) quit = StartGame();
Console.Clear();
if (quit) PrintText("Вы вышли из игры.");
else PrintText($"Поздравляю, {winner}, вы победили!");
Console.ReadLine();