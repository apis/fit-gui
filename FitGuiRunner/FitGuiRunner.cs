using System;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using fit.gui.common;

namespace fit.gui.runner
{
	public class BasicRunner : FileRunner
	{
		protected override void exit()
		{
			output.Close();
		}
	}

	class FitGuiRunner
	{
		static void RegisterCommonDataAsRemotingClient()
		{
			IDictionary channelProperties = new Hashtable();
			channelProperties["Name"] = string.Empty;
			TcpChannel tcpChannel = new TcpChannel(channelProperties, null, null);
			ChannelServices.RegisterChannel(tcpChannel);
			WellKnownClientTypeEntry wellKnownClientTypeEntry = 
				new WellKnownClientTypeEntry(typeof(CommonData), 
				new UriBuilder("tcp", IPAddress.Loopback.ToString(), 8765, typeof(CommonData).Name).ToString());
			RemotingConfiguration.RegisterWellKnownClientType(wellKnownClientTypeEntry);
		}

		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length != 3) return;

			RegisterCommonDataAsRemotingClient();

			CommonData commonData = new CommonData();

			BasicRunner basicRunner = new BasicRunner();
			
			basicRunner.run(args);

			TestRunProperties testRunProperties;

			testRunProperties.countsRight = basicRunner.fixture.counts.right;
			testRunProperties.countsWrong = basicRunner.fixture.counts.wrong;
			testRunProperties.countsIgnores = basicRunner.fixture.counts.ignores;
			testRunProperties.countsExceptions = basicRunner.fixture.counts.exceptions;
			testRunProperties.OutputFile = (string)basicRunner.fixture.summary[CommonData.OUTPUT_FILE];
			testRunProperties.InputUpdate = (DateTime)basicRunner.fixture.summary[CommonData.INPUT_UPDATE];
			testRunProperties.FixturePath = (string)basicRunner.fixture.summary[CommonData.FIXTURE_PATH];
			testRunProperties.RunDate = (DateTime)basicRunner.fixture.summary[CommonData.RUN_DATE];
			testRunProperties.InputFile = (string)basicRunner.fixture.summary[CommonData.INPUT_FILE];
			testRunProperties.RunElapsedTime = basicRunner.fixture.summary[CommonData.RUN_ELAPSED_TIME].ToString();
			commonData.TestRunProperties = testRunProperties;
		}
	}
}