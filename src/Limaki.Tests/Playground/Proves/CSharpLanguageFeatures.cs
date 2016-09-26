/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2016 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Limaki.Common.Linqish;
using NUnit.Framework;

namespace Limaki.Playground.Proves {
	
	[TestFixture]
	public class CSharp6Features : Limaki.UnitTest.TestBase
	{
		public void NameOf() {
			ReportDetail(nameof(NameOf));
		}
	}
    
}