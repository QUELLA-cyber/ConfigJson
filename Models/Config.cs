using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace ConfigJson.Models
{
    public class Config
    {// Общие настройки приложения
        public GeneralSettings GeneralSettings { get; set; } = new();

        // Настройки пользовательского интерфейса
        public UISettings UISettings { get; set; } = new();

        // Статический метод для загрузки конфигурации из файла
        public static Config Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<Config>(json) ?? new Config();
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    return new Config();
                }
            }
            return new Config();
        }

        // Метод для сохранения конфигурации в файл
        public void Save(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

    // Класс общих настроек приложения
    public class GeneralSettings
    {
        // Название приложения
        public string AppName { get; set; } = "MyApp";

        // Версия приложения
        public string Version { get; set; } = "1.0";

        // Локаль (язык и регион)
        public string Locale { get; set; } = "en-US";

        // Тема оформления (light/dark)
        public string Theme { get; set; } = "light";

        // Флаг автоматического обновления
        public bool AutoUpdate { get; set; } = false;
    }

    // Класс настроек пользовательского интерфейса
    public class UISettings
    {
        // Шрифт интерфейса
        public string Font { get; set; } = "Arial";

        // Размер шрифта
        public int FontSize { get; set; } = 14;

        // Цвет фона
        public string BackgroundColor { get; set; } = "#FFFFFF";

        // Цвет текста
        public string TextColor { get; set; } = "#000000";

        // Флаг отображения боковой панели
        public bool ShowSidebar { get; set; } = true;
    }
}

