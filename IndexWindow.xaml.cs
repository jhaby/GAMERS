﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GAMERS_TECH
{
    public partial class IndexWindow : Window
    {
        private UserData userinfo;
        private string uri;

        public IndexWindow(UserData userinfo, string uri)
        {
            InitializeComponent();
            this.userinfo = userinfo;
            this.uri = uri;
        }
    }
}
