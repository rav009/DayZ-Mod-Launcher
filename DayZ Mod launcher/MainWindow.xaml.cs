using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DayZ_Mod_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string err_path_oa = "Can not find arma2oa.exe!";
        private readonly string err_path_arma = "Can not find arma2.exe!";
        private readonly string err_path_nomods = "No mods found under the folder of Arma2 OA!";
        private readonly string err_oa_run = "Arma 2 OA already running!";
        private PathMod _pm = null;
        private Remember _rm = new Remember();

        public MainWindow()
        {
            InitializeComponent();
            if(_rm.HasRemember())
            {
                string armapath = "";
                string armaoapath = "";
                _rm.ReadRemember(ref armapath, ref armaoapath);
                pathtext_arma.Text = armapath;
                pathtext_oa.Text = armaoapath;
                this.OnListAvaible();
            }
        }

        private void Button_Click_arma(object sender, RoutedEventArgs e)
        {
            string path = this.GetPath();
            if (path != string.Empty)
            {
                pathtext_arma.Text = path;
                this.OnListAvaible();
            }
        }

        private void Button_Click_oa(object sender, RoutedEventArgs e)
        {
            string path = this.GetPath();
            if (path != string.Empty)
            {
                pathtext_oa.Text = path;
                this.OnListAvaible();
            }
        }

        private string GetPath()
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            return fbd.SelectedPath;
        }


        private void OnListAvaible()
        {
            if (pathtext_arma.Text == string.Empty || pathtext_oa.Text == string.Empty)
            {
                return;
            }
            this._pm = new PathMod(pathtext_arma.Text, pathtext_oa.Text);
            if (!_pm.TestPath(_pm._armapath, _pm._arma2exename))
            {
                MessageBox.Show(err_path_arma);
                return;
            }
            if(!_pm.TestPath(_pm._oapath, _pm._arma2oaexename))
            {
                MessageBox.Show(err_path_oa);
                return;
            }
            try
            {
                _rm.Write(_pm._armapath, _pm._oapath);
            }
            catch
            { }
            List<string> paths = _pm.LoadModNames();
            if(paths == null || paths.Count == 0)
            {
                MessageBox.Show(err_path_nomods);
                return;
            }
            foreach(string m in paths)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = m+"                                                                                                    ";
                //item.Name = m;
                AvaibleList.Items.Add(item);
            }
        }

        private void Button_Click_add(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)AvaibleList.SelectedItem;
            if(item == null ||item.Content == null || item.Content.ToString() == string.Empty)
            {
                return;
            }
            AvaibleList.Items.Remove(item);
            UseList.Items.Add(item);
        }

        private void Button_Click_remove(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)UseList.SelectedItem;
            if (item == null || item.Content == null || item.Content.ToString() == string.Empty)
            {
                return;
            }
            UseList.Items.Remove(item);
            AvaibleList.Items.Add(item);
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            if(this._pm == null)
            {
                return;
            }
            else
            {
                Process[] processes = Process.GetProcesses();
                for(int i =0;i<processes.Length;i++)
                {
                    if (processes[i].ProcessName.ToLower().Equals(_pm._arma2oaexename.ToLower().Replace(".exe","")))
                    {
                        MessageBox.Show(err_oa_run);
                        return;
                    }
                }
                List<string> mods = new List<string>();
                foreach(ListBoxItem i in UseList.Items)
                {
                    mods.Add(i.Content.ToString());
                }
                string[] rs =_pm.GetRunCmd(mods, BE_checkbox.IsChecked);
                //MessageBox.Show(rs[0] + " " + rs[1]);
                try
                {
                    System.Diagnostics.Process.Start(rs[0], rs[1]);
                    this.WindowState = WindowState.Minimized;
                }
                catch(Exception ext)
                {
                    MessageBox.Show(ext.Message);
                }
            }
        }

        private void Button_Click_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_min(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Mouse_moveHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.Source == this) this.DragMove();
        }

    }
}
