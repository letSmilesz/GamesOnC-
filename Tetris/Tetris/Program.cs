//void Figure(int x, int y)
//{
//    Console.Clear();
//    for (int i = x - 1; i <= x + 1; i++)
//    {
//        for (int j = y - 1; j <= y + 1; j++)
//        {
//            Console.SetCursorPosition(i, j);
//            Console.Write("+");
//        }
//    }
//}
//int x = 10;
//int y = 2;

//// Логика отрисовки всего
//new Thread(() =>
//{
//    while (true)
//    {
//        Figure(x, y);
//        Thread.Sleep(500);
//        y++;
//        if (y > 15) y = 1;
//    }
//}).Start();

//// Логика проверки нажатия кнопок
//while (true)
//{
//    var key = Console.ReadKey(true).Key;

//    if (key == ConsoleKey.LeftArrow)
//    {
//        x--;
//        Figure(x, y);
//    }
//    if (key == ConsoleKey.RightArrow)
//    {
//        x++;
//        Figure(x, y);
//    }
//}
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
    int[,] o = new int[2, 2] { /*{ 0, 0}, { 0, 0},*/ { 1, 1 }, { 1, 1 } };
    int[,] i = new int[4, 1] { { 1 }, { 1 }, { 1 }, { 1 } };
    int[,] j = new int[3, 2] { { 0, 1 }, { 0, 1 }, { 1, 1 } };
    int[,] l = new int[3, 2] { { 1, 0 }, { 1, 0 }, { 1, 1 } };
    int[,] s = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
    int[,] t = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
    int[,] z = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };

    return o;
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
}

void ReplaceFigure(int[,] arr, int[,] arr2, int i, int j, bool add)
{
    for (int k = 0; k < arr2.GetLength(0); k++)
    {
        int helpJ = j;
        for (int l = 0; l < arr2.GetLength(1); l++)
        {
            if (add) arr[i, helpJ] = arr2[k, l];
            else arr[i - 1, helpJ] = 0;
            helpJ++;
        }
        i++;
    }
}

int[,] field = new int[20, 10];
PrintField(field);

bool StartGame(int[,] arr)
{
    bool anotherFigure = true;
    int i = 0, j = 4;
    int[,] figure = new int[4, 2];
    bool end = false;
    //Thread game = new Thread(new ThreadStart(alsoGame));
    //void alsoGame()
    new Thread (() =>
    {
        while (true)
        {
            Console.Clear();
            if (end) break;
            if (anotherFigure)
            {
                figure = ChooseFigure();
                anotherFigure = false;
                i = 0;
                j = 4;
            }
            ReplaceFigure(field, figure, i++, j, true);
            PrintField(field);
            bool CheckField () //дописать!!!!!!!!!!
            {
                int helpI = i, helpJ = j;
                for (int k = 0; k < figure.GetLength(1); k++)
                {
                    if (field[helpI + figure.GetLength(0) - 1, helpJ] != 0) return false;
                }
                return true;
            }
            Thread.Sleep(500);
            if (i == field.GetLength(0) - (figure.GetLength(0) - 1))
            {
                anotherFigure = true;
                continue;
            }
            else if (i > 0) ReplaceFigure(field, figure, i, j, false);
        }
    }).Start();

    while (true)
    {
        var key = Console.ReadKey().Key;
        Console.Clear();
        if (key == ConsoleKey.LeftArrow && j > 0)
        {
            ReplaceFigure(field, figure, i, j, false);
            j--;
        }
        else if (key == ConsoleKey.RightArrow && j < arr.GetLength(1))
        {
            ReplaceFigure(field, figure, i, j, false);
            j++;
        }
        else if (key == ConsoleKey.Escape)
        {
            end = true;
            return true;
        }
    }
}

bool quit = StartGame(field);
if (quit) PrintText("Вы вышли из игры.");
else PrintText("Я ещё не придумал");