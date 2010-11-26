/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.UnitTest {
    [TestFixture]
    public class TestBase {
        public event ProgressChangedEventHandler ProgressChanged = null;

        public void ReportProgress(Progress progress) {
            if (ProgressChanged != null) {
                ProgressChangedEventArgs e = new ProgressChangedEventArgs(progress);
                ProgressChanged(this, e);
            }
        }

        public Tickers Tickers = new Tickers ();

        public event MessageEventHandler WriteMessage = null;

        public void ReportMessage(string message) {
            if (WriteMessage != null) {
                WriteMessage(this, message);
            }
        }

        [TestFixtureSetUp]
        public virtual void Setup() {
            WriteMessage += new MessageEventHandler(TestBase_WriteMessage);
            System.GC.Collect(0);
            
        }

        void TestBase_WriteMessage(object sender, string message) {
            System.Console.WriteLine (message);
        }

        [TestFixtureTearDown]
        public virtual void TearDown() {

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
            return (Elapsed / 1000f).ToString("#0.00");
        }
    }

    public class Tickers : Ticker {
        public Dictionary<object, Ticker> tickers = new Dictionary<object, Ticker>();
        public void Add(object key, Ticker value) {
            tickers.Add (key, value);
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
            foreach(Ticker ticker in tickers.Values) {
                ticker.Stop ();
            }
            return 0;
        }
    }
}
