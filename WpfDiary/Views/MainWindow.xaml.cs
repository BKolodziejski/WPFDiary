using System;
using System.Collections.Generic;
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
using WpfDiary.ViewModels;
using WpfDiary.Views;

namespace WpfDiary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow instance;
        public static MainWindow Instance
        {
            get
            {
                return instance;
            }
        }

        public MainWindow()
        {
            instance = this;
            this.DataContext = new DiaryViewModel();
            InitializeComponent();
            this.Content = new DiaryEntryListUserControl(new DiaryViewModel());
        }
    }
}
