using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Reflection;

namespace clientcs {
	class Program {
		
		public static Type type = null;
		static public void Push(object obj, int value) {
			type.InvokeMember("Push",
					System.Reflection.BindingFlags.InvokeMethod, null, obj,
					new object[] { value });
		}
		
		static void Main(string[] args) {
			type = Type.GetTypeFromProgID("KSR.Stos");
			if(type != null) {
				object obj = Activator.CreateInstance(type);
				Push(obj, 3);
				Push(obj, 1);
				Push(obj, 4);
			} else {
				Console.WriteLine("Cannot find type");
			}
		}
	}
}

