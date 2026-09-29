using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace TConvert.Util {
	/// <summary>Applies a small, native-looking resource palette based on the Windows version and system theme.</summary>
	public static class SystemThemeManager {
		private const string PersonalizeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

		public static void Apply(Application application) {
			if (application == null)
				throw new ArgumentNullException(nameof(application));

			string theme = Properties.Settings.Default.Theme;
			bool dark = string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase)
				|| (string.Equals(theme, "System", StringComparison.OrdinalIgnoreCase) && IsDarkMode());
			int build = Environment.OSVersion.Version.Build;
			bool windows11 = build >= 22000;
			bool windows10 = build >= 10240;

			Color window = dark ? Color.FromRgb(32, 32, 32) : Color.FromRgb(246, 246, 246);
			Color surface = window;
			Color text = dark ? Color.FromRgb(245, 245, 245) : Color.FromRgb(32, 32, 32);
			Color border = dark ? Color.FromRgb(72, 72, 72) : Color.FromRgb(210, 210, 210);
			Color menu = window;

			application.Resources["AppWindowBackground"] = new SolidColorBrush(window);
			application.Resources["AppSurfaceBackground"] = new SolidColorBrush(surface);
			application.Resources["AppMenuBackground"] = new SolidColorBrush(menu);
			application.Resources["AppBorderBrush"] = new SolidColorBrush(border);
			application.Resources["AppForeground"] = new SolidColorBrush(text);
			application.Resources["AppControlBackground"] = new SolidColorBrush(surface);
			application.Resources["AppControlHover"] = new SolidColorBrush(dark ? Color.FromRgb(48, 48, 48) : Color.FromRgb(232, 232, 232));
			application.Resources["AppSelectionBackground"] = new SolidColorBrush(dark ? Color.FromRgb(64, 64, 64) : Color.FromRgb(205, 225, 245));
			application.Resources["AppControlDisabled"] = new SolidColorBrush(dark ? Color.FromRgb(45, 45, 45) : Color.FromRgb(235, 235, 235));
			application.Resources["AppOsVersion"] = windows11 ? "Windows 11" : windows10 ? "Windows 10" : "Windows 7";
			application.Resources["AppUsesModernTheme"] = windows11;
		}

		private static bool IsDarkMode() {
			try {
				object value = Registry.GetValue(PersonalizeKey, "AppsUseLightTheme", 1);
				return value is int light && light == 0;
			}
			catch {
				return false;
			}
		}

		private static byte forty(int value) => (byte)value;
	}
}
