using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Location
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }
        //初始化i
        const int i = 16;
        //初始化坐标list
        List<location> locations = new List<location>();
        List<location> locationlist = new List<location>();
        //判断是否为整数
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        //判断是否为数字
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        //计算按钮
        private void figure_Click(object sender, RoutedEventArgs e)
        {
            double XW = 0;
            double YW = 0;
            int W = 0;
            if (locations.Count>=2)
            {
                foreach (var location in locations)
                {
                    W += location.w;
                    XW+= location.x * location.w;
                    YW += location.y * location.w;
                }
                double Xs =Math.Round(XW / W,2);
                double Ys =Math.Round(YW / W,2);
                result.Content = $"城市距离:({Xs},{Ys})";
                var bestLocation=getLocation(new location() { x = Xs, y = Ys });                               
                result2.Content = $"最优结果:({bestLocation.x},{bestLocation.y})";
                string resultliststring = "";
                foreach (var location in locationlist)
                {
                    resultliststring += $"({location.x},{location.y})  ";
                }
                resultlist.Text = $"每次迭代结果:{resultliststring}";
            }
            else
            {
                MessageBox.Show("最少需有两条数据");
            }
        }
        //递归迭代
        public location getLocation(location oldlocation)
        {
            if (locationlist.Count==0)
            {
                locationlist.Add(oldlocation);
                return getLocation(oldlocation);
            }
            else if((locationlist.Count==1)||(Math.Abs(oldlocation.x - locationlist.Last().x) >= 0.001 || Math.Abs(oldlocation.y - locationlist.Last().y) >= 0.001))
            {
                double XWU = 0;
                double XWD = 0;
                double YWU = 0;
                double YWD = 0;
                foreach (var location in locations)
                {
                    XWU += (location.w * location.x) / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                    XWD += location.w / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                    YWU += (location.w * location.y) / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                    YWD += location.w / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                }
                location newlocation=new location() { x = Math.Round(XWU / XWD, 4), y = Math.Round(YWU / YWD, 4) };
                locationlist.Add(newlocation);
                return getLocation(newlocation);
            }
            else
            {
                double XWU = 0;
                double XWD = 0;
                double YWU = 0;
                double YWD = 0;
                foreach (var location in locations)
                {
                    XWU += (location.w * location.x) / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                    XWD += location.w / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                    YWU += (location.w * location.y) / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                    YWD += location.w / Math.Sqrt(Math.Pow(location.x - oldlocation.x, 2) + Math.Pow(location.y - oldlocation.y, 2));
                }
                if (Math.Abs(XWU / XWD - locationlist.Last().x) < 0.001 || Math.Abs(YWU / YWD - locationlist.Last().y) < 0.001)
                {
                    location newlocation = new location() { x = Math.Round(XWU / XWD, 4), y = Math.Round(YWU / YWD, 4) };
                    locationlist.Add(newlocation);
                    return newlocation;
                }
                else
                {
                    location newlocation = new location() { x = Math.Round(XWU / XWD, 4), y = Math.Round(YWU / YWD, 4) };
                    locationlist.Add(newlocation);
                    return getLocation(newlocation);
                }
            }
        }
        //添加按钮
        private void add_Click(object sender, RoutedEventArgs e)
        {
            string strX = textBoxX.Text;
            string strY = textBoxY.Text;
            string strW = textBoxW.Text;
            if (IsNumeric(strX)&&IsNumeric(strY)&&IsInt(strW))
            {
                double x = double.Parse(strX);
                double y = double.Parse(strY);
                int w = int.Parse(strW);
                if (x>=-16&&y>=-16&&w>=-16)
                {
                    var location = new location() { x = x + i, y = y + i, w = w + i };           
                    locations.Add(location);
                    listView.Items.Add(location);
                    textBoxX.Text = "";
                    textBoxY.Text = "";
                    textBoxW.Text = "";
                }
                else
                {
                    MessageBox.Show("X、Y应为整数或小数，W应为整数，且大于或等于0-i");
                }
            }
            else
            {
                MessageBox.Show("X、Y应为整数或小数，W应为整数，且大于0-i");
            }
        }
        //清空按钮
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            textBoxX.Text = "";
            textBoxY.Text = "";
            textBoxW.Text = "";
            locations = new List<location>();
            locationlist = new List<location>();
            listView.Items.Clear();
            result.Content = "";
            result2.Content = "";
            resultlist.Text = "";
        }
    }
}
