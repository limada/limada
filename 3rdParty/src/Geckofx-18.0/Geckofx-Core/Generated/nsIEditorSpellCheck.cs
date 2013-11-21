// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsIEditorSpellCheck.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("334946c3-0e93-4aac-b662-e1b56f95d68b")]
	public interface nsIEditorSpellCheck
	{
		
		/// <summary>
        /// Call this on any change in installed dictionaries to ensure that the spell
        /// checker is not using a current dictionary which is no longer available.
        /// If the current dictionary is no longer available, then pick another one.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CheckCurrentDictionary();
		
		/// <summary>
        /// Returns true if we can enable spellchecking. If there are no available
        /// dictionaries, this will return false.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool CanSpellCheck();
		
		/// <summary>
        /// Turns on the spell checker for the given editor. enableSelectionChecking
        /// set means that we only want to check the current selection in the editor,
        /// (this controls the behavior of GetNextMisspelledWord). For spellchecking
        /// clients with no modal UI (such as inline spellcheckers), this flag doesn't
        /// matter
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitSpellChecker([MarshalAs(UnmanagedType.Interface)] nsIEditor editor, [MarshalAs(UnmanagedType.U1)] bool enableSelectionChecking);
		
		/// <summary>
        /// When interactively spell checking the document, this will return the
        /// value of the next word that is misspelled. This also computes the
        /// suggestions which you can get by calling GetSuggestedWord.
        ///
        /// @see nsISpellChecker::GetNextMisspelledWord
        /// </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetNextMisspelledWord();
		
		/// <summary>
        /// Used to get suggestions for the last word that was checked and found to
        /// be misspelled. The first call will give you the first (best) suggestion.
        /// Subsequent calls will iterate through all the suggestions, allowing you
        /// to build a list. When there are no more suggestions, an empty string
        /// (not a null pointer) will be returned.
        ///
        /// @see nsISpellChecker::GetSuggestedWord
        /// </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetSuggestedWord();
		
		/// <summary>
        /// Check a given word. In spite of the name, this function checks the word
        /// you give it, returning true if the word is misspelled. If the word is
        /// misspelled, it will compute the suggestions which you can get from
        /// GetSuggestedWord().
        ///
        /// @see nsISpellChecker::CheckCurrentWord
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool CheckCurrentWord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string suggestedWord);
		
		/// <summary>
        /// Use when modally checking the document to replace a word.
        ///
        /// @see nsISpellChecker::CheckCurrentWord
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ReplaceWord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string misspelledWord, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string replaceWord, [MarshalAs(UnmanagedType.U1)] bool allOccurrences);
		
		/// <summary>
        /// @see nsISpellChecker::IgnoreAll
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void IgnoreWordAllOccurrences([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string word);
		
		/// <summary>
        /// Fills an internal list of words added to the personal dictionary. These
        /// words can be retrieved using GetPersonalDictionaryWord()
        ///
        /// @see nsISpellChecker::GetPersonalDictionary
        /// @see GetPersonalDictionaryWord
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPersonalDictionary();
		
		/// <summary>
        /// Used after you call GetPersonalDictionary() to iterate through all the
        /// words added to the personal dictionary. Will return the empty string when
        /// there are no more words.
        /// </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetPersonalDictionaryWord();
		
		/// <summary>
        /// Adds a word to the current personal dictionary.
        ///
        /// @see nsISpellChecker::AddWordToDictionary
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddWordToDictionary([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string word);
		
		/// <summary>
        /// Removes a word from the current personal dictionary.
        ///
        /// @see nsISpellChecker::RemoveWordFromPersonalDictionary
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveWordFromDictionary([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string word);
		
		/// <summary>
        /// Retrieves a list of the currently available dictionaries. The strings will
        /// typically be language IDs, like "en-US".
        ///
        /// @see mozISpellCheckingEngine::GetDictionaryList
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetDictionaryList([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] ref System.IntPtr[] dictionaryList, ref uint count);
		
		/// <summary>
        /// @see nsISpellChecker::GetCurrentDictionary
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCurrentDictionary([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// @see nsISpellChecker::SetCurrentDictionary
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCurrentDictionary([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase dictionary);
		
		/// <summary>
        /// Call this to free up the spell checking object. It will also save the
        /// current selected language as the default for future use.
        ///
        /// If you have called CanSpellCheck but not InitSpellChecker, you can still
        /// call this function to clear the cached spell check object, and no
        /// preference saving will happen.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UninitSpellChecker();
		
		/// <summary>
        /// Used to filter the content (for example, to skip blockquotes in email from
        /// spellchecking. Call this before calling InitSpellChecker; calling it
        /// after initialization will have no effect.
        ///
        /// @see nsITextServicesDocument::setFilter
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFilter([MarshalAs(UnmanagedType.Interface)] nsITextServicesFilter filter);
		
		/// <summary>
        /// Like CheckCurrentWord, checks the word you give it, returning true if it's
        /// misspelled. This is faster than CheckCurrentWord because it does not
        /// compute any suggestions.
        ///
        /// Watch out: this does not clear any suggestions left over from previous
        /// calls to CheckCurrentWord, so there may be suggestions, but they will be
        /// invalid.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool CheckCurrentWordNoSuggest([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string suggestedWord);
		
		/// <summary>
        /// Update the dictionary in use to be sure it corresponds to what the editor
        /// needs.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UpdateCurrentDictionary();
	}
}