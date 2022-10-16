void PrintText(string text)
{
    Console.Write(text);
}

void NewLine()
{
    Console.WriteLine();
}

int[,] ChooseFigure()
{
    int[,] o = new int[2, 2] { { 1, 1 }, { 1, 1 } };
    int[,] i = new int[4, 1] { { 1 }, { 1 }, { 1 }, { 1 } };
    int[,] j = new int[3, 2] { { 0, 1 }, { 0, 1 }, { 1, 1 } };
    int[,] l = new int[3, 2] { { 1, 0 }, { 1, 0 }, { 1, 1 } };
    int[,] s = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
    int[,] t = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
    int[,] z = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };

    int choose = new Random().Next(0, 7);
    if (choose == 0) return o;
    else if (choose == 1) return i;
    else if (choose == 2) return j;
    else if (choose == 3) return l;
    else if (choose == 4) return s;
    else if (choose == 5) return t;
    else return z;
}

void PrintField(int[,] arr)
{
    string line = "+--------------------+", vert = "|"; //одна ячейка - два пикселя в ширину
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        if (i == 0)
        {
            PrintText(line);
            NewLine();
        }
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (j == 0) PrintText($"{vert}");
            if (arr[i, j] == 1) PrintText(" +");
            else if (arr[i, j] == 0) PrintText("  ");
            if (j == arr.GetLength(1) - 1) PrintText($"{vert}");
        }
        NewLine();
        if (i == arr.GetLength(0) - 1) PrintText(line);
    }
    PrintText("\nДля перемещения фигуры используйте стрелки 'влево' и 'вправо'." +
        "\nДля поворота фигуры нажмите стрелку 'вверх'." +
        "\nДля того, чтобы опустить фигуру нажмите стрелку 'вниз'.");
}

void ReplaceFigure(int[,] arr, int[,] arr2, int i, int j, bool add)
{
    for (int k = 0; k < arr2.GetLength(0); k++)
    {
        int helpJ = j;
        for (int l = 0; l < arr2.GetLength(1); l++)
        {
            if (arr2[k, l] == 1)
            {
                if (add) arr[i, helpJ] = arr2[k, l];
                else arr[i - 1, helpJ] = 0;
            }
            helpJ++;
        }
        i++;
    }
}

bool CheckCollision(int[,] arr, int[,] arr2, int i, int j)
{
    int helpI = i + arr2.GetLength(0) - 1; //последняя линия фигуры
    if (helpI == arr.GetLength(0)) return true;
    for (int l = 0; l < arr2.GetLength(1); l++)
    {
        for (int k = arr2.GetLength(0) - 1; k >= 0; k--)
        {
            if (arr2[k, l] == 1)
            {
                if (arr[i + k, j] == 1) return true;
                break;
            }
        }
        j++;
    }
    return false;
}

int CheckLines(int[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        int check = 0;
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[i, j] != 0) check++;
        }
        if (check == arr.GetLength(1)) return i;
    }
    return -1;
}

void DeleteLine(int[,] arr, int i)
{
    for (; i > 0; i--)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            arr[i, j] = arr[i - 1, j];
        }
    }
}

int[,] Rotation(int[,] arr)
{
    int[,] arr2 = new int[arr.GetLength(1), arr.GetLength(0)];
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1) / 2; j++)
        {
            int help = arr[i, 0 + j];
            arr[i, 0 + j] = arr[i, arr.GetLength(1) - 1 - j];
            arr[i, arr.GetLength(1) - 1 - j] = help;
        }
    }

    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            arr2[j, i] = arr[i, j];
        }
    }
    return arr2;
}

bool CheckCollisionToSide(int[,] arr, int[,] arr2, int i, int j, bool right)
{
    //int helpI = i + arr2.GetLength(0) - 1; //последняя линия фигуры
    //if (helpI == arr.GetLength(0)) return true;
    for (int l = 0; l < arr2.GetLength(0); l++)
    {
        if (right)
        {
            for (int k = arr2.GetLength(1) - 1; k >= 0; k--)
            {
                if (arr2[l,k] == 1)
                {
                    if (arr[i, j + k + 1] == 1) return true;
                    break;
                }
            }
            i++;
        }
        else
        {
            for (int k = 0; k < arr2.GetLength(1); k++)
            {
                if (arr2[l, k] == 1)
                {
                    if (arr[i, j - k - 1] == 1) return true;
                    break;
                }
            }
            i++;
        }
    }
    return false;
}

//сделать проверку на боковое столкновение

int[,] field = new int[20, 10];
int score = 0;
bool anotherFigure = true;
int i = 0, j = 4, checkLines = -1;
int[,] figure = new int[4, 2];
int endOfGame = 0;
new Thread(() =>
{
    while (true)
    {
        Console.Clear();
        if (endOfGame != 0)
        {
            if (endOfGame == 1) PrintText("Вы вышли из игры.");
            else PrintText($"Вы удалили {score} линий и набрали {score * 10} очков");
            break;
        }
        if (anotherFigure)
        {
            figure = ChooseFigure();
            anotherFigure = false;
            i = 0;
            j = 4;
        }
        ReplaceFigure(field, figure, i++, j, true);
        PrintField(field);
        Thread.Sleep(500);
        if (CheckCollision(field, figure, i, j))
        {
            if (i == 1) endOfGame = 2;
            else anotherFigure = true;
        }
        else ReplaceFigure(field, figure, i, j, false);
        checkLines = CheckLines(field);
        if (checkLines != -1)
        {
            while (checkLines != -1) //если сразу несколько линий оказались полными
            {
                DeleteLine(field, checkLines);
                score++;
                checkLines = CheckLines(field);
            }
            anotherFigure = true;
        }
    }
}).Start();

while (true)
{
    var key = Console.ReadKey().Key;
    Thread.Sleep(250);
    if (endOfGame != 0) break;
    if (key == ConsoleKey.LeftArrow && j > 0
            && !CheckCollisionToSide(field, figure, i, j, false))
    {
        ReplaceFigure(field, figure, i, j, false);
        j--;
    }
    else if (key == ConsoleKey.RightArrow
            && j < field.GetLength(1) - figure.GetLength(1)
            && !CheckCollisionToSide(field, figure, i, j, true))
    {
        ReplaceFigure(field, figure, i, j, false);
        j++;
    }
    else if (key == ConsoleKey.Escape)
    {
        endOfGame = 1;
    }
    else if (key == ConsoleKey.DownArrow)
    {
        ReplaceFigure(field, figure, i, j, false);
        while (!CheckCollision(field, figure, i, j))
        {
            i++;
        }
        ReplaceFigure(field, figure, i - 1, j, true);
        anotherFigure = true;
    }
    else if (key == ConsoleKey.UpArrow)
    {
        ReplaceFigure(field, figure, i, j, false);
        figure = Rotation(figure);
        if (j + figure.GetLength(1) > field.GetLength(1)) j -= figure.GetLength(1) - 1;
    }
}
