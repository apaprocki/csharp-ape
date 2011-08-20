// C# Monkey's Audio SDK Interface - Andrew Paprocki <andrew@ishiboo.com>
//
// Monkey's Audio - http://www.monkeysaudio.com
// This API wraps existing SDK API and all documentation can be found
// inside of the Monkey's Audio SDK. Originally written for SDK version
// 3.99, 02/08/2004.
//
// Licensed under the MIT license:
//
// Copyright (c) 2004 Andrew Paprocki <andrew@ishiboo.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to 
// deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
// sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
// IN THE SOFTWARE.

using System;
using System.Runtime.InteropServices;

namespace Ishiboo
{
    public class APE
    {
        public const int COMPRESSION_LEVEL_FAST             = 1000;
        public const int COMPRESSION_LEVEL_NORMAL           = 2000;
        public const int COMPRESSION_LEVEL_HIGH             = 3000;
        public const int COMPRESSION_LEVEL_EXTRA_HIGH       = 4000;

        public const int MAC_FORMAT_FLAG_8_BIT              = 1;
        public const int MAC_FORMAT_FLAG_CRC                = 2;
        public const int MAC_FORMAT_FLAG_HAS_PEAK_LEVEL     = 4;
        public const int MAC_FORMAT_FLAG_24_BIT             = 8;
        public const int MAC_FORMAT_FLAG_HAS_SEEK_ELEMENTS  = 16;
        public const int MAC_FORMAT_FLAG_CREATE_WAV_HEADER  = 32;

        public const int CREATE_WAV_HEADER_ON_DECOMPRESSION = -1;

        public const int MAX_AUDIO_BYTES_UNKNOWN = -1;

        public const int WAVE_HEADER_BYTES = 44;

        [StructLayout(LayoutKind.Sequential)]
        public struct WAVE_HEADER
        {
            [MarshalAs(UnmanagedType.LPArray, SizeConst=4)]
            public char[] cRIFFHeader;
            public uint nRIFFBytes;
            [MarshalAs(UnmanagedType.LPArray, SizeConst=4)]
            public char[] cDataTypeID;
            [MarshalAs(UnmanagedType.LPArray, SizeConst=4)]
            public char[] cFormatHeader;
            public uint nFormatBytes;
            public ushort nFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort nBitsPerSample;
            [MarshalAs(UnmanagedType.LPArray, SizeConst=4)]
            public char[] cDataHeader;
            public uint nDataBytes;
        }

        public const int APE_HEADER_BYTES = 32;

        [StructLayout(LayoutKind.Sequential)]
        public struct APE_HEADER
        {
            [MarshalAs(UnmanagedType.LPArray, SizeConst=4)]
            public char[] cID;               // "MAC "
            public ushort nVersion;          // version number * 1000
            public ushort nCompressionLevel; // compression level
            public ushort nFormatFlags;      // format flags (future use)
            public ushort nChannels;         // number of channels (1 or 2)
            public uint nSampleRate;         // sample rate (typically 44100)
            public uint nHeaderBytes;        // bytes after MAC header
                                             //   that contain WAV header
            public uint nTerminatingBytes;   // bytes after raw data
                                             //   (for extended info)
            public uint nTotalFrames;        // number of frames in file
            public uint nFinalFrameBlocks;   // number of samples in final frame
        }

        // Helper functions

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        GetVersionNumber();

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        GetInterfaceCompatibility(int nVersion,
                                  bool bDisplayWarningsOnFailure,
                                  IntPtr hwndParent);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        ShowFileInfoDialog(string pFilename, IntPtr hwndWindow);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        TagFileSimple(string pFilename,
                      string pArtist,
                      string pAlbum,
                      string pTitle,
                      string pComment,
                      string pGenre,
                      string pYear,
                      string pTrack,
                      bool bClearFirst,
                      bool bUseOldID3);

        public const int ID3_TAG_BYTES = 128;

        [StructLayout(LayoutKind.Sequential)]
        public struct ID3_TAG
        {
            [MarshalAs(UnmanagedType.LPArray, SizeConst=3)]
            public char[] TagHeader; // "TAG"
            [MarshalAs(UnmanagedType.LPArray, SizeConst=30)]
            public char[] Title;
            [MarshalAs(UnmanagedType.LPArray, SizeConst=30)]
            public char[] Artist;
            [MarshalAs(UnmanagedType.LPArray, SizeConst=30)]
            public char[] Album;
            [MarshalAs(UnmanagedType.LPArray, SizeConst=4)]
            public char[] Year;
            [MarshalAs(UnmanagedType.LPArray, SizeConst=29)]
            public char[] Comment;
            public byte Track;
            public byte Genre;
        }

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        GetID3Tag(string pFilename, [In] ref ID3_TAG pID3Tag);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        RemoveTag(string pFilename);

        // IAPECompress wrapper

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static IntPtr c_APECompress_Create(out int errorCode);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static void c_APECompress_Destroy(IntPtr hAPECompress);

