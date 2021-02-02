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
   /// EmberPlus-Glow.Connection [APPLICATION 16] Type
   /// </summary>
   public class GlowConnection : GlowSubContainer
   {
      /// <summary>
      /// Creates a new instance of GlowCommand.
      /// </summary>
      /// <param name="tag">Either a specific tag or null when the node
      /// is to be inserted into the connections collection of a matrix.
      /// Tag is set to GlowTags GlowTags.CollectionItem if the passed tag is null.</param>
      protected internal GlowConnection(BerTag? tag)
      : base(tag ?? GlowTags.CollectionItem, GlowType.Connection)
      {
      }

      /// <summary>
      /// Creates a new instance of GlowConnection.
      /// </summary>
      /// <param name="target">Value of the "target" field.</param>
      /// <param name="tag">Either a specific tag or null when the node
      /// is to be inserted into a matrix connection collection. The tag will be
      /// set to GlowTags.CollectionItem if the passed tag is null.</param>
      public GlowConnection(int target, BerTag? tag = null)
      : this(tag)
      {
         Target = target;
      }

      /// <summary>
      /// Gets or sets the "target" field.
      /// </summary>
      public int Target
      {
         get { return GetChildValue<int>(GlowTags.Connection.Target); }
         set
         {
            var tag = GlowTags.Connection.Target;

            Remove(tag);
            Insert(new IntegerEmberLeaf(tag, value));
         }
      }

      /// <summary>
      /// Gets or sets the "sources" field.
      /// </summary>
      public int[] Sources
      {
         get
         {
            var childNode = this[GlowTags.Connection.Sources] as EmberLeaf<int[]>;

            if(childNode != null)
               return childNode.Value;

            return null;
         }
         set
         {
            var tag = GlowTags.Connection.Sources;

            Remove(tag);
            Insert(new RelativeOidEmberLeaf(tag, value));
         }
      }

      /// <summary>
      /// Gets or sets the "operation" field. Getter returns null if field not present.
      /// </summary>
      public int? Operation
      {
         get
         {
            var childNode = this[GlowTags.Connection.Operation] as IntegerEmberLeaf;

            if(childNode != null)
               return childNode.Value;

            return null;
         }
         set
         {
            var tag = GlowTags.Connection.Operation;

            Remove(tag);
            Insert(new IntegerEmberLeaf(tag, value.Value));
         }
      }

      /// <summary>
      /// Gets or sets the "disposition" field. Getter returns null if field not present.
      /// </summary>
      public int? Disposition
      {
         get
         {
            var childNode = this[GlowTags.Connection.Disposition] as IntegerEmberLeaf;

            if(childNode != null)
               return childNode.Value;

            return null;
         }
         set
         {
            var tag = GlowTags.Connection.Disposition;

            Remove(tag);
            Insert(new IntegerEmberLeaf(tag, value.Value));
         }
      }
   }
}
