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

int[] Ships(int howMuchShips)
{
    int[] linkor = new int[4] { 3, 3, 3, 3 };//1
    int[] kreyser = new int[3] { 3, 3, 3 };//2
    int[] esminec = new int[2] { 3, 3 };//3
    int[] kater = new int[1] { 3 };//4
    int[] stop = new int[1] { 0 };

    if (howMuchShips == 0) return linkor;
    else if (howMuchShips < 3) return kreyser;
    else if (howMuchShips < 7) return esminec;
    else if (howMuchShips < 11) return kater;
    else return stop;
}

void PrintField(int[,,] arr, bool begin, int left, int now = new int())
{
    string line = "-+-+-+-+-+-+-+-+-+-", vert = "|";
    for (int i = 0; i / 2 < arr.GetLength(0); i += 2)
    {
        Console.SetCursorPosition(left, i);
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (begin) now = arr[i / 2, j, 1];
            else
            {
                if (arr[i / 2, j, 1] != 0) arr[i / 2, j, 0] = arr[i / 2, j, 1];
                now = arr[i / 2, j, 0];
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
        if (i / 2 < arr.GetLength(0) - 1)
        {
            Console.SetCursorPosition(left, i + 1);
            PrintText(line);
        }
    }
}

int CheckIndex(int[,,] arr, bool begin, bool row, int now = new int())
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
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

bool CheckFreeCells(int[,,] arr, int[] arr2, bool rotation, int helpI, int helpJ)
{
    for (int k = 0; k < arr2.Length + 2; k++) //для проверки свободных ячеек вокруг корабля и корабля 
    {
        if (rotation)
        {
            if (helpI < arr.GetLength(0) - 1
                && arr[helpI, helpJ, 1] != 0) return false;
            if (helpJ > 0
                 && helpI < arr.GetLength(0) - 1
                && arr[helpI, helpJ - 1, 1] != 0) return false;
            if (helpJ < arr.GetLength(1) - 1
                && helpI < arr.GetLength(0) - 1
                && arr[helpI, helpJ + 1, 1] != 0) return false;
            helpI++;
        }
        else
        {
            if (helpJ < arr.GetLength(1) - 1
                && arr[helpI, helpJ, 1] != 0) return false;
            if (helpI > 0
                 && helpJ < arr.GetLength(1) - 1
                && arr[helpI - 1, helpJ, 1] != 0) return false;
            if (helpI < arr.GetLength(0) - 1
                && helpJ < arr.GetLength(1) - 1
                && arr[helpI + 1, helpJ, 1] != 0) return false;
            helpJ++;
        }
    }
    return true;
}

string AskName(int n)
{
    PrintText($"По умолчанию имя 'Игрок {n}'. \n Если хотите, то введите имя игрока {n}: ");
    string name = UserEnter();
    return name;
}


bool begin = false;
int[,,] field1 = new int[10, 10, 2];
int[,,] field2 = new int[10, 10, 2];
int[] booms = new int[2];
bool quit = false;
PrintText("Введите имена игроков. Если не хотите, то оставьте поля пустыми. \n");
string player1 = AskName(1);
if (player1 == "") player1 = "Игрок 1";
NewLine();
string player2 = AskName(2);
if (player2 == "") player1 = "Игрок 2";
string winner = String.Empty;


bool PrintShips(int[,,] field, string player)
{
    int howMuchShips = 0;
    bool rotation = false;
    while (true)
    {
        int[] actualShip = Ships(howMuchShips);
        if (actualShip[0] == 0) return false;
        else
        {
            int i = CheckIndex(field, begin, true);
            int j = CheckIndex(field, begin, false);
            while (true)
            {
                Console.Clear();
                AddShip2(field, actualShip, i, j, rotation);
                PrintField(field, begin, 0);
                NewLine();
                PrintText($"Поле {player}. \nДля перемещения курсора используйте стрелочки. \nДля поворота корабля - 'Z/Я'. " +
                    $"\nДля сохранения позиции корабля - пробел. \nКорабли не могут располагаться впритык друг к другу.");
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
                    int helpI = i / 2;
                    int helpJ = j / 2 - 1;
                    if (j == 0) helpJ = j / 2;

                    if (rotation)
                    {
                        helpJ = j / 2;
                        helpI = i / 2 - 1;
                        if (i == 0) helpI = i / 2;
                    }
                    bool check = CheckFreeCells(field, actualShip, rotation, helpI, helpJ);

                    if (check)//проверить занятость всех клеток
                    {
                        for (int k = 0; k < actualShip.Length; k++)
                        {
                            field[i / 2, j / 2, 1] = actualShip[k];
                            if (rotation) i += 2;
                            else j += 2;
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
            Console.SetCursorPosition(j * 2, i * 2);
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
            Console.SetCursorPosition(j * 2, i * 2);
            if (changePlayer)
            {
                playerNow = 1;
                playerNowText = player1;
                changePlayer = false;
                first= true;
                continue;
            }
        }
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.LeftArrow && j > 0) j--;
        else if (key == ConsoleKey.RightArrow && j < field2.GetLength(1) - 1) j++;
        else if (key == ConsoleKey.UpArrow && i > 0) i--;
        else if (key == ConsoleKey.DownArrow && i < field2.GetLength(0) - 1) i++;
        else if (key == ConsoleKey.Spacebar)
        {
            if (playerNow == 1)
            {
                if (field2[i, j, 1] == 3)
                {
                    field2[i, j, 1] = 2;
                    booms[0]++;
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
quit = PrintShips(field1, player1);
if (!quit) quit = PrintShips(field2, player2);
if (!quit) quit = StartGame();
Console.Clear();
if (quit) PrintText("Вы вышли из игры.");
else PrintText($"Поздравляю, {winner}, вы победили!");