        [StructLayout(LayoutKind.Sequential)]
        public struct WAVEFORMATEX
        {
            public ushort wFormatTag;     // 1
            public ushort nChannels;      // 2
            public uint nSamplesPerSec;   // 44100
            public uint nAvgBytesPerSec;  // nBlockAlign * nSamplesPerSec
            public ushort nBlockAlign;    // wBitsPerSample / 8 * nChannels
            public ushort wBitsPerSample; // 16
            public ushort cbSize;         // 0
        }

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APECompress_Start(IntPtr hAPECompress,
                            string pOutputFilename,
                            [In] ref WAVEFORMATEX pwfeInput,
                            int nMaxAudioBytes,
                            int nCompressionLevel,
                            [MarshalAs(UnmanagedType.LPArray,
                                SizeParamIndex=6)]
                            byte[] pHeaderData,
                            int nHeaderBytes);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APECompress_AddData(IntPtr hAPECompress,
                              [MarshalAs(UnmanagedType.LPArray,
                                  ArraySubType=UnmanagedType.U1,
                                  SizeParamIndex=2)]
                              byte[] pData,
                              int nBytes);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APECompress_GetBufferBytesAvailable(IntPtr hAPECompress);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static byte[]
        c_APECompress_LockBuffer(IntPtr hAPECompress, out int pBytesAvailable);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APECompress_UnlockBuffer(IntPtr hAPECompress,
                                   int nBytesAdded,
                                   bool bProcess);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APECompress_Finish(IntPtr hAPECompress,
                             [MarshalAs(UnmanagedType.LPArray,
                                 SizeParamIndex=2)]
                             byte[] pTerminatingData,
                             int nTerminatingBytes,
                             int nWAVTerminatingBytes);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APECompress_Kill(IntPtr hAPECompress);

        // IAPEDecompress wrapper

        public enum APE_DECOMPRESS_FIELDS
        {
            APE_INFO_FILE_VERSION = 1000,
            APE_INFO_COMPRESSION_LEVEL = 1001,
            APE_INFO_FORMAT_FLAGS = 1002,
            APE_INFO_SAMPLE_RATE = 1003,
            APE_INFO_BITS_PER_SAMPLE = 1004,
            APE_INFO_BYTES_PER_SAMPLE = 1005,
            APE_INFO_CHANNELS = 1006,
            APE_INFO_BLOCK_ALIGN = 1007,
            APE_INFO_BLOCKS_PER_FRAME = 1008,
            APE_INFO_FINAL_FRAME_BLOCKS = 1009,
            APE_INFO_TOTAL_FRAMES = 1010,
            APE_INFO_WAV_HEADER_BYTES = 1011,
            APE_INFO_WAV_TERMINATING_BYTES = 1012,
            APE_INFO_WAV_DATA_BYTES = 1013,
            APE_INFO_WAV_TOTAL_BYTES = 1014,
            APE_INFO_APE_TOTAL_BYTES = 1015,
            APE_INFO_TOTAL_BLOCKS = 1016,
            APE_INFO_LENGTH_MS = 1017,
            APE_INFO_AVERAGE_BITRATE = 1018,
            APE_INFO_FRAME_BITRATE = 1019,
            APE_INFO_DECOMPRESSED_BITRATE = 1020,
            APE_INFO_PEAK_LEVEL = 1021,
            APE_INFO_SEEK_BIT = 1022,
            APE_INFO_SEEK_BYTE = 1023,
            APE_INFO_WAV_HEADER_DATA = 1024,
            APE_INFO_WAV_TERMINATING_DATA = 1025,
            APE_INFO_WAVEFORMATEX = 1026,
            APE_INFO_IO_SOURCE = 1027,
            APE_INFO_FRAME_BYTES = 1028,
            APE_INFO_FRAME_BLOCKS = 1029,
            APE_INFO_TAG = 1030,

            APE_DECOMPRESS_CURRENT_BLOCK = 2000,
            APE_DECOMPRESS_CURRENT_MS = 2001,
            APE_DECOMPRESS_TOTAL_BLOCKS = 2002,
            APE_DECOMPRESS_LENGTH_MS = 2003,
            APE_DECOMPRESS_CURRENT_BITRATE = 2004,
            APE_DECOMPRESS_AVERAGE_BITRATE = 2005,
        }

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static IntPtr
        c_APEDecompress_Create(string pFilename, out int errorCode);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static void
        c_APEDecompress_Destroy(IntPtr hAPEDecompress);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APEDecompress_GetData(IntPtr hAPEDecompress,
                                string pBuffer,
                                int nBlocks,
                                out int pBlocksReceived);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APEDecompress_Seek(IntPtr hAPEDecompress, int nBlockOffset);

        [DllImport("MACDll.dll", SetLastError=true)]
        public extern static int
        c_APEDecompress_GetInfo(IntPtr hAPEDecompress,
                                APE_DECOMPRESS_FIELDS Field,
                                int nParam1,
                                int nParam2);
    }
}
