using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Text;
using System.Globalization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System.Threading;
using Windows.Media.Playback;
using Windows.UI;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Clock
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ComboBox Sound_select_box = new ComboBox();
            Button Start_button = new Button
            {
                IsEnabled = false
            };
            Button Reset_button = new Button
            {
                IsEnabled = false
            };
            Timer.Tick += Timer_Tick;
            Stopwatch.Tick += Stopwatch_Tick;
            string theme = Application.Current.RequestedTheme.ToString();
            if (theme == "Dark")
            {
                DarkThemeBtn.IsChecked = true;
                Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                DarkThemeBtn.IsChecked = false;
                Background = new SolidColorBrush(Colors.White);
            }
        }

        DispatcherTimer Time = new DispatcherTimer();
        DispatcherTimer Stopwatch = new DispatcherTimer();
        DispatcherTimer Timer = new DispatcherTimer();
        int hour_stopwatch, min_stopwatch, sec_stopwatch = 0;
        int stopwatch_work, timer_work = 0;
        int hour_timer, min_timer, sec_timer = 0;
        MediaPlayer TimeTick = new MediaPlayer();
        MediaPlayer TimerEnd = new MediaPlayer();
        int timer_worked = 0;
        private void Time_Tick(object sender, object e)
        {
            Time_Display.Text = DateTime.Now.ToString("h:mm:ss tt");
        }

        private void Clock_checked(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            Time.Tick += Time_Tick;
            Time.Interval = new TimeSpan(0, 0, 0, 0, 100);
            Time.Start();
        }

        private void Stopwatch_checked(object sender, RoutedEventArgs e)
        {
            Time.Stop();
            if (hour_stopwatch <= 9)
            {
                Time_Display.Text = hour_stopwatch.ToString() + ":";
            }
            else
            {
                Time_Display.Text = "0" + hour_stopwatch.ToString() + ":";
            }
            if (min_stopwatch <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + min_stopwatch.ToString() + ":";
            }
            else
            {
                Time_Display.Text = Time_Display.Text + min_stopwatch.ToString() + ":";
            }
            if (sec_stopwatch <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + sec_stopwatch.ToString();
            }
            else
            {
                Time_Display.Text = Time_Display.Text + sec_stopwatch.ToString();
            }
            DataContext = this;
            Stopwatch.Interval = new TimeSpan(0, 0, 1);
            if (stopwatch_work == 0)
            {
                Stopwatch.Stop();
            }
            else if (stopwatch_work == 1)
            {
                Stopwatch.Start();
            }
        }

        private void Clock_click(object sender, RoutedEventArgs e)
        {
            Start_button.IsEnabled = false;
            Reset_button.IsEnabled = false;
            Time_Display.Text = DateTime.Now.ToString("h:mm:ss tt");
            Hours_text.IsEnabled = false;
            Minutes_text.IsEnabled = false;
            Seconds_text.IsEnabled = false;
            Timer.Stop();
            Stopwatch.Stop();
        }

        private void Start_click(object sender, RoutedEventArgs e)
        {
            if (Stopwatch_radio.IsChecked == true)
            {
                if (stopwatch_work == 0)
                {
                    Start_button.Content = "Stop";
                    stopwatch_work = 1;
                }
                else if (stopwatch_work == 1)
                {
                    Start_button.Content = "Start";
                    stopwatch_work = 0;
                }
                if (stopwatch_work == 0)
                {
                    Stopwatch.Stop();
                }
                else if (stopwatch_work == 1)
                {
                    Stopwatch.Start();
                }
            }
            else if (Timer_radio.IsEnabled == true)
            {
                if (timer_worked == 0)
                {
                    hour_timer = Convert.ToInt32(Hours_text.Text);
                    min_timer = Convert.ToInt32(Minutes_text.Text);
                    sec_timer = Convert.ToInt32(Seconds_text.Text);
                }
                TimerEnd.Pause();
                if (timer_work == 0)
                {
                    Start_button.Content = "Stop";
                    timer_work = 1;
                }
                else if (timer_work == 1)
                {
                    Start_button.Content = "Start";
                    timer_work = 0;
                }
                if (timer_work == 0)
                {
                    Timer.Stop();
                }
                else if (timer_work == 1)
                {
                    timer_worked = 1;
                    Timer.Start();
                }
                if (hour_timer <= 9)
                {
                    Time_Display.Text = hour_timer.ToString() + ":";
                }
                else
                {
                    Time_Display.Text = "0" + hour_timer.ToString() + ":";
                }
                if (min_timer <= 9)
                {
                    Time_Display.Text = Time_Display.Text + "0" + min_timer.ToString() + ":";
                }
                else
                {
                    Time_Display.Text = Time_Display.Text + min_timer.ToString() + ":";
                }
                if (sec_timer <= 9)
                {
                    Time_Display.Text = Time_Display.Text + "0" + sec_timer.ToString();
                }
                else
                {
                    Time_Display.Text = Time_Display.Text + sec_timer.ToString();
                }
            };
        }

        private void Reset_click(object sender, RoutedEventArgs e)
        {
            if (Stopwatch_radio.IsChecked == true)
            {
                stopwatch_work = 0;
                Stopwatch.Stop();
                Start_button.Content = "Start";
                sec_stopwatch = 0;
                min_stopwatch = 0;
                hour_stopwatch = 0;
                Time_Display.Text = "0:00:00";
            }
            else if (Timer_radio.IsChecked == true)
            {
                timer_work = 0;
                Timer.Stop();
                Start_button.Content = "Start";
                hour_timer = Convert.ToInt32(Hours_text.Text);
                min_timer = Convert.ToInt32(Minutes_text.Text);
                sec_timer = Convert.ToInt32(Seconds_text.Text);
                timer_worked = 0;
                if (hour_timer <= 9)
                {
                    Time_Display.Text = hour_timer.ToString() + ":";
                }
                else
                {
                    Time_Display.Text = "0" + hour_timer.ToString() + ":";
                }
                if (min_timer <= 9)
                {
                    Time_Display.Text = Time_Display.Text + "0" + min_timer.ToString() + ":";
                }
                else
                {
                    Time_Display.Text = Time_Display.Text + min_timer.ToString() + ":";
                }
                if (sec_timer <= 9)
                {
                    Time_Display.Text = Time_Display.Text + "0" + sec_timer.ToString();
                }
                else
                {
                    Time_Display.Text = Time_Display.Text + sec_timer.ToString();
                }
            }
        }

        private new void Loaded(object sender, RoutedEventArgs e)
        {
            TimeTick.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/sound1.wav"));
            TimeTick.Play();
            TimeTick.IsLoopingEnabled = true;
        }

        private void Sound_switch_Toggled(object sender, RoutedEventArgs e)
        {
            if (Sound_select_box != null)
            {
                Sound_switch_Toggled_1();
            }
        }

        private void Sound_box_changed(object sender, SelectionChangedEventArgs e)
        {
            Sound_switch_Toggled_1();
        }

        private void Sound_switch_Toggled_1()
        {
            if (Sound_select_box.SelectedIndex == 0)
            {
                TimeTick.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/sound1.wav"));
            }
            else if (Sound_select_box.SelectedIndex == 1)
            {
                TimeTick.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/sound2.wav"));
            }
            else if (Sound_select_box.SelectedIndex == 2)
            {
                TimeTick.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/sound3.wav"));
            }
            else if (Sound_select_box.SelectedIndex == 3)
            {
                TimeTick.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/sound4.wav"));
            }
            else if (Sound_select_box.SelectedIndex == 4)
            {
                TimeTick.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/sound5.wav"));
            }
            if (Sound_switch.IsOn == true)
            {
                TimeTick.Play();
            }
            else if (Sound_switch.IsOn == false)
            {
                TimeTick.Pause();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (DarkThemeBtn.IsChecked == true)
            {
                RequestedTheme = ElementTheme.Dark;
                Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                RequestedTheme = ElementTheme.Light;
                Background = new SolidColorBrush(Colors.White);
            }
        }

        private void Timer_checked(object sender, RoutedEventArgs e)
        {
            Time.Stop();
            DataContext = this;
            Timer.Interval = new TimeSpan(0, 0, 1);
            if (hour_timer <= 9)
            {
                Time_Display.Text = hour_timer.ToString() + ":";
            }
            else
            {
                Time_Display.Text = "0" + hour_timer.ToString() + ":";
            }
            if (min_timer <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + min_timer.ToString() + ":";
            }
            else
            {
                Time_Display.Text = Time_Display.Text + min_timer.ToString() + ":";
            }
            if (sec_timer <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + sec_timer.ToString();
            }
            else
            {
                Time_Display.Text = Time_Display.Text + sec_timer.ToString();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            sec_timer--;
            if (sec_timer < 0)
            {
                sec_timer = 59;
                min_timer--;
            }
            else if (min_timer < 0)
            {
                min_timer = 59;
                hour_timer--;
            }
            if ((hour_timer == 0) && (min_timer == 0) && (sec_timer == 0))
            {
                timer_worked = 0;
                timer_work = 0;
                Timer.Stop();
                TimerEnd.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/alarm.wav"));
                TimerEnd.Play();
                TimerEnd.IsLoopingEnabled = true;
            }
            if (hour_timer <= 9)
            {
                Time_Display.Text = hour_timer.ToString() + ":";
            }
            else
            {
                Time_Display.Text = "0" + hour_timer.ToString() + ":";
            }
            if (min_timer <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + min_timer.ToString() + ":";
            }
            else
            {
                Time_Display.Text = Time_Display.Text + min_timer.ToString() + ":";
            }
            if (sec_timer <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + sec_timer.ToString();
            }
            else
            {
                Time_Display.Text = Time_Display.Text + sec_timer.ToString();
            }
        }

        private void Timer_clicked(object sender, RoutedEventArgs e)
        {
            Stopwatch.Stop();
            Time.Stop();
            Start_button.IsEnabled = true;
            Reset_button.IsEnabled = true;
            Hours_text.IsEnabled = true;
            Minutes_text.IsEnabled = true;
            Seconds_text.IsEnabled = true;
        }

        private void Stopwatch_click(object sender, RoutedEventArgs e)
        {
            Time.Stop();
            Timer.Stop();
            Start_button.IsEnabled = true;
            Reset_button.IsEnabled = true;
            Hours_text.IsEnabled = false;
            Minutes_text.IsEnabled = false;
            Seconds_text.IsEnabled = false;
        }

        private void Stopwatch_Tick(object sender, object e)
        {
            sec_stopwatch++;
            if (sec_stopwatch > 59)
            {
                sec_stopwatch = 0;
                min_stopwatch++;
            }
            if (min_stopwatch > 59)
            {
                min_stopwatch = 0;
                hour_stopwatch++;
            }
            if (hour_stopwatch <= 9)
            {
                Time_Display.Text = hour_stopwatch.ToString() + ":";
            }
            else
            {
                Time_Display.Text = "0" + hour_stopwatch.ToString() + ":";
            }
            if (min_stopwatch <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + min_stopwatch.ToString() + ":";
            }
            else
            {
                Time_Display.Text = Time_Display.Text + min_stopwatch.ToString() + ":";
            }
            if (sec_stopwatch <= 9)
            {
                Time_Display.Text = Time_Display.Text + "0" + sec_stopwatch.ToString();
            }
            else
            {
                Time_Display.Text = Time_Display.Text + sec_stopwatch.ToString();
            }
        }
    }
}
