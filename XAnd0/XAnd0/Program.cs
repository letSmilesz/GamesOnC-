void FillArrayNum(int[] arr, int min, int max)
{
    for (int i = 0; i < arr.Length; i++)
    {
        arr[i] = new Random().Next(min, max + 1);
    }
}

void PrintNum(int a)
{
    Console.WriteLine(a);
}

void PrintText(string text)
{
    Console.Write(text);
}

int UserEnterNum()
{
    int a = Convert.ToInt32(Console.ReadLine());
    return a;
}

int[] CreateFillArrayNum(int length, int min, int max)
{
    int[] arr = new int[length];
    FillArrayNum(arr, min, max);
    return arr;
}

int[] arr = CreateFillArrayNum(10, -10, 10);
PrintArrayNum(arr);

void PrintArrayNum(int[] arr)
{
    for (int j = 0; j < arr.Length; j++)
    {
        Console.Write($"{arr[j]} ");
    }
    Console.WriteLine();
}

void PrintArray(char[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {   
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            Console.Write($"{arr[i,j]}");
        }
        Console.WriteLine();
    }
}

void PrintField(char[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            //if (i == 0 || i == arr.GetLength(0) - 1) arr[i, j] = '+';//внешние границы
            //else if (j == 0 || j == arr.GetLength(1) - 1) arr[i, j] = '+';
            if (i == 4 || i == arr.GetLength(0) - 5) arr[i, j] = '+';//внутренние границы
            else if (j == 4 || j == arr.GetLength(1) - 5) arr[i, j] = '+';
            else arr[i,j] = ' ';
        }
    }
}

void Figure (int player, int[,] arr)
{
    if (player == 0) arr = new int[3, 3] { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } };
    else if (player == 1) arr = new int[3, 3] { { 1, 0, 1 }, { 0, 1, 1 }, { 1, 0, 1 } };
}

//int[,] figure = new int[3,3];
//char[,] field = new char[13, 13];
//PrintField(field);
//PrintArray(field);
