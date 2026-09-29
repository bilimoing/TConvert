using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace TConvert.Util {
	/**<summary>Extract embedded resources.</summary>*/
	public static class EmbeddedResources {
		//========== EXTRACTING ==========
		#region Extracting

		/**<summary>Extract an embedded resource from byte array.</summary>*/
		public static void Extract(string resourcePath, byte[] resourceBytes) {
			string dirName = Path.GetDirectoryName(resourcePath);
			if (!Directory.Exists(dirName)) {
				Directory.CreateDirectory(dirName);
			}

			bool rewrite = true;
			if (File.Exists(resourcePath)) {
				byte[] existing = File.ReadAllBytes(resourcePath);
				if (resourceBytes.SequenceEqual(existing)) {
					rewrite = false;
				}
			}
			if (rewrite) {
				File.WriteAllBytes(resourcePath, resourceBytes);
			}
		}
		/**<summary>Extract an embedded resource from stream.</summary>*/
		public static void Extract(string resourcePath, Stream resourceStream) {
			if (resourceStream == null)
				throw new ArgumentNullException(nameof(resourceStream));

			using (var buffer = new MemoryStream()) {
				resourceStream.CopyTo(buffer);
				Extract(resourcePath, buffer.ToArray());
			}
		}
		/**<summary>Extract an embedded resource from name.</summary>*/
		public static void Extract(string resourcePath, string resourceName) {
			using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
				if (resourceStream == null)
					throw new FileNotFoundException("Embedded resource was not found.", resourceName);
				Extract(resourcePath, resourceStream);
			}
		}

		#endregion
		//=========== LOADING ============
		#region Loading

		/**<summary>Load a dll.</summary>*/
		public static void LoadDll(string dllPath) {
			IntPtr h = LoadLibrary(dllPath);
			if (h == IntPtr.Zero) {
				Exception e = new Win32Exception();
				throw new DllNotFoundException("Unable to load library: " + dllPath, e);
			}
		}

		#endregion
		//============ NATIVE ============
		#region Native

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
		static extern IntPtr LoadLibrary(string lpFileName);

		#endregion
	}
}