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
using EmberPlusProviderClassLib.Model.Parameters;

namespace EmberPlusProviderClassLib.Model
{
    public interface IElementVisitor<TState, TResult>
    {
        TResult Visit(Node element, TState state);
        TResult Visit(BooleanParameter element, TState state);
        TResult Visit(IntegerParameter element, TState state);
        TResult Visit(RealParameter element, TState state);
        TResult Visit(StringParameter element, TState state);
        TResult Visit(OneToNMatrix element, TState state);
        TResult Visit(OneToNBlindSourceMatrix element, TState state);
        TResult Visit(NToNMatrix element, TState state);
        TResult Visit(DynamicMatrix element, TState state);
        TResult Visit(OneToOneMatrix element, TState state);
        TResult Visit(Function element, TState state);
        //TResult Visit(EnumParameter enumParameter, TState state);
    }

    class InlineElementVisitor : IElementVisitor<object, object>
    {
        public InlineElementVisitor(Action<Node> onNode = null,
                                    Action<ParameterBase> onParameter = null,
                                    Action<Matrix> onMatrix = null,
                                    Action<Function> onFunction = null)
        {
            _onNode = onNode;
            _onParameter = onParameter;
            _onMatrix = onMatrix;
            _onFunction = onFunction;
        }

        readonly Action<Node> _onNode;
        readonly Action<ParameterBase> _onParameter;
        readonly Action<Matrix> _onMatrix;
        readonly Action<Function> _onFunction;

        #region IElementVisitor<object,object> Members
        object IElementVisitor<object, object>.Visit(Node element, object state)
        {
            _onNode?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(BooleanParameter element, object state)
        {
            _onParameter?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(IntegerParameter element, object state)
        {
            _onParameter?.Invoke(element);
            return null;
        }

        //object IElementVisitor<object, object>.Visit(EnumParameter element, object state)
        //{
        //    _onParameter?.Invoke(element);
        //    return null;
        //}

        object IElementVisitor<object, object>.Visit(RealParameter element, object state)
        {
            _onParameter?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(StringParameter element, object state)
        {
            _onParameter?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(OneToNMatrix element, object state)
        {
            _onMatrix?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(OneToNBlindSourceMatrix element, object state)
        {
            _onMatrix?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(NToNMatrix element, object state)
        {
            _onMatrix?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(DynamicMatrix element, object state)
        {
            _onMatrix?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(OneToOneMatrix element, object state)
        {
            _onMatrix?.Invoke(element);
            return null;
        }

        object IElementVisitor<object, object>.Visit(Function element, object state)
        {
            _onFunction?.Invoke(element);
            return null;
        }
        #endregion
    }
}