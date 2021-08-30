/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Reflection;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Query
{
    // TODO: Use DelegateEnvelope to build a generic delegate translator
    internal class DelegateEnvelope
    {
        System.Type _delegateType;
        object _target;
        System.Type _type;
        string _method;

        [NonSerialized]
        Delegate _content;

        public DelegateEnvelope()
        {
        }

        public DelegateEnvelope(Delegate content)
        {
            _content = content;
            Marshal();
        }

        protected Delegate GetContent()
        {
            if (null == _content)
            {
                _content = Unmarshal();
            }
            return _content;
        }

        private void Marshal()
        {
            _delegateType = _content.GetType();
            _target = _content.Target;
            _method = _content.Method.Name;
            _type = _content.Method.DeclaringType;
        }

        private Delegate Unmarshal()
        {
			if (null == _target)
			{
				return Delegate.CreateDelegate(_delegateType, null, _type.GetMethod(_method,  BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public));
			}
				
			return Delegate.CreateDelegate(_delegateType, _target, _target.GetType().GetMethod(_method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
        }
    }

    internal class EvaluationDelegateWrapper : DelegateEnvelope, IEvaluation
    {	
        public EvaluationDelegateWrapper()
        {
        }
		
        public EvaluationDelegateWrapper(EvaluationDelegate evaluation) : base(evaluation)
        {	
        }
		
        EvaluationDelegate GetEvaluationDelegate()
        {
            return (EvaluationDelegate)GetContent();
        }
		
        public void Evaluate(ICandidate candidate)
        {
			// use starting _ for PascalCase conversion purposes
            EvaluationDelegate _evaluation = GetEvaluationDelegate();
            _evaluation(candidate);
        }
    }
}