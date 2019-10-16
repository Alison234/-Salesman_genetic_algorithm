using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComiGenAlgoritm
{
    class Program
    {
        public static Random rnd = new Random();
        public static int[,] WayPrice = new int[31, 31];
        public static int[,] Population = new int[80, 31];
        public static void InitWayPrice() {
            for (int i = 0; i < 30; i++) {
                for (int j = 0; j < 30; j++) {
                    if (i == j) { WayPrice[i, j] = 1000; } else
                        WayPrice[i, j] = rnd.Next(1, 100);
                }
            }
        }
        public static void InitPopulation() {
            
            int[] tmp = new int[30];


            for (int a = 0; a < 80; a++) {
                for (int i = 0; i < 30; i++) {
                    tmp[i] = rnd.Next(1, 31);
                    for (int j = 0; j < i; j++) {
                        if (tmp[j] == tmp[i])
                        { i--; }
                    }
                }
                for (int k = 0; k < 30; k++)
                {
                    Population[a, k] = tmp[k];
                }
            }
        }
        public static void CalculatePrice()
        {
            int wayPrice = 0;
            for (int i = 0; i < 80; i++)
            {
                wayPrice = 0;
                for (int j = 0; j < 29; j++)
                {
                    Population[i, 30] += WayPrice[Population[i, j] - 1, Population[i, j + 1] - 1];
                    wayPrice += WayPrice[Population[i, j] - 1, Population[i, j + 1] - 1];
                }
            }
        }
        public static int[,] RemoveRepeatInChildren(int[,] Children, int pointSection) {
            List<int> RepeatIndex = new List<int>();
            int tmp = 0;
            //ищем повторы в первых двух детях
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < pointSection; j++) {
                    for (int k = pointSection; k < 30; k++) {
                        if (Children[i, j] == Children[i, k]) {
                            RepeatIndex.Add(j);
                        }
                    }
                }
            }
            // меняем повторы в первых двух детях
            for (int i = 0; i < RepeatIndex.Count / 2; i++) {
                tmp = Children[0, RepeatIndex[i]];
                Children[0, RepeatIndex[i]] = Children[1, RepeatIndex[i + RepeatIndex.Count/2]];
                Children[1, RepeatIndex[i + RepeatIndex.Count/2]] = tmp;
            }

            RepeatIndex.Clear();
            
            //ищем повторы в 3ем и 4ем ребенке
            for (int i = 2; i < 4; i++)
            {
                for (int j = 0; j < 30-pointSection; j++)
                {
                    for (int k = 30-pointSection; k < 30; k++)
                    {
                        if (Children[i, j] == Children[i, k])
                        {
                            RepeatIndex.Add(j);
                        }
                    }
                }
            }
            // меняем повторы в первых двух детях
            for (int i = 0; i < RepeatIndex.Count / 2; i++)
            {
                tmp = Children[2, RepeatIndex[i]];
                Children[2, RepeatIndex[i]] = Children[3, RepeatIndex[i + RepeatIndex.Count / 2]];
                Children[3, RepeatIndex[i + RepeatIndex.Count / 2]] = tmp;
            }
            return Children;
        }
        public static void Algoritm(int[] FirstParent,int[] SecondParent,int FirstParentIndex,int SecondParentIndex) {           
            int[,] Children = new int[4, 31];
            int PointSection = rnd.Next(2, 29);
                 // цикла скрещивание родителей
                for (int j = 0; j < PointSection; j++)
                {
                    
                    Children[0, j] = FirstParent[j];
                    Children[1  , j] = SecondParent[j];
                }
                    for (int k = PointSection; k < 30; k++) {
                        Children[0, k] = SecondParent[k];
                        Children[1, k] = FirstParent[k];
                     }

                  for (int j = 0; j < 30-PointSection; j++)
                    {
                
                     Children[2, j] = FirstParent[PointSection + j];
                    Children[3, j] = SecondParent[PointSection + j];
                     }

                     for (int k = 0; k < PointSection; k++)
                    {
                    Children[2, (30 -PointSection) + k] = SecondParent[k];
                    Children[3, (30 - PointSection) + k] = FirstParent[k];
                     }
                 Children = RemoveRepeatInChildren(Children,PointSection);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 29; j++)
                    {
                        Children[i, 30] += WayPrice[Children[i, j] - 1, Children[i, j + 1] - 1];
                    }
            }
            int IndexBestChild = 0;
            for (int i = 1; i < 4; i++) {
                if (Children[i, 30] < Children[IndexBestChild, 30]) {
                    IndexBestChild = i;
                }
             

            }

            if (Children[IndexBestChild, 30] < Population[FirstParentIndex, 30])
            {
                for (int j = 0; j < 31; j++)
                {
                    Population[FirstParentIndex, j] = Children[IndexBestChild, j];
                }
                return;
            }

            if ((Children[IndexBestChild, 30] < Population[SecondParentIndex, 30]) && (Children[IndexBestChild, 30] > Population[FirstParentIndex, 30]))
            {
                for (int j = 0; j < 31; j++)
                {
                    Population[SecondParentIndex, j] = Children[IndexBestChild, j];
                }
                return;

            }
            //Console.WriteLine();
            //for (int i = 0; i < 4; i++) {
            //    for (int j = 0; j < 30; j++) {
            //        Console.Write(Children[i, j] + "->");
            //    }
            //}


        } 

        static void Main(string[] args)
        {
            InitWayPrice();
            InitPopulation();
            CalculatePrice();
            int minPrice = 10000;
            int[] minPriceWay = new int[30];
            int MinPriceWayIndex = 0;
            int[] SecondParent = new int[30];
            int SecondParentIndex = 0;            
            for (int i = 0; i < 80; i++)
            {
                if (Population[i, 30] < minPrice) {
                    minPrice = Population[i, 30];
                    MinPriceWayIndex = i;
                }
            }
            while (SecondParentIndex == MinPriceWayIndex) {
                SecondParentIndex = rnd.Next(0, 80);
            }
            for (int i = 0; i < 30; i++) {
                minPriceWay[i] = Population[MinPriceWayIndex, i];
            }

            for (int i = 0; i < 30; i++)
            {
                SecondParent[i] = Population[SecondParentIndex, i];
            }

            Console.WriteLine("Изначальная минимальная стоимость = " + minPrice);
            Console.WriteLine("Изначальный путь ");
            for (int j = 0; j < 30; j++)
            {
                Console.Write(Population[MinPriceWayIndex, j] + "->");
            }
            Algoritm(minPriceWay, SecondParent, MinPriceWayIndex, SecondParentIndex);

            for (int k = 0; k < 20000; k++)
            {

                for (int i = 0; i < 80; i++)
                {
                    if (Population[i, 30] < minPrice)
                    {
                        minPrice = Population[i, 30];
                        MinPriceWayIndex = i;
                    }
                }
                while (SecondParentIndex == MinPriceWayIndex)
                {
                    SecondParentIndex = rnd.Next(0, 80);
                }
                for (int i = 0; i < 30; i++)
                {
                    minPriceWay[i] = Population[MinPriceWayIndex, i];
                }

                for (int i = 0; i < 30; i++)
                {
                    SecondParent[i] = Population[SecondParentIndex, i];
                }
                Algoritm(minPriceWay, SecondParent, MinPriceWayIndex, SecondParentIndex);
            }

            Console.WriteLine();
            Console.WriteLine("Стоимость после алгоритма  = " + minPrice);
            Console.WriteLine("Путь после алгоритма ");
            for (int i = 0; i < 30; i++) {
                Console.Write(Population[MinPriceWayIndex, i] + " -> ");
            }
           

            Console.ReadKey();

        }
    }
}
