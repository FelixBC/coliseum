using Microsoft.Maui.Controls;
using System;

namespace Coliseum.App.Services
{
    public class ThemeManager : BindableObject
    {
        private ResourceDictionary _currentTheme;
        private readonly ResourceDictionary _lightTheme;
        private readonly ResourceDictionary _darkTheme;

        public ThemeManager()
        {
            _lightTheme = Application.Current.Resources["LightTheme"] as ResourceDictionary;
            _darkTheme = Application.Current.Resources["DarkTheme"] as ResourceDictionary;
            CurrentTheme = _lightTheme;
        }

        public ResourceDictionary CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    OnPropertyChanged();
                    ApplyTheme();
                }
            }
        }

        public void ApplyTheme()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(CurrentTheme);
        }

        public void ToggleTheme()
        {
            CurrentTheme = CurrentTheme == _lightTheme ? _darkTheme : _lightTheme;
        }

        public bool IsDarkTheme => CurrentTheme == _darkTheme;
    }
}
