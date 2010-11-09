/*
    BlamLib: .NET SDK for the Blam Engine

    Copyright (C) 2005-2010  Kornner Studios (http://kornner.com)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
﻿using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlamLib.Test
{
	partial class Halo3
	{
		partial class ScenarioScriptInterop
		{
			public ScenarioScriptInterop(Blam.HaloOdst.CacheFile cf) : base(cf,
				new TagInterface.Block<Blam.Halo3.Tags.hs_scripts_block>(null, 0),
				0x3E0 + 0x4C,
				0x3F4 + 0x4C, 0x400 + 0x4C,
				0x4A4 + 0x4C)
			{
				hs_scripts = base.sncr_hs_scripts as TagInterface.Block<Blam.Halo3.Tags.hs_scripts_block>;
			}
		};
	};

	[TestClass]
	public partial class HaloOdst : BaseTestClass
	{
		const string kTestResultsPath = TestLibrary.kTestResultsPath + @"HaloOdst\";

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			(Program.GetManager(BlamVersion.HaloOdst_Xbox) as Managers.IStringIdController)
				.StringIdCacheOpen(BlamVersion.HaloOdst_Xbox);

			System.IO.Directory.CreateDirectory(kTestResultsPath);
		}
		[ClassCleanup]
		public static void Dispose()
		{
			(Program.GetManager(BlamVersion.HaloOdst_Xbox) as Managers.IStringIdController)
				.StringIdCacheClose(BlamVersion.HaloOdst_Xbox);
		}

		const string kMapsDirectoryXbox = @"C:\Mount\A\Bungie\Games\HaloOdst\Xbox\Maps\";
		static readonly string[] kMapNames = {
			"mainmenu.map",
			"c100.map",
			"c200.map",
			"h100.map",
			"l200.map",
			"l300.map",
			"sc100.map",
			"sc110.map",
			"sc120.map",
			"sc130.map",
			"sc140.map",
			"sc150.map",
		};

		[TestMethod]
		public void HaloOdstTestCacheOutputXbox()
		{
			CacheFileOutputInfoArgs.TestThreadedMethod(TestContext,
				Halo3.CacheOutputInformationMethod,
				BlamVersion.HaloOdst_Xbox, kMapsDirectoryXbox, kMapNames);
		}

		#region DumpZoneData
		[TestMethod]
		public void HaloOdstTestDumpZoneDataXbox()
		{
			CacheFileOutputInfoArgs.TestThreadedMethod(TestContext, Halo3.DumpZoneDataMethod,
				BlamVersion.HaloOdst_Xbox, kMapsDirectoryXbox, kMapNames);
		}
		#endregion

		#region ScanForScriptFunctions
		static void ScanForScriptFunctionsImpl(string[] script_functions, Blam.HaloOdst.CacheFile cf)
		{
			var interop = new Halo3.ScenarioScriptInterop(cf);

			interop.FindFunctionNames(script_functions);
		}
		static void ScanForScriptFunctions(BlamVersion engine, string path, string[] script_functions)
		{
			using (var handler = new CacheHandler<Blam.HaloOdst.CacheFile>(engine, path))
			{
				var cf = handler.CacheInterface;
				cf.Read();

				ScanForScriptFunctionsImpl(script_functions, handler.CacheInterface);
			}
		}
		
		[TestMethod]
		public void HaloOdstTestScanForScriptFunctions()
		{
			string[] script_functions;
			var engine = BlamVersion.HaloOdst_Xbox;

			Scripts.InitializeScriptFunctionsList(engine, out script_functions);
			foreach (var s in kMapNames)
				ScanForScriptFunctions(engine, kMapsDirectoryXbox + s, script_functions);
			Scripts.OutputFunctionNames(false, kTestResultsPath, "halo_odst.functions.xml", script_functions);
		}
		#endregion
	};
}