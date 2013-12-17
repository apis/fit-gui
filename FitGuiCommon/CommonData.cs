using System;
using System.Runtime.Remoting.Lifetime;

namespace fit.gui.common
{
	[Serializable]
	public struct TestRunProperties
	{
		public string OutputFile;
		public DateTime InputUpdate;
		public string FixturePath;
		public DateTime RunDate;
		public string InputFile;
		public string RunElapsedTime;
		public int countsRight;
		public int countsWrong;
		public int countsIgnores;
		public int countsExceptions;
	}

	public class CommonData : MarshalByRefObject
	{
		public const string OUTPUT_FILE = "output file";
		public const string INPUT_UPDATE = "input update";
		public const string COUNTS = "counts";
		public const string FIXTURE_PATH = "fixture path";
		public const string RUN_DATE = "run date";
		public const string INPUT_FILE = "input file";
		public const string RUN_ELAPSED_TIME = "run elapsed time";
		private  TestRunProperties _testRunProperties;

		public TestRunProperties TestRunProperties
		{
			get
			{
				lock (this)
				{
					return _testRunProperties;
				}
			}
			set
			{
				lock (this)
				{
					_testRunProperties = value;
				}
			}
		}

		public override Object InitializeLifetimeService()
		{
			ILease lease = (ILease)base.InitializeLifetimeService();
			if (lease.CurrentState == LeaseState.Initial)
			{
				lease.InitialLeaseTime = TimeSpan.FromSeconds(0);
			}
			return lease;
		}
	}
}
