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
            else now = arr[i, j, 0];

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

int CheckI(int[,,] arr, bool begin, int now = new int())
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (begin) now = arr[i, j, 1];
            else now = arr[i, j, 0];

            if (now == 0)
            {
                return i;
            }
        }
    }
    return 0;
}

int CheckJ(int[,,] arr, bool begin, int now = new int())
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (begin) now = arr[i, j, 1];
            else now = arr[i, j, 0];

            if (now == 0)
            {
                return j;
            }
        }
    }
    return 0;
}

bool CheckWinner (int[,,] arr, int booms1, int booms2)
{
    if(booms1 == 20 || booms2 == 20) return true;
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

bool CheckRotation(bool rotation)
{
    if (rotation) return false;
    else return true;
}

void AddShip(int[,,] array1, int[] array2, int i, int j, bool rotation) //нужно сократить
{ 
    if (rotation)
    {
        try
        {
            for (int k = 0; k < array2.Length; k++)
            {
                array1[i / 2, j / 2, 0] = array2[k];
                i += 2;
            }
            i -= array2.Length * 2;
        }
        catch (System.IndexOutOfRangeException) 
        {            
            if (i/2 + array2.Length > array1.GetLength(1))
            {
                i -= array2.Length * 2;
                for (int k = 0; k < array2.Length; k++)
                {
                    array1[i / 2, j / 2, 0] = array2[k];
                    i += 2;
                }
                i -= array2.Length * 2;
            }
        }
    }
    else
    { 
        try //написать функцию для поворота фигуры
        { 
            for (int k = 0; k < array2.Length; k++)
            {
                array1[i / 2, j / 2, 0] = array2[k];
                j += 2;
            }
            j -= array2.Length * 2;
        }
        catch (System.IndexOutOfRangeException) 
        {    
            if (j/2 + array2.Length > array1.GetLength(1))
            {
                j -= array2.Length * 2;
                for (int k = 0; k < array2.Length; k++)
                {
                    array1[i / 2, j / 2, 0] = array2[k];
                    j += 2;
                }
                j -= array2.Length * 2;
            }
        }
    }
}

void DeleteOldShip (int[,,] array1, int[] array2, int i, int j, bool rotation, System.ConsoleKey key)
{ 
    int helpJ = j / 2, helpI = i / 2;
    if (key == ConsoleKey.LeftArrow && (i <= array1.GetLength(1)*2-2 || j >= 0)) 
    {
        for (int k = 0; k < array2.Length; k++)
        {
            array1[helpI, helpJ+1, 0] = 0;
            if (rotation) helpI++;
            else helpJ++;
        }
    }
    else if (key == ConsoleKey.RightArrow && (j > 0 || j < array1.GetLength(0)*2-4)) 
    {
        for (int k = 0; k < array2.Length; k++)
        {
            array1[helpI, helpJ-1, 0] = 0;
            if (rotation) helpI++;
            else helpJ++;
        }
    }
    else if (key == ConsoleKey.UpArrow && (j <= array1.GetLength(0)*2-2 || i >= 0))//вверх по правому
    {
        for (int k = 0; k < array2.Length; k++)
        {
            array1[helpI+1, helpJ, 0] = 0;
            if (rotation) helpI++;
            else helpJ++;
        }
    }
    else if (key == ConsoleKey.DownArrow && (j >= 0 || i <= array1.GetLength(1)*2-2))//вниз по левому краю
    {
        for (int k = 0; k < array2.Length; k++)
        {
            array1[helpI-1, helpJ, 0] = 0;
            if (rotation) helpI++;
            else helpJ++;
        }
    }
    else if (key == ConsoleKey.Z)
    {
        for (int k = 0; k < array2.Length; k++)
        {
            if (rotation) 
            {
                array1[helpI, helpJ, 0] = 0;
                helpI++; 
            }
            else
            {
                array1[helpI, helpJ, 0] = 0;
                helpJ++; 
            }
        }
    }    
}

bool begin = false;
int[,,] field1 = new int [10, 10, 2];
PrintField(field1, begin);
int[,,] field2 = new int [10, 10, 2];
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
            int i = CheckI(field, begin);
            int j = CheckJ(field, begin);
            while (true)
            {
                Console.Clear();
                AddShip(field, actualShip, i, j, rotation);
                PrintField(field, begin);
                Console.SetCursorPosition(j, i);
                
                var key = Console.ReadKey(true).Key;
                if (rotation)
                {
                    if (key == ConsoleKey.LeftArrow && j > 0) j -= 2;
                    else if (key == ConsoleKey.RightArrow
                        && j < field.GetLength(0) * 2 - 2) j += 2;
                    else if (key == ConsoleKey.UpArrow && i > 0) i -= 2;
                    else if (key == ConsoleKey.DownArrow 
                        && i / 2 < field.GetLength(1) - actualShip.Length) i += 2;
                }
                else
                {
                    if (key == ConsoleKey.LeftArrow && j > 0) j -= 2;
                    else if (key == ConsoleKey.RightArrow
                        && j / 2 < field.GetLength(1) - actualShip.Length) j += 2;
                    else if (key == ConsoleKey.UpArrow && i > 0) i -= 2;
                    else if (key == ConsoleKey.DownArrow 
                        && i < field.GetLength(0) * 2 - 2) i += 2;
                }
                /*else if (key == ConsoleKey.Spacebar)
                {
                    int check = 0;
                    for (int k = 0; k < actualShip.Length; k++)
                    {
                        if (field[i, j] == 0) check++;
                        j++;
                    }
                    if (check == actualShip.Length - 1)//проверить занятость всех клеток
                    {
                        //for (int k = 0; k < actualShip.Length; k++)
                        //{
                        //    field[i, j] = actualShip[k];
                        //    j++;
                        //}
                        howMuchShips++;
                        break;
                    }
                }*/
                if (key != ConsoleKey.Spacebar) //удаление старого расположения корабля
                {
                    DeleteOldShip(field, actualShip, i, j, rotation, key);
                }
                if (key == ConsoleKey.Z)
                {
                    rotation = CheckRotation(rotation);   
                }
                else if (key == ConsoleKey.Escape)
                {
                    return true;
                }
            }
            //функция вывода корабля на поле, перемещения и поворота
        }
    }
    return false;
}
quit = PrintShips(field1);


Console.Read();