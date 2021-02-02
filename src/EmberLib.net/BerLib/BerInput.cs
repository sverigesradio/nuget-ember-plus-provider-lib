/*
   EmberLib.net -- .NET implementation of the Ember+ Protocol

   Copyright (C) 2012-2019 Lawo GmbH (http://www.lawo.com).
   Distributed under the Boost Software License, Version 1.0.
   (See accompanying file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BerLib
{
   /// <summary>
   /// Simple interface for byte-wise reading.
   /// </summary>
   public interface IBerInput
   {
      /// <summary>
      /// Reads the next byte on the input, blocking until
      /// data is available.
      /// </summary>
      /// <returns>The read byte.</returns>
      byte ReadByte();
   }

   /// <summary>
   /// Abstract base class for buffered BerInput implementations.
   /// Uses in-memory buffer for block-wise reading from inner input.
   /// </summary>
   public abstract class BerBufferedInput : IBerInput
   {
      /// <summary>
      /// Reads one kilobyte of data from the inner input, blocking
      /// if less bytes are available.
      /// </summary>
      /// <param name="buffer">Byte array to store buffered data to.</param>
      /// <returns>Number of buffered bytes. May be less than buffer.Length
      /// if EOF of inner input is reached.</returns>
      protected abstract int BufferInput(byte[] buffer);

      /// <summary>
      /// Reads the next byte on the input, blocking until
      /// data is available.
      /// </summary>
      /// <returns>The read byte.</returns>
      public byte ReadByte()
      {
         Debug.Assert(_index <= _bufferLength);

         if(_index == _bufferLength)
         {
            _index = 0;

            _bufferLength = BufferInput(_buffer);
         }

         return _buffer[_index++];
      }

      #region Implementation
      byte[] _buffer = new byte[1024];
      int _bufferLength;
      int _index;
      #endregion
   }

   /// <summary>
   /// Provides buffered reading from a stream.
   /// </summary>
   public class BerStreamInput : BerBufferedInput
   {
      /// <summary>
      /// Constructs a new instance of BerStreamInput.
      /// </summary>
      /// <param name="stream">The stream to read from.</param>
      public BerStreamInput(Stream stream)
      {
         if(stream == null)
            throw new ArgumentNullException("stream");

         if(stream.CanRead == false)
            throw new ArgumentException("stream is not readable");

         _stream = stream;
      }

      /// <summary>
      /// Overriden to read data from the base stream.
      /// </summary>
      /// <param name="buffer">Byte array to store buffered data to.</param>
      /// <returns>Number of buffered bytes. May be less than buffer.Length
      /// if EOF of inner input is reached.</returns>
      protected override int BufferInput(byte[] buffer)
      {
         return _stream.Read(buffer, 0, buffer.Length);
      }

      #region Implementation
      Stream _stream;
      #endregion
   }

   /// <summary>
   /// Provides non-buffered reading from memory.
   /// </summary>
   public class BerMemoryInput : IBerInput
   {
      /// <summary>
      /// Constructs a new instance of BerMemoryInput.
      /// </summary>
      /// <param name="memory">Memory chunk to read data from.</param>
      public BerMemoryInput(IList<byte> memory)
      {
         _memory = memory;
         _bytesAvailable = memory != null
                           ? memory.Count
                           : 0;
      }

      /// <summary>
      /// Overriden to read the next byte from the base memory location.
      /// Throws an exception if EOF is reached.
      /// </summary>
      /// <returns>The read byte.</returns>
      public byte ReadByte()
      {
         if(IsEof)
            throw new InvalidOperationException("Memory EOF");

         return _memory[_bytesRead++];
      }

      /// <summary>
      /// Gets a value indicating whether EOF of the base memory
      /// has been reached.
      /// </summary>
      public bool IsEof
      {
         get { return _bytesRead >= _bytesAvailable; }
      }

      #region Implementation
      IList<byte> _memory;
      int _bytesAvailable;
      int _bytesRead;
      #endregion
   }
}