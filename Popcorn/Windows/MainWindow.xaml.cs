﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Extensions;
using Popcorn.Messaging;
using Popcorn.ViewModels.Windows;

namespace Popcorn.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var vm = DataContext as WindowViewModel;
            if (vm != null)
            {
                vm.NavigationService = MainFrame.NavigationService;
            }

            StateChanged += OnStateChanged;
            Messenger.Default.Register<DropFileMessage>(this, e =>
            {
                if (e.Event == DropFileMessage.DropFileEvent.Enter)
                {
                    BorderThickness = new Thickness(1);
                    BorderBrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#CCE51400");
                    GlowBrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#CCE51400");
                    DoubleAnimation da = new DoubleAnimation
                    {
                        To = 0.5d,
                        Duration = new Duration(TimeSpan.FromMilliseconds(750)),
                        EasingFunction = new PowerEase
                        {
                            EasingMode = EasingMode.EaseInOut,
                            Power = 2d
                        }
                    };
                    BeginAnimation(OpacityProperty, da);
                }
                else
                {
                    BorderThickness = new Thickness(0);
                    BorderBrush = Brushes.Transparent;
                    GlowBrush = Brushes.Transparent;
                    DoubleAnimation da = new DoubleAnimation
                    {
                        To = 1.0d,
                        Duration = new Duration(TimeSpan.FromMilliseconds(750)),
                        EasingFunction = new PowerEase
                        {
                            EasingMode = EasingMode.EaseInOut,
                            Power = 2d
                        }
                    };
                    BeginAnimation(OpacityProperty, da);
                }
            });
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            MovieDetailsUc.Margin = WindowState == WindowState.Maximized
                ? new Thickness(0, 0, 16, 0)
                : new Thickness(0, 0, 0, 0);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.I)
            {
                var vm = DataContext as WindowViewModel;
                vm?.OpenAboutCommand.Execute(null);
            }
            else if (e.Key == Key.F1)
            {
                var vm = DataContext as WindowViewModel;
                vm?.OpenHelpCommand.Execute(null);
            }
            else if (e.Key == Key.F3 || (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift &&
                     e.Key == Key.F)
            {
                var searchBox =
                    this.FindChild<TextBox>("SearchBox");
                searchBox.Focus();
            }
        }
    }
}