namespace CSharp.AI.Examples;

internal class BubbleSortExample
{
    public static void Run()
    {
        int[] array = { 5, 3, 8, 4, 2 };

        Console.WriteLine("Orignial Array");

        PrintArray(array);

        // sort the array using Bubble Sort
        BubbleSort(array);

        Console.WriteLine("\nSorted Array");

        PrintArray(array);
    }

    static void BubbleSort(int[] array)
    {
        int n = array.Length;

        // out loop for passes
        for (int i = 0; i < n - 1; i++)
        {
            // innter loop for comparisions
            for (int j = 0; j < n - i - 1; j++)
            {
                // swap if the element is greater than the next element
                if (array[j] < array[j + 1])
                {
                    int temp = array[j];

                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
    }

    static void PrintArray(int[] array)
    {
        foreach (int i in array)
        {
            Console.WriteLine(i + " ");
        }

        Console.WriteLine();
    }
}
