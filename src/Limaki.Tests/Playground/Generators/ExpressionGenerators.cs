/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Limaki.Common.Linqish;
using NUnit.Framework;

namespace Limaki.Tests.Generators {
    [TestFixture]
    public class ExpressionGenerators : Limaki.UnitTest.TestBase {
        [Test]
        public void GenerateExpressionFuncVisitor () {
            Func<MethodInfo, string> MethodDeclaration = method =>
             string.Format ("protected override {0} {1}{2} ({3})", TypeName (method.ReturnType), method.Name,
             GenericParameters (method),
             Parameters (method));

            foreach (var m in typeof (ExpressionVisitor).GetMethods (BindingFlags.NonPublic | BindingFlags.Instance)
                .Where (
                    m => typeof (Expression).IsAssignableFrom (m.ReturnType) && m.GetParameters ().Length == 1
                )
                ) {
                    Console.WriteLine ("\t\tpublic Func<{0}, Func<{0}, Expression>, Expression> {1}Func {{ get; set; }}",
                    TypeName (m.GetParameters ()[0].ParameterType), m.Name);
                Console.WriteLine ("\t\t{0} {{", MethodDeclaration (m));
                Console.WriteLine ("\t\t\treturn EvalFunc ({0}Func,node,base.{0});", m.Name);
                Console.WriteLine ("\t\t}");

            }
        }

        static string MethodCall (MethodInfo method) {
            return string.Format ("{0}{1} ({2})", method.Name, GenericParameters (method), Arguments (method));
        }

        static string Arguments (MethodInfo method) {
            return string.Join (", ", method.GetParameters ().Select (ParameterName).ToArray ());
        }

        static string GenericParameters (MethodInfo method) {
            return method.IsGenericMethodDefinition ? string.Format ("<{0}>", method.GetGenericArguments ()[0].Name) : "";
        }

        static string Parameters (MethodInfo method) {
            return string.Join (", ", method.GetParameters ().Select (p => string.Format ("{0}{1} {2}", Params (p), TypeName (p.ParameterType), ParameterName (p))).ToArray ());
        }

        static string Params (ParameterInfo parameter) {
            return IsParams (parameter) ? "params " : "";
        }

        static bool IsParams (ParameterInfo parameter) {
            return Attribute.IsDefined (parameter, typeof (ParamArrayAttribute));
        }

        static string ParameterName (ParameterInfo parameter) {
            switch (parameter.Name) {
                case "break":
                case "continue":
                case "finally":
                    return "@" + parameter.Name;
            }

            return parameter.Name;
        }

        static string TypeName (Type type) {
            switch (type.Name) {
                case "Expression`1":
                    return "Expression<" + type.GetGenericArguments ()[0].Name + ">";
                case "IEnumerable`1":
                    return "IEnumerable<" + type.GetGenericArguments ()[0].Name + ">";
                case "Boolean":
                    return "bool";
                case "Object":
                    return "object";
                case "Int32":
                    return "int";
                case "String":
                    return "string";
            }

            return type.Name;
        }
    }

}