using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace minutnik;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer = new();
    private int _remainingSeconds = 25 * 60;
    private int _cycles = 0;
    private bool _isRunning = false;
    private bool _isBreak = false;

    public MainWindow()
    {
        InitializeComponent();

        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += OnTimerTick;
        UpdateDisplay();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (_remainingSeconds > 0)
        {
            _remainingSeconds--;
            UpdateDisplay();
            return;
        }

        if (!_isBreak)
        {
            _cycles++;
            _isBreak = true;
            _remainingSeconds = GetDurationSeconds(BreakMinutesBox.Text, 5);
            UpdateDisplay();
        }
        else
        {
            _isBreak = false;
            _remainingSeconds = GetDurationSeconds(WorkMinutesBox.Text, 25);
            UpdateDisplay();
        }
    }

    private void OnStartPauseClick(object? sender, RoutedEventArgs e)
    {
        if (_isRunning)
        {
            _timer.Stop();
            _isRunning = false;
            StartPauseButton.Content = "Start";
        }
        else
        {
            _timer.Start();
            _isRunning = true;
            StartPauseButton.Content = "Pauza";
        }
    }

    private void OnResetClick(object? sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _isRunning = false;
        _isBreak = false;
        StartPauseButton.Content = "Start";
        _remainingSeconds = GetDurationSeconds(WorkMinutesBox.Text, 25);
        UpdateDisplay();
    }

    private int GetDurationSeconds(string? input, int defaultMinutes)
    {
        if (int.TryParse(input, out int minutes) && minutes > 0)
        {
            return minutes * 60;
        }
        return defaultMinutes * 60;
    }

    private void UpdateDisplay()
    {
        int minutes = _remainingSeconds / 60;
        int seconds = _remainingSeconds % 60;
        TimeText.Text = $"{minutes:00}:{seconds:00}";
        PhaseText.Text = _isBreak ? "Tryb: Przerwa" : "Tryb: Praca";
        CyclesText.Text = $"Skończone cykle: {_cycles}";
    }
}
