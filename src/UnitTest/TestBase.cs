/*
 * Limaki 
 * Version 0.071
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Common;
using NUnit.Framework;
using System.Diagnostics;

namespace Limaki.UnitTest {
    [TestFixture]
    public class TestBase {
        public event ProgressChangedEventHandler ProgressChanged = null;

        public void ReportProgress( Progress progress ) {
            if (ProgressChanged != null) {
                ProgressChangedEventArgs e = new ProgressChangedEventArgs(progress);
                ProgressChanged(this, e);
            }
        }

        public Tickers Tickers = new Tickers();

        Ticker defaultTicker = new Ticker();

        public event MessageEventHandler WriteDetail = null;
        public event MessageEventHandler WriteSummary = null;

        public void ReportDetail( string message ) {
            if (WriteDetail != null) {
                WriteDetail(this, message);
            }
        }

        bool summaryDone = false;

        public void ReportSummary( string testName, string message ) {
            if (WriteSummary != null) {
                WriteSummary(this, "*** " + testName + " ***\t" + message);
            }
            summaryDone = true;
        }

        public void ReportSummary() {
            Tickers.Stop();
            string testName = "";
            StackTrace st1 = new StackTrace(0);
            foreach (StackFrame frame in st1.GetFrames()) {
                string oldName = testName;
                testName = frame.GetMethod().Name;
                if (oldName == "ReportSummary") {
                    testName = frame.GetMethod().ReflectedType.Name + "." + testName;
                    break;
                }
            }
            ReportSummary(testName);
        }
        public void ReportSummary( string testName ) {
            Tickers.Stop();
            string message = "*** " + testName + " ***\ttime:\t" + defaultTicker.ElapsedInSec(); ;
            if (WriteSummary != null) {
                WriteSummary(this, message);
            }
            summaryDone = true;
            Tickers.Resume();

        }

        public bool DoDetail = true;
        [TestFixtureSetUp]
        public virtual void Setup() {
            summaryDone = false;
            if (DoDetail)
                WriteDetail += new MessageEventHandler(TestBase_WriteMessage);
            WriteSummary += new MessageEventHandler(TestBase_WriteMessage);
            System.GC.Collect(0);
            Tickers.Add(defaultTicker, defaultTicker);
            Tickers.Start();

        }

        void TestBase_WriteMessage( object sender, string message ) {
            System.Console.WriteLine(message);
        }

        [TestFixtureTearDown]
        public virtual void TearDown() {
            Tickers.Stop();
            if (!summaryDone) {
                string message = "time:\t" + defaultTicker.ElapsedInSec();
                ReportSummary();
            }
        }
    }

    public class Ticker {
        public int StartTick;
        public int Elapsed;
        public virtual void Start() {
            Elapsed = 0;
            Resume();
        }
        public virtual int Stop() {
            Elapsed += Environment.TickCount - StartTick;
            return Elapsed;
        }
        public virtual void Resume() {
            StartTick = Environment.TickCount;
        }
        public virtual string ElapsedInSec() {
            return ( Elapsed / 1000f ).ToString("#0.00");
        }
    }

    public class Tickers : Ticker {
        public Dictionary<object, Ticker> tickers = new Dictionary<object, Ticker>();
        public void Add( object key, Ticker value ) {
            tickers.Add(key, value);
        }
        public Ticker this[object key] {
            get { return tickers[key]; }
        }

        public override void Start() {
            foreach (Ticker ticker in tickers.Values) {
                ticker.Start();
            }
        }
        public override void Resume() {
            foreach (Ticker ticker in tickers.Values) {
                ticker.Resume();
            }
        }
        public override int Stop() {
            foreach (Ticker ticker in tickers.Values) {
                ticker.Stop();
            }
            return 0;
        }
    }
}
