/*
 * Limada 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */


using System;

namespace Limaki.Common
{
	/// <summary>
	/// Field or Property is transient
	/// </summary>
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
	public class TransientAttribute : Attribute {}
}
