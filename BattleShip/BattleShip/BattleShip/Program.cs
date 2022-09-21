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

void PrintField(int[,] arr, bool begin)
{
    string line = "-+-+-+-+-+-+-+-+-+-", vert = "|";
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[i, j] == 0) Console.Write(" ");
            else if (arr[i, j] == 1) Console.Write("о");
            else if (arr[i, j] == 2) Console.Write("Ж");
            else if (arr[i, j] == 3)
            {
                if (begin) Console.Write(" ");
                else Console.Write("#");
            }
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

int CheckI(int[,] arr)
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

bool CheckWinner (int[,] arr, int booms1, int booms2)
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

void CheckRotation(bool rotation, int i, int j)
{
    if (rotation) rotation = false;
    else rotation = true;
    int help = j;
    j = i;
    i = help;
}

bool begin = false;
int[,] field1 = new int [10,10];
PrintField(field1, begin);
int[,] field2 = new int [10,10];
bool quit = false;

bool PrintShips(int[,] field)
{
    int howMuchShips = 0;
    bool rotation = false;
    while (true)
    {
        int[] actualShip = Ships(howMuchShips);
        if (actualShip[0] == 0) break;
        else
        {
            int i = CheckI(field);
            int j = CheckJ(field);
            while (true)
            {
                Console.Clear();
                try //написать функцию для поворота фигуры
                { 
                    for (int k = 0; k < actualShip.Length; k++)
                    {
                        field[i / 2, j / 2] = actualShip[k];
                        j += 2;
                    }
                    j -= actualShip.Length * 2;
                }
                catch (System.IndexOutOfRangeException) 
                {
                    
                    if (j/2 + actualShip.Length > field.GetLength(1))
                    {
                        j -= actualShip.Length * 2;
                        for (int k = 0; k < actualShip.Length; k++)
                        {
                            field[i / 2, j / 2] = actualShip[k];
                            j += 2;
                        }
                        j -= actualShip.Length * 2;
                    }
                }
                PrintField(field, begin);
                if (rotation) Console.SetCursorPosition(i, j);
                else Console.SetCursorPosition(j, i);
                
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow && j > 0) j -= 2;
                else if (key == ConsoleKey.RightArrow) j += 2;
                else if (key == ConsoleKey.UpArrow && i > 0) i -= 2;
                else if (key == ConsoleKey.DownArrow 
                    && i < field.GetLength(0) * 2 - 2) i += 2;
                else if (key == ConsoleKey.Z)
                {
                    CheckRotation(rotation, i, j);   
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
                else if (key == ConsoleKey.Escape)
                {
                    return true;
                }
                if (key != ConsoleKey.Spacebar) //удаление старого расположения корабля
                {//написать функцию для стирания повернутой фигуры
                    int helpJ = j / 2, helpI = i / 2;
                    if (key == ConsoleKey.LeftArrow && j >= 0) 
                    {
                        for (int k = 0; k < actualShip.Length; k++)
                        {
                            field[helpI, helpJ+1] = 0;
                            helpJ++;        
                        }

                    }
                    if (key == ConsoleKey.RightArrow && j < field.GetLength(1)*2-2) 
                    {

                        for (int k = 0; k < actualShip.Length; k++)
                        {
                            field[helpI, helpJ-1] = 0;
                            helpJ++;        
                        }

                    }
                    if (key == ConsoleKey.UpArrow && i >= 0)
                    {
                        
                        for (int k = 0; k < actualShip.Length; k++)
                        {
                            field[helpI+1, helpJ] = 0;
                            helpJ++;        
                        }

                    }
                    if (key == ConsoleKey.DownArrow && i <= field.GetLength(0)*2-2)
                    {

                        for (int k = 0; k < actualShip.Length; k++)
                        {
                            field[helpI-1, helpJ] = 0;
                            helpJ++;        
                        }

                    }
                }
            }
            //функция вывода корабля на поле, перемещения и поворота
        }
    }
    return false;
}
quit = PrintShips(field1);


//Console.Read();