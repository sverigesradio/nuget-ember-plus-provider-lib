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
using BerLib;

namespace EmberLib.Legacy
{
   [Obsolete("This class is a feature exclusively used by the Glow DTD version 1. Please consider using Glow DTD version 2 or higher.")]
   public class EmberFrame : EmberSequence
   {
      public static readonly BerTag FrameTag = new BerTag(BerClass.Application, 0xFFFFACDC);
      public static readonly BerTag CrcTag = new BerTag(BerClass.Application, 0xFFFFACDD);

      public EmberFrame()
      : base(FrameTag, BerType.Sequence)
      {
      }

      internal override sealed void EncodeOverride(IBerOutput output)
      {
         if(IsDirty)
            Update();

         output.WriteBytes(_encodedFrameHeader);

         foreach(var node in this)
            node.Encode(output);

         output.WriteBytes(BerEncoding.IndefiniteLengthTerminator);
         output.Flush();
      }

      internal ushort Crc
      {
         get
         {
            var crcLeaf = _crcNode as OctetStringEmberLeaf;

            if(crcLeaf != null)
            {
               var bytes = crcLeaf.Value;

               if(bytes != null
               && bytes.Length == 2)
                  return (ushort)((bytes[0] << 8) | bytes[1]); // big endian
            }

            return 0;
         }
      }

      protected internal override void InsertChildNode(EmberNode node)
      {
         if(node.Tag == CrcTag
         && node.BerTypeNumber == BerType.OctetString)
            _crcNode = node;
         else
            base.InsertChildNode(node);
      }

      internal override bool IsDirty
      {
         get { return _encodedFrameHeader == null; }
      }

      internal override int Update()
      {
         var output = new BerMemoryOutput();
         var implicitTag = new BerTag(BerClass.Universal, BerTypeNumber, true);

         var childrenLength = 0;

         foreach(var child in this)
         {
            childrenLength += child.IsDirty
                              ? child.Update()
                              : child.EncodedLength;
         }

         BerEncoding.EncodeTag(output, Tag.ToContainer());
         BerEncoding.EncodeLength(output, BerDefinitions.IndefiniteLength);

         BerEncoding.EncodeTag(output, implicitTag);
         BerEncoding.EncodeLength(output, BerDefinitions.IndefiniteLength);

         _encodedFrameHeader = output.ToArray();
         EncodedLength = childrenLength + _encodedFrameHeader.Length + BerEncoding.IndefiniteLengthTerminator.Length;

         return EncodedLength;
      }

      #region Implementation
      readonly byte[] EmptyCrc = new byte[2];
      EmberNode _crcNode;
      byte[] _encodedFrameHeader;
      #endregion
   }
}
