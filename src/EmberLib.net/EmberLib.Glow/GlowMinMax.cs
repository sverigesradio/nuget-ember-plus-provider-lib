﻿/*
   EmberLib.net -- .NET implementation of the Ember+ Protocol

   Copyright (C) 2012-2019 Lawo GmbH (http://www.lawo.com).
   Distributed under the Boost Software License, Version 1.0.
   (See accompanying file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
*/
// XXX: Changes has been made, IsNull added and GlowParameterType.None
using System;
using System.Collections.Generic;
using System.Text;
using BerLib;
using System.Globalization;

namespace EmberLib.Glow
{
   /// <summary>
   /// A type modelling a conceptual union of an integer and a real value.
   /// </summary>
   public class GlowMinMax
   {
      /// <summary>
      /// Create a new instance of GlowMinMax containing an integer value.
      /// Type is set to GlowParameterType.Integer.
      /// </summary>
      /// <param name="value">The integer value to contain</param>
      public GlowMinMax(long value)
      : this(GlowParameterType.Integer)
      {
         _integer = value;
      }

      /// <summary>
      /// Create a new instance of GlowMinMax containing a real value.
      /// Type is set to GlowParameterType.Real.
      /// </summary>
      /// <param name="value">The real value to contain</param>
      public GlowMinMax(double value)
      : this(GlowParameterType.Real)
      {
         _real = value;
      }

      /// <summary>
      /// Creates an instance of GlowMinMax only specifying the type of
      /// value to contain.
      /// </summary>
      /// <param name="type">The parameter type of the value to contain</param>
      protected GlowMinMax(int type)
      {
         Type = type;
      }

      /// <summary>
      /// Gets a value indicating whether this instance contains a valid value or not. If
      /// <c>false</c> is being returned, the <see cref="Type"/> is <see cref="GlowParameterType.None"/>.
      /// </summary>
      public bool IsNull
      {
         get
         {
            return Type == GlowParameterType.None;
         }
      }

      /// <summary>
      /// Gets the parameter type of the contained value.
      /// Either GlowParameterType.Integer or GlowParameterType.Real.
      /// </summary>
      public int Type { get; private set; }

      /// <summary>
      /// Gets the contained integer value. Throws an exception
      /// if type if not GlowParameterType.Integer.
      /// </summary>
      public long Integer
      {
         get
         {
            AssertType(GlowParameterType.Integer);

            return _integer;
         }
      }

      /// <summary>
      /// Gets the contained real value. Throws an exception
      /// if type if not GlowParameterType.Real.
      /// </summary>
      public double Real
      {
         get
         {
            AssertType(GlowParameterType.Real);

            return _real;
         }
      }

      /// <summary>
      /// Throws an InvalidOperationException if Type is not equal to the passed parameter type.
      /// </summary>
      protected void AssertType(int type)
      {
         if(Type != type)
            throw new InvalidOperationException("type mismatch");
      }

      /// <summary>
      /// Converts the contained value into a string, according to
      /// the format provided.
      /// </summary>
      /// <param name="provider">The format to use.</param>
      /// <returns>A string representation of the contained value.</returns>
      public virtual string ToString(IFormatProvider provider)
      {
         switch(Type)
         {
            case GlowParameterType.Integer:
               return Integer.ToString(provider);

            case GlowParameterType.Real:
               return Real.ToString(provider);
         }

         throw new NotSupportedException("Invalid type");
      }

      /// <summary>
      /// Converts the contained value into a string, according to
      /// the format specified by CultureInfo.CurrentCulture.
      /// </summary>
      /// <returns>A string representation of the contained value.</returns>
      public override string ToString()
      {
         return ToString(CultureInfo.CurrentCulture);
      }

      /// <summary>
      /// Tries to parse a GlowMinMax value of the specified type from a string.
      /// </summary>
      /// <param name="str">The string to parse.</param>
      /// <param name="type">The expected parameter type of the value. Must be either
      /// GlowParameterType.Integer or GlowParameterType.Real.</param>
      /// <param name="provider">The string format to use.</param>
      /// <param name="value">If successful, receives the parse GlowMinMax value.</param>
      /// <returns>True if the passed string could be parsed, otherwise false.</returns>
      public static bool TryParse(string str, int type, IFormatProvider provider, out GlowMinMax value)
      {
         switch(type)
         {
            case GlowParameterType.Integer:
            {
               long integer;

               if(Int64.TryParse(str, NumberStyles.Any, provider, out integer))
               {
                  value = new GlowMinMax(integer);
                  return true;
               }

               break;
            }

            case GlowParameterType.Real:
            {
               double real;

               if(Double.TryParse(str, NumberStyles.Any, provider, out real))
               {
                  value = new GlowMinMax(real);
                  return true;
               }

               break;
            }
            case GlowParameterType.None:
            {
               value = new GlowMinMax(type);
               return true;
            }

            default:
               throw new ArgumentException("Unsupported type");
         }

         value = null;
         return false;
      }

      #region Implementation
      long _integer;
      double _real;
      #endregion
   }
}
