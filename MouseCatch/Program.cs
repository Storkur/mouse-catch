using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MouseCatch
{
	 

    class Program
    {
		[Flags]
		public enum MouseEventFlags
		{
			Move = 0x0001,
			LeftDown = 0x0002,
			LeftUp = 0x0004,
			RightDown = 0x0008,
			RightUp = 0x0010,
			MiddleDown = 0x0020,
			MiddleUp = 0x0040,
			Wheel = 0x0800,
			Absolute = 0x8000
		}
		

        static void Main(string[] args)
        {
			var vm_host = "remote-pc";
			var client = new System.Net.Sockets.TcpClient(vm_host, 7001);
			var clientStream = client.GetStream();
			var clientWriter = new System.IO.BinaryWriter(clientStream);

			Action<MouseEventFlags, int, int> mouse_event = (flags, x, y) =>
			{
				var messageStream = new System.IO.MemoryStream();
				var messageWriter = new System.IO.BinaryWriter(messageStream);
				messageWriter.Write(0);
				messageWriter.Write((uint)flags);
				messageWriter.Write(x);
				messageWriter.Write(y);
				messageWriter.Write(0);
				var message = messageStream.ToArray();
				clientWriter.Write(message.Length);
				clientWriter.Write(message);
				clientStream.Flush();
			};
        }

		[DllImport("user32.dll")]
		public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);
    }
}
