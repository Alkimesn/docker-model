using System;
using System.Collections.Generic;
using System.IO;
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

namespace dockerModel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ModelComponentBase system = null;

        string Calculate()
        {
            float maxtime = 100000;
            system.FillTimeline(maxtime);
            float step = 1f;
            int total = 0;
            int failed = 0;
            while (total*step < maxtime)
            {
                if (!system.IsFunctional(total*step)) failed++;
                total++;
            }
            return "Total:" + total + "; failed:" + failed + "; failed coef:" + ((float)failed) / total;
        }
        void Calculate2()
        {
            List<string> output=new List<string>();
            float maxtime = 100000;
            system.FillTimeline(maxtime);
            float step = 0.2f;
            int total = 0;
            Dictionary<string, int> result = new Dictionary<string, int>();
            while (total * step < maxtime)
            {
                float time = (float)(Math.Round(total * step * 100) / 100);
                string res = system.GetFunctionalTotal(time);
                string res2 = system.IsFunctional(time).ToString();
                output.Add(res+res2);
                if (result.ContainsKey(res)) result[res]++;
                else result.Add(res, 1);
                if (result.ContainsKey(res2)) result[res2]++;
                else result.Add(res2, 1);
                total++;
            }
            foreach (var key in result.Keys.OrderByDescending(x => x))
                output.Insert(0, "("+key + "): " + result[key]);
            File.WriteAllLines("output.txt", output);
        }

        async Task CalculateMain()
        {
            tb.Content = "Calculating...";
            string res=await Task.Run(() => Calculate());
            tb.Content = res;
        }
        async Task CalculateMain2()
        {
            tb.Content = "Calculating...";
            await Task.Run(() => Calculate2());
            tb.Content = "Log ready";
        }

        UIElement Fill(ModelComponentBase component)
        {
            if(component is ModelComponentComplexAND and)
            {
                StackPanel newPanel = new StackPanel();
                newPanel.VerticalAlignment = VerticalAlignment.Center;
                newPanel.HorizontalAlignment = HorizontalAlignment.Center;
                newPanel.Orientation = Orientation.Horizontal;
                foreach (var child in and.children)
                    newPanel.Children.Add(Fill(child));
                return newPanel;
            }
            if (component is ModelComponentComplexOR or)
            {
                StackPanel newPanel = new StackPanel();
                newPanel.VerticalAlignment = VerticalAlignment.Center;
                newPanel.HorizontalAlignment = HorizontalAlignment.Center;
                newPanel.Orientation = Orientation.Vertical;
                foreach (var child in or.children)
                    newPanel.Children.Add(Fill(child));
                return newPanel;
            }
            if(component is ModelComponentPrimitive prim)
            {
                Label lbl = new Label();
                Brush gray = Brushes.Gray;
                lbl.Background = gray;
                lbl.Margin = new Thickness(3);
                lbl.VerticalAlignment = VerticalAlignment.Stretch;
                lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                lbl.Content = "Fail effective time: "+prim.failTime + "\nRestore effective time: " + prim.restoreTime;
                return lbl;
            }
            return null;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //CalculateMain();
            ModelSystemParser.SerializeTest();
            system= ModelSystemParser.Parse("res.xml");
            gr.Children.Clear();
            StackPanel newPanel = new StackPanel();
            newPanel.VerticalAlignment = VerticalAlignment.Center;
            newPanel.HorizontalAlignment = HorizontalAlignment.Center;
            newPanel.Orientation = Orientation.Horizontal;
            {
                Label lbl = new Label();
                Brush gray = Brushes.LightBlue;
                lbl.Background = gray;
                lbl.Margin = new Thickness(3);
                lbl.VerticalAlignment = VerticalAlignment.Stretch;
                lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                lbl.Content = "Client";
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                newPanel.Children.Add(lbl);
            }

            newPanel.Children.Add(Fill(system));

            {
                Label lbl = new Label();
                Brush gray = Brushes.LightBlue;
                lbl.Background = gray;
                lbl.Margin = new Thickness(3);
                lbl.VerticalAlignment = VerticalAlignment.Stretch;
                lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                lbl.Content = "Data";
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                newPanel.Children.Add(lbl);
            }

            gr.Children.Add(newPanel);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CalculateMain();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CalculateMain2();
        }
    }
}
