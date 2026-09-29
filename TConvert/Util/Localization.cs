using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TConvert.Util {
	public static class Localization {
		private static readonly Dictionary<string, string> Chinese = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
			{ "File", "文件" }, { "Options", "选项" }, { "Help", "帮助" }, { "Exit", "退出" },
			{ "Launch Terraria", "启动 Terraria" }, { "Open Terraria Folder", "打开 Terraria 文件夹" },
			{ "Premultiply Alpha", "预乘 Alpha" }, { "Compress Images", "压缩图像" }, { "Completion Sound", "完成提示音" },
			{ "Auto-Close", "自动关闭" }, { "Window Progress", "窗口进度" }, { "File Drop Progress", "文件拖放进度" },
			{ "Cmd Line Progress", "命令行进度" }, { "About", "关于" }, { "Credits", "鸣谢" }, { "View on GitHub", "在 GitHub 上查看" },
			{ "Settings", "设置" }, { "Extract", "提取" }, { "Convert", "转换" }, { "Backup", "备份" }, { "Script", "脚本" },
			{ "Extract Mode", "提取模式" }, { "Convert Mode", "转换模式" }, { "Folder", "文件夹" },
			{ "Input Folder", "输入文件夹" }, { "Output Folder", "输出文件夹" }, { "Input File", "输入文件" }, { "Output File", "输出文件" },
			{ "Use Terraria", "使用 Terraria" }, { "Images", "图像" }, { "Sounds", "声音" }, { "Fonts", "字体" }, { "Wave Bank", "Wave Bank" },
			{ "Use Input as Output", "将输入用作输出" }, { "Backup or Restore", "备份或还原" }, { "Content Folder", "内容文件夹" },
			{ "Backup Folder", "备份文件夹" }, { "Restore", "还原" }, { "Run an Extract, Convert, or Restore Script", "运行提取、转换或还原脚本" },
			{ "Script File", "脚本文件" }, { "Run Script", "运行脚本" }, { "Browse", "浏览" },
			{ "Terraria's Content Folder", "Terraria 内容文件夹" }, { "Extract or Convert Files", "拖放文件以提取或转换" }
		};

		public static bool IsChinese { get { return !String.Equals(Properties.Settings.Default.Language, "en-US", StringComparison.OrdinalIgnoreCase); } }

		public static string Text(string value) {
			if (value == null) return null;
			if (IsChinese) {
				string translated;
				return Chinese.TryGetValue(value, out translated) ? translated : value;
			}
			foreach (var pair in Chinese)
				if (String.Equals(pair.Value, value, StringComparison.OrdinalIgnoreCase)) return pair.Key;
			return value;
		}

		public static void Apply(Window window) {
			if (window != null) ApplyElement(window);
		}

		private static void ApplyElement(DependencyObject element) {
			if (element is MenuItem menuItem) ApplyString(menuItem, HeaderedItemsControl.HeaderProperty, menuItem.Header as string);
			if (element is Label { Content: string } label)
				ApplyString(label, ContentControl.ContentProperty, (string)label.Content);
			if (element is HeaderedContentControl headerContent) ApplyString(headerContent, HeaderedContentControl.HeaderProperty, headerContent.Header as string);
			if (element is HeaderedItemsControl headerItems) ApplyString(headerItems, HeaderedItemsControl.HeaderProperty, headerItems.Header as string);
			if (element is ContentControl content && !(content is HeaderedContentControl) && content.Content is string)
				ApplyString(content, ContentControl.ContentProperty, (string)content.Content);
			if (element is TextBlock textBlock) ApplyString(textBlock, TextBlock.TextProperty, textBlock.Text);
			if (element is FrameworkElement framework && framework.ToolTip is string)
				ApplyString(framework, FrameworkElement.ToolTipProperty, (string)framework.ToolTip);
			if (element is ComboBox combo) {
				foreach (var item in combo.Items) {
					if (item is ComboBoxItem comboItem && comboItem.Content is string)
						ApplyString(comboItem, ContentControl.ContentProperty, (string)comboItem.Content);
				}
			}
			foreach (object child in LogicalTreeHelper.GetChildren(element)) {
				if (child is DependencyObject dependencyChild) ApplyElement(dependencyChild);
			}
		}

		private static void ApplyString(DependencyObject target, DependencyProperty property, string current) {
			if (current == null) return;
			string original = current;
			if (IsChinese) {
				foreach (var pair in Chinese)
					if (String.Equals(pair.Value, current, StringComparison.OrdinalIgnoreCase)) { original = pair.Key; break; }
			}
			target.SetValue(property, Text(original));
		}
	}
}
