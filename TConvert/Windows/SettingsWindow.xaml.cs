using System.Windows;
using System.Windows.Controls;
using TConvert.Properties;
using TConvert.Util;

namespace TConvert.Windows {
	public partial class SettingsWindow : Window {
		public SettingsWindow(Window owner) {
			Owner = owner;
			InitializeComponent();
			Select(languageCombo, Settings.Default.Language, "zh-CN");
			Select(themeCombo, Settings.Default.Theme, "System");
			ApplyLanguage();
			languageCombo.SelectionChanged += OnLanguageChanged;
		}

		private void OnLanguageChanged(object sender, SelectionChangedEventArgs e) {
			ApplyLanguage();
		}

		private void ApplyLanguage() {
			bool chinese = languageCombo.SelectedItem is ComboBoxItem item && (string)item.Tag == "zh-CN";
			Title = chinese ? "设置" : "Settings";
			languageLabel.Text = chinese ? "语言" : "Language";
			themeLabel.Text = chinese ? "主题" : "Theme";
			okButton.Content = chinese ? "确定" : "OK";
			cancelButton.Content = chinese ? "取消" : "Cancel";
		}

		private static void Select(ComboBox combo, string value, string fallback) {
			string selected = string.IsNullOrEmpty(value) ? fallback : value;
			for (int i = 0; i < combo.Items.Count; i++) {
				var item = combo.Items[i] as ComboBoxItem;
				if (item != null && (string)item.Tag == selected) {
					combo.SelectedIndex = i;
					return;
				}
			}
		}

		private void OnOk(object sender, RoutedEventArgs e) {
			var language = languageCombo.SelectedItem as ComboBoxItem;
			var theme = themeCombo.SelectedItem as ComboBoxItem;
			Settings.Default.Language = language == null ? "zh-CN" : (string)language.Tag;
			Settings.Default.Theme = theme == null ? "System" : (string)theme.Tag;
			Settings.Default.Save();
			SystemThemeManager.Apply(Application.Current);
			foreach (Window window in Application.Current.Windows)
				TConvert.Util.Localization.Apply(window);
			DialogResult = true;
			Close();
		}

		private void OnCancel(object sender, RoutedEventArgs e) {
			DialogResult = false;
			Close();
		}
	}
}