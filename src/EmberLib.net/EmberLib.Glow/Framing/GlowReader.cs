﻿/*
   EmberLib.net -- .NET implementation of the Ember+ Protocol

   Copyright (C) 2012-2019 Lawo GmbH (http://www.lawo.com).
   Distributed under the Boost Software License, Version 1.0.
   (See accompanying file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EmberLib.Framing;

namespace EmberLib.Glow.Framing
{
   /// <summary>
   /// Convenience class for reading framed Ember+ packages.
   /// </summary>
   public class GlowReader : FramingReader
   {
      /// <summary>
      /// Creates a new instance of GlowReader.
      /// </summary>
      /// <param name="baseReader">An AsyncEmberReader used for decoding incoming Ember+ packages.</param>
      /// <param name="keepAliveCallback">A callback to be subscribed to the KeepAliveRequest event.</param>
      public GlowReader(AsyncEmberReader baseReader, EventHandler<KeepAliveRequestReceivedArgs> keepAliveCallback)
      : base(baseReader, keepAliveCallback)
      {
         PackageReceived += new EventHandler<PackageReceivedArgs>(HandlePackageReceived);
      }

      /// <summary>
      /// Creates a new instance of GlowReader.
      /// </summary>
      /// <param name="rootReady">A callback to be called when a complete glow tree has been decoded.</param>
      /// <param name="keepAliveCallback">A callback to be subscribed to the KeepAliveRequest event.</param>
      public GlowReader(EventHandler<AsyncDomReader.RootReadyArgs> rootReady, EventHandler<KeepAliveRequestReceivedArgs> keepAliveCallback)
      : this(CreateAsyncReader(rootReady), keepAliveCallback)
      {
      }

      /// <summary>
      /// Turns a version number contained in a uint16 into its string representation.
      /// Version numbers like this are used for versioning the EmBER encoding and the
      /// Glow schema.
      /// </summary>
      /// <param name="version">Upper byte contains major version number, lower byte contains
      /// minor version number.</param>
      /// <returns>A string in the form "Major.Minor"</returns>
      public static string UshortVersionToString(ushort version)
      {
         return String.Format("{0}.{1}", version >> 8, version & 0xff);
      }

      #region Error Event
      public class ErrorArgs : EventArgs
      {
         public int ErrorCode { get; private set; }
         public string Message { get; private set; }

         public ErrorArgs(int errorCode, string message)
         {
            ErrorCode = errorCode;
            Message = message;
         }
      }

      /// <summary>
      /// Raised when an Ember+ package with the wrong schema (DTD) or with a
      /// different schema version has been received. (note: second case is
      /// non-fatal and decoding of the payload will proceed!).
      /// Only the Glow schema is supported by this Reader.
      /// </summary>
      public event EventHandler<ErrorArgs> Error;

      protected virtual void OnError(ErrorArgs e)
      {
         Debug.WriteLine($"Error: GlowReader / OnError", e);
         if(Error != null)
            Error(this, e);
      }
      #endregion

      #region Implementation
      static AsyncDomReader CreateAsyncReader(EventHandler<AsyncDomReader.RootReadyArgs> rootReady)
      {
         if(rootReady == null)
         {
             Debug.WriteLine($"Exception: GlowReader / CreateAsyncReader");
             throw new ArgumentNullException("rootReady");
         }

         var reader = new AsyncDomReader(new GlowApplicationInterface());
         reader.RootReady += rootReady;
         return reader;
      }

      void HandlePackageReceived(object sender, FramingReader.PackageReceivedArgs e)
      {
         var dtd = e.Dtd;

         if(e.MessageId == ProtocolParameters.MessageId
         && e.Command == ProtocolParameters.Commands.Payload)
         {
            if(dtd == Dtd.Glow)
            {
               var appBytes = e.ApplicationBytes;

               if(appBytes != null)
               {
                  var version = 0;
                  version |= appBytes[0] << 0;
                  version |= appBytes[1] << 8;

                  if(version != GlowDtd.Version)
                  {
                     var message = String.Format("Glow DTD version mismatch: found version {0}, expected {1}!",
                                                 UshortVersionToString((ushort)version),
                                                 UshortVersionToString(GlowDtd.Version));

                     OnError(new ErrorArgs(1, message));
                  }
               }
               else
               {
                  OnError(new ErrorArgs(2, "Glow DTD version mismatch: no version information received!"));
               }
            }
            else
            {
               var message = String.Format("DTD mismatch: found '{0}', expected '{1}'", dtd, Dtd.Glow);

               OnError(new ErrorArgs(3, message));
            }
         }
      }
      #endregion
   }
}
