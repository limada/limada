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
// Generated by IDLImporter from file nsITransactionListener.idl
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
	using System.Windows.Forms;
	
	
	/// <summary>
    /// The nsITransactionListener interface.
    /// <P>
    /// This interface is implemented by an object that tracks transactions.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("58e330c4-7b48-11d2-98b9-00805f297d89")]
	public interface nsITransactionListener
	{
		
		/// <summary>
        /// Called before a transaction manager calls a transaction's
        /// doTransaction() method.
        /// @param aManager the transaction manager doing the transaction.
        /// @param aTransaction the transaction being executed.
        /// @result boolean value returned by listener which indicates
        /// it's desire to interrupt normal control flow. Listeners should
        /// return true if they want to interrupt normal control flow, without
        /// throwing an error.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool WillDo([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransaction);
		
		/// <summary>
        /// Called after a transaction manager calls the doTransaction() method of
        /// a transaction.
        /// @param aManager the transaction manager that did the transaction.
        /// @param aTransaction the transaction that was executed.
        /// @param aDoResult the nsresult returned after executing
        /// the transaction.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DidDo([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransaction, int aDoResult);
		
		/// <summary>
        /// Called before a transaction manager calls the Undo() method of
        /// a transaction.
        /// @param aManager the transaction manager undoing the transaction.
        /// @param aTransaction the transaction being undone.
        /// @result boolean value returned by listener which indicates
        /// it's desire to interrupt normal control flow. Listeners should
        /// return true if they want to interrupt normal control flow, without
        /// throwing an error. Note that listeners can also interrupt normal
        /// control flow by throwing an nsresult that indicates an error.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool WillUndo([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransaction);
		
		/// <summary>
        /// Called after a transaction manager calls the Undo() method of
        /// a transaction.
        /// @param aManager the transaction manager undoing the transaction.
        /// @param aTransaction the transaction being undone.
        /// @param aUndoResult the nsresult returned after undoing the transaction.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DidUndo([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransaction, int aUndoResult);
		
		/// <summary>
        /// Called before a transaction manager calls the Redo() method of
        /// a transaction.
        /// @param aManager the transaction manager redoing the transaction.
        /// @param aTransaction the transaction being redone.
        /// @result boolean value returned by listener which indicates
        /// it's desire to interrupt normal control flow. Listeners should
        /// return true if they want to interrupt normal control flow, without
        /// throwing an error. Note that listeners can also interrupt normal
        /// control flow by throwing an nsresult that indicates an error.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool WillRedo([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransaction);
		
		/// <summary>
        /// Called after a transaction manager calls the Redo() method of
        /// a transaction.
        /// @param aManager the transaction manager redoing the transaction.
        /// @param aTransaction the transaction being redone.
        /// @param aRedoResult the nsresult returned after redoing the transaction.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DidRedo([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransaction, int aRedoResult);
		
		/// <summary>
        /// Called before a transaction manager begins a batch.
        /// @param aManager the transaction manager beginning a batch.
        /// @result boolean value returned by listener which indicates
        /// it's desire to interrupt normal control flow. Listeners should
        /// return true if they want to interrupt normal control flow, without
        /// throwing an error. Note that listeners can also interrupt normal
        /// control flow by throwing an nsresult that indicates an error.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool WillBeginBatch([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager);
		
		/// <summary>
        /// Called after a transaction manager begins a batch.
        /// @param aManager the transaction manager that began a batch.
        /// @param aResult the nsresult returned after beginning a batch.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DidBeginBatch([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, int aResult);
		
		/// <summary>
        /// Called before a transaction manager ends a batch.
        /// @param aManager the transaction manager ending a batch.
        /// @result boolean value returned by listener which indicates
        /// it's desire to interrupt normal control flow. Listeners should
        /// return true if they want to interrupt normal control flow, without
        /// throwing an error. Note that listeners can also interrupt normal
        /// control flow by throwing an nsresult that indicates an error.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool WillEndBatch([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager);
		
		/// <summary>
        /// Called after a transaction manager ends a batch.
        /// @param aManager the transaction manager ending a batch.
        /// @param aResult the nsresult returned after ending a batch.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DidEndBatch([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, int aResult);
		
		/// <summary>
        /// Called before a transaction manager tries to merge
        /// a transaction, that was just executed, with the
        /// transaction at the top of the undo stack.
        /// @param aManager the transaction manager ending a batch.
        /// @param aTopTransaction the transaction at the top of the undo stack.
        /// @param aTransactionToMerge the transaction to merge.
        /// @result boolean value returned by listener which indicates
        /// it's desire to interrupt normal control flow. Listeners should
        /// return true if they want to interrupt normal control flow, without
        /// throwing an error. Note that listeners can also interrupt normal
        /// control flow by throwing an nsresult that indicates an error.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool WillMerge([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTopTransaction, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransactionToMerge);
		
		/// <summary>
        /// Called after a transaction manager tries to merge
        /// a transaction, that was just executed, with the
        /// transaction at the top of the undo stack.
        /// @param aManager the transaction manager ending a batch.
        /// @param aTopTransaction the transaction at the top of the undo stack.
        /// @param aTransactionToMerge the transaction to merge.
        /// @param aDidMerge true if transaction was merged, else false.
        /// @param aMergeResult the nsresult returned after the merge attempt.
        /// @param aInterrupt listeners should set this to PR_TRUE if they
        /// want to interrupt normal control flow, without throwing an error.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DidMerge([MarshalAs(UnmanagedType.Interface)] nsITransactionManager aManager, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTopTransaction, [MarshalAs(UnmanagedType.Interface)] nsITransaction aTransactionToMerge, [MarshalAs(UnmanagedType.U1)] bool aDidMerge, int aMergeResult);
	}
}
