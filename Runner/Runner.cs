using System;
using System.Collections;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using fit.gui.common;

namespace fit.gui
{
	public class Runner : FileRunner
	{
		protected override void exit()
		{
			output.Close();
		}
	}

	class RunnerApplication
	{
		static void RegisterCommonDataAsRemotingClient()
		{
			IDictionary channelProperties = new Hashtable();
			channelProperties["name"] = string.Empty;
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

			Runner runner = new Runner();
			
			runner.run(args);

			TestRunProperties testRunProperties;

			testRunProperties.countsRight = runner.fixture.counts.right;
			testRunProperties.countsWrong = runner.fixture.counts.wrong;
			testRunProperties.countsIgnores = runner.fixture.counts.ignores;
			testRunProperties.countsExceptions = runner.fixture.counts.exceptions;
			testRunProperties.OutputFile = (string)runner.fixture.summary[CommonData.OUTPUT_FILE];
			testRunProperties.InputUpdate = (DateTime)runner.fixture.summary[CommonData.INPUT_UPDATE];
			testRunProperties.FixturePath = (string)runner.fixture.summary[CommonData.FIXTURE_PATH];
			testRunProperties.RunDate = (DateTime)runner.fixture.summary[CommonData.RUN_DATE];
			testRunProperties.InputFile = (string)runner.fixture.summary[CommonData.INPUT_FILE];
			testRunProperties.RunElapsedTime = runner.fixture.summary[CommonData.RUN_ELAPSED_TIME].ToString();
			commonData.TestRunProperties = testRunProperties;
		}
	}
}