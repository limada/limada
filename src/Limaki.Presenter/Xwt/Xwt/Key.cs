// 
// Key.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

/*  
 * REMARK lytico
 * this is to refactor Limaki.Presenter to run with 
 * Mono.Xwt https://github.com/mono/xwt
 * will be removed after full support is reached
*/

using System;

namespace Limaki.Xwt
{
	public enum Key
	{
		Cancel = 0xff69,
		BackSpace = 0xff08,
		Tab = 0xff09,
		LineFeed = 0xff0a,
		Clear = 0xff0b,
		Return = 0xff0d,
		Pause = 0xff13,
		CapsLock = 0xffe5,
		Escape = 0xff1b,
		Space = 0x20,
		PageUp = 0xff55,
		PageDown = 0xff56,
		End = 0xff57,
		Begin = 0xff58,
		Home = 0xff50,
		Left = 0xff51,
		Up = 0xff52,
		Right = 0xff53,
		Down = 0xff54,
		Select = 0xff60,
		Print = 0xff61,
		Execute = 0xff62,
		Delete = 0xffff,
		Help = 0xff6a,
		K0 = 0x30,
		K1 = 0x31,
		K2 = 0x32,
		K3 = 0x33,
		K4 = 0x34,
		K5 = 0x35,
		K6 = 0x36,
		K7 = 0x37,
		K8 = 0x38,
		K9 = 0x39,
		a = 0x61,
		b = 0x62,
		c = 0x63,
		d = 0x64,
		e = 0x65,
		f = 0x66,
		g = 0x67,
		h = 0x68,
		i = 0x69,
		j = 0x6a,
		k = 0x6b,
		l = 0x6c,
		m = 0x6d,
		n = 0x6e,
		o = 0x6f,
		p = 0x70,
		q = 0x71,
		r = 0x72,
		s = 0x73,
		t = 0x74,
		u = 0x75,
		v = 0x76,
		w = 0x77,
		x = 0x78,
		y = 0x79,
		z = 0x7a,
		NumPadSpace = 0xff80,
		NumPadTab = 0xff89,
		NumPadEnter = 0xff8d,
		NumPadF1 = 0xff91,
		NumPadF2 = 0xff92,
		NumPadF3 = 0xff93,
		NumPadF4 = 0xff94,
		NumPadHome = 0xff95,
		NumPadLeft = 0xff96,
		NumPadUp = 0xff97,
		NumPadRight = 0xff98,
		NumPadDown = 0xff99,
		NumPadPrior = 0xff9a,
		NumPadNext = 0xff9b,
		NumPadEnd = 0xff9c,
		NumPadBegin = 0xff9d,
		NumPadInsert = 0xff9e,
		NumPadDelete = 0xff9f,
		NumPadMultiply = 0xffaa,
		NumPadAdd = 0xffab,
		NumPadSeparator = 0xffac,
		NumPadSubtract = 0xffad,
		NumPadDecimal = 0xffae,
		NumPadDivide = 0xffaf,
		NumPad0 = 0xffb0,
		NumPad1 = 0xffb1,
		NumPad2 = 0xffb2,
		NumPad3 = 0xffb3,
		NumPad4 = 0xffb4,
		NumPad5 = 0xffb5,
		NumPad6 = 0xffb6,
		NumPad7 = 0xffb7,
		NumPad8 = 0xffb8,
		NumPad9 = 0xffb9,
		F1 = 0xffbe,
		F2 = 0xffbf,
		F3 = 0xffc0,
		F4 = 0xffc1,
		F5 = 0xffc2,
		F6 = 0xffc3,
		F7 = 0xffc4,
		F8 = 0xffc5,
		F9 = 0xffc6,
		F10 = 0xffc7,
		
		Insert = 0xff63,
		ScrollLock = 0xff14,
		SysReq = 0xff15,
		Undo = 0xff65,
		Redo = 0xff66,
		Menu = 0xff67,
		Find = 0xff68,
		Break = 0xff6b,
		NumLock = 0xff7f,
		Equal = 0xffbd,
		ShiftLeft = 0xffe1,
		ShiftRight = 0xffe2,
		ControlLeft = 0xffe3,
		ControlRight = 0xffe4,
		ShiftLock = 0xffe6,
		MetaLeft = 0xffe7,
		MetaRight = 0xffe8,
		AltLeft = 0xffe9,
		AltRight = 0xffea,
		SuperLeft = 0xffeb,
		SuperRight = 0xffec,
		HyperLeft = 0xffed,
		HyperRight = 0xffee,
		
		Asterisk = 0x2a,
		Plus = 0x2b,
		Comma = 0x2c,
		Minus = 0x2d,
		Period = 0x2e,
		Slash = 0x2f,
		Colon = 0x3a,
		Semicolon = 0x3b,
		Less = 0x3c,
		Greater = 0x3e,
		Question = 0x3f,
		At = 0x40,
		A = 0x41,
		B = 0x42,
		C = 0x43,
		D = 0x44,
		E = 0x45,
		F = 0x46,
		G = 0x47,
		H = 0x48,
		I = 0x49,
		J = 0x4a,
		K = 0x4b,
		L = 0x4c,
		M = 0x4d,
		N = 0x4e,
		O = 0x4f,
		P = 0x50,
		Q = 0x51,
		R = 0x52,
		S = 0x53,
		T = 0x54,
		U = 0x55,
		V = 0x56,
		W = 0x57,
		X = 0x58,
		Y = 0x59,
		Z = 0x5a,
	}
}

