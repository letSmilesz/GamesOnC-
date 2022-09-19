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
        if (arr[i, 0] == arr[i, 1] && arr[i, 1] == arr[i, 2] && arr[i,2] != 0) return true;
    }
    return false;
}

bool CheckCol(int[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        if (arr[0, i] == arr[1, i] && arr[1, i] == arr[2, i] && arr[2,i] != 0) return true;
    }
    return false;
}

bool CheckDiag(int[,] arr)
{
    if (arr[0, 0] == arr[1, 1] && arr[1, 1] == arr[2, 2] && arr[2,2] != 0) return true;
    else if (arr[2, 0] == arr[1, 1] && arr[1, 1] == arr[0, 2] && arr[0, 2] != 0 ) return true;
    return false;
}

bool CheckWinner (int[,] arr)
{
    if(CheckLine(arr) || CheckCol(arr) || CheckDiag(arr)) return true;
    return false;

}

bool Cont(int turn, bool winner, int player, bool quit)
{
    if (turn == 9 && winner == false)
    {
        PrintText("Это ничья.");
        return false;
    }
    else if (winner == true)
    {
        if (player == 2) PrintText("Победил игрок X!");
        else PrintText("Победил игрок 0!");
        return false;
    }
    else if (quit == true)
    {
        PrintText("Вы вышли из игры.");
        return false;
    }
    else
    {
        if (player == 1) PrintText("Ходит игрок X");
        else PrintText("Ходит игрок 0");
        Console.WriteLine();
        return true;
    }
}

int[,] battleField = new int[3,3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
int player = 1;
int turn = 0;
bool winner = false;
bool quit = false;
bool cont = true;
while (true)
{
    Console.Clear();
    cont = Cont(turn, winner, player, quit);
    if (cont == false) break; 
    PrintField(battleField);
    int i = CheckI(battleField);
    int j = CheckJ(battleField);
    while (true)
    {
        Console.SetCursorPosition(i, j + 1);
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.LeftArrow && i > 0) i -= 2;
        if (key == ConsoleKey.RightArrow && i <= 3) i += 2;
        if (key == ConsoleKey.UpArrow && j > 1) j -= 2;
        if (key == ConsoleKey.DownArrow && j <= 3) j += 2;
        if (key == ConsoleKey.Spacebar)
        {
            if (battleField[j / 2, i / 2] == 0)
            {
                battleField[j / 2, i / 2] = player;
                if (player == 1) player = 2;
                else player = 1;
                break;
            }
        }
        if (key == ConsoleKey.Escape)
        {
            quit = true;
            break;
        }
    }
    turn++;
    winner = CheckWinner(battleField);
}

//Console.Read();