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

			if (runner.fixture.summary.ContainsKey(CommonData.OUTPUT_FILE))
			{
				testRunProperties.OutputFile = (string)runner.fixture.summary[CommonData.OUTPUT_FILE];
			}
			else
			{
				testRunProperties.OutputFile = "";
			}

			if (runner.fixture.summary.ContainsKey(CommonData.INPUT_UPDATE))
			{
				testRunProperties.InputUpdate = (DateTime)runner.fixture.summary[CommonData.INPUT_UPDATE];
			}
			else
			{
				testRunProperties.InputUpdate = DateTime.MinValue;
			}

			if (runner.fixture.summary.ContainsKey(CommonData.FIXTURE_PATH))
			{
				testRunProperties.FixturePath = (string)runner.fixture.summary[CommonData.FIXTURE_PATH];
			}
			else
			{
				testRunProperties.FixturePath = "";
			}

			if (runner.fixture.summary.ContainsKey(CommonData.RUN_DATE))
			{
				testRunProperties.RunDate = (DateTime)runner.fixture.summary[CommonData.RUN_DATE];
			}
			else
			{
				testRunProperties.RunDate = DateTime.MinValue;
			}

			if (runner.fixture.summary.ContainsKey(CommonData.INPUT_FILE))
			{
				testRunProperties.InputFile = (string)runner.fixture.summary[CommonData.INPUT_FILE];
			}
			else
			{
				testRunProperties.InputFile = "";
			}

			if (runner.fixture.summary.ContainsKey(CommonData.RUN_ELAPSED_TIME))
			{
				testRunProperties.RunElapsedTime = (string)runner.fixture.summary[CommonData.RUN_ELAPSED_TIME].ToString();
			}
			else
			{
				testRunProperties.RunElapsedTime = "0:00.00";
			}

			commonData.TestRunProperties = testRunProperties;
		}
	}
}