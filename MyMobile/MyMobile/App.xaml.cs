﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyMobile
{
    public partial class App : Application
    {

        private const string DATABASE_NAME = "mainbase.db";
        private static DbProxy database;

        public static DbProxy Database
        {
            get
            {
                if (database == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);
                    database = new DbProxy(path);
                }

                return database;
            }
        }


        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
