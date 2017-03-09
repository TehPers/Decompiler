using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using MOpCodes = Mono.Cecil.Cil.OpCodes;

namespace Teh.Decompiler
{
    public class CodeBuilder
    {
        private static object This { get; } = new object();

        public MethodDefinition Method { get; }

        public CodeBuilder(MethodDefinition method)
        {
            this.Method = method;
        }

        public string[] generateCode()
        {
            List<string> lines = new List<string>();
            int length = Method.Body.Instructions.Count();

            Stack<string> stack = new Stack<string>();
            Dictionary<int, string> locals = new Dictionary<int, string>();
            int localCounter = 0;
            foreach (Instruction il in Method.Body.Instructions)
            {
                if (il.OpCode == MOpCodes.Nop) continue;
                else if (il.OpCode == MOpCodes.Pop)
                {
                    lines.Add(stack.Pop() + ";");
                }
                else if (il.OpCode == MOpCodes.Ldstr)
                {
                    stack.Push("\"" + il.Operand as string + "\"");
                }
                else if (il.OpCode == MOpCodes.Ldarg_0)
                {
                    if (!Method.HasThis) stack.Push(Method.Parameters[0].Name);
                }
                else if (il.OpCode == MOpCodes.Ldarg_1)
                {
                    if (!Method.HasThis) stack.Push(Method.Parameters[1].Name);
                    else stack.Push(Method.Parameters[0].Name);
                }
                else if (il.OpCode == MOpCodes.Ldarg_2)
                {
                    if (!Method.HasThis) stack.Push(Method.Parameters[2].Name);
                    else stack.Push(Method.Parameters[1].Name);
                }
                else if (il.OpCode == MOpCodes.Ldarg_3)
                {
                    if (!Method.HasThis) stack.Push(Method.Parameters[3].Name);
                    else stack.Push(Method.Parameters[2].Name);
                }
                else if (il.OpCode == MOpCodes.Add)
                {
                    string b = stack.Pop();
                    string a = stack.Pop();
                    stack.Push(a + " + " + b);
                }
                else if (il.OpCode == MOpCodes.Sub)
                {
                    string b = stack.Pop();
                    string a = stack.Pop();
                    stack.Push(a + " - " + b);
                }
                else if (il.OpCode == MOpCodes.Mul)
                {
                    string b = stack.Pop();
                    string a = stack.Pop();
                    stack.Push(a + " * " + b);
                }
                else if (il.OpCode == MOpCodes.Div)
                {
                    string b = stack.Pop();
                    string a = stack.Pop();
                    stack.Push(a + " / " + b);
                }
                else if (il.OpCode == MOpCodes.Stloc_0)
                {
                    locals[0] = stack.Pop();
                }
                else if (il.OpCode == MOpCodes.Stloc_1)
                {
                    locals[1] = stack.Pop();
                }
                else if (il.OpCode == MOpCodes.Stloc_2)
                {
                    locals[2] = stack.Pop();
                }
                else if (il.OpCode == MOpCodes.Stloc_3)
                {
                    locals[3] = stack.Pop();
                }
                else if (il.OpCode == MOpCodes.Ldloc_0)
                {
                    stack.Push(locals[0]);
                }
                else if (il.OpCode == MOpCodes.Ldloc_1)
                {
                    stack.Push(locals[1]);
                }
                else if (il.OpCode == MOpCodes.Ldloc_2)
                {
                    stack.Push(locals[2]);
                }
                else if (il.OpCode == MOpCodes.Ldloc_3)
                {
                    stack.Push(locals[3]);
                }
                else if (il.OpCode == MOpCodes.Br_S)
                {
                    // Goto?
                }
                else if (il.OpCode == MOpCodes.Ret)
                {
                    string line = "return";
                    if (stack.Count > 0) line += " " + stack.Pop();
                    lines.Add(line + ";");
                }
                else if (il.OpCode == MOpCodes.Call)
                {
                    object func = il.Operand;
                    MethodReference called = func as MethodReference;
                    if (called != null)
                    {
                        string call = "";
                        if (called.HasThis)
                        {
                            call += "this.";
                        }
                        call += GetMethodName(called);
                        if (called.HasGenericParameters)
                        {
                            call += "<";
                            call += string.Join(", ", (
                                from param in called.GenericParameters
                                select param.Type
                                ));
                            call += ">";
                        }

                        call += "(";
                        List<string> paramStrings = new List<string>();
                        foreach (ParameterDefinition param in called.Parameters)
                        {
                            string thing = stack.Pop();
                            paramStrings.Add(thing);
                        }
                        call += string.Join(", ", paramStrings);
                        call += ")";

                        if (called.ReturnType.FullName != "System.Void") stack.Push(call);
                        else lines.Add(call + ";");
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return lines.ToArray();
        }

        public string GetMethodName(MethodReference method)
        {
            MethodDefinition definition = method.Resolve();
            if (definition.IsConstructor)
            {
                return "base";
            }

            return method.DeclaringType.FullName + "." + method.Name;
        }

        public string GetString(object o)
        {
            if (o is string)
            {
                string str = o as string;
                return "\"" + str + "\"";
            }
            else if (o is short || o is int || o is long || o is float || o is double || o is decimal || o is byte)
            {
                return o.ToString();
            }
            else if (o is char)
            {
                return o.ToString();
            }

            return "<" + o.ToString() + ">";
        }
    }
}
