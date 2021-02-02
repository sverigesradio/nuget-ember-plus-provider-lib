﻿/*
   EmberLib.net -- .NET implementation of the Ember+ Protocol

   Copyright (C) 2012-2019 Lawo GmbH (http://www.lawo.com).
   Distributed under the Boost Software License, Version 1.0.
   (See accompanying file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
*/
// XXX: Changes has been made, BerType.Null, GlowValue.Null, GlowParameterType.None, NullEmberLeaf added
using System;
using System.Collections.Generic;
using System.Text;
using BerLib;

namespace EmberLib.Glow
{
   static class InternalTools
   {
      public static GlowValue GetValue(EmberContainer container, BerTag tag)
      {
         var node = container[tag];

         return node != null
                ? GetValue(node)
                : null;
      }

      public static GlowValue GetValue(EmberNode leaf)
      {
         var value = null as GlowValue;

         switch(leaf.BerTypeNumber)
         {
            case BerType.Integer:
               value = new GlowValue(GetIntegerNodeValue(leaf));
               break;

            case BerType.Real:
               value = new GlowValue(((RealEmberLeaf)leaf).Value);
               break;

            case BerType.UTF8String:
               value = new GlowValue(((StringEmberLeaf)leaf).Value);
               break;

            case BerType.Boolean:
               value = new GlowValue(((BooleanEmberLeaf)leaf).Value);
               break;

            case BerType.OctetString:
               value = new GlowValue(((OctetStringEmberLeaf)leaf).Value);
               break;

            case BerType.Null:
               value = GlowValue.Null;
               break;
         }

         return value;
      }

      public static void InsertValue(EmberContainer container, BerTag tag, GlowValue value)
      {
         var leaf = ValueToLeaf(tag, value);

         if(leaf == null)
            throw new ArgumentException("Type not supported");

         container.Insert(leaf);
      }

      public static EmberNode ValueToLeaf(BerTag tag, GlowValue value)
      {
         switch(value.Type)
         {
            case GlowParameterType.Integer: return new LongEmberLeaf(tag, value.Integer);
            case GlowParameterType.Real:    return new RealEmberLeaf(tag, value.Real);
            case GlowParameterType.String:  return new StringEmberLeaf(tag, value.String);
            case GlowParameterType.Boolean: return new BooleanEmberLeaf(tag, value.Boolean);
            case GlowParameterType.Octets:  return new OctetStringEmberLeaf(tag, value.Octets);
            case GlowParameterType.None:    return new NullEmberLeaf(tag);
         }

         return null;
      }

      public static GlowMinMax GetMinMax(EmberContainer container, BerTag tag)
      {
         var node = container[tag];
         var value = null as GlowMinMax;

         if(node != null)
         {
            switch(node.BerTypeNumber)
            {
               case BerType.Integer:
                  value = new GlowMinMax(GetIntegerNodeValue(node));
                  break;

               case BerType.Real:
                  value = new GlowMinMax(((RealEmberLeaf)node).Value);
                  break;
            }
         }

         return value;
      }

      public static void InsertMinMax(EmberContainer container, BerTag tag, GlowMinMax value)
      {
         switch(value.Type)
         {
            case GlowParameterType.Integer:
               container.Insert(new LongEmberLeaf(tag, value.Integer));
               break;

            case GlowParameterType.Real:
               container.Insert(new RealEmberLeaf(tag, value.Real));
               break;

            default:
               throw new ArgumentException("Type not supported");
         }
      }

      public static bool IsIdentifierValid(string identifier)
      {
         if(identifier == null
         || identifier.Length == 0
         || IsValidIdentifierBegin(identifier[0]) == false
         || identifier.Contains("/"))
            return false;

         return true;
      }

      public static void AssertIdentifierValid(string identifier)
      {
         if(identifier == null)
            throw new ArgumentNullException("identifier");

         if(identifier.Length == 0)
            throw new ArgumentException("identifier must not be null!");

         if(IsValidIdentifierBegin(identifier[0]) == false)
            throw new ArgumentException("identifier must begin with a letter or underscore!");

         if(identifier.Contains("/"))
            throw new ArgumentException("identifier must not contain the '/' character!");
      }

      public static IEnumerable<T> EnumerateChildren<T>(EmberContainer container) where T : EmberNode
      {
         foreach(var ember in container)
         {
            var item = ember as T;

            if(item != null)
               yield return item;
         }
      }

      public static IEnumerable<GlowValue> EnumerateValues(EmberContainer container)
      {
         foreach(var ember in container)
         {
            var value = GetValue(ember);

            if(value != null)
               yield return value;
         }
      }

      public static bool CompliesWithSchema(string schemaIdentifiers, string schemaIdentifier)
      {
         if(schemaIdentifier == null)
            throw new ArgumentNullException("schemaIdentifier");

         if(schemaIdentifiers == null)
            throw new ArgumentNullException("schemaIdentifiers");

         return schemaIdentifiers.Contains(schemaIdentifier);
      }

      #region Implementation
      static long GetIntegerNodeValue(EmberNode node)
      {
         if(node is LongEmberLeaf)
            return ((LongEmberLeaf)node).Value;

         return ((IntegerEmberLeaf)node).Value;
      }

      static bool IsValidIdentifierBegin(char ch)
      {
         return ch >= 'a' && ch <= 'z'
             || ch >= 'A' && ch <= 'Z'
             || ch == '_';
      }
      #endregion
   }
}
