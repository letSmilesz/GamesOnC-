//морской бой

void PrintText(string text)
{
    Console.Write(text);
}

void PrintField(int[,] arr)
{
    string line = "-+-+-+-+-+-+-+-+-+-";
    string vert = "|";
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[i, j] == 0) Console.Write(" ");
            else if (arr[i, j] == 1) Console.Write("о");
            else if (arr[i, j] == 2) Console.Write("Ж");
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

bool CheckWinner (int[,] arr)
{
    //if() return true; посчитать количество ячеек
    return false;

}

bool Cont(bool winner, int player, bool quit)
{
    else if (winner == true)
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
        if (player == 1) PrintText("Ходит игрок X");
        else PrintText("Ходит игрок 0");
        Console.WriteLine();
        return true;
    }
}

int[,] field1 = [10,10];
int[,] field2 = [10,10];
