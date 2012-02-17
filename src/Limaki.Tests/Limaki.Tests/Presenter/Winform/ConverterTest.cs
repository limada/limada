using System;
using System.Windows.Forms;
using Limaki.Presenter.UI;
using Limaki.Presenter.Winform;
using NUnit.Framework;

namespace Limaki.Tests.Winform {
    public class ConverterTest:DomainTest {
        [Test]
        public void KeyConverterTest () {
            for (Keys keys = Keys.None; keys <= Keys.OemClear; keys++) {
                
                Key key = Converter.Convert(keys);
                
                string sKey = Enum.GetName(typeof(Key), key);
                string sKeys = Enum.GetName(typeof(Keys), keys);
                
                this.ReportDetail(sKeys + "\t" + sKey);

                if (key != Key.None) {

                    if (key != Key.Ctrl &&
                        key != Key.Shift &&
                        key != Key.Menu &&
                        keys != Keys.Prior &&
                        keys != Keys.Next &&
                        sKeys != null) {

                        Assert.AreEqual (sKey, sKeys, sKey + "\t" + sKeys);
                    }
                }
            }
        }

        [Test]
        public void KeyEventArgsTest() {
            for (Keys keys = Keys.None; keys <= Keys.BrowserBack; keys++) {
                var args1 = new KeyEventArgs(keys);
                var args2 = Converter.Convert(args1,System.Drawing.Point.Empty);
                Key key2 = Converter.Convert(args1.KeyCode);

                this.ReportDetail(
                    Enum.GetName(typeof(Keys), keys) + "\t" +
                    Enum.GetName(typeof(Key), key2)
                    );

                if (keys == Keys.LWin || keys == Keys.RWin) {
                    Assert.IsTrue(
                       args2.ModifierKeys == ModifierKeys.Windows,
                       Enum.GetName(typeof(ModifierKeys), args2.ModifierKeys));
                    continue;
                }

                Assert.AreEqual (args2.Key, key2, Enum.GetName(typeof(Key),key2));

                args1 = new KeyEventArgs(keys|Keys.Control);
                args2 = Converter.Convert(args1,System.Drawing.Point.Empty);
                key2 = Converter.Convert(args1.KeyCode);
                
                Assert.AreEqual(args2.Key, key2, 
                    Enum.GetName(typeof(Key), key2)+
                    "\t.." + Enum.GetName(typeof(Keys), keys));
                
                Assert.IsTrue (
                    args2.ModifierKeys == ModifierKeys.Control,
                    Enum.GetName(typeof(ModifierKeys), args2.ModifierKeys));

                args1 = new KeyEventArgs(keys | Keys.Alt);
                args2 = Converter.Convert(args1,System.Drawing.Point.Empty);
                key2 = Converter.Convert(args1.KeyCode);

                Assert.AreEqual(args2.Key, key2,
                    Enum.GetName(typeof(Key), key2) +
                    "\t.." + Enum.GetName(typeof(Keys), keys));

                Assert.IsTrue(
                    (args2.ModifierKeys == ModifierKeys.Alt),
                    Enum.GetName(typeof(ModifierKeys), args2.ModifierKeys));

                args1 = new KeyEventArgs(keys | Keys.Shift);
                args2 = Converter.Convert(args1,System.Drawing.Point.Empty);
                key2 = Converter.Convert(args1.KeyCode);

                Assert.AreEqual(args2.Key, key2,
                    Enum.GetName(typeof(Key), key2) +
                    "\t.." + Enum.GetName(typeof(Keys), keys));

                Assert.IsTrue(
                    (args2.ModifierKeys == ModifierKeys.Shift),
                    Enum.GetName(typeof(ModifierKeys), args2.ModifierKeys));

                args1 = new KeyEventArgs(keys | Keys.Shift | Keys.Control);
                args2 = Converter.Convert(args1,System.Drawing.Point.Empty);
                key2 = Converter.Convert(args1.KeyCode);

                Assert.AreEqual(args2.Key, key2,
                    Enum.GetName(typeof(Key), key2) +
                    "\t.." + Enum.GetName(typeof(Keys), keys));

                Assert.IsTrue(
                    args2.ModifierKeys == (ModifierKeys.Shift|ModifierKeys.Control),
                    Enum.GetName(typeof(ModifierKeys), args2.ModifierKeys));
            }
        }
    }
}
