using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SortAlgorithm
{
    public partial class MainWindow : Window
    {
        private int[] nArray;
        private Stopwatch stopwatch;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RandomArray()
        {
            nArray = new int[100];
            Random random = new Random();
            int nRand;

            for (int i = 0; i < nArray.Length; i++)
            {
                nRand = random.Next(1, 101);

                while (Array.IndexOf(nArray, nRand) != -1)
                {
                    nRand = random.Next(1, 101);
                }

                nArray[i] = nRand;
            }
        }

        private void InitializeDrawBars(int[] arr, Canvas canvas)
        {
            canvas.Children.Clear();
            double dBarWidth = canvas.ActualWidth / arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                Rectangle rectangle = new Rectangle { Width = dBarWidth, Height = arr[i] * 2, Fill = Brushes.LightGray };
                Canvas.SetLeft(rectangle, i * dBarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }
        }

        private void SelectDrawBars(int[] arr, Canvas canvas, int nMin, int nCheck, int nsorted)
        {
            canvas.Children.Clear();
            double dBarWidth = canvas.ActualWidth / arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = dBarWidth;
                rectangle.Height = arr[i] * 2;
                if (i == nMin)
                    rectangle.Fill = Brushes.Red;
                else if (i == nCheck)
                    rectangle.Fill = Brushes.Blue;
                else
                    rectangle.Fill = Brushes.LightGray;

                if (i < nsorted)
                    rectangle.Fill = Brushes.Green;
                Canvas.SetLeft(rectangle, i * dBarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }
        }

        private async Task SelectSort(int[] arr)
        {
            int nMin;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                nMin = i;
                for (int j = i + 1; j < arr.Length; j++)
                {
                    SelectDrawBars(arr, Can1, nMin, j, i);
                    await Task.Delay(1);
                    Select_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
                    if (arr[j] < arr[nMin])
                    {
                        nMin = j;
                    }
                }
                (arr[nMin], arr[i]) = (arr[i], arr[nMin]);
                if (i == arr.Length - 2)
                    SelectDrawBars(arr, Can1, -1, -1, i + 2);
            }
        }

        private void BubleDrawBars(int[] arr, Canvas canvas, int nCheck, int nCheckNext, int nsorted)
        {
            canvas.Children.Clear();
            double dBarWidth = canvas.ActualWidth / arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = dBarWidth;
                rectangle.Height = arr[i] * 2;
                if (i == nCheck)
                    rectangle.Fill = Brushes.Blue;
                else if (i == nCheckNext)
                {
                    if (arr[nCheck] < arr[nCheckNext])
                        rectangle.Fill = Brushes.Blue;
                    else
                        rectangle.Fill = Brushes.Red;
                }
                else if (i > arr.Length - (1 + nsorted))
                    rectangle.Fill = Brushes.Green;
                else
                    rectangle.Fill = Brushes.LightGray;
                Canvas.SetLeft(rectangle, i * dBarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }
        }
        private async Task BubbleSort(int[] arr)
        {
            int temp = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length - (1 + i); j++)
                {
                    BubleDrawBars(arr, Can2, j, j + 1, i);
                    await Task.Delay(1);
                    Bubble_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
                    if (arr[j] > arr[j + 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
                BubleDrawBars(arr, Can2, -1, -1, i + 1);
            }
        }

        private void InSertionDrawBars(int[] arr, Canvas canvas, int nCheck)
        {
            canvas.Children.Clear();
            double dBarWidth = canvas.ActualWidth / arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = dBarWidth;
                rectangle.Height = arr[i] * 2;
                if (i == nCheck)
                    rectangle.Fill = Brushes.Blue;
                else
                    rectangle.Fill = Brushes.LightGray;
                Canvas.SetLeft(rectangle, i * dBarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }
        }

        private async Task InsertionSort(int[] arr)
        {
            int j = 0;
            int nKey = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                nKey = arr[i];
                for (j = i - 1; j >= 0; j--)
                {
                    InSertionDrawBars(arr, Can3, j);
                    await Task.Delay(1);
                    Insertion_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
                    if (arr[j] > nKey)
                        arr[j + 1] = arr[j];
                    else
                        break;

                }
                arr[j + 1] = nKey;
                if (i == arr.Length - 1)
                    await SortFinish(arr, Can3);
            }
        }

        private async Task MergeSort(int[] arr, int nLeft, int nRight)
        {
            if (nLeft < nRight)
            {
                int nMid = nLeft + (nRight - nLeft) / 2;

                await MergeSort(arr, nLeft, nMid);
                await MergeSort(arr, nMid + 1, nRight);

                await Merge(arr, nLeft, nMid, nRight);

                if (nLeft == 0 && nRight == arr.Length - 1)
                    await SortFinish(arr, Can4);
            }
        }

        private async Task Merge(int[] arr, int nLeft, int nMid, int nRight)
        {
            int n1 = nMid - nLeft + 1;
            int n2 = nRight - nMid;

            int[] L = new int[n1];
            int[] R = new int[n2];

            Array.Copy(arr, nLeft, L, 0, n1);
            Array.Copy(arr, nMid + 1, R, 0, n2);

            int i = 0, j = 0, k = nLeft;

            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }
                k++;

                MergeDrawBars(arr, nLeft, nMid, nRight, Can4, false);

                await Task.Delay(1);
                Merge_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
            }

            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;

                MergeDrawBars(arr, nLeft, nMid, nRight, Can4, false);

                await Task.Delay(1);
                Merge_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
            }

            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;

                MergeDrawBars(arr, nLeft, nMid, nRight, Can4, false);

                await Task.Delay(1);
                Merge_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
            }
        }

        private void MergeDrawBars(int[] arr, int nLeft, int nMid, int nRight, Canvas canvas, bool bFinish)
        {
            canvas.Children.Clear();
            double dbarWidth = canvas.ActualWidth / arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = dbarWidth;
                rectangle.Height = arr[i] * 2;

                if (bFinish)
                    rectangle.Fill = Brushes.Green;
                else if (i >= nLeft && i <= nRight)
                {
                    if (i >= nLeft && i <= nMid)
                        rectangle.Fill = Brushes.Blue;
                    else if (i > nMid && i <= nRight)
                        rectangle.Fill = Brushes.Red;
                }
                else
                {
                    rectangle.Fill = Brushes.LightGray;
                }

                Canvas.SetLeft(rectangle, i * dbarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }
        }

        private async Task HeapSort(int[] arr)
        {
            for (int i = arr.Length / 2 - 1; i >= 0; i--)
            {
                Heapify(arr, arr.Length, i);
                HeapDrawBars(arr, Can5, i, false);
                await Task.Delay(1);
                Heap_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
            }

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                Heapify(arr, i, 0);
                HeapDrawBars(arr, Can5, i, false);
                await Task.Delay(1);
                Heap_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
            }
            HeapDrawBars(arr, Can5, -1, true);
            Heap_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
        }

        private void Heapify(int[] arr, int nLength, int nCheck)
        {
            int nMax = nCheck;
            int left = 2 * nCheck + 1;
            int right = 2 * nCheck + 2;

            if (left < nLength && arr[left] > arr[nMax])
                nMax = left;

            if (right < nLength && arr[right] > arr[nMax])
                nMax = right;

            if (nMax != nCheck)
            {
                int swap = arr[nCheck];
                arr[nCheck] = arr[nMax];
                arr[nMax] = swap;

                Heapify(arr, nLength, nMax);
            }
        }

        private void HeapDrawBars(int[] arr, Canvas canvas, int nCheck, bool bFinish)
        {
            canvas.Children.Clear();
            double dbarWidth = canvas.ActualWidth / arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                if (bFinish)
                    break;
                Rectangle rectangle = new Rectangle();
                rectangle.Width = dbarWidth;
                rectangle.Height = arr[i] * 2;
                if (bFinish)
                    rectangle.Fill = Brushes.Green;
                else if (i == nCheck)
                    rectangle.Fill = Brushes.Blue;
                else if (i < nCheck)
                    rectangle.Fill = Brushes.Red;
                else
                    rectangle.Fill = Brushes.LightGray;


                Canvas.SetLeft(rectangle, i * dbarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }
            if (bFinish)
            {
                Task test = SortFinish(arr, Can5);
            }
                
        }

        private async Task QuickSort(int[] arr, int nLow, int nHigh)
        {
            int nPivot = arr[(nLow + nHigh) / 2];
            int nStartIndex = nLow;
            int nEndIndex = nHigh;

            while (nStartIndex <= nEndIndex)
            {
                while (arr[nStartIndex] < nPivot)
                    ++nStartIndex;

                while (arr[nEndIndex] > nPivot)
                    --nEndIndex;
                
                if (nStartIndex <= nEndIndex)
                {
                    (arr[nStartIndex], arr[nEndIndex]) = (arr[nEndIndex], arr[nStartIndex]);
                    ++nStartIndex;
                    --nEndIndex;
                }
            }
            QuickDrawBars(arr, Can6, -1, false);
            await Task.Delay(1);
            Quick_lb.Content = $"{stopwatch.ElapsedMilliseconds} ms";
            if (nStartIndex == nHigh)
                QuickDrawBars(arr, Can6, -1, true);
            if (nLow < nEndIndex)
                await QuickSort(arr, nLow, nEndIndex);
            if (nStartIndex < nHigh)
                await QuickSort(arr, nStartIndex, nHigh);
        }

        private void QuickDrawBars(int[] arr, Canvas canvas, int nLow, bool bFinish)
        {
            canvas.Children.Clear();
            double dbarWidth = canvas.ActualWidth / arr.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = dbarWidth;
                rectangle.Height = arr[i] * 2;
                if (i <= nLow || bFinish)
                    rectangle.Fill = Brushes.Green;
                else
                    rectangle.Fill = Brushes.LightGray;
                Canvas.SetLeft(rectangle, i * dbarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }
        }

        private async Task SortFinish(int[] arr, Canvas canvas)
        {
            canvas.Children.Clear();
            double dbarWidth = canvas.ActualWidth / arr.Length;
            Rectangle[] arrayRect = new Rectangle[100];

            for (int i = 0; i < arr.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                arrayRect[i] = rectangle;
                rectangle.Width = dbarWidth;
                rectangle.Height = arr[i] * 2;
                rectangle.Fill = Brushes.LightGray;

                Canvas.SetLeft(rectangle, i * dbarWidth);
                Canvas.SetBottom(rectangle, 0);
                canvas.Children.Add(rectangle);
            }

            for (int i = 0; i < arr.Length; i++)
            {
                arrayRect[i].Fill = Brushes.Green;
                await Task.Delay(1);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RandomArray();
            InitializeDrawBars(nArray, Can1);
            InitializeDrawBars(nArray, Can2);
            InitializeDrawBars(nArray, Can3);
            InitializeDrawBars(nArray, Can4);
            InitializeDrawBars(nArray, Can5);
            InitializeDrawBars(nArray, Can6);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            int[] nCopyarray1 = nArray.ToArray();
            int[] nCopyarray2 = nArray.ToArray();
            int[] nCopyarray3 = nArray.ToArray();
            int[] nCopyarray4 = nArray.ToArray();
            int[] nCopyarray5 = nArray.ToArray();
            int[] nCopyarray6 = nArray.ToArray();
            stopwatch = Stopwatch.StartNew();
            Task task1 = SelectSort(nCopyarray1);
            Task task2 = BubbleSort(nCopyarray2);
            Task task3 = InsertionSort(nCopyarray3);
            Task task4 = MergeSort(nCopyarray4, 0, nCopyarray4.Length - 1);
            Task task5 = HeapSort(nCopyarray5);
            Task task6 = QuickSort(nCopyarray6,0,nCopyarray6.Length - 1);

            await Task.WhenAll(task1, task2, task3, task4, task5, task6);
            stopwatch.Stop();
        }
    }
}
