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
   /// EmberPlus-Glow.Invocation [APPLICATION 22] Type
   /// </summary>
   public class GlowInvocation : GlowSubContainer
   {
      /// <summary>
      /// Creates a new instance of GlowInvocation.
      /// </summary>
      /// <param name="tag">The tag of the EmberNode to create.</param>
      public GlowInvocation(BerTag tag)
      : base(tag, GlowType.Invocation)
      {
      }

      /// <summary>
      /// Gets or sets the "invocationId" field.
      /// </summary>
      public int? InvocationId
      {
         get
         {
            var childNode = this[GlowTags.Invocation.InvocationId] as IntegerEmberLeaf;

            if(childNode != null)
               return childNode.Value;

            return null;
         }
         set
         {
            var tag = GlowTags.Invocation.InvocationId;

            Remove(tag);
            Insert(new IntegerEmberLeaf(tag, value.Value));
         }
      }

      /// <summary>
      /// Gets or sets the "arguments" field.
      /// Getter returns null if field not present.
      /// </summary>
      public EmberSequence Arguments
      {
         get { return this[GlowTags.Invocation.Arguments] as EmberSequence; }
         set
         {
            var tag = GlowTags.Invocation.Arguments;

            if(value.Tag != tag)
               throw new ArgumentException("Tag mismatch");

            Remove(tag);
            Insert(value);
         }
      }

      /// <summary>
      /// Gets or sets an typed enumeration over the tuple values contained in
      /// in the "arguments" field.
      /// Getter returns null if field "arguments" not present.
      /// </summary>
      public IEnumerable<GlowValue> ArgumentValues
      {
         get
         {
            var arguments = Arguments;

            return arguments != null
                   ? InternalTools.EnumerateValues(arguments)
                   : null;
         }
         set
         {
            if(value == null)
               throw new ArgumentNullException();

            var sequence = new EmberSequence(GlowTags.Invocation.Arguments);

            foreach(var glowValue in value)
               sequence.Insert(InternalTools.ValueToLeaf(GlowTags.CollectionItem, glowValue));

            Arguments = sequence;
         }
      }

      /// <summary>
      /// Ensures that the "arguments" field is present, creating a
      /// new EmberSequence and assigning it to the "arguments" field
      /// if necessary.
      /// </summary>
      /// <returns>The value of the "arguments" field, never null.</returns>
      public EmberSequence EnsureArguments()
      {
         var arguments = Arguments;

         if(arguments == null)
         {
            arguments = new EmberSequence(GlowTags.Invocation.Arguments);
            Insert(arguments);
         }

         return arguments;
      }
   }
}
