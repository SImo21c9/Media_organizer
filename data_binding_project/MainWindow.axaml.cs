using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Avalonia.Controls;
using System.Net.Http;
using System.Text.Json.Nodes;
using Avalonia.Interactivity;
using Newtonsoft.Json;

namespace data_binding_project;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private string _time;
    public string Time { get => _time; set {_time = value; OnPropertyChanged(nameof(Time)); } }

    private string _windspeed; 
    public string Windspeed { get => _windspeed; set { _windspeed = value; OnPropertyChanged(nameof (Windspeed)); } }

    private string _temperature;
    public string Temperature { get => _temperature; set { _temperature = value; OnPropertyChanged(nameof(Temperature)); } }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetStringAsync(
                "https://api.open-meteo.com/v1/forecast?latitude=55.6761&longitude=12.5683&current_weather=true");

            var data = JsonConvert.DeserializeObject<WeatheResponse>(response);
            if (data != null && data.current_weather != null )
            {
                Time = "Time: " + data.current_weather.time ;
                Windspeed = "WindSpeed: " + data.current_weather.windspeed + " km/h";
                Temperature = "Temperature: " + data.current_weather.temperature + "\ud83c\udf21\ufe0f ";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occured while getting weather data");
        }
        
       
    }

    public class WeatheResponse
    {
        public CurrentWeather current_weather { get; set; }
    }

    public class CurrentWeather
    {
        public string time { get; set; }
        public double windspeed { get; set; }
        public double temperature { get; set; }
    } 
}