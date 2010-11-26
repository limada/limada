
==================================================
GeckoFX
(C) 2008 Skybound Software. All Rights Reserved.
http://www.geckofx.org
==================================================


Getting Started
---------------
GeckoFX is a .NET wrapper around XULRunner, a runtime based on the same source
code as Firefox.  You can add the control to your windows forms app and use it much the
same way as System.Windows.Forms.WebBrowser.

Since GeckoFX is a wrapper, you need to the XULRunner runtime somewhere on your
development system (and redistribute it with your application).  GeckoFX now works best
with XULRunner 1.9 (Firefox 3).

(1) Download XULRunner 1.9 from:

	ftp://ftp.mozilla.org/pub/xulrunner/releases/1.9.0.0/runtimes/xulrunner-1.9.en-US.win32.zip

(2) In your application startup code, call:

	Skybound.Gecko.Xpcom.Initialize(xulrunnerPath);

where "xulrunnerPath" is the full path to the location where you extracted the "xulrunner" directory
(containing xul.dll, xpcom.dll, etc).

(3) OPTIONAL: Specify a profile directory by setting Xpcom.ProfileDirectory.

(4) OPTIONAL: There are some files included with XULRunner which GeckoFX doesn't need.  You may
safely delete them:

AccessibleMarshal.dll
dependentlibs.list
mozctl.dll
mozctlx.dll
java*.*
*.ini
*.txt
*.exe

(5) OPTIONAL:  XULRunner does not support about:config out of the box.  If you want to provide
access to this configuration page, copy the files from the "chrome" directory that came with
GeckoFX into the "chrome" directory in your XULRunner path.

The files that need to be copied are "geckofx.jar" and "geckofx.manifest".


Notes about XULRunner 1.8
---------------------
XULRunner 1.8 is based on the same source code as Firefox 2.  If your application
requires an embedded Firefox 2 browser, rebuild GeckoFX using the "Debug 1.8" build
configuration.  The generated assembly will be put in it's own directory (bin\Debug 1.8).

You can download XULRunner 1.8.1 from:

	ftp://ftp.mozilla.org/pub/xulrunner/releases/1.8.1.3/contrib/win32/

Support for XULRunner 1.8 will probably be removed from a future version of GeckoFX.


Known Bugs
----------
- The right-click menu is still missing some standard features like cut, paste.

Changes in 1.9.0.0/1.8.1.5
--------------------------
- Added support for XULRunner 1.9. (Geckofx now works with both XULRunner 1.8.1.3 and 1.9.0.0)
- Downloading files works, as does the download manager. (requires XULRunner 1.9)
- Added GeckoPreferences class providing programmatic access to preferences.
- Also fixed about:config so that preferences are editable at runtime.
- Added DomClick and DomSubmit events.  Also fixed the "Cancel" property in the DomEventArgs so
that returning Cancel=true from any Dom event will prevent the event from reaching the web browser.
- Fixed a crash bug when right-clicking on an XML or XUL page.

Changes in 1.8.1.4
------------------
- HTTPS is now supported.  Remember that if you disabled the PSM by removing "pipnss.dll" and "pipnss.xpt" from
your XULRunner "components" directory, you need to put those files back now to support HTTPS.
- Displaying an alert or navigating to an invalid page when the browser control was first loaded would cause it to crash
- Confirm dialog can't be resized any more
- Added a Response property to the Navigated event to get additional information about the HTTP response
- CreateWindow event works properly; handle it to allow javascript code to open new windows.  Also handle the
WindowSetBounds event to update and set the position & size of the new window.
- Added Cut, Copy, Paste, Undo, Redo methods to WebBrowser, plus CanCut, CanCopy etc.  Also added many of these to the context menu.
- Added ToolTips
- Added GetAttributeNS() and SetAttributeNS() methods to GeckoElement
- Added Xpcom.ProfileDirectory property to specify where the user profile & cache is stored

Changes in 1.8.1.3
------------------
- The Geckofx assembly is now signed with a strong name
- DocumentTitle wasn't being updated properly
- Added: GeckoStyleSheet.OwnerNode, GeckoRule.ParentStyleSheet, StyleRuleCollection.IsReadOnly
- Added: WebBrowser.SaveDocument, WebBrowser.History
- nsURI class makes it easier to interop with nsIURI parameters in mozilla interfaces (for anyone importing their own mozilla interfaces)
- Fixed various bugs

Changes in 1.8.1.2
------------------
- Fixed form submission (by disabling the PSM)
- Added lots of new properties to the DOM objects
- Updated the docs
- Page Properties and View Source use the target IFRAME when you right-click on one
- Various DOM bugs