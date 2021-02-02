﻿/*
   EmberLib.net -- .NET implementation of the Ember+ Protocol

   Copyright (C) 2012-2019 Lawo GmbH (http://www.lawo.com).
   Distributed under the Boost Software License, Version 1.0.
   (See accompanying file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
*/

using System;
using System.Collections.Generic;
using System.Text;
using BerLib;

namespace EmberLib.Glow
{
   /// <summary>
   /// EmberPlus-Glow.Function [APPLICATION 19] Type
   /// </summary>
   public class GlowFunction : GlowFunctionBase
   {
      /// <summary>
      /// Creates a new instance of GlowFunction.
      /// </summary>
      /// <param name="tag">Either a specific tag or null when the fnction
      /// is to be inserted into a GlowElementCollection. The tag will be
      /// set to GlowTags.CollectionItem if the passed tag is null.</param>
      protected internal GlowFunction(BerTag? tag)
      : base(tag, GlowType.Function)
      {
      }

      /// <summary>
      /// Creates an encodable instance of GlowFunction, specifying the value
      /// of the "number" field.
      /// </summary>
      /// <param name="number">The value of the "number" field.</param>
      /// <param name="tag">Either a specific tag or null when the function
      /// is to be inserted into a GlowElementCollection. The tag will be
      /// set to GlowTags.CollectionItem if the passed tag is null.</param>
      public GlowFunction(int number, BerTag? tag = null)
      : this(tag)
      {
         Number = number;
      }

      /// <summary>
      /// Gets the "number" field
      /// </summary>
      public int Number
      {
         get { return GetChildValue<int>(GlowTags.Function.Number); }
         private set
         {
            var tag = GlowTags.Function.Number;

            AssertNotPresent(tag);

            Insert(new IntegerEmberLeaf(tag, value));
         }
      }

      /// <summary>
      /// Overriden to return GlowTags.Node.Contents
      /// </summary>
      internal override BerLib.BerTag ContentsTag
      {
         get { return GlowTags.Function.Contents; }
      }

      /// <summary>
      /// Overriden to return GlowTags.Node.Children
      /// </summary>
      internal override BerLib.BerTag ChildrenTag
      {
         get { return GlowTags.Function.Children; }
      }

      /// <summary>
      /// Accepts a visitor of type IGlowVisitor
      /// </summary>
      /// <typeparam name="TState">Visitor state type</typeparam>
      /// <typeparam name="TResult">Visitor result type</typeparam>
      /// <param name="visitor">The visitor instance to accept</param>
      /// <param name="state">State object to pass to the visitor</param>
      /// <returns>The result of the visiting method</returns>
      public override TResult Accept<TState, TResult>(IGlowVisitor<TState, TResult> visitor, TState state)
      {
         return visitor.Visit(this, state);
      }
   }
}
