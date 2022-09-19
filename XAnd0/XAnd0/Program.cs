void PrintText(string text)
{
    Console.Write(text);
}

void PrintField(int[,] arr)
{
    string line = "-+-+-";
    string vert = "|";
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[i, j] == 0) Console.Write(" ");
            else if (arr[i, j] == 1) Console.Write("X");
            else if (arr[i, j] == 2) Console.Write("O");
            if (j < arr.GetLength(1) - 1)
            {
                Console.Write($"{vert}");
            }
        }
        if (i < arr.GetLength(0) - 1)
        {
            Console.WriteLine();
            Console.Write(line);
            Console.WriteLine();
        }
    }
}

int CheckI (int[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[i, j] == 0)
            {
                return i;
            }
        }
    }
    return 0;
}

int CheckJ(int[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[i, j] == 0)
            {
                return j;
            }
        }
    }
    return 0;
}

bool CheckLine(int[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        if (arr[i, 0] == arr[i, 1] && arr[i, 1] == arr[i, 2]) return true;
    }
    return false;
}

bool CheckCol(int[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        if (arr[0, i] == arr[1, i] && arr[1, i] == arr[2, i]) return true;
    }
    return false;
}

bool CheckDiag(int[,] arr)
{
    if (arr[0, 0] == arr[1, 1] && arr[1, 1] == arr[2, 2]) return true;
    else if (arr[2, 0] == arr[1, 1] && arr[1, 1] == arr[0, 2]) return true;
    return false;
}

bool CheckWinner (int[,] arr)
{
    if(CheckLine(arr)) return true;
    else if(CheckCol(arr)) return true;
    else if(CheckDiag(arr)) return true;
    return false;

}

int[,] battleField = new int[3,3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
int player = 1;
int turn = 0;
bool winner = false;
while (true)
{
    Console.Clear();
    if (turn == 9 && winner == false)
    {
        PrintText("Это ничья.");
        break;
    }
    else if (winner == true)
    {
        if (player == 1) PrintText("Победил игрок X!");
        else PrintText("Победил игрок 0!");
    }
    else
    {
        if (player == 1) PrintText("Ходит игрок X");
        else PrintText("Ходит игрок 0");
        Console.WriteLine();
    }
    
    PrintField(battleField);
    int i = CheckI(battleField);
    int j = CheckJ(battleField);
    while (true)
    {
        Console.SetCursorPosition(i, j);
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.LeftArrow && i > 0) i -= 2;
        if (key == ConsoleKey.RightArrow && i <= 3) i += 2;
        if (key == ConsoleKey.UpArrow && j > 0) j -= 2;
        if (key == ConsoleKey.DownArrow && j <= 3) j += 2;
        if (key == ConsoleKey.Spacebar)
        {
            battleField[j / 2,i /2] = player;
            if (player == 1) player = 2;
            else player = 1;
            break;
        }
    }
    turn++;
    winner = CheckWinner(battleField);
}

Console.Read();