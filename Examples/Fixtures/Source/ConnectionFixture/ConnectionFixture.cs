using System;
using fit;

namespace fit.gui.examples
{
	public class ConnectionFixture : ActionFixture
	{
		private bool isConnected = false;

		public void Connect()
		{
			isConnected = true;
		}

		public bool IsConnected()
		{
			return isConnected;
		}

		public void Disconnect()
		{
			isConnected = false;
		}
	}
}
