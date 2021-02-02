﻿/*
   EmberLib.net -- .NET implementation of the Ember+ Protocol

   Copyright (C) 2012-2019 Lawo GmbH (http://www.lawo.com).
   Distributed under the Boost Software License, Version 1.0.
   (See accompanying file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
*/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EmberLib.Glow
{
   /// <summary>
   /// Provides a way to walk a glow tree easier than implementing the IGlowVisitor interface.
   /// The GlowWalker class itself implements the IGlowVisitor interface, therefore you can
   /// pass an instance  of a class derived from GlowWalker to the Accept method of the glow
   /// root container.
   /// You can also call the Walk method of the GlowWalker class.
   /// </summary>
   public abstract class GlowWalker : IGlowVisitor<object, object>
   {
      /// <summary>
      /// Walks a complete glow tree and calls the abstract methods
      /// OnCommand, OnNode, OnParameter, OnMatrix and OnStreamEntry
      /// in the process.
      /// </summary>
      /// <param name="glow">The root of the glow tree to walk.</param>
      public void Walk(GlowContainer glow)
      {
         if(glow == null)
            throw new ArgumentNullException("glow");

         glow.Accept(this, null);
      }

      /// <summary>
      /// Called for every GlowCommand found in the glow tree.
      /// </summary>
      /// <param name="glow">The GlowCommand to process.</param>
      /// <param name="path">The path of the element containing the GlowCommand.</param>
      protected abstract void OnCommand(GlowCommand glow, int[] path);

      /// <summary>
      /// Called for every GlowNode containing a "contents" set found in the glow tree.
      /// </summary>
      /// <param name="glow">The GlowNode to process.</param>
      /// <param name="path">The path of the node. The last number in the path
      /// is the node's number.</param>
      protected abstract void OnNode(GlowNodeBase glow, int[] path);

      /// <summary>
      /// Called for every GlowParameter found in the glow tree.
      /// </summary>
      /// <param name="glow">The GlowParameter to process.</param>
      /// <param name="path">The path of the parameter. The last number in the path
      /// is the parameter's number.</param>
      protected abstract void OnParameter(GlowParameterBase glow, int[] path);

      /// <summary>
      /// Called for every GlowMatrix found in the glow tree.
      /// </summary>
      /// <param name="glow">The GlowMatrix to process.</param>
      /// <param name="path">The path of the matrix. The last number in the path
      /// is the matrix's number.</param>
      protected abstract void OnMatrix(GlowMatrixBase glow, int[] path);

      /// <summary>
      /// Called for every GlowFunction found in the glow tree.
      /// </summary>
      /// <param name="glow">The GlowFunction to process.</param>
      /// <param name="path">The path of the function. The last number in the path
      /// is the function's number.</param>
      protected abstract void OnFunction(GlowFunctionBase glow, int[] path);

      /// <summary>
      /// Called for every GlowStreamEntry found in the glow tree.
      /// Since stream entries can only be contained in a GlowStreamCollection,
      /// which itself is the root of the glow tree, there is no path parameter
      /// passed to this method.
      /// </summary>
      /// <param name="glow">The GlowStreamEntry to process.</param>
      protected abstract void OnStreamEntry(GlowStreamEntry glow);

      /// <summary>
      /// Called for every GlowInvocationResult found in the glow tree.
      /// Since InvocationResult nodes can only be at the root level,
      /// there is no path parameter passed to this method.
      /// </summary>
      /// <param name="glow">The GlowInvocationResult to process.</param>
      protected abstract void OnInvocationResult(GlowInvocationResult glow);

      /// <summary>
      /// Called for every GlowTemplate found in the glow tree.
      /// </summary>
      /// <param name="glow">The GlowTemplate to process.</param>
      protected abstract void OnTemplate(GlowTemplateBase glow, int[] path);

      #region Implementation
      LinkedList<int> _path = new LinkedList<int>();

      void Push(int number)
      {
         _path.AddLast(number);
      }

      void Pop()
      {
         _path.RemoveLast();
      }

      int[] PathToArray()
      {
         var array = new int[_path.Count];
         var index = 0;

         foreach(var number in _path)
         {
            array[index] = number;
            index++;
         }

         return array;
      }
      #endregion

      #region IGlowVisitor<object,object> Members
      // Suppress "interface members should be visible to sub-classes".
      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowCommand glow, object state)
      {
         OnCommand(glow, PathToArray());

         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowElementCollection glow, object state)
      {
         foreach(var glowElement in glow.Elements)
            glowElement.Accept(this, state);

         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowNode glow, object state)
      {
         Push(glow.Number);

         if(glow.HasContents || glow.Children == null)
            OnNode(glow, PathToArray());

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         Pop();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowParameter glow, object state)
      {
         Push(glow.Number);

         OnParameter(glow, PathToArray());

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         Pop();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowStreamCollection glow, object state)
      {
         foreach(var glowEntry in glow.StreamEntries)
            OnStreamEntry(glowEntry);

         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowQualifiedParameter glow, object state)
      {
         var glowPath = glow.Path;
         foreach(var number in glowPath)
            Push(number);

         OnParameter(glow, glowPath);

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         _path.Clear();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowQualifiedNode glow, object state)
      {
         var glowPath = glow.Path;
         foreach(var number in glowPath)
            Push(number);

         if(glow.HasContents || glow.Children == null)
            OnNode(glow, glowPath);

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         _path.Clear();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowRootElementCollection glow, object state)
      {
         foreach(var glowElement in glow.Elements)
            glowElement.Accept(this, state);

         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowSubContainer glow, object state)
      {
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowMatrix glow, object state)
      {
         Push(glow.Number);

         OnMatrix(glow, PathToArray());

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         Pop();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowQualifiedMatrix glow, object state)
      {
         var glowPath = glow.Path;
         foreach(var number in glowPath)
            Push(number);

         OnMatrix(glow, glowPath);

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         _path.Clear();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowFunction glow, object state)
      {
         Push(glow.Number);

         OnFunction(glow, PathToArray());

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         Pop();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowQualifiedFunction glow, object state)
      {
         var glowPath = glow.Path;
         foreach(var number in glowPath)
            Push(number);

         OnFunction(glow, glowPath);

         var glowChildren = glow.Children;

         if(glowChildren != null)
            glowChildren.Accept(this, state);

         _path.Clear();
         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowInvocationResult glow, object state)
      {
         OnInvocationResult(glow);

         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowTemplate glow, object state)
      {
         Push(glow.Number);
         OnTemplate(glow, PathToArray());
         Pop();

         return null;
      }

      [SuppressMessage("Microsoft.Design", "CA1033")]
      object IGlowVisitor<object, object>.Visit(GlowQualifiedTemplate glow, object state)
      {
         var path = glow.Path;

         foreach (var number in path)
            Push(number);

         OnTemplate(glow, path);

         _path.Clear();
         return null;
      }
      #endregion
    }
}
