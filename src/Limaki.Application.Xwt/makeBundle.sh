#!/bin/bash
cd ../bin/Release

# needs Limaki.View.XwtGtk to referenced in project to run
# also use "MAKEBUNDLE" in compiler directives

#gtk-libs are here: /usr/lib/mono/gac/<dll>/2.12.0.0__35e10195dab3c99f/

GTKLIBS="atk-sharp gdk-sharp gtk-sharp glib-sharp glade-sharp glib-sharp pango-sharp Mono.Cairo"

for f in $GTKLIBS; do
   GTKDEPS="$GTKDEPS /usr/lib/mono/gac/$f/2.12.0.0__35e10195dab3c99f/$f.dll"
done

# it doesnt run on machines without mono installed until now
# fails loading images in view-resources, loading libc: 
# DllImport error loading library './libc.so': './libc.so: Ungültiger ELF-Header'.

# used gtk-dll and -so from /usr/lib/cli/, removed absolute paths from config-files, and mono-cairo from /usr/lib/mono/gac/Mono.Cairo/4.0.0.0__0738eb9f132ed756
# changed configs (and libs) in ../3rdParty/bin/linux.gtk-mkbundle

LIMCORE="Limaki.Application.Xwt.exe Limaki.Core.dll"
LIMVIEW="Limaki.View.dll Limaki.View.Resources.dll Limaki.View.Xwt.dll Limaki.View.Html5.dll Xwt.Hmtl5.dll Xwt.dll"
LIMGTK="Limaki.View.Gtk.dll Xwt.Gtk.dll $GTKDEPS" 
LIMDB4O="Db4objects.Db4o.CS.dll Db4objects.Db4o.dll Db4objects.Db4o.Linq.dll Db4objects.Db4o.Optional.dll DB4objects.Db4o.CS.Optional.dll Limaki.db4o.dll Mono.Reflection.dll"
LIMPDF="Limada.PdfProvider.dll itextsharp.dll TidyManaged.dll"
LIMTESTS="Limaki.Tests.dll Limaki.View.Xwt.Tests.dll nunit.framework.dll"
machineconfig="/etc/mono/4.5/machine.config"
PROGNAME="limada.linuxX64"
mkbundle --deps --static --machine-config "$machineconfig" -o $PROGNAME $LIMCORE $LIMVIEW $LIMDB4O #$LIMGTK 
TESTDIR="mkbundle"
if [ ! -d "$TESTDIR" ]; then
   mkdir $TESTDIR
fi
cp $PROGNAME $TESTDIR/
cd $TESTDIR
MONO_LOG_LEVEL=debug ./$PROGNAME
sleep 5

