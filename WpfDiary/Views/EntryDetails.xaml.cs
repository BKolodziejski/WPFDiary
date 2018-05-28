﻿using System;
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

namespace WpfDiary.Views
{
    /// <summary>
    /// Interaction logic for EntryDetails.xaml
    /// </summary>
    public partial class EntryDetails : UserControl
    {
        public EntryDetails(DiaryEntryViewModel entry)
        {
            DataContext = entry;
            InitializeComponent();
        }
    }
}