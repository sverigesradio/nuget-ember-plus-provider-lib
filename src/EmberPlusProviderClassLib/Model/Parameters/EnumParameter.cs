﻿#region copyright
/*
 * This code is from the Lawo/ember-plus GitHub repository and is licensed with
 *
 * Boost Software License - Version 1.0 - August 17th, 2003
 *
 * Permission is hereby granted, free of charge, to any person or organization
 * obtaining a copy of the software and accompanying documentation covered by
 * this license (the "Software") to use, reproduce, display, distribute,
 * execute, and transmit the Software, and to prepare derivative works of the
 * Software, and to permit third-parties to whom the Software is furnished to
 * do so, all subject to the following:
 *
 * The copyright notices in the Software and this entire statement, including
 * the above license grant, this restriction and the following disclaimer,
 * must be included in all copies of the Software, in whole or in part, and
 * all derivative works of the Software, unless such copies or derivative
 * works are solely in the form of machine-executable object code generated by
 * a source language processor.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
 * SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
 * FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
 #endregion

using System;
using System.Diagnostics;

//namespace EmberPlusProviderClassLib.Model.Parameters
//{
//    public class EnumParameter : Parameter<long>
//    {
//        public EnumParameter(int number, Element parent, string identifier, Dispatcher dispatcher, int min, int max, bool isWritable, bool isPersistable = false)
//        : base(number, parent, identifier, dispatcher, isWritable, isPersistable)
//        {
//            Minimum = min;
//            Maximum = max;
//            Enumeration = "hej hej2 hej3";
//        }

//        public long Minimum { get; }
//        public long Maximum { get; }

//        public string Enumeration { get; }

//        public override TResult Accept<TState, TResult>(IElementVisitor<TState, TResult> visitor, TState state)
//        {
//            return visitor.Visit(this, state);
//        }

//        public override void SetValue(object newValue)
//        {
//            try
//            {
//                long l = Convert.ToInt32(newValue);
//                if (Value != l)
//                {
//                    Value = l;
//                }
//            }
//            catch(Exception)
//            {
//                Debug.WriteLine($"Failed to set enum parameter {Identifier} value to {newValue}");
//            }
//        }
//    }
//}
