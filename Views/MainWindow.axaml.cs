using Avalonia.Controls;
using ConfigJson.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace ConfigJson.Views
{
    public partial class MainWindow : Window
    {
        // Путь к файлу конфигурации
        private readonly string _configFilePath = "config.json";

        // Конструктор главного окна
        public MainWindow()
        {
            InitializeComponent();

            this.FindControl<Button>("ApplyButton").Click += OnApplyButtonClick;
            this.FindControl<Button>("OpenFileButton").Click += OnOpenFileButtonClick;

            LoadConfig();
        }

        // Загрузка конфигурации из файла
        private void LoadConfig()
        {
            if (File.Exists(_configFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_configFilePath);
                    var config = JsonConvert.DeserializeObject<Config>(json) ?? new Config();
                    UpdateUI(config);
                }
                catch
                {
                    SaveConfig(new Config());
                }
            }
            else
            {
                SaveConfig(new Config());
            }
        }

        // Сохранение конфигурации в файл
        private void SaveConfig(Config config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(_configFilePath, json);
        }

        // Обновление элементов интерфейса на основе конфигурации
        private void UpdateUI(Config config)
        {
            this.FindControl<TextBox>("AppNameTextBox").Text = config.GeneralSettings.AppName;
            this.FindControl<TextBox>("VersionTextBox").Text = config.GeneralSettings.Version;
            this.FindControl<TextBox>("LocaleTextBox").Text = config.GeneralSettings.Locale;
            this.FindControl<ComboBox>("ThemeComboBox").SelectedIndex = config.GeneralSettings.Theme == "light" ? 0 : 1;
            this.FindControl<CheckBox>("AutoUpdateCheckBox").IsChecked = config.GeneralSettings.AutoUpdate;

            this.FindControl<TextBox>("FontTextBox").Text = config.UISettings.Font;
            this.FindControl<TextBox>("FontSizeTextBox").Text = config.UISettings.FontSize.ToString();
            this.FindControl<TextBox>("BackgroundColorTextBox").Text = config.UISettings.BackgroundColor;
            this.FindControl<TextBox>("TextColorTextBox").Text = config.UISettings.TextColor;
            this.FindControl<CheckBox>("ShowSidebarCheckBox").IsChecked = config.UISettings.ShowSidebar;
        }

        // Обработчик нажатия кнопки "Применить"
        private void OnApplyButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var config = new Config
            {
                GeneralSettings = new GeneralSettings
                {
                    AppName = this.FindControl<TextBox>("AppNameTextBox").Text,
                    Version = this.FindControl<TextBox>("VersionTextBox").Text,
                    Locale = this.FindControl<TextBox>("LocaleTextBox").Text,
                    Theme = this.FindControl<ComboBox>("ThemeComboBox").SelectedIndex == 0 ? "light" : "dark",
                    AutoUpdate = this.FindControl<CheckBox>("AutoUpdateCheckBox").IsChecked ?? false
                },
                UISettings = new UISettings
                {
                    Font = this.FindControl<TextBox>("FontTextBox").Text,
                    FontSize = int.TryParse(this.FindControl<TextBox>("FontSizeTextBox").Text, out var fontSize) ? fontSize : 14,
                    BackgroundColor = this.FindControl<TextBox>("BackgroundColorTextBox").Text,
                    TextColor = this.FindControl<TextBox>("TextColorTextBox").Text,
                    ShowSidebar = this.FindControl<CheckBox>("ShowSidebarCheckBox").IsChecked ?? false
                }
            };

            SaveConfig(config);
        }

        // Обработчик нажатия кнопки "Открыть файл"
        private void OnOpenFileButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (File.Exists(_configFilePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _configFilePath,
                    UseShellExecute = true
                });
            }
        }
    }
}