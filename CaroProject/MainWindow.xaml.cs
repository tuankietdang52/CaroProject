using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaroProject
{
    public partial class MainWindow : Window
    {
        private readonly string path = $"{Directory.GetCurrentDirectory()}\\Picture\\";
        public int index;
        public int grab = 1;
        public int countend = 0;
        public int x, y;
        public int newgame = 0;
        public int xscore = 0, oscore = 0;
        public int[,] checker = new int[3,3];
        public MainWindow()
        {
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            ScoreX.Text += Convert.ToString(xscore);
            ScoreO.Text += Convert.ToString(oscore);
            Create2dTable();
        }
        public void Create2dTable()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    checker[i, j] = -1;
                }
            }
        }
        private void NextMoveShow(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            if (grab == 0)
            {
                button.Content = new Image
                {
                    Source = new BitmapImage(new(path + "X.png")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Fill,
                };
            }
            if (grab == 1)
            {
                button.Content = new Image
                {
                    Source = new BitmapImage(new(path + "O.png")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Fill,
                };
            }
        }
        private void NextMoveCancel(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Content = null;
            button.Background = Brushes.White;
        }
        private void Drawline(int side, int x, int y)
        {
            Canvas linewin = new Canvas();
            linewin.Name = "Linewin";
            RegisterName(linewin.Name, linewin);
            CoCaro.Children.Add(linewin);
            Line line = new Line();
            line.Name = "line";
            line.Stroke = Brushes.GreenYellow;
            if (side == 1)
            {
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 900;
                line.Y2 = 0;
                line.StrokeThickness = 10;
                if (x == 0) Canvas.SetTop(line, 160);
                if (x == 1) Canvas.SetTop(line, 468);
                if (x == 2) Canvas.SetTop(line, 750);
            }
            if (side == 2)
            {
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = 900;
                line.StrokeThickness = 10;
                if (y == 0)
                {
                    Canvas.SetTop(line, -8);
                    Canvas.SetLeft(line, 148);
                }
                if (y == 1)
                {
                    Canvas.SetTop(line, -8);
                    Canvas.SetLeft(line, 448);
                }
                if (y == 2)
                {
                    Canvas.SetTop(line, -8);
                    Canvas.SetLeft(line, 745);
                }
            }
            if (side == 3)
            {
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 900;
                line.Y2 = 900;
                line.StrokeThickness = 10;
                Canvas.SetLeft(line, 10);
            }
            if (side == 4)
            {
                line.X1 = 900;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = 900;
                line.StrokeThickness = 10;
                Canvas.SetLeft(line, -4);
                Canvas.SetTop(line, -10);
            }
            linewin.Children.Add(line);
        }
        private bool End(int count, int side, int x, int y)
        {
            if (count == 3)
            {
                if (grab == 0)
                {
                    oscore++;
                    ScoreO.Text = "O: " + Convert.ToString(oscore);
                    Drawline(side, x, y);
                    if (MessageBox.Show("BAN O THAT THONG MINH\n" + "Choi lai?", "Ket thuc", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        countend = 0;
                        Replay();
                        return true;
                    }
                    else this.Close();
                }
                if (grab == 1)
                {
                    xscore++;
                    ScoreX.Text = "X: " + Convert.ToString(xscore);
                    Drawline(side, x, y);
                    if (MessageBox.Show("BAN X THAT THONG MINH\n" + "Choi lai?", "Ket thuc", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        countend = 0;
                        Replay();
                        return true;
                    }
                    else this.Close();
                }
                return true;
            }
            else return false;
        }
        private void Check(int pos, int grab)
        {
            bool endturn = false;
            int count = 0;
            //check trai phai
            if (pos <= 3)
            {
                x = 0;
                y = pos - 1;
            }
            else if (pos > 3 && pos <= 6)
            {
                x = 1;
                y = pos - 4;
            }
            else if (pos > 6)
            {
                x = 2;
                y = pos - 7;
            }
            int i, j;
            while (count != 3 && endturn == false)
            {
                //check tren duoi
                j = 0;
                while (j <= 2)
                {
                    if (checker[x, j] == grab) count++;
                    j++;
                }
                if (End(count, 1, x, y) == true) continue;
                i = 0;
                j = y;
                count = 0;
                while (i <= 2)
                {
                    if (checker[i, y] == grab) count++;
                    i++;
                }
                if (End(count, 2, x, y) == true) continue;
                //check cheo chinh
                j = y + 1;
                i = x + 1;
                count = 1;
                while (i <= 2 && j <= 2)
                {
                    if (checker[i, j] == grab) count++;
                    i++;
                    j++;
                }
                i = x - 1;
                j = y - 1;
                while (i >= 0 && j >= 0)
                {
                    if (checker[i, j] == grab) count++;
                    i--;
                    j--;
                }
                if (End(count, 3, x, y) == true) continue;
                i = x + 1;
                j = y - 1;
                count = 1;
                //check cheo phu
                while (i <= 2 && j >= 0)
                {
                    if (checker[i, j] == grab) count++;
                    i++;
                    j--;
                }
                i = x - 1;
                j = y + 1;
                while (i >= 0 && j <= 2)
                {
                    if (checker[i, j] == grab) count++;
                    i--;
                    j++;
                }
                if (End(count, 4, x, y) == true) continue;
                endturn = true;
            }
        }
        private void XDraw(int index)
        {
            newgame = 1;
            if (grab == 0)
            {
                grab = 1;
            }
            else
            {
                grab = 0;
            }
            Button but = (Button)CoCaro.FindName("button" + Convert.ToString(index));
            but.Visibility = Visibility.Hidden;
            Rectangle square = (Rectangle)CoCaro.FindName("square" + Convert.ToString(index));
            int x = 0;
            if (index <= 3) x = 0;
            else if (index > 3 && index <= 6) x = 1;
            else if (index > 6) x = 2; 
            if (grab == 0)
            {
                if (index <= 3) checker[x, index - 1] = 0;
                else if (index > 3 && index <= 6) checker[x, index - 4] = 0;
                else if (index > 6) checker[x, index - 7] = 0;
                    square.Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new(path + "O.png"))
                };
            }
            else
            {
                if (index <= 3) checker[x, index - 1] = 1;
                else if (index > 3 && index <= 6) checker[x, index - 4] = 1;
                else if (index > 6) checker[x, index - 7] = 1;
                square.Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new(path + "X.png"))
                };
            }
            countend++;
            if (newgame != 0) Check(index, grab);
            if (countend == 9)
            {
                if (MessageBox.Show("HOA\n" + "Choi lai?", "Ket thuc", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Replay();
                }
                else this.Close();
            }
        }
        private void Draw1(object sender, RoutedEventArgs e)
        {
            index = 1;
            Button Xbut = (Button)sender;
            XDraw(index);
        }

        private void Draw2(object sender, RoutedEventArgs e)
        {
            index = 2;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Draw3(object sender, RoutedEventArgs e)
        {
            index = 3;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Draw4(object sender, RoutedEventArgs e)
        {
            index = 4;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Draw5(object sender, RoutedEventArgs e)
        {
            index = 5;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Draw6(object sender, RoutedEventArgs e)
        {
            index = 6;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Draw7(object sender, RoutedEventArgs e)
        {
            index = 7;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Draw8(object sender, RoutedEventArgs e)
        {
            index = 8;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Draw9(object sender, RoutedEventArgs e)
        {
            index = 9;
            Button Xbut = (Button)sender;
            XDraw(index);
        }
        private void Replay()
        {
            if (countend != 9)
            {
                Canvas linewin = (Canvas)CoCaro.FindName("Linewin");
                CoCaro.Children.Remove(linewin);
                UnregisterName(linewin.Name);
            }
            for (int i = 1; i <= 9; i++)
            {
                Rectangle square = (Rectangle)CoCaro.FindName("square" + Convert.ToString(i));
                square.Fill = Brushes.White;
                Button but = (Button)CoCaro.FindName("button" + Convert.ToString(i));
                but.Visibility = Visibility.Visible;
            }
            newgame = 0;
            countend = 0;
            grab = 1;
            Create2dTable();
        }
    }
}
